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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.BE
{
    public class SearchObject
    {
        [JsonProperty("objects")]
        public List<SearchPackage> Packages { get; set; }

        [JsonProperty("total")]
        public int Total
        {
            get
            {
                if (Packages != null)
                {
                    return Packages.Count;
                }
                else { return 0; }
            }
        }

        [JsonProperty("time")]
        public DateTime Date { get { return DateTime.Now; } }
    }
}
