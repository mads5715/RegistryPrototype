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
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.BE
{
    public class Links
    {
        public string npm { get; set; }
        public string homepage { get; set; }
        public string repository { get; set; }
        public string bugs { get; set; }
    }

    public class Publisher
    {
        public string username { get; set; }
        public string email { get; set; }
    }

    public class Maintainer
    {
        public string username { get; set; }
        public string email { get; set; }
    }

    public class Package
    {
        public string name { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public List<string> keywords { get; set; }
        public DateTime date { get; set; }
        public Links links { get; set; }
        public Publisher publisher { get; set; }
        public List<Maintainer> maintainers { get; set; }
    }

    public class Detail
    {
        public double quality { get; set; }
        public double popularity { get; set; }
        public double maintenance { get; set; }
    }

    public class Score
    {
        public double final { get; set; }
        public Detail detail { get; set; }
    }

    public class SObject
    {
        public Package package { get; set; }
        public Score score { get; set; }
        public double searchScore { get; set; }
    }

    public class SearchRootObject
    {
        public List<SObject> objects { get; set; }
        public int total { get; set; }
        public string time { get; set; }
    }
}
