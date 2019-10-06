using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
            return Ok("{ \"versions\":{ \"0.0.0\":{ \"name\":\"helloworld\",\"version\":\"0.0.0\",\"dependencies\":{ },\"devDependencies\":{ },\"_hasShrinkwrap\":false,\"directories\":{ },\"dist\":{ \"shasum\":\"2e5cc78b0fe5708bde8410e0f4dd2b9e328dd357\",\"tarball\":\"https://registry.npmjs.org/helloworld/-/helloworld-0.0.0.tgz\"},\"engines\":{ \"node\":\"*\"} } },\"name\":\"helloworld\",\"dist-tags\":{ \"latest\":\"0.0.0\"},\"modified\":\"2011-09-20T23:58:58.133Z\"}");
        }

        // GET api/values/5
        [HttpGet]
        [Route("./")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
