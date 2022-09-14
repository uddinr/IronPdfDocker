using Microsoft.AspNetCore.Mvc;

namespace IronPdfDocker
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        public DocumentController()
        {
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

                IronPdf.Installation.LinuxAndDockerDependenciesAutoConfig = false;
                IronPdf.Installation.ChromeGpuMode = IronPdf.Engines.Chrome.ChromeGpuModes.Disabled;

                var render = new IronPdf.ChromePdfRenderer();
                using var doc = render.RenderHtmlAsPdf($"<h1>HELLO WORLD at {DateTime.UtcNow.ToLocalTime():yyyy-MMM-dd HH:mm:ss \"GMT\"zzz}</h1>");
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

                IronPdf.Installation.LinuxAndDockerDependenciesAutoConfig = false;
                IronPdf.Installation.ChromeGpuMode = IronPdf.Engines.Chrome.ChromeGpuModes.Disabled;

                var render = new IronPdf.ChromePdfRenderer();
                using var doc = await render.RenderHtmlAsPdfAsync($"<h1>HELLO WORLD at {DateTime.UtcNow.ToLocalTime():yyyy-MMM-dd HH:mm:ss \"GMT\"zzz}</h1>");
                doc.SaveAs("output.pdf");

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}