using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace TelegramBot.Utilities
{
    public class HandlebarsEngine
    {
        private static readonly IDictionary<string, string> FileCache = new ConcurrentDictionary<string, string>();

        private readonly Func<string, string> _mapPathAction;

        private readonly bool _cacheTemplates;

        public HandlebarsEngine(Func<string, string> mapPath, bool cacheTemplates = false)
        {
            _mapPathAction = mapPath;
            _cacheTemplates = cacheTemplates;
        }

        public static string ProcessTemplate(string templateText, object model)
        {
            var template = HandlebarsDotNet.Handlebars.Compile(templateText);
            return template(model);
        }

        public string ProcessFileTemplate(string path, object model)
        {
            var templateText = GetTemplateText(path);
            return ProcessTemplate(templateText, model);
        }

        public string GetTemplateText(string path)
        {
            return GetFileContent(_mapPathAction(path));
        }

        private string GetFileContent(string fullAbsolutePath)
        {
            if (_cacheTemplates)
            {
                if (FileCache.ContainsKey(fullAbsolutePath))
                {
                    return FileCache[fullAbsolutePath];
                }
                FileCache[fullAbsolutePath] = File.ReadAllText(fullAbsolutePath);

                return FileCache[fullAbsolutePath];
            }

            return File.ReadAllText(fullAbsolutePath);
        }
    }
}