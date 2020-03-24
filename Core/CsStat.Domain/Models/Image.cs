using System;
using Newtonsoft.Json;

namespace CsStat.Domain.Models
{
    public class Image
    {
        public string Url { get; set; }
        public string FullUrl => $"{Settings.AdminPath}{Url}";
    }
}