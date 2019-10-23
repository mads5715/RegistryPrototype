using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RegistryPrototype.Controllers
{
    [Route("/-/v1")]
    [ApiController]
    public class v1Controller : ControllerBase
    {
        
        [Route("login")]
        [HttpPost]
        public IActionResult Login()
        {
            var headers = HttpContext.Request.Headers;
            foreach (var item in headers)
            {
                Console.WriteLine(item.Key + " / " + item.Value);
            }
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var body = reader.ReadToEnd();

                Console.WriteLine("Body: " + body);
            }
            return StatusCode(403);
        }
    }


    [Route("/-/npm/v1")]
    [ApiController]
    public class AuditController : ControllerBase 
    {
        /*
            This controller is in the same v1Controller.cs file because the endpoints are both v1, althrough they do have seperate functions. 
        */
        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }

        [Route("security/audits/quick")]
        [HttpPost]
        public IActionResult Security()
        {
            //The header says it's gzip but the GZipStream fails when trying to unzip it...
            //Perhaps we just forward this call to the main npm repo to be on the safe side..
            var headers = HttpContext.Request.Headers;
            foreach (var item in headers)
            {
                Console.WriteLine(item.Key + " / " + item.Value);
            }
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var body = reader.ReadToEnd();

                Console.WriteLine("Body: " + body);
            }
            return StatusCode(200);
        }
    
    }
}