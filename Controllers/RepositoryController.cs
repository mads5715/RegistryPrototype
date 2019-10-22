using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RegistryPrototype.DAL;
using RestSharp;
using SystemFile = System.IO.File;

namespace RegistryPrototype.Controllers
{
    [Route("/")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private RestClient forwardClient;
        public RepositoryController() {
            forwardClient = new RestClient("https://registry.npmjs.org/");
        }
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
            //Always passthrough for now, perhaps make a shallow copy of every package asked for,
            // so we have a fast local copy instead of relying on this slow forwarding.
            //Check if we have a copy in the DB or on file possibly, if use of files get's implemented
            var request = new RestRequest("{name}", Method.GET);
            request.AddUrlSegment("name",HttpUtility.UrlDecode(name));
            request.AddHeader("Accept", "application/vnd.npm.install-v1+json");
            var returnContent = "";
            var response = forwardClient.Execute(request);
            returnContent = response.Content;
            if (returnContent != string.Empty)
            {
                //Always passthrough...
                return Ok(returnContent);
            }
            else
            {
                return StatusCode(404);
            }
            //return Ok(new GetRawPackageQuery().Execute(name));
        }

        // GET api/values/5
        //This is unused code, it doesn't work as intended...
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
