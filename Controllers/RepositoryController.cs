using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RegistryPrototype.DAL;
using RegistryPrototype.DAL.Repositories;
using RestSharp;
using SystemFile = System.IO.File;

namespace RegistryPrototype.Controllers
{
    [Route("/")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private IRestClient forwardClient;
        public RepositoryController() {
            forwardClient = new RestClient("https://registry.npmjs.org/");
        }
        // GET api/values
        [Produces("application/vnd.npm.install-v1+json")]
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            Debug.WriteLine("NPM want's package with name: "+ name);
            //var headers = HttpContext.Request.Headers;
            //foreach (var item in headers)
            //{
            //    Console.WriteLine(item.Key + " / " + item.Value);
            //}
            //using (var reader = new StreamReader(HttpContext.Request.Body))
            //{
            //    var body = reader.ReadToEnd();
            //
            //    Console.WriteLine("Body: " + body);
            //}
            using (var repo = new PackageRepository())
            {
                var decodedname = HttpUtility.UrlDecode(name);
                if (repo.ElementExist(decodedname))
                {
                    return await Task.FromResult(Ok(repo.GetSingleElement(decodedname)));
                }
            }
            /*Always passthrough for now, perhaps make a shallow copy of every package asked for,
             so we have a fast local copy instead of relying on this slower forwarding.
            Check if we have a copy in the DB or on file possibly, if use of files get's implemented
            CORRECTION:We only passthrough if we can't find the package in the DB
            TODO: Pass to a background task to save the new package in the DB for future use, so we have a copy
             */
            var request = new RestRequest("{name}", Method.GET);
            request.AddUrlSegment("name",HttpUtility.UrlDecode(name));
            request.AddHeader("Accept", "application/vnd.npm.install-v1+json");
            var returnContent = "";
            var response = forwardClient.Execute(request);
            returnContent = response.Content;
            if (returnContent != string.Empty)
            {
                //We parse it to get it to return a proper JSON object, kinda weird but it works...
                return await Task.FromResult(Ok(JObject.Parse(returnContent)));
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

        //The publish method, we still print the headers here just to be sure everything is okay
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
