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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RegistryPrototype.BE;
using RegistryPrototype.DAL;
using RegistryPrototype.DAL.Repositories;

namespace RegistryPrototype.DAL.Commands
{
    public static class DownloadPackageAndUpdateDB
    {
        private const string _exMessage = "Size is too large for the DB!";

        public static async Task<int> DownloadLocalCopyOfPackage(string path)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(new Uri(path)).ConfigureAwait(true))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync().ConfigureAwait(true))
                {
                    using (var ms = new MemoryStream())
                    {
                        streamToReadFrom.CopyTo(ms);
                        LocalFilesystemRegistry.SaveFile(path.Split("/").Last(), ms.ToArray());
                    }
                    return 1;
                }
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
            if (match.Value.Contains("http", StringComparison.CurrentCulture))
            {
                var stringSplit = match.Value.Split("/-/");
                var endString = "";
                foreach (var item in stringSplit)
                {
                    if (item.EndsWith(packname, StringComparison.CurrentCulture))
                    {
                        endString = item.Replace(packname, "api/download/", StringComparison.CurrentCulture);
                    }
                    else
                        endString += item;
                }

                var urlSplit = endString.Split("//").Last().Split("/").First();
                input = Regex.Replace(rawInput, httpRegex, endString, options);
                //The last is just for debugging purposes while developing locally
                input = input.Replace(urlSplit, "192.168.0.10:5000", StringComparison.CurrentCulture).Replace("https", "http", StringComparison.CurrentCulture);
            }
            return input;
        }
        public static void Execute()
        {
            var collection = new List<MinimalPackage>();
            var pack = new PackageRepository();


            foreach (var item in pack.GetAllElements())
            {
                if (!item.RawMetaData.Contains("http://192.168.0.10:5000", StringComparison.CurrentCulture))
                {
                    collection.Add(item);
                }
            }

            foreach (var item in collection)
            {
                var jsonObjPreFix = JObject.Parse(item.RawMetaData);
                var latestVersion = jsonObjPreFix["dist-tags"].First.First.ToString().Replace("{", "", StringComparison.CurrentCulture).Replace("}", "", StringComparison.CurrentCulture);
                //The following code is ment to download the tgz so we can serve it from our own repository, but there's some issues with race conditions...
                var dist = jsonObjPreFix["versions"][latestVersion]["dist"]["tarball"].ToString();
                _ = DownloadLocalCopyOfPackage(dist);
                var input = ReplaceDownloadURL(item.RawMetaData);
                var jsonObj = JObject.Parse(input);
                var stringfu = JsonConvert.SerializeObject(jsonObj);
                if (stringfu.Length >= 5 * 1024 * 1024)
                {
                    Debug.WriteLine("Size greater than 5MB");
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    throw new Exception(message: _exMessage);
                    return;
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                }
                var packageVersions = jsonObj["versions"].ToString();
                var connectionString = ServerStrings.GetMySQLConnectionString();
                using (var conn = new MySqlConnection(connectionString))
                {
                    var result = conn.Execute("INSERT INTO Packages (_ID,RawMetaData,Versions,IsFromPublicRepo) " +
                        "VALUES (@name,@rawMetaData,@versions,1) ON DUPLICATE KEY UPDATE " +
                        "RawMetaData = @rawMetaDataOne, Versions = @versionsOne",
                        new
                        {
                            name = item._ID,
                            rawMetaData = stringfu,
                            versions = packageVersions,
                            rawMetaDataOne = stringfu,
                            versionsOne = packageVersions
                        });
                }
            }
        }
    }
}
