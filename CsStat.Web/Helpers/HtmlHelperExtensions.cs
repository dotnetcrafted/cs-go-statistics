using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CsStat.Web.Helpers
{
    public class HtmlHelperExtensions
    {
        private const string DistPath = "wwwroot\\dist";
        private readonly string ManifestJsonPath = null;

        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HtmlHelperExtensions(IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment)
        {
            _cache = memoryCache;
            _webHostEnvironment = webHostEnvironment;
            ManifestJsonPath = Path.Combine(DistPath, "manifest.json");
        }

        public bool FileExistsInManifest(string fileKey)
        {
            var parsedManifestJson = GetManifestJsonData();
            var fileWebPath = parsedManifestJson[fileKey]?.ToString();
            return !string.IsNullOrEmpty(fileWebPath);
        }

        private dynamic GetManifestJsonData()
        {
            if (_cache.Get(ManifestJsonPath) != null) return _cache.Get(ManifestJsonPath);

            var serverManifestJsonFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, ManifestJsonPath);
            var isManifestExist = !string.IsNullOrEmpty(serverManifestJsonFilePath) && File.Exists(serverManifestJsonFilePath);
            if (!isManifestExist)
            {
                throw new FileNotFoundException("File not found", ManifestJsonPath);
            }



            var fileContent = File.ReadAllText(serverManifestJsonFilePath);
            object manifestFileContent = JsonConvert.DeserializeObject(fileContent);
            
            var _fileProvider = new PhysicalFileProvider(_webHostEnvironment.ContentRootPath);

            IChangeToken token = _fileProvider.Watch(ManifestJsonPath);

            var options = new MemoryCacheEntryOptions()
                        .AddExpirationToken(token);
            _cache.Set(ManifestJsonPath, manifestFileContent, options);
            return _cache.Get(ManifestJsonPath);
        }


        public string GetFileWebPath(string fileKey)
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