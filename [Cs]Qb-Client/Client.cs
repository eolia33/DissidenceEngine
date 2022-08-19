using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class Client : BaseScript
    {
        private Player player;
        public int lastBlip { get; set; }
        public NuiState nuiState { get; set; }
        public string template { get; set; }
        public Point[] points { get; set; }
        public bool isDisplaying { get; set; }
        public bool isMouse { get; set; }
        public Tracker tracker { get; set; }
        public SharedConfig config { get; }

        public ShootingZone shootingZone { get; set; }


        public Client()
        {
            var _config       = JsonConvert.DeserializeObject<SharedConfig>(LoadResourceFile(GetCurrentResourceName(), "config.json"));
            config            = _config;
            var bridge        = new Bridge(config);
            var _nuiState     = new NuiState();
            var tracker       = new Tracker(config, bridge, Game.Player, _nuiState);
            var _shootingZone = new ShootingZone(config);
            var _lastBlip     = 0;
            nuiState          = _nuiState;
            config            = _config;
            lastBlip          = _lastBlip;
            shootingZone      = _shootingZone;

            EventHandlers["onClientResourceStart"]                    +=
                new Action<string>(OnClientResourceStart);

            EventHandlers["securityBraceletRespFromServ"]             +=
                new Action<string, string, Vector3>(securityBraceletRespFromServ);

            EventHandlers["cn90437589fh7avbn98c7w53987cvwcwe"]        +=
                new Action<string>(loadFromJsonTemplate);

            EventHandlers["cs:engine:client:tracker:open"]            +=
                new Action(tracker.trackerOpen);

            EventHandlers["cs:engine:client:tracker:ping"]            +=
                new Action<string, int, int>(tracker.trackerServerPing);

            EventHandlers["cs:engine:client:tracker:connected"]       +=
                new Action(tracker.trackerOk);

            EventHandlers["cs:engine:client:tracker:off:forced"]      +=
                new Action<int>(tracker.trackerLeave);

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
                    Debug.WriteLine("color chanbge");
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

            Tick += OnTick;
        }

        [Tick]
        private async Task OnTick()
        {
            player = Game.Player;
            SetNuiFocus(nuiState.visible, nuiState.mouse);

            if (IsPedShooting(PlayerPedId()))
                shootingZone.checkZone(PlayerPedId());
        }


        private void loadFromJsonTemplate(string response)
        {
            template = response;
        }

        private void OnClientResourceStart(string resourceName)
        {
            player = Game.Player;
            if (GetCurrentResourceName() != resourceName) return;

            RegisterCommand("securityBracelet", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var targetId = string.Join(" ", args.ToArray());
                TriggerServerEvent("C#:Engine:Server:Bracelet:CheckPosition" + template, player.ServerId, targetId);
            }), false);

            RegisterCommand("dataupdate", new Action<int, List<object>, string>((source, args, raw) =>
            {
                TriggerServerEvent("cs:engine:client:qbcore:getdata", GetPlayerServerId(player.Handle));
            }), false);

            RegisterCommand("duty", new Action<string>((source) =>
            {
                TriggerServerEvent("QBCore:ToggleDuty", GetPlayerServerId(player.Handle));
            }), false);


        }
        
        private void securityBraceletRespFromServ(string copsId, string args, Vector3 vector)
        {
            TriggerServerEvent("C#:Engine:Server:Bracelet:PoliceNotification" + template, copsId,
                               "[Security Corporation] -  Localisation : " + World.GetStreetName(vector));
            tracker.MakeBlip(vector);
        }
    }
}