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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemFile = System.IO.File;

namespace RegistryPrototype.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        [HttpGet("{packagename}")]
        public IActionResult DownloadFile(string packagename) {
            if (SystemFile.Exists(packagename))
            {
                Stream stream = new MemoryStream(SystemFile.ReadAllBytes(packagename));

                if (stream == null)
                    return NotFound(); // returns a NotFoundResult with Status404NotFound response.

                return File(stream, "application/octet-stream"); // returns a FileStreamResult
            }
            else { return NotFound(); }
           
        }
    }
}