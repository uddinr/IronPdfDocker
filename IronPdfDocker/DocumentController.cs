using Microsoft.AspNetCore.Mvc;
using IronPdf;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
using System.Drawing;

namespace IronPdfDocker
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocumentController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("Get")]
        public ActionResult Get()
        {
            try
            {
                IronPdf.Logging.Logger.EnableDebugging = true;
                IronPdf.Logging.Logger.LogFilePath = "default.log";
                IronPdf.Logging.Logger.LoggingMode = IronPdf.Logging.Logger.LoggingModes.All;

                Installation.LinuxAndDockerDependenciesAutoConfig = false;
                Installation.ChromeGpuMode = IronPdf.Engines.Chrome.ChromeGpuModes.Disabled;

                const string emcImageName = "emc.jpg";

                var base64EmcImage = GetImageTagAsBase64(emcImageName);

                var render = new ChromePdfRenderer();
                using var doc = render.RenderHtmlAsPdf($"<h1>HELLO WORLD at {DateTime.UtcNow.ToLocalTime():yyyy-MMM-dd HH:mm:ss \"GMT\"zzz}</h1><br/>{base64EmcImage}");
                
                doc.SaveAs("output.pdf");

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAsync")]
        public async Task<ActionResult> GetAsync()
        {
            try
            {
                IronPdf.Logging.Logger.EnableDebugging = true;
                IronPdf.Logging.Logger.LogFilePath = "default.log";
                IronPdf.Logging.Logger.LoggingMode = IronPdf.Logging.Logger.LoggingModes.All;

                Installation.LinuxAndDockerDependenciesAutoConfig = false;
                Installation.ChromeGpuMode = IronPdf.Engines.Chrome.ChromeGpuModes.Disabled;

                var render = new ChromePdfRenderer();
                using var doc = await render.RenderHtmlAsPdfAsync($"<h1>HELLO WORLD at {DateTime.UtcNow.ToLocalTime():yyyy-MMM-dd HH:mm:ss \"GMT\"zzz}</h1>");
                doc.SaveAs("output.pdf");

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        private string GetImageTagAsBase64(string filename)
        {
            var imagePath = $"{_webHostEnvironment.ContentRootPath}/Images/";
            var fileAsBytes = System.IO.File.ReadAllBytes($"{imagePath}{filename}");
            var base64String = Convert.ToBase64String(fileAsBytes);
            var imgDataURI = $"data:image/jpg;base64,{base64String}";

            return $"<img src='{imgDataURI}' />";
        }
    }
}