using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
}