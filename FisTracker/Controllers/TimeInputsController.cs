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
using Google.Apis.Auth.OAuth2;

namespace FisTracker.Controllers
{
    [Route("api/[controller]")]
    public class TimeInputsController : BaseController
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
            _logger.LogInformation($"request for timesheet of {month}/{year} from user {this.CurrentUser.UserId}");
            DateTime d = new DateTime(year, month, 1);
            var times = await _context.TimeInputs.Where(ti =>
                ti.UserId == this.CurrentUser.UserId &&
                ti.Date >= d && ti.Date < d.AddMonths(1)
            ).OrderBy(ti => ti.Date).ToListAsync();
            var result = new TimeSheet(new DateTime(year, month, 1),
                new DateTime(year, month, 1).AddMonths(1).AddDays(-1),
                times);
            return result;
        }

        // POST: api/TimeInputs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// checks if provided time input exists. Is it does, it is updated. New entry created otherwise.
        /// </summary>
        /// <param name="timeInput"></param>
        /// <returns>new/updated timeinput</returns>
        [HttpPost]
        public async Task<ActionResult<TimeInput>> PostTimeInput(TimeInputRequest timeInput)
        {
            var ti = _context.TimeInputs.Find(timeInput.Date, this.CurrentUser.UserId);
            if (ti == null)
            {
                ti = new()
                {
                    Date = timeInput.Date,
                    UserId = this.CurrentUser.UserId,
                    HomeOffice = timeInput.HomeOffice,
                    In = Helpers.ParseTimeSpan(timeInput.In, true).Value,
                    Out = Helpers.ParseTimeSpan(timeInput.Out),
                    LunchOut = Helpers.ParseTimeSpan(timeInput.LunchOut),
                    LunchIn = Helpers.ParseTimeSpan(timeInput.LunchIn)
                };
                _context.TimeInputs.Add(ti);
            }
            else
            {
                ti.HomeOffice = timeInput.HomeOffice;
                ti.In = Helpers.ParseTimeSpan(timeInput.In);
                ti.Out = Helpers.ParseTimeSpan(timeInput.Out);
                ti.LunchOut = Helpers.ParseTimeSpan(timeInput.LunchOut);
                ti.LunchIn = Helpers.ParseTimeSpan(timeInput.LunchIn);
            }

            await _context.SaveChangesAsync();

            return Ok(new TimeInputResponse(ti));
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
            else if (timeInput.UserId != this.CurrentUser.UserId)
            {
                return BadRequest("not yours");
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
                return BadRequest(new MessageResult() { Message = "wrong file type (image/png allowed)" });
            }

            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (image.Length > 0)
                {
                    var fileName = $"image_{this.CurrentUser.UserId}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.png";
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                    //var text = GetImageText(fullPath);

                    //SaveParsedText(text);

                    return Ok(new MessageResult() { Message = "Image saved" });
                }
                else
                {
                    return BadRequest(new MessageResult() { Message = "Request does not contain image" });
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("Failed to process image", ex);
                return StatusCode(500, new MessageResult() { Message = "Failed to process image" });
            }

        }

        #region private methods

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
                        ti.UserId = this.CurrentUser.UserId;
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
            Google.Cloud.Vision.V1.Image image1 = Google.Cloud.Vision.V1.Image.FromFile(path);
            // using Google credentials from environment variable
            /* ImageAnnotatorClient client = ImageAnnotatorClient.Create();*/

            // Explicitly use service account credentials by specifying the private key file.
            var builder = new ImageAnnotatorClientBuilder();
            builder.CredentialsPath = "fis-tracker-333313-6721d1592185.json";
            ImageAnnotatorClient client = builder.Build();

            IReadOnlyList<EntityAnnotation> textAnnotations = client.DetectText(image1);
            /*foreach (EntityAnnotation text in textAnnotations)
            {
                System.Diagnostics.Debug.WriteLine($"Description: {text.Description}");
            }*/
            return textAnnotations;
        }
        #endregion
    }
}
