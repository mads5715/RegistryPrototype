using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemFile = System.IO.File;

namespace RegistryPrototype.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        [HttpGet("{packagename}")]
        public IActionResult DownloadFile(string packagename) {
            Stream stream = new MemoryStream(SystemFile.ReadAllBytes(packagename));

            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.

            return File(stream, "application/octet-stream"); // returns a FileStreamResult
        }
    }
}