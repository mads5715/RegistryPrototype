using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL
{
    public class AddPackageCommand : ICommand<string>
    {
        public string Execute(string input)
        {
            using (var conn = new MySqlConnection("server = 192.168.0.18; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = NPMRegistryClone;"))
            {
                var filename = "";
                var regex = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$";
                var data = new byte[10240];
                var jsonObj = JObject.Parse(input);
                var name = jsonObj["name"].ToString();
                var newInput = input.Replace("/" + name + "/-/", "/api/download/");
                jsonObj = JObject.Parse(newInput);
                var latestVersion = jsonObj["dist-tags"]["latest"].ToString();
                var packageAuther = jsonObj["versions"][latestVersion]["author"]["name"].ToString();
                var packageDesc = jsonObj["description"].ToString();
                var packageVersions = jsonObj["versions"].ToString();
                var distTags = jsonObj["dist-tags"].ToString();
                var tarballUrl = jsonObj["versions"][latestVersion]["dist"]["tarball"].ToString();
                //String magic....
                var guid = jsonObj["_id"].ToString();
                jsonObj["versions"][latestVersion]["dist"]["tarball"] = tarballUrl;
                filename = name + "-" + latestVersion + ".tgz";
                data = Convert.FromBase64String(jsonObj["_attachments"][filename]["data"].ToString());
                var attachmentlenth = jsonObj["_attachments"][filename]["length"];
                Console.WriteLine("Upload length: " + attachmentlenth);
                if (Convert.ToInt32(attachmentlenth) == data.Length && Regex.Match(jsonObj["_attachments"][filename]["data"].ToString(), regex, RegexOptions.CultureInvariant).Success)
                {
                    Console.WriteLine("The length matched so we can be sure it's at least somewhat not corrupted!");
                }
                Console.WriteLine("Filename: " + filename);
                Console.WriteLine("Body: " + input);
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
