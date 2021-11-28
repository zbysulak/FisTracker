using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FisTracker.Data;
using Google.Cloud.Vision.V1;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace FisTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TimeInputsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TimeInputsController> _logger;

        public TimeInputsController(ILogger<TimeInputsController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/TimeInputs
        [HttpGet]
        public async Task<ActionResult<TimeSheet>> GetTimeInputs(int month, int year)
        {
            int userId = int.Parse(this.User.Claims.First(c => c.Type.Equals("userId")).Value);
            DateTime d = new DateTime(year, month, 1);
            var times = await _context.TimeInputs.Where(ti =>
                ti.UserId == userId &&
                ti.Date >= d && ti.Date < d.AddMonths(1)
            ).OrderBy(ti => ti.Date).ToListAsync();
            var result = new TimeSheet(new DateTime(year, month, 1), 
                new DateTime(year, month, 1).AddMonths(1).AddDays(-1), 
                times);
            return result;
        }

        // GET: api/TimeInputs
        [HttpGet("test")]
        public async Task<ActionResult<TimeSheet>> GetTimeInputs(DateTime from, DateTime to)
        {
            int userId = int.Parse(this.User.Claims.First(c => c.Type.Equals("userId")).Value);
            var times = await _context.TimeInputs.Where(ti =>
                ti.UserId == userId &&
                ti.Date >= from && ti.Date <= to
            ).OrderBy(ti => ti.Date).ToListAsync();
            var result = new TimeSheet(from,
                to,
                times);
            return result;
        }

        // PUT: api/TimeInputs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeInput(int id, TimeInput timeInput)
        {
            if (id != timeInput.Id)
            {
                return BadRequest();
            }

            _context.Entry(timeInput).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeInputExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TimeInputs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TimeInput>> PostTimeInput(TimeInput timeInput)
        {
            _context.TimeInputs.Add(timeInput);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimeInput", new { id = timeInput.Id }, timeInput);
        }

        // DELETE: api/TimeInputs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeInput(int id)
        {
            var timeInput = await _context.TimeInputs.FindAsync(id);
            if (timeInput == null)
            {
                return NotFound();
            }

            _context.TimeInputs.Remove(timeInput);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("parseimage")]
        public ActionResult<MessageResult> ParseImage(IFormFile image)
        {
            if (image.ContentType != "image/png")
            {
                return BadRequest("wrong file type (image/png allowed)");
            }

            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (image.Length > 0)
                {
                    var fileName = $"image_{this.User.Claims.First(c => c.Type == "userId").Value}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.png";
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                    var text = GetImageText(fullPath);

                    SaveParsedText(text);

                    return Ok(new MessageResult() { Message = "image saved" });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }

        private bool CheckDate(DayOfWeek actual, string expected)
        {
            if (string.IsNullOrEmpty(expected)) return false;
            expected = expected.ToUpper().Replace('Ú', 'U').Replace('Č', 'C').Replace('Á', 'A');
            return actual switch
            {
                DayOfWeek.Sunday => expected.Equals("NE"),
                DayOfWeek.Monday => expected.Equals("PO"),
                DayOfWeek.Tuesday => expected.Equals("UT"),
                DayOfWeek.Wednesday => expected.Equals("ST"),
                DayOfWeek.Thursday => expected.Equals("CT"),
                DayOfWeek.Friday => expected.Equals("PA"),
                DayOfWeek.Saturday => expected.Equals("SO"),
                _ => false,
            };
        }

        private DateTime ParseDate(string d)
        {
            _logger.LogDebug($"trying to parse {d} to datetime");
            Regex r = new Regex(@"(\d{1,2}.\d{1,2}. \d{4}).{1,3}(\S{2})");
            var m = r.Match(d);
            if (!m.Success)
            {
                _logger.LogInformation($"failed to parse {d} (regex not passed)");
                return DateTime.MinValue;
            }
            var split = m.Groups[1].Value.Split('.', ',') // sometimes is there , instead of .
                .Select(s =>
                {
                    if (int.TryParse(s, out int i))
                    {
                        return i;
                    }
                    return 0;
                })
                .ToArray();
            if (split.Any(i => i == 0))
            {
                _logger.LogInformation($"failed to parse {d} (date is not a number)");
                return DateTime.MinValue;
            }

            var newDate = new DateTime(split[2], split[1], split[0]);
            if (CheckDate(newDate.DayOfWeek, m.Groups[2].Value))
            {
                return newDate;
            }
            _logger.LogInformation($"failed to parse {d} (day check failed)");
            return DateTime.MinValue;

        }

        private void SaveParsedText(IEnumerable<EntityAnnotation> text)
        {
            var all = text.First().Description.Split("\n");
            var userId = int.Parse(this.User.Claims.First(c => c.Type.Equals("userId")).Value);
            DateTime currentRow = DateTime.MinValue;
            var times = new List<TimeSpan>();
            foreach (var desc in all)
            {
                var date = ParseDate(desc);
                if (date != DateTime.MinValue) // new row
                {
                    if (currentRow != DateTime.MinValue)
                    {
                        var ti = new TimeInput();
                        switch (times.Count)
                        {
                            case 1:
                                ti.In = times[0];
                                break;
                            case 2:
                                ti.In = times[0];
                                ti.Out = times[1];
                                break;
                            case 4:
                                ti.In = times[0];
                                ti.LunchOut = times[1];
                                ti.LunchIn = times[2];
                                ti.Out = times[3];
                                break;
                        }
                        ti.Date = currentRow;
                        ti.UserId = userId;
                        _context.TimeInputs.Add(ti);
                        _context.SaveChanges();
                        times.Clear();
                    }
                    currentRow = date;
                }
                else if (TimeSpan.TryParse(desc, out TimeSpan t)) // time row
                {
                    times.Add(t);
                }
            }

            //attempt to parse by individual pieces
            /*
            text = text.Skip(1); // first is element with all data 
            // i expect it to be sorted left-right, top-bottom
            var currentRowY = 0; // y coordinate
            var currentRowCount = 0;
            var enumerator = text.GetEnumerator();
            string dateString = "";
            var times = new List<string>();
            while (enumerator.MoveNext())
            {
                var cur = enumerator.Current;
                if (currentRowY == 0) {
                    currentRowY = cur.BoundingPoly.Vertices.First().Y;
                }
                else // check if i am still at same row
                {
                    int cY = cur.BoundingPoly.Vertices.First().Y;
                    if (Math.Abs(currentRowY - cY) > 6)
                    {
                        //I think im at next row (or just wrong)
                        var ti = new TimeInput();
                        switch (times.Count) { 
                            case 1:
                                ti.In = TimeSpan.Parse(times[0]);
                                break;
                            case 2:
                                ti.In = TimeSpan.Parse(times[0]);
                                ti.Out = TimeSpan.Parse(times[1]);
                                break;
                            case 4:
                                ti.In = TimeSpan.Parse(times[0]);
                                ti.LunchOut = TimeSpan.Parse(times[1]);
                                ti.LunchIn = TimeSpan.Parse(times[2]);
                                ti.Out = TimeSpan.Parse(times[3]);
                                break;
                        }
                        ti.Date = this.ParseDate(dateString);
                        _context.TimeInputs.Add(ti);
                        _context.SaveChanges();
                        System.Diagnostics.Debug.WriteLine($"Inserted new row ({ti})");
                        currentRowCount = 0;
                        dateString = "";
                        times.Clear();
                    }
                }
                System.Diagnostics.Debug.WriteLine($"Description: {cur.Description}, [{cur.BoundingPoly.Vertices.First().X},{cur.BoundingPoly.Vertices.First().Y}]");
                switch (currentRowCount)
                {
                    case 0: // d.m.
                    case 1: // yyyy
                    case 2: // should be dash
                    case 3: // day name
                        dateString += cur.Description;
                        break;
                    default:
                        times.Add(cur.Description);
                        break;
                }
                currentRowCount++;
            }*/
        }

        private IReadOnlyList<EntityAnnotation> GetImageText(string path)
        {
            // using Google credentials from environment variable
            Google.Cloud.Vision.V1.Image image1 = Google.Cloud.Vision.V1.Image.FromFile(path);
            ImageAnnotatorClient client = ImageAnnotatorClient.Create();

            // Explicitly use service account credentials by specifying the private key file.
            /*var credential = GoogleCredential.FromFile("client_secret_1076643029146-8jd4hna3iigl09bbj4no6ppt56j7diqg.apps.googleusercontent.com.json");
            var builder = new ImageAnnotatorClientBuilder();
            builder.CredentialsPath = "client_secret_1076643029146-8jd4hna3iigl09bbj4no6ppt56j7diqg.apps.googleusercontent.com.json";
            ImageAnnotatorClient client = builder.Build();*/

            IReadOnlyList<EntityAnnotation> textAnnotations = client.DetectText(image1);
            /*foreach (EntityAnnotation text in textAnnotations)
            {
                System.Diagnostics.Debug.WriteLine($"Description: {text.Description}");
            }*/
            return textAnnotations;
        }

        private bool TimeInputExists(int id)
        {
            return _context.TimeInputs.Any(e => e.Id == id);
        }
    }
}
