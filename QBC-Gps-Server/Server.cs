using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class Server : BaseScript
    {
        public static PlayerList PlayerList { get; set; }
        public Dictionary<string, GpsDic> gpsListing { get; set; }
        public List<string> frequencyList { get; set; }

        public Server()
        {
            List<string> _frequencyList = new List<string>();
            Dictionary<string, GpsDic> _gpsListing = new Dictionary<string, GpsDic>();

            gpsListing = _gpsListing;
            frequencyList = _frequencyList;

            frequencyList.Add("1");
            frequencyList.Add("2");

            EventHandlers["getSecurityBraceletCallFromClient"] += new Action<string, string>(getSecurityBraceletCallFromClient);
            EventHandlers["getSecurityBraceletNotificationForPlolice"] += new Action<string, string>(getSecurityBraceletNotificationForPlolice);
            EventHandlers["setNewGpsClient"] += new Action<string, string, string>(setNewGpsClient);

            _ = Task.Run(() => { GpsListeningClients(gpsListing, frequencyList); });
        }

        public GpsDic buildClient(string id, string name, string frequency, Vector3 vector, int gpsType, float direction)
        {
            var newGpsClient = new GpsDic
            {
                PedId = id,
                PedName = name,
                PedFrequency = frequency,
                PedColor = 4,
                PedDirection = direction,
                PedCoordinats = vector
            };
            return newGpsClient;
        }
        public void setNewGpsClient(string id, string name, string frequency)
        {
            if (!frequencyList.Contains(frequency))
            {
                frequencyList.Add(frequency);
            }
            else
            {
                var linq = gpsListing.Where(x => x.Value.PedFrequency == frequency);
                foreach (var result in linq)
                {
                    Players[Convert.ToInt32(result.Value.PedId)].TriggerEvent("QBCore:Notify", "L'utilisateur " + name + " rejoint la balise");
                }
            }

            if (!gpsListing.ContainsKey(id))
                gpsListing.Add(id, buildClient(id, name, frequency, GetEntityCoords(GetPlayerPed(id)), 1,0));
            else
                gpsListingUpdate(id, name, frequency, 3, GetEntityCoords(GetPlayerPed(id)), 1);
        }

        public void gpsListingUpdate(string id, string name, string frequency, int color, Vector3 vector, int gpsType)
        {
            var linq = gpsListing.Where(x => x.Value.PedId == id).First();
                gpsListing[linq.Key].PedId = id;
                gpsListing[linq.Key].PedName = name;
                gpsListing[linq.Key].PedFrequency = frequency;
                gpsListing[linq.Key].PedColor = color;
                gpsListing[linq.Key].PedDirection = 0;
                gpsListing[linq.Key].PedCoordinats = vector;
        }

        public void getSecurityBraceletCallFromClient(string playerId, string targetId)
        {
            Players[Convert.ToInt32(targetId)].TriggerEvent("QBCore:Notify", "Votre bracelet electronique vient d'être activé");

            Vector3 playerCoords = GetEntityCoords(GetPlayerPed(targetId));


            Players[Convert.ToInt32(playerId)].TriggerEvent("securtyBraceletRespFromServ", playerId, targetId, playerCoords);           
        }
        public void getSecurityBraceletNotificationForPlolice(string playerId, string message)
        {
         Players[Convert.ToInt32(playerId)].TriggerEvent("QBCore:Notify", message);
        }


        private async Task GpsListeningClients(Dictionary<string, GpsDic> gpsClient, List<string> frequencyList)
        {
            while (true)
            {
                foreach (var entry in gpsListing)
                { 
                    entry.Value.PedCoordinats = GetEntityCoords(GetPlayerPed(entry.Value.PedId));
                    entry.Value.PedDirection = GetEntityHeading(GetPlayerPed(entry.Value.PedId));
                }

                foreach (var frequency in frequencyList)
                {
                    var linq = gpsClient.Where(x => x.Value.PedFrequency == frequency);
                    Dictionary<string, GpsDic> GpsListingJson = new Dictionary<string, GpsDic>();

                    foreach (var result in linq)
                    {
                        GpsListingJson.Add(result.Value.PedId, buildClient(result.Value.PedId, result.Value.PedName, result.Value.PedFrequency, result.Value.PedCoordinats,4, result.Value.PedDirection));
                    }

                    var JsonToPush = JsonConvert.SerializeObject(GpsListingJson);

                    var linqJson = gpsClient.Where(x => x.Value.PedFrequency == frequency);


                    foreach (var result in linqJson)
                    {
                        Players[Convert.ToInt32(result.Value.PedId)].TriggerEvent("gpsPositionsFromServer", JsonToPush);
                    }
                }
            System.Threading.Thread.Sleep(1000);

            }
        }

        public class GpsDic
        {
            public string PedId;
            public string PedName;
            public string PedFrequency;
            public int PedColor;
            public float PedDirection;
            public Vector3 PedCoordinats;
        }

        public class GpsListJson
        {
            public string PedId;
            public string PedName;
            public string PedFrequency;
            public int PedColor;
            public float PedDirection;
            public Vector3 PedCoordinats;
        }
    }
}