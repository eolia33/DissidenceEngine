﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Configuration;
using QbBridge;
using noSql;


namespace Server
{
    public class Tracker : BaseScript
    {
        public int _serverId;
        public int _playerId;
        public Config config;
        public QbSql qbSql;
        public bool awaiting { get; set; }
        public Dictionary<string, GpsDic> gpsListing { get; set; }
        public Dictionary<string, PlayerNoSql> playersNoSql { get; set; }
        public List<string> frequencyList { get; set; }
        public Tracker(Config _config,QbSql _qbSql)
        {
            config = _config;
            qbSql = _qbSql;

            List<string> _frequencyList = new List<string>();
            Dictionary<string, GpsDic> _gpsListing = new Dictionary<string, GpsDic>();

            frequencyList = _frequencyList;
            gpsListing = _gpsListing;

            _ = Task.Run(() => { GpsListeningClients(gpsListing, frequencyList); });
        }
        #region IsConnecting / IsUpdatingSettings²

       public async void setNewGpsClient([FromSource] Player player, string frequency, string name, string color)
        {
            var id = player.Handle.ToString();
            var licence = player.Identifiers["license"];

            if (isThisFrequencyProtected(Convert.ToInt32(frequency), player))
            {
                Players[player.Character.NetworkId].TriggerEvent(config.playerNotification, config.radioAcessDeniedMsg);
                return;
            }

            if (!frequencyList.Contains(frequency))
            {
                frequencyList.Add(frequency);
            }
            else
            {
                var linq = gpsListing.Where(x => x.Value.PedFrequency == frequency);
                foreach (var result in linq)
                {
                    Players[Convert.ToInt32(result.Value.PedId)].TriggerEvent(config.playerNotification, "L'utilisateur " + name + " rejoint la balise");
                }
            }

            Players[Convert.ToInt32(id)].TriggerEvent(config.playerNotification, config.radioAcessSuccesfull + frequency);

            if (!gpsListing.ContainsKey(licence))
                gpsListing.Add(licence, dictionaryConstruct(licence, id, name, frequency, GetEntityCoords(GetPlayerPed(id)), color, 1, 0));
            else
                dictionaryUpdate(licence, id, name, frequency, GetEntityCoords(GetPlayerPed(id)), color, 1);

            await Task.FromResult(0);
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

                    var linqb = gpsListing.Where(x => x.Value.PedFrequency == linq.Value.PedFrequency);

                    foreach (var result in linqb)
                    {
                        Players[Convert.ToInt32(result.Value.PedId)].TriggerEvent("QBCore:Notify", "L'utilisateur " + linq.Value.PedName + " quitte la balise");
                    }
                }
            }

            catch (Exception)
            {

            }
        }
       public void userIsLeaving([FromSource] Player player, int reason)
        {
            Debug.WriteLine("triggerQB");
            TriggerEvent("QBBridge:GetJob", player.Handle, 1);
            try
            {
                var licence = player.Identifiers["license"];
                var linq = gpsListing.FirstOrDefault(x => x.Value.PedLicence == licence);
                if (linq.Value.PedLicence != null)
                {
                    if (isItTheLastManStanding(linq.Value.PedFrequency, linq.Value.PedId))
                        frequencyList.Remove(linq.Value.PedFrequency);

                    gpsListing.Remove(linq.Value.PedLicence);
                    Players[Convert.ToInt32(player.Handle)].TriggerEvent(config.playerNotification, "Vous venez de couper votre balise");
                    var linqb = gpsListing.Where(x => x.Value.PedFrequency == linq.Value.PedFrequency);
                    foreach (var result in linqb)
                    {
                        switch (reason)
                        {
                            case 1:
                                Players[Convert.ToInt32(result.Value.PedId)].TriggerEvent("QBCore:Notify", "L'utilisateur " + linq.Value.PedName + " quitte la balise");
                                break;

                            case 2:
                                Players[Convert.ToInt32(result.Value.PedId)].TriggerEvent("QBCore:Notify", "Warning : " + linq.Value.PedName + " balise retirée ! ");
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

       public bool isItTheLastManStanding(string frequency, string pedId)
        {
            var linq = gpsListing.Where(x => x.Value.PedFrequency == frequency && x.Value.PedId != pedId);

            if (linq.Any())
                return false;
            else
                return true;

        }
        #endregion

        #region IsProtectedFrequence
        public void playerJob(string job, string id)
        {
            Debug.WriteLine(job);
            var welcome6 = QbBridge.QbCore.FromJson(job);
            Debug.WriteLine(welcome6.PlayerData.Name);
        }

        public bool checkPlayerJob(Player player)
        {       
            for (int i = 0; i < 5; i++)
            {
                if (!awaiting)
                    return true;
                else
                    System.Threading.Thread.Sleep(100);
            }
            return false;
        }
        public bool isThisFrequencyProtected(int frequency, Player player)
        {

            if (frequency == config.s1)
            {
                checkPlayerJob(player);

                foreach (string f in config.m1)
                {
                    if ("tot" == f)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Task for GPS refreshing
        private Task GpsListeningClients(Dictionary<string, GpsDic> gpsClient, List<string> frequencyList)
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

                        Dictionary<string, GpsDic> GpsListingJson = new Dictionary<string, GpsDic>();

                        foreach (var result in linq)
                        {
                            GpsListingJson.Add(result.Value.PedId, dictionaryConstruct("0", result.Value.PedId, result.Value.PedName, result.Value.PedFrequency, result.Value.PedCoordinats, result.Value.PedColor, 1, result.Value.PedDirection));
                        }

                        var JsonToPush = JsonConvert.SerializeObject(GpsListingJson);

                        var linqJson = gpsClient.Where(x => x.Value.PedFrequency == frequency);
                        foreach (var result in linqJson)
                        {
                            Players[Convert.ToInt32(result.Value.PedId)].TriggerEvent("gpsPositionsFromServer", JsonToPush, config.pollingRate, config.blipSprite);
                        }
                    }
                }

                System.Threading.Thread.Sleep(config.pollingRate);
            }
        }
        #endregion

        #region Gps Core
        public GpsDic dictionaryConstruct(string licence, string id, string name, string frequency, Vector3 vector, string color, int gpsType, float direction)
        {
            var newGpsClient = new GpsDic
            {
                PedLicence = licence,
                PedId = id,
                PedName = name,
                PedFrequency = frequency,
                PedColor = color,
                PedDirection = direction,
                PedCoordinats = vector
            };
            return newGpsClient;
        }
        public void dictionaryUpdate(string licence, string id, string name, string frequency, Vector3 vector, string color, int gpsType)
        {
            var linq = gpsListing.Where(x => x.Value.PedLicence == licence).First();
            gpsListing[linq.Key].PedId = id;
            gpsListing[linq.Key].PedName = name;
            gpsListing[linq.Key].PedFrequency = frequency;
            gpsListing[linq.Key].PedColor = color;
            gpsListing[linq.Key].PedDirection = 0;
            gpsListing[linq.Key].PedCoordinats = vector;
        }

        public class GpsDic
        {
            public string PedLicence;
            public string PedId;
            public string PedName;
            public string PedFrequency;
            public string PedColor;
            public float PedDirection;
            public Vector3 PedCoordinats;
        }
        #endregion



    }
}
