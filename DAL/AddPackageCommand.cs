using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL
{
    public class AddPackageCommand : ICommand<string>
    {
        private string ReplaceDownloadURL(string rawInput) {
            var tempjsonObj = JObject.Parse(rawInput);
            var packname = tempjsonObj["name"].ToString();
            //Find the URL Regex Expression
            var httpRegex = @"((\w+:\/\/)[-a-zA-Z0-9:@;?&=\/%\+\.\*!'\(\),\$_\{\}\^~\[\]`#|]+)";
            //We want the Regex engine to run as if it was JS, as it's easier to test against...
            var options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ECMAScript;
            var match = Regex.Match(rawInput, httpRegex, options);
            var input = rawInput;
            //Old school find, split, replace, build string
            if (match.Value.Contains("http"))
            {
                var stringSplit = match.Value.Split("/-/");
                var endString = "";
                foreach (var item in stringSplit)
                {
                    if (item.EndsWith(packname))
                    {
                        endString = item.Replace(packname, "api/download/");
                    }
                    else
                        endString += item;
                }
                
                input = Regex.Replace(rawInput, httpRegex, endString, options);
            }
            return input;
        }
        public string Execute(string rawInput)
        {
            var input = ReplaceDownloadURL(rawInput);
            using (var conn = new MySqlConnection("server = 192.168.0.18; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = NPMRegistryClone;"))
            {
                var filename = "";
                var regex = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$";
                var data = new byte[10240];
                var jsonObj = JObject.Parse(input);
                var name = jsonObj["name"].ToString();
                //Assume a latest version...
                //TODO: get the 'latest' or only tag, or multiple tags.. We got the first Tag, and we remove {} from the string....
                var latestVersion = jsonObj["dist-tags"].First.First.ToString().Replace("{","").Replace("}","");

                var packageAuther = jsonObj["versions"][latestVersion]["author"]["name"].ToString();
                var packageDesc = jsonObj["description"].ToString();
                var packageVersions = jsonObj["versions"].ToString();
                var distTags = jsonObj["dist-tags"].ToString();
                //String magic....
                var guid = jsonObj["_id"].ToString();
                filename = name + "-" + latestVersion + ".tgz";
                data = Convert.FromBase64String(jsonObj["_attachments"][filename]["data"].ToString());
                var attachmentlenth = jsonObj["_attachments"][filename]["length"];
                new LocalFilesystemRegistry().SaveFile(filename,data);
                if (Convert.ToInt32(attachmentlenth) == data.Length && Regex.Match(jsonObj["_attachments"][filename]["data"].ToString(), regex, RegexOptions.CultureInvariant).Success)
                {
                    
                }
                var result = conn.Execute("INSERT INTO Packages (Name,_ID,Filename,Author,PackageDescription,RawMetaData,Versions,DistTags) " +
                    "VALUES (@packagename,@uid,@filenameDB,@auther,@desc,@raw,@versions,@dists)" +
                    " ON DUPLICATE KEY UPDATE " +
                    "Name=@packagename," +
                    "Filename=@filenameDB," +
                    "Author=@auther," +
                    "PackageDescription=@desc," +
                    "RawMetaData=@raw," +
                    "Versions=@versions," +
                    "DistTags=@dists",
                    new
                    {
                        packagename = name,
                        uid = guid,
                        filenameDB = filename,
                        auther = packageAuther,
                        desc= packageDesc,
                        raw = input,
                        versions = packageVersions,
                        dists = distTags
                        
                    });
                return filename;
            }


        }
    }
}
