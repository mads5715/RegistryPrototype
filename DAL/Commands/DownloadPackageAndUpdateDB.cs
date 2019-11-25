using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using RegistryPrototype.BE;
using RegistryPrototype.DAL;
using RegistryPrototype.DAL.Repositories;

namespace RegistryPrototype.DAL.Commands
{
    public static class DownloadPackageAndUpdateDB
    {
        public static async Task<int> DownloadLocalCopyOfPackage(string path)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(path))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    var ms = new MemoryStream();
                    streamToReadFrom.CopyTo(ms);
                    new LocalFilesystemRegistry().SaveFile(path.Split("/").Last(), ms.ToArray());
                    return 1;
                }
                return -1;
            }
        }
        public static string ReplaceDownloadURL(string rawInput)
        {
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

                var urlSplit = endString.Split("//").Last().Split("/").First();
                input = Regex.Replace(rawInput, httpRegex, endString, options);
                //The last is just for debugging purposes while developing locally
                input = input.Replace(urlSplit, "192.168.0.10:5000").Replace("https", "http");
            }
            return input;
        }
        public static void Execute()
        {
            var collection = new List<MinimalPackage>();
            using (var pack = new PackageRepository())
            {
                foreach (var item in pack.GetAllElements())
                {
                    if (!item.RawMetaData.Contains("http://192.168.0.10:5000"))
                    {
                        collection.Add(item);
                    }
                }
            }
            foreach (var item in collection)
            {
                var jsonObjPreFix = JObject.Parse(item.RawMetaData);
                var latestVersion = jsonObjPreFix["dist-tags"].First.First.ToString().Replace("{", "").Replace("}", "");
                //The following code is ment to download the tgz so we can serve it from our own repository, but there's some issues with race conditions...
                var dist = jsonObjPreFix["versions"][latestVersion]["dist"]["tarball"].ToString();
                _ = DownloadLocalCopyOfPackage(dist);
                var input = ReplaceDownloadURL(item.RawMetaData);
                var jsonObj = JObject.Parse(input);
                var packageVersions = jsonObj["versions"].ToString();
                using (var conn = new MySqlConnection("server = mysqlServer; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = NPMRegistryClone;"))
                {
                    var result = conn.Execute("INSERT INTO Packages (_ID,RawMetaData,Versions,IFromPublicRepo) " +
                        "VALUES (@name,@rawMetaData,@versions,'1') ON DUPLICATE KEY UPDATE " +
                        "RawMetaData = @rawMetaDataOne, Versions = @versionsOne",
                        new
                        {
                            name = item._ID,
                            rawMetaData = input,
                            versions = packageVersions,
                            rawMetaDataOne = input,
                            versionsOne = packageVersions
                        });
                }
            }
        }
    }
}
