using System;
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
    public class Server : BaseScript
    {
        public static PlayerList PlayerList { get; set; }
        public Config config { get; private set; }
        public Dictionary<string, PlayerNoSql> playersNoSql { get; set; }
        public string jobQb { get; set; }
        public string securityKey { get; set; }
        public Server()
        {
            config = JsonConvert.DeserializeObject<Config>(LoadResourceFile(GetCurrentResourceName(), "config.json"));
           
            Dictionary<string, PlayerNoSql> _playersNoSql = new Dictionary<string, PlayerNoSql>();

            playersNoSql = _playersNoSql;

            QbSql noSql = new QbSql(playersNoSql);
            Tracker tracker = new Tracker(config,noSql);

            #region EventHandlers
            EventHandlers["returnQbJobFromQbCore"] += new Action<string>(noSql.playerDataManagement);
            EventHandlers["M9Pef449Slk40GDbdsrt304t4506gkKDR3230GDXsdfkjhsfd"] += new Action<Player>(sendingSecurityKey);
            EventHandlers["getSecurityBraceletCallFromClient"] += new Action<string, string>(getSecurityBraceletCallFromClient);
            EventHandlers["getSecurityBraceletNotificationForPlolice"] += new Action<string, string>(getSecurityBraceletNotificationForPlolice);
            EventHandlers["setNewGpsClient"] += new Action<Player, string, string,string>(tracker.setNewGpsClient);
            EventHandlers["playerDropped"] += new Action<Player, string>(tracker.OnPlayerDropped);
            EventHandlers["playerOff"] += new Action<Player,int>(tracker.userIsLeaving);
            #endregion 

    
        }


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