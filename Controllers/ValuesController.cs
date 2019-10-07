using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SystemFile = System.IO.File;

namespace RegistryPrototype.Controllers
{
    [Route("/")]
    [ApiController]
    public class ValuesController : ControllerBase
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

            
            return Ok("{ \"versions\":{ \"0.0.0\":{ \"name\":\"testpackaging\",\"version\":\"1.0.0\",\"dependencies\":{ },\"devDependencies\":{ },\"_hasShrinkwrap\":false,\"directories\":{ },\"dist\":{ \"shasum\":\"2e5cc78b0fe5708bde8410e0f4dd2b9e328dd357\",\"tarball\":\"http://192.168.0.10/api/download/testpackage-0.0.0.tgz\"},\"engines\":{ \"node\":\"*\"} } },\"name\":\"helloworld\",\"dist-tags\":{ \"latest\":\"0.0.0\"},\"modified\":\"2011-09-20T23:58:58.133Z\"}");
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
            var data = new byte[10240];//10meg buffer...
            var filename = "";
            var regex = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$";
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var body = reader.ReadToEnd();
                var jsonObj = JObject.Parse(body);
                var attachmentlenth = jsonObj["_attachments"]["testpackage-1.0.0.tgz"]["length"];
                filename = jsonObj["name"].ToString()+ "-" + jsonObj["dist-tags"]["latest"].ToString()+".tgz";
                data = Convert.FromBase64String(jsonObj["_attachments"][filename]["data"].ToString());
                Console.WriteLine("Upload length: "+ attachmentlenth);
                if (Convert.ToInt32(attachmentlenth) == data.Length)
                {
                    Console.WriteLine("The length matched so we can be sure it's at least somewhat not corrupted!");
                }
                Console.WriteLine("Filename: "+ filename);
                Console.WriteLine("Body: " + body);
            }
            SystemFile.WriteAllBytes(filename, data);
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
