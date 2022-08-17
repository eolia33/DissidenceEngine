using System;
using System.Collections.Generic;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;


namespace Server
{
    public class Server : BaseScript
    {
        public SharedConfig config { get; }
        public Dictionary<string, PlayerNoSql> bridgeQbCore { get; set; }
        public string securityKey { get; set; }

        public Server()
        {
            config = JsonConvert.DeserializeObject<SharedConfig>(LoadResourceFile(GetCurrentResourceName(),
                                                                     "config.json"));

            var _bridgeQbCore = new Dictionary<string, PlayerNoSql>();

            bridgeQbCore = _bridgeQbCore;

            var noSql    = new BridgeQBCore(bridgeQbCore);
            var tracker  = new Tracker(config, noSql);
            var bracelet = new Bracelet();

            #region EventHandlers

            EventHandlers["cs:engine:server:playerdata:update"] +=
                new Action<string>(noSql.getDataFromQbCore);

            EventHandlers["C#:Engine:Server:Bracelet:CheckPosition"] +=
                new Action<string, string>(bracelet.getSecurityBraceletCallFromClient);

            EventHandlers["C#:Engine:Server:Bracelet:PoliceNotification"] +=
                new Action<string, string>(bracelet.getSecurityBraceletNotificationForPlolice);

            EventHandlers["cs:engine:server:tracker:on"] +=
                new Action<Player, string, string, string, int>(tracker.setNewGpsClient);

            EventHandlers["cs:engine:server:tracker:leave"] +=
                new Action<Player, int>(tracker.userIsLeaving);

            EventHandlers["cs:engine:server:tracker:color:change"] +=
                new Action<Player, string>(tracker.userColorChange);

            EventHandlers["cs:engine:server:tracker:notification"] +=
                new Action<Player, int>(tracker.userNotification);

            EventHandlers["playerDropped"] +=
                new Action<Player, string>(tracker.OnPlayerDropped);

            EventHandlers["M9Pef449Slk40GDbdsrt304t4506gkKDR3230GDXsdfkjhsfd"] +=
                new Action<Player>(sendingSecurityKey);

            #endregion
        }


        #region EventTriggerProtection

        private string eventProtection()
        {
            var keyBase = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var keySize = new char[50];
            var random  = new Random();

            for (var i = 0; i < keySize.Length; i++) keySize[i] = keyBase[random.Next(keyBase.Length)];

            var securityKey = new string(keySize);
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