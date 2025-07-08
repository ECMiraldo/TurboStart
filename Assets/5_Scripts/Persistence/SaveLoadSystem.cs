using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityUtils;

namespace Persistence
{
    public class SaveLoadSystem : Singleton<SaveLoadSystem>
    {
        [field: SerializeField] public ProfileData data { get; private set; }

        private string filePath;
        private JsonSerializerSettings serializerSettings;
        protected override  void Awake()
        {
            base.Awake();
            filePath = Application.persistentDataPath + "/PersistentData.json";
            serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };
            LoadLastSavedProfile();
            Application.wantsToQuit += OnApplicationWantsToQuit;
        }
        public void SaveProfile(bool overwrite = true)
        {
            if (!overwrite && File.Exists(filePath))
            {
                throw new IOException($"The save file already exists and cannot be overwritten.");
            }
            string savedData = JsonConvert.SerializeObject(data, serializerSettings);
            //Logger.LogPersistence($"Saved Data: {savedData} ");
            File.WriteAllText(filePath, savedData);
        }

        public void LoadProfile()
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"No Persitent Data file found");
            }
            string loadedData = File.ReadAllText(filePath);
            //Logger.LogPersistence($"Loaded Data: {loadedData}");
            data = JsonConvert.DeserializeObject<ProfileData>(loadedData);
        }

        public void Delete()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public List<string> ListSaves()
        {
            List<string> list = new List<string>();
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(path) == ".json")
                {
                    string saveFile = Path.GetFileNameWithoutExtension(path);
                    //Logger.LogPersistence($"Found save file with name {saveFile}");
                    list.Add(saveFile);
                }
            }
            return list;
        }
        public void LoadLastSavedProfile()
        {
            var folder = new DirectoryInfo(Application.persistentDataPath);
            var files = folder.EnumerateFiles();
            var filtered = files.Where(f => f.Extension == ".json").ToList();
            if (filtered.Count > 0)
            {
                var lastModifiedFile = filtered.OrderBy(fi => fi.LastWriteTime).Last();
                LoadProfile();
            }
            else
            {
                //Logger.LogPersistence("No saved data found");
                data = new();
            }
        }

        private bool OnApplicationWantsToQuit()
        {
            SaveProfile();
            return true;
        }
    }
}

