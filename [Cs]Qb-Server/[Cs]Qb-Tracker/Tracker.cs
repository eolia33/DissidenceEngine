using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class Tracker : BaseScript
    {

        private string          Licence;
        private SharedConfig    Config;
        private Dictionary<string, TrackerDic>      TrackerClients    { get; set; }
        public  Dictionary<string, PlayerData> PlayerData   { get; set; }
        private List<string>                    FrequencyList { get; set; }
        
        private Server Server { get; set; }
        
        readonly CancellationTokenSource Cts = new CancellationTokenSource();  

        public Tracker(SharedConfig config, BridgeQbCore bridge, Server server)
        {
    
            Config      = config;
            PlayerData = bridge.PlayerData;
            var licence = "";
            Licence = licence;
            var frequencyList  = new List<string>();
            var trackerClients = new Dictionary<string, TrackerDic>();

            FrequencyList  = frequencyList;
            TrackerClients = trackerClients;
            Server         = server;

            //_ = Task.Run(() => { trackerTask(TrackerClients, FrequencyList); });
            Server.C("Tracker :: Objet initialisé");
        }
        
        #region Connecting to Tracker
        public async void setNewGpsClient([FromSource] Player player, string frequency, string name, string color, int notification)
        {
            var id = player.Handle;
            Licence      =  player.Identifiers["license"];

            if (isRestricted(frequency, id, Licence))
            {
                nuiNotify(id, Config.msg_selfUserNameFrequencyRestricted, 1, "");
                Players[Convert.ToInt32(id)].TriggerEvent("cs:engine:client:tracker:off:forced", 1);
                return;
            }

            if (!FrequencyList.Contains(frequency))
                FrequencyList.Add(frequency);
            else
            {
                var linq = TrackerClients.Where(x => x.Value.PedFrequency == frequency && x.Value.PedNotification == 1);
                foreach (var client in linq)
                    nuiNotify(client.Value.PedId, Config.msg_otherUserJoinTracker, 1, "");
            }

            nuiNotify(id, Config.msg_selfUserJoinTracker, 1, frequency);

            Players[Convert.ToInt32(id)].TriggerEvent("cs:engine:client:tracker:connected");

            if (!TrackerClients.ContainsKey(Server.buildKey(Licence,id)))
            {
                TrackerClients.Add((Server.buildKey(Licence,id)),dictionaryConstruct(Licence, id, name, frequency, notification, GetEntityCoords(GetPlayerPed(id)),
                                                                  color,   1,  0));

            }
            else
                dictionaryUpdate(Server.buildKey(Licence,id), id, name, notification, frequency, GetEntityCoords(GetPlayerPed(id)), color,
                                 1);
            
            await Task.FromResult(0);
        }

        #endregion

        #region Leaving Tracker

        public void userLeaving([FromSource] Player player, int reason)
        {
            userIsLeaving(player.Handle, player.Identifiers["license"], 1);
        }

        public void userLeavingDrop([FromSource] Player player, string reason)
        {
            userIsLeaving(player.Handle, player.Identifiers["license"], 1);
        }

        private void userIsLeaving(string id, string licence, int reason)
        {
            Server.C("Tracker : userIsLeaving :: start");
            if (TrackerClients.Any())
            {
                var linq = TrackerClients.FirstOrDefault(x => x.Key.Contains(Server.buildKey( licence,id)));

                if (linq.Value.PedLicence != null)
                {
                    if (isItTheLastManStanding(linq.Value.PedFrequency, linq.Value.PedId))
                        FrequencyList.Remove(linq.Value.PedFrequency);

                    TrackerClients.Remove(Server.buildKey(linq.Value.PedLicence,linq.Value.PedId));

                    var linqb = TrackerClients.Where(x => x.Value.PedFrequency == linq.Value.PedFrequency &&
                                                      x.Value.PedNotification == 1);

                    switch (reason)
                    {
                        case 1:
                            nuiNotify(id, Config.msg_selfUserLeaveTracker, 1, "");
                            break;
                        case 2:
                            nuiNotify(id, Config.msg_selfUserLeaveTracker, 1, "");
                            break;
                        case 3:
                            nuiNotify(id, Config.msg_selfUserLeaveTrackerDuty, 1, "");
                            break;
                    }

                    foreach (var result in linqb)
                        switch (reason)
                        {
                            case 1:
                                nuiNotify(result.Value.PedId, Config.msg_otherUserLeaveTracker, 1,
                                          result.Value.PedName);
                                break;

                            case 2:
                                nuiNotify(result.Value.PedId, Config.msg_otherUserLeaveTrackerByForce, 2,
                                          result.Value.PedName);
                                break;

                            case 3:
                                nuiNotify(result.Value.PedId, Config.msg_otherUserLeaveTrackerByDuty, 1,
                                          result.Value.PedName);
                                break;
                        }
                }
            }
        }

        public void userColorChange([FromSource] Player player, string color)
        {
            Server.C("Tracker : usercolorChange:: start");
            var linq    = TrackerClients.FirstOrDefault(x => x.Key == Server.buildKey(player.Identifiers["license"],player.Handle));
            var colorOk = Convert.ToInt32(color) + 1;
            TrackerClients[linq.Key].PedColor = colorOk.ToString();
        }


        private bool isItTheLastManStanding(string frequency, string pedId)
        {
            Server.C("Tracker : isTheLastManStanding :: start");
            if (TrackerClients.Where(x => x.Value.PedFrequency == frequency && x.Value.PedId != pedId).Any())
                return false;

            return true;
        }

        #endregion

        #region Is This Frequency Restricted ?

        public void dutySwitcher(string id, string status)
        {           
            Server.C("Tracker : dutySwitcher :: start");
            
            if (status == "True")
                return;

            var linqPlayerData = PlayerData.FirstOrDefault(x => x.Key.Contains(Server.buildKey(Licence,id)));

            if (linqPlayerData.Key != null)
            {
                linqPlayerData.Value.jobOnDuty = status;

                var tracker = TrackerClients.FirstOrDefault(x => x.Key.Contains(Server.buildKey(Licence,id)));

                if (tracker.Key != null)
                {
                    Debug.WriteLine("ceci est mon id" + id);
                    Players[Convert.ToInt32(id)].TriggerEvent("cs:engine:client:tracker:off:forced", 2);
                    userIsLeaving(linqPlayerData.Value.id, Licence, 3);
                }
            }
        }
        
           public async Task trackerTask(Dictionary<string, TrackerDic> trackerClients, List<string> frequencyList)
        {
            Server.C("Tracker : début de la boucle Task");
            while (true)
            {      
                if (Cts.IsCancellationRequested) throw new OperationCanceledException();
                
                foreach (var entry in trackerClients)
                {               
                    entry.Value.PedCoordinats = GetEntityCoords(GetPlayerPed(entry.Value.PedId));
                    entry.Value.PedDirection  = GetEntityHeading(GetPlayerPed(entry.Value.PedId));
                }

                foreach (var frequency in frequencyList)
                {
                    Server.C("Tracker : Boucle Foreach #2 :: entrée");

                    if (trackerClients.Where(x => x.Value.PedFrequency == frequency).Count() > 1)
                    {
                        var linq = trackerClients.Where(x => x.Value.PedFrequency == frequency);

                        var trackerJson = new Dictionary<string, TrackerDicNetwork>();

                        foreach (var client in linq)
                            trackerJson.Add(client.Value.PedId, new TrackerDicNetwork
                            {
                                PedId         = client.Value.PedId,
                                PedName       = client.Value.PedName,
                                PedColor      = client.Value.PedColor,
                                PedDirection  = client.Value.PedDirection,
                                PedCoordinats = client.Value.PedCoordinats
                            });

                        var jsonToPush = JsonConvert.SerializeObject(trackerJson);

                        var linqJson = trackerClients.Where(x => x.Value.PedFrequency == frequency);
                        foreach (var client in linqJson)
                            Players[Convert.ToInt32(client.Value.PedId)].TriggerEvent("cs:engine:client:tracker:ping",
                                jsonToPush, Config.trackerServerPollingRate, Config.trackerBlipSprite);
                    }
                    Server.C("Tracker : Boucle Foreach #2 :: sortie");  
                }

                //await Task.Delay(Config.trackerServerPollingRate);
               Thread.Sleep(Config.trackerServerPollingRate);
            }
        }

        public bool isRestricted(string frequency, string id, string licence)
        { 
            Server.C("Tracker : isRestricted :: start");
            
            var linqFrequency = Config.restrictedFrequencies.Where(x => x.frequency == frequency);

            if (linqFrequency.Any())
            {
                var linqPlayerData = PlayerData.FirstOrDefault(x => x.Key.Contains(Server.buildKey(licence,id)));

                foreach (var f in linqFrequency)
                    if (f.frequency == frequency)
                        foreach (var job in f.jobs)
                            if (job == linqPlayerData.Value.jobName)
                                if (linqPlayerData.Value.jobOnDuty == "True")
                                    return false;
                return true;
            }
            return false;
        }

        #endregion

        #region Background Tracker Engine

     

        #endregion

        #region Tracker Functions / Other

        public TrackerDic dictionaryConstruct(string licence, string id, string name, string frequency, int notification, Vector3 vector,
            string color, int gpsType, float direction)
        {
            var newTrackerClient = new TrackerDic
            {
                PedLicence      = licence,
                PedId           = id,
                PedName         = name,
                PedFrequency    = frequency,
                PedColor        = color,
                PedDirection    = direction,
                PedNotification = notification,
                PedCoordinats   = vector
            };
            return newTrackerClient;
        }

        private void dictionaryUpdate(string licence, string id, string name, int notification, string frequency,
            Vector3  vector, string color, int trackerType)
        {
            var linq = TrackerClients.FirstOrDefault(x => x.Key == Server.buildKey(licence,id));
            TrackerClients[linq.Key].PedId           = id;
            TrackerClients[linq.Key].PedName         = name;
            TrackerClients[linq.Key].PedFrequency    = frequency;
            TrackerClients[linq.Key].PedColor        = color;
            TrackerClients[linq.Key].PedDirection    = 0;
            TrackerClients[linq.Key].PedCoordinats   = vector;
            TrackerClients[linq.Key].PedNotification = notification;
        }
        
        private class TrackerDicNetwork
        {
            public string  PedId;
            public string  PedName;
            public string  PedColor;
            public float   PedDirection;
            public Vector3 PedCoordinats;
        }

        #endregion

        #region NUI / Notifications system

        public void nuiNotify(string player, string[] msg, int type, string replace = null)
        {
            switch (type)
            {
                case 1:
                    Players[Convert.ToInt32(player)].TriggerEvent(Config.notificationEngine, msg[0],
                                                                  msg[1].Replace("{replace}", replace), msg[2], msg[3]);
                    break;
                case 2:
                    Players[Convert.ToInt32(player)].TriggerEvent(Config.notificationEngine, msg[0],
                                                                  msg[1].Replace("{replace}", replace), msg[2], msg[3]);
                    break;
            }
        }

        public void userNotification([FromSource] Player player, int value)
        {
            var linq = TrackerClients.FirstOrDefault(x => x.Key == Server.buildKey(player.Identifiers["license"],player.Handle));
            TrackerClients[linq.Key].PedNotification = value;

            switch (value)
            {
                case 0:
                    nuiNotify(player.Handle, Config.msg_selfTrackerNotificationOff, 1);
                    break;

                case 1:
                    nuiNotify(player.Handle, Config.msg_selfTrackerNotificationOn, 1);
                    break;
            }
        }
        #endregion
    }
}