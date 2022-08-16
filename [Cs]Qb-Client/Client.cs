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

        public Client()
        {
            var _config = JsonConvert.DeserializeObject<SharedConfig>(LoadResourceFile(GetCurrentResourceName(), "config.json"));
            config = _config;
            var bridge = new Bridge(config);
            var _nuiState = new NuiState();
            var tracker = new Tracker(config, bridge, Game.Player, _nuiState);
            nuiState = _nuiState;
            var _lastBlip = 0;
            lastBlip = _lastBlip;

            EventHandlers["onClientResourceStart"] +=
                new Action<string>(OnClientResourceStart);

            EventHandlers["securityBraceletRespFromServ"] +=
                new Action<string, string, Vector3>(securityBraceletRespFromServ);
            
            EventHandlers["cn90437589fh7avbn98c7w53987cvwcwe"] += 
                new Action<string>(loadFromJsonTemplate);
            
            EventHandlers["cs:engine:client:tracker:open"] += 
                new Action(tracker.trackerOpen);

            EventHandlers["cs:engine:client:tracker:connected"] +=
                new Action(tracker.trackerOk);

            RegisterNuiCallbackType("cs:engine:client:tracker:close");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:close"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) => { tracker.trackerClose(); });

            RegisterNuiCallbackType("cs:engine:client:tracker:join");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:join"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) => { tracker.trackerJoin(data); });

            RegisterNuiCallbackType("cs:engine:client:tracker:leave");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:leave"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) => { tracker.trackerLeave(); });

            RegisterNuiCallbackType("cs:engine:client:tracker:color");
            EventHandlers["__cfx_nui:cs:engine:client:tracker:color"] +=
                new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) =>
                {
                    tracker.trackerSetColor(data);
                });

            RegisterCommand("test",
                new Action<int, List<object>, string>((source, args, raw) =>
                {
                    TriggerServerEvent("cs:engine:client:qbcore:checkplayerdata");
                }), false);

            Tick += OnTick;
        }


        [Tick]
        private async Task OnTick()
        {
            player = Game.Player;
            SetNuiFocus(nuiState.visible, nuiState.mouse);

            if (IsPedShooting(PlayerPedId()))
            {
                var playerCoords = GetEntityCoords(PlayerPedId(), false);
                var point = new Point(Convert.ToInt32(playerCoords.X), Convert.ToInt32(playerCoords.Y));
                Point[] points =
                {
                    new Point {X = -1368, Y = -1944}, new Point {X = -2514, Y = -392}, new Point {X = -2356, Y = 621},
                    new Point {X = -1814, Y = 744}, new Point {X = -1780, Y = 492}, new Point {X = -1026, Y = 905},
                    new Point {X = -553, Y = 883}, new Point {X = -444, Y = 1268}, new Point {X = 277, Y = 1277},
                    new Point {X = 592, Y = 668}, new Point {X = 1331, Y = 268}, new Point {X = 1080, Y = -153},
                    new Point {X = 1489, Y = -565}, new Point {X = 1511, Y = -850}, new Point {X = 1065, Y = -862},
                    new Point {X = 541, Y = -583}, new Point {X = 526, Y = -1305}, new Point {X = 638, Y = -1692},
                    new Point {X = 592, Y = -2417}, new Point {X = 514, Y = -2417}, new Point {X = 408, Y = -2257},
                    new Point {X = -405, Y = -2295}, new Point {X = -789, Y = -3059}, new Point {X = -617, Y = -3217},
                    new Point {X = -865, Y = -3780}, new Point {X = -2235, Y = -3205}, new Point {X = -2114, Y = -2714}
                };
                var result = Math.IsInZone(points, point);
                Debug.WriteLine("Est ce que le shoot est dans la zone définie ? " + result);

                if (result)
                    Debug.WriteLine("Probabilité de 80%, résultat du tirage : " + Math.IsASuccess(new Random(), 0.8));

                await Delay(15000);
            }
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


            //TriggerServerEvent("M9Pef449Slk40GDbdsrt304t4506gkKDR3230GDXsdfkjhsfd" + template);
        }

        private void getZone()
        {
            var playerCoords = GetEntityCoords(PlayerPedId(), false);
            var point = new Point(Convert.ToInt32(playerCoords.X), Convert.ToInt32(playerCoords.Y));
            Point[] points =
            {
                new Point {X = -1368, Y = -1944}, new Point {X = -2514, Y = -392}, new Point {X = -2356, Y = 621},
                new Point {X = -1814, Y = 744}, new Point {X = -1780, Y = 492}, new Point {X = -1026, Y = 905},
                new Point {X = -553, Y = 883}, new Point {X = -444, Y = 1268}, new Point {X = 277, Y = 1277},
                new Point {X = 592, Y = 668}, new Point {X = 1331, Y = 268}, new Point {X = 1080, Y = -153},
                new Point {X = 1489, Y = -565}, new Point {X = 1511, Y = -850}, new Point {X = 1065, Y = -862},
                new Point {X = 541, Y = -583}, new Point {X = 526, Y = -1305}, new Point {X = 638, Y = -1692},
                new Point {X = 592, Y = -2417}, new Point {X = 514, Y = -2417}, new Point {X = 408, Y = -2257},
                new Point {X = -405, Y = -2295}, new Point {X = -789, Y = -3059}, new Point {X = -617, Y = -3217},
                new Point {X = -865, Y = -3780}, new Point {X = -2235, Y = -3205}, new Point {X = -2114, Y = -2714}
            };
            var result = Math.IsInZone(points, point);
        }

        private void securityBraceletRespFromServ(string copsId, string args, Vector3 vector)
        {
            TriggerServerEvent("C#:Engine:Server:Bracelet:PoliceNotification" + template, copsId,
                "[Security Corporation] -  Localisation : " + World.GetStreetName(vector));
            tracker.MakeBlip(vector);
        }


    }
}