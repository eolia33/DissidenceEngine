using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using QbBridge;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class Tracker : BaseScript
    {
        public int _serverId;
        public int _playerId;
        public SharedConfig config;
        public BridgeQBCore bridgeQBcore;
        public bool awaiting { get; set; }
        public Dictionary<string, GpsDic> gpsListing { get; set; }
        public Dictionary<string, PlayerNoSql> playersNoSql { get; set; }
        public List<string> frequencyList { get; set; }

        public Tracker(SharedConfig _config, BridgeQBCore _bridgeQBcore)
        {
            config = _config;
            bridgeQBcore = _bridgeQBcore;

            var _frequencyList = new List<string>();
            var _gpsListing = new Dictionary<string, GpsDic>();

            frequencyList = _frequencyList;
            gpsListing = _gpsListing;

            _ = Task.Run(() => { gpsListeningClients(gpsListing, frequencyList); });
        }

        #region IsConnecting / IsUpdatingSettings²

        public async void setNewGpsClient([FromSource] Player player, string frequency, string name, string color, int notification)
        {
            var id = player.Handle;
            var licence = player.Identifiers["license"];


            if (!frequencyList.Contains(frequency))
            {
                frequencyList.Add(frequency);
            }
            else
            {
                var linq = gpsListing.Where(x => x.Value.PedFrequency == frequency && x.Value.PedNotification == 1);
                foreach (var result in linq)
                    nuiNotify(result.Value.PedId, config.msg_otherUserJoinTracker, 1,"");

            }
           
            nuiNotify(id, config.msg_selfUserJoinTracker, 1, frequency);

            Players[Convert.ToInt32(id)].TriggerEvent("cs:engine:client:tracker:connected");

            if (!gpsListing.ContainsKey(licence))
                gpsListing.Add(licence,
                    dictionaryConstruct(licence, id, name, frequency, notification, GetEntityCoords(GetPlayerPed(id)), color, 1, 0));
            else
                dictionaryUpdate(licence, id, name,notification, frequency, GetEntityCoords(GetPlayerPed(id)), color, 1);

            await Task.FromResult(0);
        }

        public void test(string test)
        {
            Debug.WriteLine(test);
        }

        #endregion

        #region IsDisconnecting / Is Leaving
        public void OnPlayerDropped([FromSource] Player player, string reason)
        {
            try
            {
                var licence = player.Identifiers["license"];
                var linq = gpsListing.FirstOrDefault(x => x.Value.PedLicence == licence);

                if (linq.Value.PedLicence != null)
                {
                    if (isItTheLastManStanding(linq.Value.PedFrequency, linq.Value.PedId))
                        frequencyList.Remove(linq.Value.PedFrequency);

                    gpsListing.Remove(linq.Value.PedLicence);

                    var linqb = gpsListing.Where(x => x.Value.PedFrequency == linq.Value.PedFrequency && x.Value.PedNotification == 1);

                    foreach (var result in linqb)
                        nuiNotify(result.Value.PedId, config.msg_otherUserLeaveTracker, 1, result.Value.PedName);
                }
            }

            catch (Exception)
            {
            }
        }

        public void userDuty(string id, bool duty)
        {
            Debug.WriteLine("player duty" + id + "is " + duty);
        }

        public void userIsLeaving([FromSource] Player player, int reason)
        {
            try
            {
                var licence = player.Identifiers["license"];
                var linq = gpsListing.FirstOrDefault(x => x.Value.PedLicence == licence);
                if (linq.Value.PedLicence != null)
                {
                    if (isItTheLastManStanding(linq.Value.PedFrequency, linq.Value.PedId))
                        frequencyList.Remove(linq.Value.PedFrequency);

                    gpsListing.Remove(linq.Value.PedLicence);
                    
                    nuiNotify(player.Handle, config.msg_selfUserLeaveTracker,1, "");

                    var linqb = gpsListing.Where(x => x.Value.PedFrequency == linq.Value.PedFrequency && x.Value.PedNotification == 1);

                    foreach (var result in linqb)
                        switch (reason)
                        {
                            case 1:
                                nuiNotify(result.Value.PedId, config.msg_otherUserLeaveTracker, 1, result.Value.PedName);
                                break;

                            case 2:
                                nuiNotify(result.Value.PedId, config.msg_otherUserLeaveTrackerByForce, 1, result.Value.PedName);
                                break;
                        }
                }
            }
            catch (Exception)
            {
            }
        }

        public bool isItTheLastManStanding(string frequency, string pedId)
        {
           if(gpsListing.Where(x => x.Value.PedFrequency == frequency && x.Value.PedId != pedId).Any())
                return false;

            return true;
        }

        #endregion

        #region IsProtectedFrequence

        public void playerJob(string job, string id)
        {
            Debug.WriteLine(job);
            var welcome6 = QbCore.FromJson(job);
            Debug.WriteLine(welcome6.PlayerData.Name);
        }

        public bool checkPlayerJob(Player player)
        {
            for (var i = 0; i < 5; i++)
                if (!awaiting)
                    return true;
                else
                    Thread.Sleep(100);

            return false;
        }

        public bool isThisFrequencyProtected(int frequency, Player player)
        {
            if (frequency == config.s1)
            {
                checkPlayerJob(player);

                foreach (var f in config.m1)
                    if ("tot" == f)
                        return false;

                return true;
            }

            return false;
        }

        #endregion

        #region Task for GPS refreshing

        private Task gpsListeningClients(Dictionary<string, GpsDic> gpsClient, List<string> frequencyList)
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
                    var linqC = gpsClient.Where(x => x.Value.PedFrequency == frequency).Count();

                    if (linqC > 1)
                    {
                        var linq = gpsClient.Where(x => x.Value.PedFrequency == frequency);

                        var gpsListingJson = new Dictionary<string, GpsNetworkClient>();

                        foreach (var result in linq)
                            gpsListingJson.Add(result.Value.PedId, new GpsNetworkClient
                            {
                                PedId = result.Value.PedId,
                                PedName = result.Value.PedName,
                                PedColor = result.Value.PedColor,
                                PedDirection = result.Value.PedDirection,
                                PedCoordinats = result.Value.PedCoordinats
                            });

                        var jsonToPush = JsonConvert.SerializeObject(gpsListingJson);

                        var linqJson = gpsClient.Where(x => x.Value.PedFrequency == frequency);
                        foreach (var result in linqJson)
                            Players[Convert.ToInt32(result.Value.PedId)].TriggerEvent("cs:engine:client:tracker:ping",
                                jsonToPush, config.trackerServerPollingRate, config.trackerBlipSprite);
                    }
                }

                Thread.Sleep(config.trackerServerPollingRate);
            }
        }

        #endregion

        #region Gps Core

        public GpsDic dictionaryConstruct(string licence, string id, string name, string frequency,int notification, Vector3 vector,
            string color, int gpsType, float direction)
        {
            var newGpsClient = new GpsDic
            {
                PedLicence = licence,
                PedId = id,
                PedName = name,
                PedFrequency = frequency,
                PedColor = color,
                PedDirection = direction,
                PedNotification = notification,
                PedCoordinats = vector
            };
            return newGpsClient;
        }

        public void dictionaryUpdate(string licence, string id, string name, int notification, string frequency, Vector3 vector,
            string color, int gpsType)
        {
            var linq = gpsListing.Where(x => x.Value.PedLicence == licence).First();
            gpsListing[linq.Key].PedId = id;
            gpsListing[linq.Key].PedName = name;
            gpsListing[linq.Key].PedFrequency = frequency;
            gpsListing[linq.Key].PedColor = color;
            gpsListing[linq.Key].PedDirection = 0;
            gpsListing[linq.Key].PedCoordinats = vector;
            gpsListing[linq.Key].PedNotification = notification;
        }

        #endregion
        public class GpsNetworkClient
        {
            public string PedId;
            public string PedName;
            public string PedColor;
            public float PedDirection;
            public Vector3 PedCoordinats;
        }

        public void nuiNotify(string player, string[] msg, int type, string replace = null)
        {
            switch (type)
            {
                case 1:
                    Players[Convert.ToInt32(player)].TriggerEvent(config.notificationEngine,msg[0], msg[1].Replace("{replace}", replace), msg[2], msg[3]);
                 break;
                case 2:
                    Players[Convert.ToInt32(player)].TriggerEvent(config.notificationEngine,msg[0], msg[1].Replace("{replace}", replace), msg[2], msg[3]);
                    break;
            }
        }
       public void userNotification([FromSource] Player player,int value)
        {
            var linq = gpsListing.Where(x => x.Value.PedLicence == player.Identifiers["license"]).First();
            gpsListing[linq.Key].PedNotification = value;

            switch (value)
            {
                case 0:
                    nuiNotify(player.Handle, config.msg_selfTrackerNotificationOff, 1);
                    break;

                case 1:
                    nuiNotify(player.Handle, config.msg_selfTrackerNotificationOn, 1);
                    break;
            }

        }
    }
}