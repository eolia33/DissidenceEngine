using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class Client : BaseScript
    {
        private Player       player;
        public  int          LastLip     { get; set; }
        public  NuiState     NuiState     { get; set; }
        public  string       template     { get; set; }
        public  Tracker      tracker      { get; set; }
        public  SharedConfig Config       { get; }
        public  FireShot     FireShot     { get; set; }
        public  Bridge       Bridge       { get; set; }


        public Client()
        {
            var config        = JsonConvert.DeserializeObject<SharedConfig>(LoadResourceFile(GetCurrentResourceName(), "Config.json"));
            Config            = config;
            var bridge        = new Bridge(Config);
            var nuiState      = new NuiState();
            NuiState          = nuiState;
            var tracker       = new Tracker(Config, bridge, Game.Player, NuiState);
            var fireShot      = new FireShot(Config);
            var lastBlip      = 0;
            LastLip           = lastBlip;
            FireShot          = fireShot;
            Bridge            = bridge;

            EventHandlers["onClientResourceStart"]                    +=
                new Action<string>(OnClientResourceStart);

            EventHandlers["securityBraceletRespFromServ"]             +=
                new Action<string, string, Vector3>(securityBraceletRespFromServ);

            EventHandlers["cs:engine:client:tracker:open"]            +=
                new Action(tracker.trackerOpen);

            EventHandlers["cs:engine:client:tracker:ping"]            +=
                new Action<string, int, int>(tracker.trackerServerPing);

            EventHandlers["cs:engine:client:tracker:connected"]       +=
                new Action(tracker.trackerOk);

            EventHandlers["cs:engine:client:tracker:off:forced"]      +=
                new Action<int>(tracker.trackerLeave);

           EventHandlers["cs:engine:client:playerdata:update"]       += 
              new Action<string>(bridge.decodingData);

            RegisterNuiCallbackType("cs:engine:client:tracker:close");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:close"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) => { tracker.trackerClose(); });

            RegisterNuiCallbackType("cs:engine:client:tracker:join");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:join"]  +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) => { tracker.trackerJoin(data); });

            RegisterNuiCallbackType("cs:engine:client:tracker:leave");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:leave"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) => { tracker.trackerLeave(1); });

            RegisterNuiCallbackType("cs:engine:client:tracker:color");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:color"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) =>
                {
                    tracker.trackerSetColor(data);
                });

            RegisterNuiCallbackType("cs:engine:client:tracker:coloronline");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:coloronline"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) =>
                {
                    TriggerServerEvent("cs:engine:server:tracker:color:change", data["x"]);
                });

            RegisterNuiCallbackType("cs:engine:client:tracker:notification");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:notification"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) =>
                {
                    TriggerServerEvent(
                        "cs:engine:server:tracker:notification",
                        data["x"]);
                });

            EventHandlers["cs:engine:client:fireshot:alert"] +=
                new Action<int,int,int,int,bool,string>(fireShot.fireShotServerResponse);

            Tick += OnTick;
        }

        [Tick]
        private async Task OnTick()
        {
           SetNuiFocus(NuiState.visible, NuiState.mouse);
           
           if (IsPedShooting(PlayerPedId()))
           {
               if (Bridge.Player.jobName == "police")
               {
                   if (Bridge.Player.jobOnDuty != "True")
                   {
                       await FireShot.checkZone(PlayerPedId());
                   }
               }
               else
               {
                   await FireShot.checkZone(PlayerPedId());
               }
           }
 
        }
        private void OnClientResourceStart(string resourceName)
        {
            player = Game.Player;
            if (GetCurrentResourceName() != resourceName) return;

            RegisterCommand("securityBracelet", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var targetId = string.Join(" ", args.ToArray());
                TriggerServerEvent("cs:engine:server:cmd:cdv" + template, player.ServerId, targetId);
            }), false);

            RegisterCommand("cUpdate", new Action<int, List<object>, string>((source, args, raw) =>
            {
                TriggerServerEvent("cs:engine:server:cmd:cdv", GetPlayerServerId(player.Handle));
            }), false);

            RegisterCommand("duty", new Action<string>((source) =>
            {
                TriggerServerEvent("QBCore:ToggleDuty", GetPlayerServerId(player.Handle));
            }), false);
            
            RegisterCommand("cDv", new Action<int, List<object>, string>((source, args, raw) =>
            {
                TriggerServerEvent("cs:engine:server:cmd:cdv", GetPlayerServerId(player.Handle));
            }), false);

            RegisterCommand("cTrackerTImer", new Action<string>((source) =>
            {
                TriggerServerEvent("cs:engine:server:cmd:ctracker:timer", GetPlayerServerId(player.Handle));
            }), false);

        }
        
        private void securityBraceletRespFromServ(string copsId, string args, Vector3 vector)
        {
            TriggerServerEvent("C#:Engine:Server:Bracelet:PoliceNotification" + template, copsId,
                               "[Security Corporation] -  Localisation : " + World.GetStreetName(vector)); 
        }
    }
}