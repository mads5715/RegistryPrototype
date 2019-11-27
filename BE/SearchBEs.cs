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
