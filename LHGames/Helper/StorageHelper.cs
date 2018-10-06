using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LHGames.Helper
{
    public static class StorageHelper
    {
        private static Dictionary<string, string> _document;
        private static string _path;

        public static void Write(string key, object data)
        {
            Init();

            try
            {
                _document[key] = JsonConvert.SerializeObject(data);
                Store();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static T Read<T>(string key)
        {
            Init();

            try
            {
                if (_document.TryGetValue(key, out var data))
                {
                    return JsonConvert.DeserializeObject<T>(data);
                }

                return default(T);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(T);
            }
        }

        private static void Init()
        {
            try
            {
                if (Environment.GetEnvironmentVariable("LOCAL_STORAGE") != null)
                {
                    _path = $"{Environment.GetEnvironmentVariable("LOCAL_STORAGE")}/document.json";
                }
                else
                {
                    _path = "/data/document.json";
                }

                if (System.IO.File.Exists(_path))
                {
                    var data = System.IO.File.ReadAllText(_path);
                    _document = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                }
                else
                {
                    _document = new Dictionary<string, string>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Store()
        {
            try
            {
                System.IO.File.WriteAllText(_path, JsonConvert.SerializeObject(_document));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}