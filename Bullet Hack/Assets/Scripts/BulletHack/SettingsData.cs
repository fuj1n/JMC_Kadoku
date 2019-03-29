using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BulletHack
{
    public static class SettingsData
    {        
        private static Dictionary<string, object> values = new Dictionary<string, object>();
        private static readonly Dictionary<string, List<Action<object>>> listeners = new Dictionary<string, List<Action<object>>>();

        private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
                TypeNameHandling = TypeNameHandling.Objects
        };
        
        public static void SetValue(string name, object value)
        {
            values[name] = value;

            NotifyListeners(name, value);
        }

        public static T GetValue<T>(string name) where T : class
        {
            if (!values.ContainsKey(name) || !(values[name] is T))
                return null;

            return (T) values[name];
        }

        public static void Flush()
        {
            PlayerPrefs.SetString("settings", JsonConvert.SerializeObject(values, Formatting.None, jsonSettings));
        }

        public static void RegisterListener(string name, Action<object> callback)
        {
            if(!listeners.ContainsKey(name))
                listeners.Add(name, new List<Action<object>>());
            
            listeners[name].Add(callback);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            if (PlayerPrefs.HasKey("settings"))
            {
                values = JsonConvert.DeserializeObject<Dictionary<string, object>>(PlayerPrefs.GetString("settings"), jsonSettings);

                foreach (KeyValuePair<string, object> kvp in values)
                {                    
                    NotifyListeners(kvp.Key, kvp.Value);
                }
            }

            Application.quitting += Flush;
        }

        private static void NotifyListeners(string key, object value)
        {
            if (!listeners.ContainsKey(key))
                return;
            
            foreach (Action<object> listener in listeners[key])
                listener(value);
        }
    }
}
