using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using System.Drawing;

namespace Client
{
    public class Client : BaseScript
    {
        Player player;
        public int lastBlip { get; set; }
        public string template { get; set; }

        public Point[] points { get; set; }

         public Client()
        {
            int _lastBlip = 0;
            lastBlip = _lastBlip;
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["securityBraceletRespFromServ"] += new Action<string, string, Vector3>(securityBraceletRespFromServ);
            EventHandlers["gpsPositionsFromServer"] += new Action<string, int, int>(gpsPositionsFromServer);
            EventHandlers["cn90437589fh7avbn98c7w53987cvwcwe"] += new Action<string>(loadFromJsonTemplate);
            Tick += OnTick;
        }

        [Tick]
         
        private async Task OnTick()
        {
            player = Game.Player;

            if (IsPedShooting(PlayerPedId()))
                {
                    Vector3 playerCoords = GetEntityCoords(PlayerPedId(), false);
                    Point point = new Point(Convert.ToInt32(playerCoords.X), Convert.ToInt32(playerCoords.Y));
                    Point[] points = new Point[] { new Point { X = -1368, Y = -1944 }, new Point { X = -2514, Y = -392 }, new Point { X = -2356, Y = 621 }, new Point { X = -1814, Y = 744 }, new Point { X = -1780, Y = 492 }, new Point { X = -1026, Y = 905 }, new Point { X = -553, Y = 883 }, new Point { X = -444, Y = 1268 }, new Point { X = 277, Y = 1277 }, new Point { X = 592, Y = 668 }, new Point { X = 1331, Y = 268 }, new Point { X = 1080, Y = -153 }, new Point { X = 1489, Y = -565 }, new Point { X = 1511, Y = -850 }, new Point { X = 1065, Y = -862 }, new Point { X = 541, Y = -583 }, new Point { X = 526, Y = -1305 }, new Point { X = 638, Y = -1692 }, new Point { X = 592, Y = -2417 }, new Point { X = 514, Y = -2417 }, new Point { X = 408, Y = -2257 }, new Point { X = -405, Y = -2295 }, new Point { X = -789, Y = -3059 }, new Point { X = -617, Y = -3217 }, new Point { X = -865, Y = -3780 }, new Point { X = -2235, Y = -3205 }, new Point { X = -2114, Y = -2714 } };
                    var result = Math.IsInZone(points, point);
                    await BaseScript.Delay(15000);
                 }
        }
        private void OnGameEventTriggered()
        {
            Debug.WriteLine($"game event");
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
                TriggerServerEvent("getSecurityBraceletCallFromClient" + template, player.ServerId, targetId);
            }), false);

            RegisterCommand("gpsOn", new Action<int, List<object>, string>((source, args, raw) =>
            {
                string c = string.Join(" ", args.ToArray());
                string[] ts = c.Split(',');
                TriggerServerEvent("setNewGpsClient" + template, ts[0], ts[1], ts[2]);
            }), false);


            RegisterCommand("gpsOff", new Action<int, List<object>, string>((source, args, raw) =>
            {
                TriggerServerEvent("playerOff" + template, 1);
            }), false);

            TriggerServerEvent("M9Pef449Slk40GDbdsrt304t4506gkKDR3230GDXsdfkjhsfd" + template);
        }

        private void getZone()
        {
            Vector3 playerCoords = GetEntityCoords(PlayerPedId(), false);
            Point point = new Point(Convert.ToInt32(playerCoords.X), Convert.ToInt32(playerCoords.Y));
            Point[] points = new Point[] { new Point { X = -1368, Y = -1944 }, new Point { X = -2514, Y = -392 }, new Point { X = -2356, Y = 621 }, new Point { X = -1814, Y = 744 }, new Point { X = -1780, Y = 492 }, new Point { X = -1026, Y = 905 }, new Point { X = -553, Y = 883 }, new Point { X = -444, Y = 1268 }, new Point { X = 277, Y = 1277 }, new Point { X = 592, Y = 668 }, new Point { X = 1331, Y = 268 }, new Point { X = 1080, Y = -153 }, new Point { X = 1489, Y = -565 }, new Point { X = 1511, Y = -850 }, new Point { X = 1065, Y = -862 }, new Point { X = 541, Y = -583 }, new Point { X = 526, Y = -1305 }, new Point { X = 638, Y = -1692 }, new Point { X = 592, Y = -2417 }, new Point { X = 514, Y = -2417 }, new Point { X = 408, Y = -2257 }, new Point { X = -405, Y = -2295 }, new Point { X = -789, Y = -3059 }, new Point { X = -617, Y = -3217 }, new Point { X = -865, Y = -3780 }, new Point { X = -2235, Y = -3205 }, new Point { X = -2114, Y = -2714 } };
            var result = Math.IsInZone(points, point);
            Debug.WriteLine("Coord : " + playerCoords.ToString());
            Debug.WriteLine("Le resultat est : " + result);
        }
        private void securityBraceletRespFromServ(string copsId, string args, Vector3 vector)
        {
            TriggerServerEvent("getSecurityBraceletNotificationForPlolice" + template, copsId, "[Security Corporation] -  Localisation : " + World.GetStreetName(vector));
            MakeBlip(vector); // 
        }

        private void MakeBlip(Vector3 location)
        {
            var blip = AddBlipForCoord(location[0], location[1], location[2]);
            SetBlipSprite(blip, 1);
            SetBlipColour(blip, 3);
        }
        private async void removeBlip(int blip, int time)
        {
            await Delay(time); //
            RemoveBlip(ref blip);
        }

        private void gpsPositionsFromServer(string json, int pollingRate, int blipSprite)
        {
            Dictionary<string, GpsDic> gps = JsonConvert.DeserializeObject<Dictionary<string, GpsDic>>(json);
            foreach (var v in gps)
            {
                if (v.Value.PedId != player.ServerId.ToString())
                {
                    var blip = AddBlipForCoord(v.Value.PedCoordinats.X, v.Value.PedCoordinats.Y, v.Value.PedCoordinats.Z);
                    SetBlipSprite(blip, blipSprite);
                    SetBlipColour(blip, Convert.ToInt32(v.Value.PedColor));
                    SetBlipScale(blip, 0.7f);
                    SetBlipRotation(blip, Ceil(v.Value.PedDirection));
                    SetBlipAsShortRange(blip, true);
                    BeginTextCommandSetBlipName("STRING");
                    AddTextComponentString("." + v.Value.PedName);
                    EndTextCommandSetBlipName(blip);
                    removeBlip(blip, pollingRate);
                }
            }

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
      
    }
}