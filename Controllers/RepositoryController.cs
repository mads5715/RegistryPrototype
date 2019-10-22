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
            //Can the raw published json really be used for the download? Nope...
            //TODO: get the "passthrough" mode working
            var request = new RestRequest("{name}", Method.GET);
            request.AddUrlSegment("name",name);
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
