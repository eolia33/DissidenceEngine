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
        public bool awaiting { get; set; }
        public Dictionary<string, GpsDic> gpsListing { get; set; }
        public Dictionary<string, PlayerNoSql> playerNoSql { get; set; }
        public List<string> frequencyList { get; set; }

        public Tracker(SharedConfig _config, BridgeQBCore bridge)
        {
            config       = _config;
            playerNoSql = bridge.playerNoSql;

             var _frequencyList = new List<string>();
            var _gpsListing    = new Dictionary<string, GpsDic>();

            frequencyList = _frequencyList;
            gpsListing    = _gpsListing;

            _ = Task.Run(() => { gpsListeningClients(gpsListing, frequencyList); });
        }

        #region Connecting to Tracker

        public async void setNewGpsClient([FromSource] Player player, string frequency, string name, string color,int notification)
        {
            var id        = player.Handle;
            var licence   = player.Identifiers["license"];
            var canAccess = true;

            if (isRestricted(frequency, id,licence))
            {
                nuiNotify(id, config.msg_selfUserNameFrequencyRestricted, 1, "");
                return;
            }

            if (!frequencyList.Contains(frequency))
                frequencyList.Add(frequency);
            else
            {
                var linq = gpsListing.Where(x => x.Value.PedFrequency == frequency && x.Value.PedNotification == 1);
                foreach (var result in linq)
                nuiNotify(result.Value.PedId, config.msg_otherUserJoinTracker, 1, "");
            }

            nuiNotify(id, config.msg_selfUserJoinTracker, 1, frequency);

            Players[Convert.ToInt32(id)].TriggerEvent("cs:engine:client:tracker:connected");

            if (!gpsListing.ContainsKey(licence))
                gpsListing.Add(licence,dictionaryConstruct(licence,id, name, frequency, notification,GetEntityCoords(GetPlayerPed(id)), color, 1, 0));
            else
                dictionaryUpdate(licence, id, name, notification, frequency, GetEntityCoords(GetPlayerPed(id)), color,1);

            await Task.FromResult(0);
        }

        #endregion

        #region Leaving Tracker

        public void userLeaving([FromSource] Player player, int reason)
        {
            userIsLeaving(player.Handle, player.Identifiers["license"], reason);
        }

        public void userIsLeaving(string id, string licence, int reason)
        {
            try
            {
                var linq = gpsListing.FirstOrDefault(x => x.Value.PedLicence == licence);
                if (linq.Value.PedLicence != null)
                {
                    if (isItTheLastManStanding(linq.Value.PedFrequency, linq.Value.PedId))
                        frequencyList.Remove(linq.Value.PedFrequency);

                    gpsListing.Remove(linq.Value.PedLicence);

                    var linqb = gpsListing.Where(x => x.Value.PedFrequency == linq.Value.PedFrequency &&
                                                      x.Value.PedNotification == 1);

                    switch (reason)
                    {
                        case 1:
                            nuiNotify(id, config.msg_selfUserLeaveTracker, 1, "");
                            break;
                        case 2:
                            nuiNotify(id, config.msg_selfUserLeaveTracker, 1, "");
                            break;
                        case 3:
                            nuiNotify(id, config.msg_selfUserLeaveTrackerDuty, 1, "");
                            break;
                    }

                    foreach (var result in linqb)
                        switch (reason)
                        {
                            case 1:
                                nuiNotify(result.Value.PedId, config.msg_otherUserLeaveTracker, 1,
                                          result.Value.PedName);
                                break;

                            case 2:
                                nuiNotify(result.Value.PedId, config.msg_otherUserLeaveTrackerByForce, 1,
                                          result.Value.PedName);
                                break;

                            case 3:
                                nuiNotify(result.Value.PedId, config.msg_otherUserLeaveTrackerByDuty, 1,
                                          result.Value.PedName);
                                break;
                        }
                }
            }
            catch (Exception)
            {
            }
        }

        public void userColorChange([FromSource] Player player, string color)
        {
            var linq    = gpsListing.Where(x => x.Value.PedLicence == player.Identifiers["license"]).First();
            var colorOk = Convert.ToInt32(color) + 1;
            gpsListing[linq.Key].PedColor = colorOk.ToString();
        }


        public bool isItTheLastManStanding(string frequency, string pedId)
        {
            if (gpsListing.Where(x => x.Value.PedFrequency == frequency && x.Value.PedId != pedId).Any())
                return false;

            return true;
        }

        #endregion

        #region Is This Frequency Restricted ?

        public void dutySwitcher(string id, string status )
        {
            var linqNoSql = playerNoSql.SingleOrDefault(x => x.Value.id == id);
            linqNoSql.Value.jobOnDuty = status;

            var tracker = gpsListing.SingleOrDefault(x => x.Value.PedId == id );

            if (tracker.Value.PedId != null)
            {
                if (isRestricted(tracker.Value.PedFrequency, tracker.Value.PedId, tracker.Value.PedLicence))
                    userIsLeaving(tracker.Value.PedId, tracker.Value.PedLicence, 3);
            }
        }

        public bool isRestricted(string frequency, string id, string licence)
        {
            var linqFrequency = config.restrictedFrequencies.Where(x => x.frequency == frequency);

            if (linqFrequency.Any())
            {
                    var linqNoSql = playerNoSql.FirstOrDefault(x => x.Key.Contains(licence));

                    foreach (var _frequency in linqFrequency)
                    {
                        if (_frequency.frequency == frequency)
                        {
                            foreach (var job in _frequency.jobs)
                            {
                            if (job == linqNoSql.Value.jobName) 
                                if (linqNoSql.Value.jobOnDuty == "True")
                                    return false;     
                            }
                        }
                    }
                return true;
            }
            return false;
        }
                #endregion

        #region Background Tracker Engine

        private Task gpsListeningClients(Dictionary<string, GpsDic> gpsClient, List<string> frequencyList)
        {
            while (true)
            {
                foreach (var entry in gpsListing)
                {
                    entry.Value.PedCoordinats = GetEntityCoords(GetPlayerPed(entry.Value.PedId));
                    entry.Value.PedDirection  = GetEntityHeading(GetPlayerPed(entry.Value.PedId));
                }

                foreach (var frequency in frequencyList)
                {
                    var linqC = gpsClient.Where(x => x.Value.PedFrequency == frequency).Count();

                    if (linqC > 1)
                    {
                        var linq = gpsClient.Where(x => x.Value.PedFrequency == frequency);

                        var gpsListingJson = new Dictionary<string, GpsNetworkClient>();

                        foreach (var result in linq)
                        {
                            gpsListingJson.Add(result.Value.PedId, new GpsNetworkClient
                            {
                                PedId         = result.Value.PedId,
                                PedName       = result.Value.PedName,
                                PedColor      = result.Value.PedColor,
                                PedDirection  = result.Value.PedDirection,
                                PedCoordinats = result.Value.PedCoordinats
                            });
                        }

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

        #region Tracker Functions / Other

        public GpsDic dictionaryConstruct(string licence, string id, string name, string frequency, int notification,
            Vector3 vector,
            string color, int gpsType, float direction)
        {
            var newGpsClient = new GpsDic
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
            return newGpsClient;
        }

        public void dictionaryUpdate(string licence, string id, string name, int notification, string frequency,
            Vector3 vector,
            string color, int gpsType)
        {
            var linq = gpsListing.Where(x => x.Value.PedLicence == licence).First();
            gpsListing[linq.Key].PedId           = id;
            gpsListing[linq.Key].PedName         = name;
            gpsListing[linq.Key].PedFrequency    = frequency;
            gpsListing[linq.Key].PedColor        = color;
            gpsListing[linq.Key].PedDirection    = 0;
            gpsListing[linq.Key].PedCoordinats   = vector;
            gpsListing[linq.Key].PedNotification = notification;
        }


        public class GpsNetworkClient
        {
            public string PedId;
            public string PedName;
            public string PedColor;
            public float PedDirection;
            public Vector3 PedCoordinats;
        }

        #endregion

        #region NUI / Notifications system
        public void nuiNotify(string player, string[] msg, int type, string replace = null)
        {
            switch (type)
            {
                case 1:
                    Players[Convert.ToInt32(player)].TriggerEvent(config.notificationEngine, msg[0],
                                                                  msg[1].Replace("{replace}", replace), msg[2], msg[3]);
                    break;
                case 2:
                    Players[Convert.ToInt32(player)].TriggerEvent(config.notificationEngine, msg[0],
                                                                  msg[1].Replace("{replace}", replace), msg[2], msg[3]);
                    break;
            }
        }

        public void userNotification([FromSource] Player player, int value)
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

        #endregion
    }
}