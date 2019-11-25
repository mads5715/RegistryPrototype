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
                Debug.WriteLine(item.Key + " / " + item.Value);
            }
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var body = reader.ReadToEnd();

                Debug.WriteLine("Body: " + body);
            }
            return StatusCode(403);
        }
        [Route("search")]
        [HttpGet]
        public IActionResult Search()
        {
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