using Aspose.OCR;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using IronOcr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Tesseract;

namespace FisTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("parse")]
        public string Parse()
        {
            //return "necum";

            TesseractEngine ocr = new TesseractEngine("./TessData", "eng");
            var img = Pix.LoadFromFile("tyden.png");
            //var img = Pix.LoadFromFile("Snímek obrazovky 2021-11-25 092615.png");//cely
            //var img = Pix.LoadFromFile("Snímek obrazovky 2021-11-25 095809.png"); 
            ocr.SetVariable("tessedit_pageseg_mode", 6);
            ocr.SetVariable("tessedit_char_whitelist", ".:0123456789");
            var res = ocr.Process(img);
            System.Diagnostics.Debug.WriteLine("updated12");
            System.Diagnostics.Debug.WriteLine(res.GetText());
            //ocr.SetVariable("tessedit_char_whitelist", "0123456789"); // If digit only
            return res.GetText();
        }


        [HttpGet("parseiron")]
        public string ParseIron()
        {

            IronTesseract IronOcr = new IronTesseract();
            //IronOcr.Language = OcrLanguage.Czech;
            //var Result = IronOcr.Read("C:\\Users\\zsedlacek\\Development\\FisTracker\\FisTracker\\Snímek obrazovky 2021-11-25 092615.png");//cely
            var Result = IronOcr.Read(System.Drawing.Image.FromFile("Snímek obrazovky 2021-11-25 095809.png"));

            //var a = 

            System.Diagnostics.Debug.WriteLine(Result.Text);
            /*
            TesseractEngine ocr = new TesseractEngine("./TessData", "eng");
            var img = Pix.LoadFromFile("C:\\Users\\zsedlacek\\Development\\FisTracker\\FisTracker\\Snímek obrazovky 2021-11-25 092615.png");
            ocr.SetVariable("tessedit_char_whitelist", ".:0123456789");
            var res = ocr.Process(img); 
            System.Diagnostics.Debug.WriteLine(res.GetText());*/
            //Console.WriteLine(res.GetText());
            //ocr.SetVariable("tessedit_char_whitelist", "0123456789"); // If digit only
            return "precteno" + Result.Text;
        }

        [HttpGet("parseaspose")]
        public string ParseAspose()
        {

            // The path to the documents directory.

            string path = "Snímek obrazovky 2021-11-25 092615.png";

            // Initialize an instance of AsposeOcr

            AsposeOcr libVar = new AsposeOcr();

            // Recognize image

            string slResult = libVar.RecognizeLine(path);

            // Print single line recognized text

            System.Diagnostics.Debug.WriteLine(slResult);

            return "precteno" + slResult;
        }


        [HttpGet("parsegoogle")]
        public string ParseGogle()
        {

            // The path to the documents directory.

            Google.Cloud.Vision.V1.Image image1 = Google.Cloud.Vision.V1.Image.FromFile("cely s daty.png");
            // Explicitly use service account credentials by specifying 
            // the private key file.
            //var credential = GoogleCredential.FromFile("client_secret_1076643029146-8jd4hna3iigl09bbj4no6ppt56j7diqg.apps.googleusercontent.com.json");
            ImageAnnotatorClient client = ImageAnnotatorClient.Create();

            /*var builder = new ImageAnnotatorClientBuilder();
            builder.CredentialsPath = "client_secret_1076643029146-8jd4hna3iigl09bbj4no6ppt56j7diqg.apps.googleusercontent.com.json";
            ImageAnnotatorClient client = builder.Build();*/

            IReadOnlyList<EntityAnnotation> textAnnotations = client.DetectText(image1);
            foreach (EntityAnnotation text in textAnnotations)
            {
                System.Diagnostics.Debug.WriteLine($"Description: {text.Description}");
            }
            return "precteno";
        }
    }
}
