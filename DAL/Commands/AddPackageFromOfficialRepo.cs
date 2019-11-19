using Dapper;
using Hangfire;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL.Commands
{
    public class AddPackageFromOfficialRepo : ICommand<string>
    {
        public async Task<int> DownloadLocalCopyOfPackage(string path)
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
        private string ReplaceDownloadURL(string rawInput)
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

                // var urlSplit = endString.Split("//").Last().Split("/").First();


                input = Regex.Replace(rawInput, httpRegex, endString, options);
                //The last is just for debugging purposes while developing locally
                //input = input.Replace(urlSplit, "192.168.0.10:5000").Replace("https","http");
            }
            return input;
        }
        public string Execute(string rawInput)
        {
            var jsonObjPreFix = JObject.Parse(rawInput);
            var latestVersion = jsonObjPreFix["dist-tags"].First.First.ToString().Replace("{", "").Replace("}", "");
            //The following code is ment to download the tgz so we can serve it from our own repository, but there's some issues with race conditions...
            var dist = jsonObjPreFix["versions"][latestVersion]["dist"]["tarball"].ToString();
            if (!File.Exists(dist.Split("/").Last()))
            {
                //string jobId = BackgroundJob.Enqueue(() => DownloadPackageAndUpdateDB.Execute());
           
            }
            //Let's just get the dirty stuff fixed first, making the download url proper, so we do not have to thing about it later.
            //var input = ReplaceDownloadURL(rawInput);
            var input = rawInput;
            using (var conn = new MySqlConnection("server = 192.168.0.18; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = NPMRegistryClone;"))
            {
                var filename = "";
                var regex = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$";
                //var data = new byte[10240];
                var jsonObj = JObject.Parse(input);
                var name = jsonObj["name"].ToString();
                var packageVersions = jsonObj["versions"].ToString();
                var distTags = jsonObj["dist-tags"].ToString();

                //String magic....
                var guid = name;
                filename = name + "-" + latestVersion + ".tgz";

                //'m leaving the following code in, we might use it in the future even through we are just retreiving the one on the official repo...
                //data = Convert.FromBase64String(jsonObj["_attachments"][filename]["data"].ToString());
                //var attachmentlenth = jsonObj["_attachments"][filename]["length"];
                //new LocalFilesystemRegistry().SaveFile(filename, data);
                //Save file directly to FS, perhaps we can use this for something later
                //new LocalFilesystemRegistry().SaveFile(name + ".json", Encoding.UTF8.GetBytes(input));
                //if (Convert.ToInt32(attachmentlenth) == data.Length && Regex.Match(jsonObj["_attachments"][filename]["data"].ToString(), regex, RegexOptions.CultureInvariant).Success)
                //{
                //There's a slight chance that it might not have been tampered too much with, well just checking size isn't enough but a fair starting point   
                //}
                var result = conn.Execute("INSERT INTO Packages (Name,_ID,RawMetaData,Versions,DistTags,Filename) " +
                    "VALUES (@packagename,@uid,@raw,@versions,@dists,@fileName)" +
                    " ON DUPLICATE KEY UPDATE " +
                    "Name=@packagename," +
                    "RawMetaData=@raw," +
                    "Versions=@versions," +
                    "DistTags=@dists",
                    new
                    {
                        packagename = name,
                        uid = guid,
                        raw = input,
                        versions = packageVersions,
                        dists = distTags,
                        fileName = filename

                    });
                return filename;
            }
        }
    }
}
