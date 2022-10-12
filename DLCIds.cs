using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;

namespace APPID
{
    internal class DLCIds
    {

        public static dynamic getDLCs(string APPID)
        {
            try
            {
                string BaseURL = $"https://store.steampowered.com/api/appdetails/";
                var client = new RestClient(BaseURL);
                Console.WriteLine($"Requesting: {BaseURL}{APPID}");
                var request = new RestRequest($"?appids={APPID}", Method.Get);
                request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                var queryResult = client.Execute(request);
                var obj = JsonConvert.DeserializeObject<dynamic>(queryResult.Content);
                return obj;
            }
            catch
            {
                return null;
            }
        }

        public static string DLC(string APPID) 
        {
            string DLCList = "";
            var bah = getDLCs(APPID);
            foreach (var DLC in bah)
            {
                foreach (var bah2 in DLC)
                {
                    foreach (var bah3 in bah2.data.dlc)
                    {
                       var ballss = bah2.data.Achivements;
                        foreach (var ball in ballss)
                        {
                            var bally = ball;
                        
                            string balls2 = ball.name;
                            string description = ball.description;
                            DLCList += $"{bah3.ToString()}={balls2}";
                        }
                        var gah = getDLCs(bah3.ToString());
                        foreach (var Name in gah)
                        {
                            foreach (var gah2 in Name)
                            {
                                if (!String.IsNullOrWhiteSpace(DLCList))
                                {
                                    DLCList += "\n";
                                }
                                string balls = gah2.data.name.ToString();
                                DLCList += $"{bah3.ToString()}={balls}";
                            }
                        }
                    }
                
                }
            }

            return DLCList;
        }
    }
}
