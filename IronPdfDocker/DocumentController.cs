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
                using var doc = render.RenderHtmlAsPdf("<h1>HELLO WORLD</h1>");
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