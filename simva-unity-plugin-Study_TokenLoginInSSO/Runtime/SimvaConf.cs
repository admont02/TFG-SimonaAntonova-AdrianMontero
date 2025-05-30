﻿using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Simva
{
    public class SimvaConf
    {
        private static SimvaConf local;

        public static SimvaConf Local
        {
            get 
            {
                return local;
            }
            set
            {
                local = value;
            }
        }

        public string Host { get; set; }

        public string Port { get; set; }

        public string Protocol { get; set; }

        public string SSO { get; set; }

        public string ClientId { get; set; }

        public string Study { get; set; }

        public string Realm { get; set; }

        public string URL
        {
            get
            {
                return Protocol + "://" + Host + ":" + Port;
            }
        }

        public SimvaConf()
        {
            SimvaPlugin.Instance?.Log("[SIMVA CONF] Created...");
        }

        public IEnumerator LoadAsync()
        {
            string contents = "";
            SimvaPlugin.Instance?.Log("[SIMVA CONF] Loading...");

            // WebGL and Android have to use WWW to load from streaming assets
#if (UNITY_WEBPLAYER || UNITY_WEBGL || UNITY_ANDROID) && !UNITY_EDITOR
            SimvaPlugin.Instance?.Log("[SIMVA CONF] Doing WebGL / Android read...");
            UnityWebRequest reader = GetReader();
            yield return reader.SendWebRequest();
            if (string.IsNullOrEmpty(reader.error))
            {
                SimvaPlugin.Instance?.Log("[SIMVA CONF] Request failed: (" + reader.responseCode + " ) " + reader.error + " - " + reader.downloadHandler.data);
                contents = System.Text.Encoding.UTF8.GetString(reader.downloadHandler.data);
            }
#else       // The others can read from System.IO       
            string filePath = GetFilePath();     
            if (System.IO.File.Exists(filePath))
            {
                contents = System.IO.File.ReadAllText(filePath);
            }
#endif
            ParseContents(contents);
            yield return null;
        }

        private void ParseContents(string contents)
        {
            if (!string.IsNullOrEmpty(contents))
            {
                SimvaPlugin.Instance?.Log("[SIMVA CONF] Simva.conf content: " + contents);
                var simvaconf = JObject.Parse(contents);
                Study = simvaconf.Value<string>("study");
                Realm = simvaconf.Value<string>("realm");
                Host = simvaconf.Value<string>("host");
                Protocol = simvaconf.Value<string>("protocol");
                Port = simvaconf.Value<string>("port");
                SSO = simvaconf.Value<string>("sso");
                ClientId = simvaconf.Value<string>("client_id");
            }
        }

        private static UnityWebRequest GetReader()
        {
#if UNITY_WEBPLAYER || UNITY_WEBGL || UNITY_ANDROID
            var FileName = "simva.conf";
#endif
            UnityWebRequest reader = null;
            // Platform dependent StreamingAssets Load https://docs.unity3d.com/Manual/StreamingAssets.html
#if UNITY_WEBPLAYER || UNITY_WEBGL
            reader = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + FileName);
#elif UNITY_ANDROID
            reader = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + FileName);
#endif
            SimvaPlugin.Instance?.Log("[SIMVA CONF] Requesting simva.conf from: " + reader.uri);
            return reader;
        }

        private static string GetFilePath()
        {
            var FileName = "simva.conf";

            // Platform dependent StreamingAssets Load https://docs.unity3d.com/Manual/StreamingAssets.html
#if UNITY_IOS && !UNITY_EDITOR
            var filePath = System.IO.Path.Combine(Application.dataPath + "/Raw", FileName);
#else
            var filePath = System.IO.Path.Combine(Application.streamingAssetsPath + "/", FileName);
#endif
            return filePath;
        }

        public void Save()
        {
            var path = GetFilePath();
            if (Application.isEditor && !Application.isPlaying)
            {
                var simvaconf = new JObject
                {
                    ["study"] = Study,
                    ["host"] = Host,
                    ["protocol"] = Protocol,
                    ["port"] = Port,
                    ["sso"] = SSO,
                    ["client_id"] = ClientId,
                };
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                System.IO.File.WriteAllText(path, simvaconf.ToString(Newtonsoft.Json.Formatting.Indented));
            }
        }
    }
}
