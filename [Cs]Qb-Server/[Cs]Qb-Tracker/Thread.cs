using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class T: BaseScript
    {    
        private string                         Licence;
        private SharedConfig                   Config;
        public  Dictionary<string, TrackerDic> TrackerClients              { get; set; }
        public  Dictionary<string, PlayerData> PlayerData                  { get; set; }
        public List<string>                   FrequencyList               { get; set; }
        private Tracker                        Track                       { get; set; }
        public  Dictionary<string, TrackerDic> TrackerDic                  { get; set; }

   
        private Server                         Server                      { get; set; }

        public T(Tracker tracker, SharedConfig config, BridgeQbCore bridge, Server server, Dictionary<string, TrackerDic> trackerDic,List<string> frequencyList)
        {
            Track   = tracker;
            Config     = config;
            Server     = server;
            TrackerDic = trackerDic;

        }
        
           public void tsv()
        {
            Server.C("Tracker : début de la boucle Task");
            while (true)
            {
                foreach (var entry in Track.TrackerClients)
                {    
                    Server.C("Tracker : Boucle Foreach #1 :: Fréquence");
                    entry.Value.PedCoordinats = GetEntityCoords(GetPlayerPed(entry.Value.PedId));
                    entry.Value.PedDirection  = GetEntityHeading(GetPlayerPed(entry.Value.PedId));
                               }

                foreach (var frequency in Track.FrequencyList)
                {
                    Server.C("Tracker : Boucle Foreach #2 :: entrée");

                    if (Track.TrackerClients.Where(x => x.Value.PedFrequency == frequency).Count() > 1)
                    {
                        var linq = Track.TrackerClients.Where(x => x.Value.PedFrequency == frequency);

                        var trackerJson = new Dictionary<string, Tracker.TrackerDicNetwork>();

                        foreach (var client in linq)
                            trackerJson.Add(client.Value.PedId, new Tracker.TrackerDicNetwork
                            {
                                PedId         = client.Value.PedId,
                                PedName       = client.Value.PedName,
                                PedColor      = client.Value.PedColor,
                                PedDirection  = client.Value.PedDirection,
                                PedCoordinats = client.Value.PedCoordinats
                            });

                        var jsonToPush = JsonConvert.SerializeObject(trackerJson);

                        var linqJson = Track.TrackerClients.Where(x => x.Value.PedFrequency == frequency);
                        foreach (var client in linqJson)
                            Players[Convert.ToInt32(client.Value.PedId)].TriggerEvent("cs:engine:client:tracker:ping",
                                jsonToPush, Config.trackerServerPollingRate, Config.trackerBlipSprite);
                    }
                    Server.C("Tracker : Boucle Foreach #2 :: sortie");  
                }
                
                Thread.Sleep(7000);
            }
        }
    }
}