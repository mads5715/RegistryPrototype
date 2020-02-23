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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RegistryPrototype.BE;
using RegistryPrototype.DAL;
using RegistryPrototype.DAL.Commands;

namespace RegistryPrototype.Controllers
{
    [Route("/-/user/")]
    [ApiController]
    public class CouchDBController : ControllerBase
    {
        private readonly IRepository<User, string> _userRepo;
        public CouchDBController(IRepository<User,string> repo) {
            _userRepo = repo;
        }
        // GET: api/CouchDB
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CouchDB/5
        [Route("org.couchdb.user:")]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var headers = HttpContext.Request.Headers;
            Console.WriteLine("UserGet being called:");
            foreach (var item in headers)
            {
                Console.WriteLine(item.Key + " / " + item.Value);
            }
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var body = reader.ReadToEnd();
               
                Console.WriteLine("Body: " + body);
            }
            
            Console.WriteLine("Username: " + id);
            return Ok(new LoginResponse { Token = TokenCache.Instance.GetTokenForUser(id), IsOk = true });
        }

        // POST: api/CouchDB
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/CouchDB/5
        [Route("org.couchdb.user:")]
        [HttpPut("{username}")]
        public IActionResult CreateUser(string username)
        {
            //Console.WriteLine("CreateUser, but being used as a Login call, is being called:");
            //var headers = HttpContext.Request.Headers;
            //foreach (var item in headers)
            //{
            //    Console.WriteLine(item.Key + " / " + item.Value);
            //}
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var body = reader.ReadToEnd();
                var userToInsert = JsonConvert.DeserializeObject<User>(body);
                
                    var userExists = _userRepo.ElementExist(userToInsert.Name);
                    if (userExists)
                    {
                        var dbUser = _userRepo.GetSingleElement(userToInsert.Name);
                        var result = new PasswordHasher<User>().VerifyHashedPassword(userToInsert, dbUser.Password,userToInsert.Password);
                        Console.WriteLine(result);
                        if (result == PasswordVerificationResult.Success)
                        {
                            return Ok(new LoginResponse { Token = TokenCache.Instance.GetTokenForUser(username), IsOk = true });
                        }
                    }
                    else
                    {
                        return StatusCode(401, new { ok = false, message = "Possibly wrong username, password or email try again" });
                    }
                
                //If user doesn't exist return error and only allow admin controllers to add users...
                //if (true)
                //{
                //    var result = new PasswordHasher<User>().HashPassword(userToInsert, userToInsert.Password);
                //    userToInsert.Password = result;
                //    _ = new AddUserCommand().Execute(userToInsert);
                //}
               
            }
            return Unauthorized();
        }

        // DELETE: user token
        [Route("token/{tokeninput}")]
        [HttpDelete]
        public IActionResult Delete(string tokeninput)
        {
            TokenCache.Instance.RemoveToken(tokeninput);
            return Ok();
        }
    }
}
