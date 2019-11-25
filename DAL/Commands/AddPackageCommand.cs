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
            //Let's just get the dirty stuff fixed first, making the download url proper, so we do not have to thing about it later.
            var input = ReplaceDownloadURL(rawInput);
            using (var conn = new MySqlConnection("server = mysqlServer; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = NPMRegistryClone;"))
            {
                var filename = "";
                var regex = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$";
                var data = new byte[10240];
                var jsonObj = JObject.Parse(input);
                var name = jsonObj["name"].ToString();
                //Assume a latest version...
                //TODO: get the 'latest' or only tag, or multiple tags.. We got the first Tag, and we remove {} from the string....
                //jsonObj["dist-tags"].First.First -> jsonOjs[dist-tags]['latest'][version-number]
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
                //Save file directly to FS, perhaps we can use this for something later
                new LocalFilesystemRegistry().SaveFile(name+".json", Encoding.UTF8.GetBytes(input));
                if (Convert.ToInt32(attachmentlenth) == data.Length && Regex.Match(jsonObj["_attachments"][filename]["data"].ToString(), regex, RegexOptions.CultureInvariant).Success)
                {
                    //There's a slight chance that it might not have been tampered too much with, well just checking size isn't enough but a fair starting point   
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
