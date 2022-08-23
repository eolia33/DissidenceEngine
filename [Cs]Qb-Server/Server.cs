using System;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;


namespace Server
{
    public class Server : BaseScript
    {
        public SharedConfig Config { get; }
        public Server()
        {
            Config = JsonConvert.DeserializeObject<SharedConfig>(LoadResourceFile(GetCurrentResourceName(),
                                                                     "config.json"));
            var playerData = new BridgeQbCore(this);
            var tracker = new Tracker(Config, playerData, this);
            var bracelet   = new Bracelet();
            var fireShot   = new FireShot(playerData, this);
            var cmd        = new Cmd(this, tracker, playerData, fireShot, Config);

            #region EventHandlers

            EventHandlers["cs:server:shootingzone:new:policealert"] +=
            new Action<string>(fireShot.getActiveCops);

            EventHandlers["cs:engine:server:playerdata:update"] += 
                new Action<string,string>( playerData.getDataFromQbCore);

            EventHandlers["cs:engine:server:duty:tracker"] +=
                new Action<string,string>(tracker.dutySwitcher);

            EventHandlers["C#:Engine:Server:Bracelet:CheckPosition"] +=
                new Action<string, string>(bracelet.getSecurityBraceletCallFromClient);

            EventHandlers["C#:Engine:Server:Bracelet:PoliceNotification"] +=
                new Action<string, string>(bracelet.getSecurityBraceletNotificationForPlolice);
                EventHandlers["cs:engine:server:tracker:on"] +=
               new Action<Player, string, string, string, int>(tracker.setNewGpsClient);

           EventHandlers["cs:engine:server:tracker:leave"] +=
                new Action<Player,int>(tracker.userLeaving);

           EventHandlers["cs:engine:server:tracker:color:change"] +=
                new Action<Player, string>(tracker.userColorChange);

            EventHandlers["cs:engine:server:tracker:notification"] +=
                new Action<Player, int>(tracker.userNotification);

            EventHandlers["playerDropped"] +=
              new Action<Player,string>(tracker.userLeavingDrop);
            
            #endregion
        }
        
        public void C(string msg)
        {
            Debug.WriteLine("\x1b[36m" + msg);
        }

        public string buildKey(string license, string id)
        {
            return license + id;
        }
        
        
    }
}