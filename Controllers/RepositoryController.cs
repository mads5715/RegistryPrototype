using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RegistryPrototype.DAL;
using SystemFile = System.IO.File;

namespace RegistryPrototype.Controllers
{
    [Route("/")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        // GET api/values
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            Debug.WriteLine("NPM want's package with name: "+ name);
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

            
            return Ok("{ \"versions\":{ \"0.0.0\":{ \"name\":\"testpackaging\",\"version\":\"1.0.0\",\"dependencies\":{ },\"devDependencies\":{ },\"_hasShrinkwrap\":false,\"directories\":{ },\"dist\":{ \"shasum\":\"2e5cc78b0fe5708bde8410e0f4dd2b9e328dd357\",\"tarball\":\"http://192.168.0.14/api/download/testpackage-0.0.0.tgz\"},\"engines\":{ \"node\":\"*\"} } },\"name\":\"helloworld\",\"dist-tags\":{ \"latest\":\"0.0.0\"},\"modified\":\"2011-09-20T23:58:58.133Z\"}");
        }

        // GET api/values/5
        [HttpGet]
        [Route("{packagename}/-/{filename}")]
        public IActionResult GetFile(string packagename, string filename)
        {
            Console.WriteLine("NPM requested the following: "+packagename+"/-/"+filename);
            return Ok();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{name}")]
        public IActionResult Put(string name)
        {
            Debug.WriteLine("NPM want's to publish package with name: " + name);
            var headers = HttpContext.Request.Headers;
            foreach (var item in headers)
            {
                Console.WriteLine(item.Key + " / " + item.Value);
            }
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
               _ =new AddPackageCommand().Execute(reader.ReadToEnd());
            }
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
        }
    }
}
