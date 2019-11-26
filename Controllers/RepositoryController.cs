/*
    Copyright (C) 2019  Mads Dürr-Wium

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RegistryPrototype.BE;
using RegistryPrototype.DAL;
using RegistryPrototype.DAL.Commands;
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
        IRestRequest request = new RestRequest("{name}", Method.GET).AddHeader("Accept", "application/vnd.npm.install-v1+json");
        private readonly IRepository<MinimalPackage, string> _packageRepo;
        public RepositoryController(IRepository<MinimalPackage,string> repository) {
            forwardClient = new RestClient("https://registry.npmjs.org/");
            _packageRepo = repository;
        }
        // GET api/values
        [Produces("application/vnd.npm.install-v1+json")]
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var headers = HttpContext.Request.Headers;
            foreach (var item in headers)
            {
                if (item.Key == "User-Agent")
                {
                    if (item.Value.ToString().StartsWith("npm/"))
                    {
                        Debug.WriteLine("NPM is calling");
                    }
                }
            }
            
            Debug.WriteLine("NPM want's package with name: " + name);
            using (_packageRepo)
            {
                var decodedname = HttpUtility.UrlDecode(name);
                if (_packageRepo.ElementExist(decodedname))
                {
                    Debug.WriteLine("In the DB!");
                    return Ok(_packageRepo.GetSingleElement(decodedname));
                }
            }
            /*Always passthrough for now, perhaps make a shallow copy of every package asked for,
             so we have a fast local copy instead of relying on this slower forwarding.
            Check if we have a copy in the DB or on file possibly, if use of files get's implemented
            CORRECTION:We only passthrough if we can't find the package in the DB
            TODO: Pass to a background task to save the new package in the DB for future use, so we have a copy
             */
            request.AddUrlSegment("name", HttpUtility.UrlDecode(name));
            var returnContent = "";
            var response = forwardClient.Execute(request);
            returnContent = response.Content;
            if (returnContent != string.Empty)
            {
                //Save to disk, and to DB, then return response
                new Thread(() => { _ = AddPackageFromOfficialRepo.Execute(JObject.Parse(returnContent).ToString()); }).Start();
                using (_packageRepo)
                {
                    var decodedname = HttpUtility.UrlDecode(name);
                    if (_packageRepo.ElementExist(decodedname))
                    {
                        Debug.WriteLine("Passing on to the official repo");
                        return Ok(_packageRepo.GetSingleElement(decodedname));
                    }
                    else
                    {
                        var jobj = JObject.Parse(returnContent);
                        _packageRepo.InsertElement(new MinimalPackage { RawMetaData = returnContent });
                    }
                }
                //We parse it to get it to return a proper JSON object, it's also really slow, kinda weird but it works...
                return Ok(JObject.Parse(returnContent));
                //return StatusCode(404);
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
                Debug.WriteLine(item.Key + " / " + item.Value);
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
