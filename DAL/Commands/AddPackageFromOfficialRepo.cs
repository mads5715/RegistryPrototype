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
    public static class AddPackageFromOfficialRepo 
    {
 
        public static string Execute(string rawInput)
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
            using (var conn = new MySqlConnection("server = mysqlServer; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = NPMRegistryClone;"))
            {
                var filename = "";
                var regex = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$";
                //var data = new byte[10240];
                var jsonObj = JObject.Parse(input);
                var name = jsonObj["name"].ToString();
                var packageVersions = jsonObj["versions"].ToString();
                var distTags = jsonObj["dist-tags"].ToString();
                var ofModified = jsonObj["modified"].ToString();
                var modified = DateTime.Parse(ofModified);
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
                var result = conn.Execute("INSERT INTO Packages (Name,_ID,RawMetaData,Versions,DistTags,Filename,Modified,IsFromPublicRepo) " +
                    "VALUES (@packagename,@uid,@raw,@versions,@dists,@fileName,@offModified,1)" +
                    " ON DUPLICATE KEY UPDATE " +
                    "Name=@packagename," +
                    "RawMetaData=@raw," +
                    "Versions=@versions," +
                    "DistTags=@dists," +
                    "Modified=@offModified",
                    new
                    {
                        packagename = name,
                        uid = guid,
                        raw = input,
                        versions = packageVersions,
                        dists = distTags,
                        fileName = filename,
                        offModified = modified
                    });
                return filename;
            }
        }
    }
}
