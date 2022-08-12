using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Configuration;

namespace Server
{
    public class Server : BaseScript
    {
        public static PlayerList PlayerList { get; set; }
        public Config config { get; private set; }
        public Dictionary<string, GpsDic> gpsListing { get; set; }
        public List<string> frequencyList { get; set; }

        public bool awaiting { get; set; }
        public string jobQb { get; set; }
        public string securityKey { get; set; }
        public Server()
        {
            config = JsonConvert.DeserializeObject<Config>(LoadResourceFile(GetCurrentResourceName(), "config.json"));
            List<string> _frequencyList = new List<string>();
            Dictionary<string, GpsDic> _gpsListing = new Dictionary<string, GpsDic>();

            gpsListing = _gpsListing;
            frequencyList = _frequencyList;
            awaiting = false;

            #region EventHandlers
            EventHandlers["returnQbJobFromQbCore"] += new Action<string,string>(playerJob);
            EventHandlers["M9Pef449Slk40GDbdsrt304t4506gkKDR3230GDXsdfkjhsfd"] += new Action<Player>(sendingSecurityKey);
            EventHandlers["getSecurityBraceletCallFromClient"] += new Action<string, string>(getSecurityBraceletCallFromClient);
            EventHandlers["getSecurityBraceletNotificationForPlolice"] += new Action<string, string>(getSecurityBraceletNotificationForPlolice);
            EventHandlers["setNewGpsClient"] += new Action<Player, string, string,string>(setNewGpsClient);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
            EventHandlers["playerOff"] += new Action<Player,int>(userIsLeaving);

            #endregion 

            _ = Task.Run(() => { GpsListeningClients(gpsListing, frequencyList); });
        }

        #region IsConnecting / IsUpdatingSettings²
        private async void setNewGpsClient([FromSource] Player player, string frequency, string name, string color)
        {
            var id = player.Handle.ToString();
            var licence = player.Identifiers["license"];

            if (isThisFrequencyProtected(Convert.ToInt32(frequency),player))
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
                gpsListing.Add(licence, dictionaryConstruct(licence,id, name, frequency, GetEntityCoords(GetPlayerPed(id)),color, 1, 0)) ;
          else
               dictionaryUpdate(licence,id, name,  frequency, GetEntityCoords(GetPlayerPed(id)),color, 1);

            await Task.FromResult(0);
        }
       
        #endregion

        #region IsDisconnecting / Is Leaving
        private void OnPlayerDropped([FromSource] Player player, string reason)
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
        private void userIsLeaving([FromSource] Player player, int reason)
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

        private bool isItTheLastManStanding(string frequency, string pedId)
        {
            var linq = gpsListing.Where(x => x.Value.PedFrequency == frequency && x.Value.PedId != pedId);

            if (linq.Any())
                return false;
            else
                return true;
            
        }
        #endregion

        #region IsProtectedFrequence
        private void playerJob(string job, string id)
        {
            if (id == securityKey)
            {
                awaiting = false;
            }
        }

        private bool checkPlayerJob(Player player)
        {
            TriggerEvent("QBBridge:GetJob", player.Handle, securityKey);

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
                    if(jobQb == f)
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

                    Debug.WriteLine("Nombre de fréquences actives en boucle " + linqC);

                    if (linqC > 0)
                    {
                        var linq = gpsClient.Where(x => x.Value.PedFrequency == frequency);

                        Dictionary<string, GpsDic> GpsListingJson = new Dictionary<string, GpsDic>();

                        foreach (var result in linq)
                        {
                            GpsListingJson.Add(result.Value.PedId, dictionaryConstruct("0", result.Value.PedId, result.Value.PedName, result.Value.PedFrequency, result.Value.PedCoordinats, result.Value.PedColor,1,result.Value.PedDirection));
                        }

                        var JsonToPush = JsonConvert.SerializeObject(GpsListingJson);

                        var linqJson = gpsClient.Where(x => x.Value.PedFrequency == frequency);
                        foreach (var result in linqJson)
                        {
                        Debug.WriteLine(JsonToPush);
                            Players[Convert.ToInt32(result.Value.PedId)].TriggerEvent("gpsPositionsFromServer", JsonToPush,config.pollingRate,config.blipSprite);
                        }      
                    }
                }

                System.Threading.Thread.Sleep(config.pollingRate);
            }
        }
        #endregion

        #region Gps Core
        public GpsDic dictionaryConstruct(string licence, string id, string name, string frequency, Vector3 vector,string color, int gpsType, float direction)
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

        #region Security Bracelt
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
        #endregion

        #region EventTriggerProtection
        private string eventProtection()
        {
            var keyBase = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var keySize = new char[50];
            var random = new Random();

            for (int i = 0; i < keySize.Length; i++)
            {
                keySize[i] = keyBase[random.Next(keyBase.Length)];
            }

            var securityKey = new String(keySize);
            return securityKey;
        }

        public void kick([FromSource] Player source)
        {
            DropPlayer(source.Handle, "Bien le bonjour, tu tentes de trigger des events d'un script qui n'est pas fait en LUA, c'est con.");
        }

        public void sendingSecurityKey([FromSource] Player source)
        {
            Players[source.Character.NetworkId].TriggerEvent("cn90437589fh7avbn98c7w53987cvwcwe", securityKey);
        }
        #endregion
    }
}