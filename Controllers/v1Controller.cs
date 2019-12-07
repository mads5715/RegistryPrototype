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
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using RegistryPrototype.BE;
using RegistryPrototype.DAL;
using RegistryPrototype.DAL.Commands;
using RestSharp;

namespace RegistryPrototype.Controllers
{
    [Route("/-/v1")]
    [ApiController]
    public class v1Controller : ControllerBase
    {
        private IRestClient forwardClient;
        IRestRequest request = new RestRequest(Method.GET);
        private readonly IRepository<MinimalPackage, string> _repo;
        public v1Controller(IRepository<MinimalPackage,string> repository)
        {
            forwardClient = new RestClient("https://registry.npmjs.org/-/v1/search");
            _repo = repository;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login()
        {
            var headers = HttpContext.Request.Headers;
            foreach (var item in headers)
            {
                Console.WriteLine(item.Key + " / " + item.Value);
                if (headers.ContainsKey("Authorization"))
                {
                    var token = new StringValues();
                    headers.TryGetValue("Authorization", out token);
                    var isValid = TokenCache.Instance.ValidateToken(token.ToString());
                    if (isValid)
                    {
                        return StatusCode(201);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            //using (var reader = new StreamReader(HttpContext.Request.Body))
            //{
            //    var body = reader.ReadToEnd();
            //
            //    Console.WriteLine("Body: " + body);
            //}
            
           //Just return 403?? Seems weird but seem to work so far...  -.-
            return StatusCode(403);
        }
        [Route("search")]
        [HttpGet]
        public IActionResult Search(string text, int size, int from,float quality,float popularity, float maintenance)
        {
            //Is it just me or is does this seem like an ugly hack?...
            using (_repo)
            {
                if (_repo.GetAllElements().First(x => x._ID.Contains(text)) != null)
                {
                    if (!_repo.GetAllElements().First(x => x._ID.Contains(text)).IsFromPublicRepo)
                    {
                        var item = _repo.GetAllElements().First(x => x._ID.Contains(text));
                        return Ok(new SearchObject { Packages = new List<SearchPackage> { new SearchPackage { RawMetaData = item.RawMetaData, Modified = item.Modified  } } });
                    }
                }
            }
            request.AddQueryParameter("text",text);
            var returnContent = "";
            var response = forwardClient.Execute(request);
            returnContent = response.Content;
            if (returnContent != string.Empty)
            {
                return Ok(returnContent);
            }
           //var headers = HttpContext.Request.Headers;
           //foreach (var item in headers)
           //{
           //    Debug.WriteLine(item.Key + " / " + item.Value);
           //}
           //using (var reader = new StreamReader(HttpContext.Request.Body))
           //{
           //    var body = reader.ReadToEnd();
           //
           //    Debug.WriteLine("Body: " + body);
           //}
           //Console.WriteLine("Search String: " + text);
           //if (text == "yargs")
           //{
           //    return Ok(LocalFilesystemRegistry.ReadStringFile("JSONExamples/json.json"));
           //}
            return StatusCode(403);
        }
    }


    [Route("/-/npm/v1")]
    [ApiController]
    public class AuditController : ControllerBase 
    {
        private IRestClient forwardClient;
        IRestRequest request = new RestRequest("{name}", Method.GET).AddHeader("Accept", "application/vnd.npm.install-v1+json");
        public AuditController()
        {
            forwardClient = new RestClient("https://registry.npmjs.org/-/npm/v1/security/audits/quick");
        }
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
            request.AddBody(Request.Body);
            var returnContent = "";
            var response = forwardClient.Execute(request);
            returnContent = response.Content;
            if (returnContent != string.Empty)
            {
                Console.WriteLine(returnContent);
            }
            
            
            
            
            //The header says it's gzip but the GZipStream fails when trying to unzip it...
            //Perhaps we just forward this call to the main npm repo to be on the safe side..
            var headers = HttpContext.Request.Headers;
            foreach (var item in headers)
            {
                Debug.WriteLine(item.Key + " / " + item.Value);
            }
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var body = reader.ReadToEnd();

                Debug.WriteLine("Body: " + body);
            }
            return StatusCode(200);
        }
    
    }
}