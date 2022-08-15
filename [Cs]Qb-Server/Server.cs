using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Configuration;
using QbBridge;


namespace Server
{
    public class Server : BaseScript
    {
        public static PlayerList PlayerList { get; set; }
        public SharedConfig config { get; private set; }
        public Dictionary<string, Configuration.PlayerNoSql> bridgeQbCore { get; set; }
        public string jobQb { get; set; }
        public string securityKey { get; set; }
        public Server()
        {
            Debug.WriteLine("c# engine is Starting");
            config = JsonConvert.DeserializeObject<SharedConfig>(LoadResourceFile(GetCurrentResourceName(), "config.json"));

            Dictionary<string, Configuration.PlayerNoSql> _bridgeQbCore = new Dictionary<string, Configuration.PlayerNoSql>();

            bridgeQbCore = _bridgeQbCore;

            BridgeQBCore noSql = new BridgeQBCore(bridgeQbCore);
            Tracker tracker = new Tracker(config, noSql);
            Bracelet bracelet = new Bracelet();

            #region EventHandlers

            EventHandlers["C#:Engine:Server:Bridge:GetData"] += new Action<string,string>(noSql.playerDataManagement);
            EventHandlers["C#:Engine:Server:Bracelet:CheckPosition"] += new Action<string, string>(bracelet.getSecurityBraceletCallFromClient);
            EventHandlers["C#:Engine:Server:Bracelet:PoliceNotification"] += new Action<string, string>(bracelet.getSecurityBraceletNotificationForPlolice);
            EventHandlers["cs:engine:server:tracker:on"] += new Action<Player, string, string, string>(tracker.setNewGpsClient);
            EventHandlers["C#:Engine:Server:Tracker:Off"] += new Action<Player, int>(tracker.userIsLeaving);

            EventHandlers["playerDropped"] += new Action<Player, string>(tracker.OnPlayerDropped);
            EventHandlers["M9Pef449Slk40GDbdsrt304t4506gkKDR3230GDXsdfkjhsfd"] += new Action<Player>(sendingSecurityKey);
            #endregion 

            Debug.WriteLine("c# engine is Running");

        }

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
            DropPlayer(source.Handle, "A Bientôt !");
        }

        public void sendingSecurityKey([FromSource] Player source)
        {
            Players[source.Character.NetworkId].TriggerEvent("cn90437589fh7avbn98c7w53987cvwcwe", securityKey);
        }
        #endregion
    }

}