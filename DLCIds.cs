using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;
using SteamAppIdIdentifier;
using static SteamAppIdIdentifier.DataTableGeneration;

namespace APPID
{
    public class DLCIds
    {
        public Dictionary<int, string> DLCDict = new Dictionary<int, string>();
        public DLCIds() { }
        public async void getDLCsAsync(string APPID)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string content = await httpClient.GetStringAsync("https://api.steampowered.com/ISteamApps/GetAppList/v2/");
                Data DLCi = JsonConvert.DeserializeObject<Data>(content);
                foreach (var item in DLCi.Dlc)
                {
                    string name = await getDLCNameAsync(item);
                    DLCDict.Add(item, name);
                }
            }
            catch
            {
            }
        }

        public async Task<string> getTypeAsync(string APPID)
        {
            string type = "";
            try
            {
                HttpClient httpClient = new HttpClient();
                string content = await httpClient.GetStringAsync($"https://store.steampowered.com/api/appdetails/{APPID}");
                Data DLCi = JsonConvert.DeserializeObject<Data>(content);
                 type =DLCi.Type;
            }
            catch
            {
            }
            return type;
        }
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public async Task<string> getDLCNameAsync(int DLCID)
        {
            string name = "";
            try
            {
                HttpClient httpClient = new HttpClient();
                string content = await httpClient.GetStringAsync($"https://store.steampowered.com/api/appdetails/{DLCID}");
                Data DLCi = JsonConvert.DeserializeObject<Data>(content);
                name = DLCi.Name;
            }
            catch
            { }
            return name;
        }
        public async Task<dynamic> getAchievsAsync(int APPID)
        {
            string name = "";
            try
            {
                HttpClient httpClient = new HttpClient();
                string content = await httpClient.GetStringAsync($"https://store.steampowered.com/api/appdetails/{APPID}");
                Data DLCi = JsonConvert.DeserializeObject<Data>(content);
                name = DLCi.Name;
            }
            catch
            { }
            return name;
        }
        #region JSON Properties
        public class APPID
        {
            [JsonProperty("data")]
            public Data Data { get; set; }
        }

        public class Achievements
        {
            [JsonProperty("total")]
            public int Total { get; set; }

        }


        public class Data
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("steam_appid")]
            public int SteamAppid { get; set; }

            [JsonProperty("dlc")]
            public List<int> Dlc { get; set; }

            [JsonProperty("achievements")]
            public Achievements Achievements { get; set; }

            #endregion

        }

    }
}
