using Newtonsoft.Json;
using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace CsStat.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        private const string DistPath = "/dist";
        private const string ManifestJsonPath = DistPath + "/manifest.json";

        public static bool FileExistsInManifest(this HtmlHelper htmlHelper, string fileKey)
        {
            var parsedManifestJson = GetManifestJsonData();
            var fileWebPath = parsedManifestJson[fileKey]?.ToString();
            return !string.IsNullOrEmpty(fileWebPath);
        }

        private static dynamic GetManifestJsonData()
        {
            if (HttpRuntime.Cache[ManifestJsonPath] != null) return HttpRuntime.Cache[ManifestJsonPath];

            var serverManifestJsonFilePath = HttpContext.Current.Server.MapPath(ManifestJsonPath);
            var isManifestExist = !string.IsNullOrEmpty(serverManifestJsonFilePath) && File.Exists(serverManifestJsonFilePath);
            if (!isManifestExist)
            {
                throw new FileNotFoundException("File not found", ManifestJsonPath);
            }

            var fileContent = File.ReadAllText(serverManifestJsonFilePath);
            dynamic manifestFileContent = JsonConvert.DeserializeObject(fileContent);
            HttpRuntime.Cache.Insert(ManifestJsonPath, manifestFileContent, new CacheDependency(serverManifestJsonFilePath));
            return HttpRuntime.Cache[ManifestJsonPath];
        }


        public static string GetFileWebPath(this HtmlHelper htmlHelper, string fileKey)
        {
            var parsedManifestJson = GetManifestJsonData();
            var fileWebPath = parsedManifestJson[fileKey]?.ToString();
            if (string.IsNullOrEmpty(fileWebPath))
            {
                throw new Exception($"File with {fileKey} key not found");
            }

            return fileWebPath;
        }
    }
}