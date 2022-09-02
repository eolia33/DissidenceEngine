using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using SaltyTalkieClient;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class Client : BaseScript
    {
        private Player player;
        public int LastLip { get; set; }
        public NuiState NuiState { get; set; }
        public string template { get; set; }
        public Tracker tracker { get; set; }
        public SharedConfig Config { get; }
        public FireShot FireShot { get; set; }
        public Bridge Bridge { get; set; }
        public Billing Billing { get; set; }
        public Radio Talkie { get; set; }

        public int TickLspd { get; set; }

        public bool Trig { get; set; }


        public Client()
        {
            var config =
                JsonConvert.DeserializeObject<SharedConfig>(LoadResourceFile(GetCurrentResourceName(), "Config.json"));
            Config = config;
            var bridge   = new Bridge(Config);
            var nuiState = new NuiState();
            NuiState = nuiState;
            var tracker  = new Tracker(Config, bridge, Game.Player, NuiState);
            var fireShot = new FireShot(Config);
            var billing  = new Billing(Config, bridge, Game.Player, NuiState);
            var Talkie   = new Radio(Config, bridge, Game.Player, NuiState);
            var lastBlip = 0;
            var TickLspd = 0;
            LastLip  = lastBlip;
            FireShot = fireShot;
            Bridge   = bridge;
            Billing  = billing;


            var Trig = false;


            EventHandlers["onClientResourceStart"] +=
                new Action<string>(OnClientResourceStart);

            EventHandlers["securityBraceletRespFromServ"] +=
                new Action<string, string, Vector3>(securityBraceletRespFromServ);

            EventHandlers["cs:engine:client:tracker:open"] +=
                new Action(tracker.trackerOpen);

            EventHandlers["cs:engine:client:tracker:ping"] +=
                new Action<string, int, int>(tracker.trackerServerPing);

            EventHandlers["cs:engine:client:tracker:connected"] +=
                new Action(tracker.trackerOk);

            EventHandlers["cs:engine:client:tracker:off:forced"] +=
                new Action<int>(tracker.trackerLeave);

            EventHandlers["cs:engine:client:playerdata:update"] +=
                new Action<string>(bridge.decodingData);

            EventHandlers["cs:engine:client:billing:open"] +=
                new Action<string>(billing.open);

            EventHandlers["SaltyChat_RadioChannelChanged"] +=
                new Action<string, bool>(Talkie.OnPrimaryRadioChannelChanged);

            EventHandlers["SaltyChat_RadioTrafficStateChanged"] +=
                new Action<bool, bool, bool, bool>(Talkie.OnRadioTrafficStateChanged);

            RegisterNuiCallbackType("cs:engine:client:tracker:close");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:close"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) => { tracker.trackerClose(); });

            RegisterNuiCallbackType("cs:engine:client:tracker:join");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:join"] +=
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
                    TriggerServerEvent(
                        "cs:engine:server:tracker:color:change",
                        data["x"]);
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
                new Action<int, int, int, int, bool, string>(fireShot.fireShotServerResponse);


            RegisterNuiCallbackType("ready");
            EventHandlers["__cfx_nui:ready"] +=
                new Action<dynamic, dynamic>(Talkie.OnNuiReady);

            RegisterNuiCallbackType("setPrimaryChannel");
            EventHandlers["__cfx_nui:setPrimaryChannel"] +=
                new Action<dynamic, dynamic>(Talkie.OnNuiSetPrimaryChannel);

            RegisterNuiCallbackType("setSecondaryChannel");
            EventHandlers["__cfx_nui:setSecondaryChannel"] +=
                new Action<dynamic, dynamic>(Talkie.OnNuiSetSecondaryChannel);


            RegisterNuiCallbackType("radioVolumeUp");
            EventHandlers["__cfx_nui:radioVolumeUp"] +=
                new Action<dynamic, dynamic>(Talkie.OnNuiVolumeUp);

            RegisterNuiCallbackType("radioVolumeDown");
            EventHandlers["__cfx_nui:radioVolumeDown"] +=
                new Action<dynamic, dynamic>(Talkie.OnNuiVolumeDown);
            EventHandlers["__cfx_nui:togglePower"] +=
                new Action<dynamic, dynamic>(Talkie.OnNuiTogglePower);


            EventHandlers["__cfx_nui:toggleMicClick"] += new Action<dynamic, dynamic>(Talkie.OnNuiToggleMicClick);
            EventHandlers["__cfx_nui:toggleSpeaker"]  += new Action<dynamic, dynamic>(Talkie.OnNuiToggleSpeaker);
            EventHandlers["__cfx_nui:unfocus"]        += new Action<dynamic, dynamic>(Talkie.OnNuiUnfocus);
            Tick                                      += OnTick;
        }

        [Tick]
        private async Task OnTick()
        {
            if (IsPedShooting(PlayerPedId()))
            {
                if (Bridge.Player.jobName == "police")
                {
                    if (Bridge.Player.jobOnDuty != "True") await FireShot.checkZone(PlayerPedId());
                }
                else
                {
                    await FireShot.checkZone(PlayerPedId());
                }
            }
        }

        public async void goAway(Player player)
        {
            var timer = 30;
            Debug.WriteLine("dans la boucel");
            TriggerEvent(Config.notificationEngine, "Mass RP",
                         "Vous entrez en service dans une zone sous protection de la Cabale, vous devez la quitter immédiatement.",
                         5000, "warning");

            for (var i = 1; i < 15; i++)
            {
                await Delay(2000);
                timer = timer - 2;
                TriggerEvent(Config.notificationEngine,                                "Mass RP",
                             "Vous devez quitter la zone sous " + timer + " secondes", 2000, "warning");

                if (!FireShot.parking(PlayerPedId()))
                {
                    Trig = false;
                    break;
                }
            }

            var vehicule = GetVehiclePedIsIn(PlayerId(), false);
            //StartPlayerTeleport(PlayerId(), 2399, 3081, 49, 0, false, false, true);
            //SetEntityCoords(vehicule, 2389, 3095, 48, true, true, true, true);
            //TriggerEvent(Config.notificationEngine, "Mass RP", "Vous vous reveillez dans un lieu inconnu, vous avez perdu la mémoire des 15 dernières minutes. Il semblerait opportun de ne pas retenter en service votre action. Les conséquences pourraient être plus grave à l'avenir.", 5000, "warning");   
            Trig = false;
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

            RegisterCommand(
                "cUpdate",
                new Action<int, List<object>, string>((source, args, raw) =>
                {
                    TriggerServerEvent(
                        "cs:engine:server:cmd:cdv",
                        GetPlayerServerId(player.Handle));
                }), false);

            RegisterCommand(
                "duty",
                new Action<string>(source =>
                {
                    TriggerServerEvent("QBCore:ToggleDuty", GetPlayerServerId(player.Handle));
                }), false);

            RegisterCommand(
                "cDv",
                new Action<int, List<object>, string>((source, args, raw) =>
                {
                    TriggerServerEvent(
                        "qbcore:cs:internal:billing:add:bank", 1, "bank", 400);
                }), false);

            RegisterCommand("cTrackerTImer",
                            new Action<string>(source =>
                            {
                                TriggerServerEvent("cs:engine:server:cmd:ctracker:timer",
                                                   GetPlayerServerId(player.Handle));
                            }), false);
        }

        private void securityBraceletRespFromServ(string copsId, string args, Vector3 vector)
        {
            TriggerServerEvent("C#:Engine:Server:Bracelet:PoliceNotification" + template, copsId,
                               "[Security Corporation] -  Localisation : " + World.GetStreetName(vector));
        }
    }
}