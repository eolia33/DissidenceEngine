using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace Client
{
    public class Client : BaseScript
    {
        Player player;
        public int lastBlip { get; set; }
        public Client()
        {
            int _lastBlip = 0;
            lastBlip = _lastBlip;
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["securityBraceletRespFromServ"] += new Action<string, string, Vector3>(securityBraceletRespFromServ);
            EventHandlers["gpsPositionsFromServer"] += new Action<string>(gpsPositionsFromServer);
            
        }

        private void OnClientResourceStart(string resourceName)
        {
            player = Game.Player;
            if (GetCurrentResourceName() != resourceName) return;

            RegisterCommand("securityBracelet", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var targetId = string.Join(" ", args.ToArray());
                TriggerServerEvent("getSecurityBraceletCallFromClient", player.ServerId, targetId);
            }), false);

            RegisterCommand("gpsOn", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var frequency = string.Join(" ", args.ToArray());
                TriggerServerEvent("setNewGpsClient", player.ServerId, player.Name, frequency);
            }), false);
        }
        private void securityBraceletRespFromServ(string copsId, string args, Vector3 vector)
        {
            TriggerServerEvent("getSecurityBraceletNotificationForPlolice", copsId, "[Security Corporation] -  Localisation : " + World.GetStreetName(vector));
            MakeBlip(vector); // 
        }

        private void MakeBlip(Vector3 location)
        {
            var blip = AddBlipForCoord(location[0], location[1], location[2]);
            SetBlipSprite(blip, 1);
            SetBlipColour(blip, 3);
        }
        private async void removeBlip(int blip,int time)
        {
            await Delay(time); // 🡠 change this to the amount of exparation
            RemoveBlip(ref blip);
        }

        private void gpsPositionsFromServer(string json)
        {
            Dictionary<string, GpsDic> gps = JsonConvert.DeserializeObject<Dictionary<string, GpsDic>>(json);

            foreach(var v in gps)
            {
                if (v.Value.PedId != player.ServerId.ToString())
                {
                    if (lastBlip != 0)
                    {
                        removeBlip(lastBlip, 0);
                    }

                    AddTextEntry("MYBLIP", "." + v.Value.PedName);
                    var blip = AddBlipForCoord(v.Value.PedCoordinats.X, v.Value.PedCoordinats.Y, v.Value.PedCoordinats.Z);
                    lastBlip = blip;
                    SetBlipSprite(blip, 11);
                    SetBlipColour(blip, 15);
                    SetBlipRotation(blip, Ceil(v.Value.PedDirection));
                    BeginTextCommandSetBlipName("." + v.Value.PedName);
                    EndTextCommandSetBlipName(blip);

                }

            }
        }

        public class GpsDic
        {
            public string PedId;
            public string PedName;
            public string PedFrequency;
            public int PedColor;
            public float PedDirection;
            public Vector3 PedCoordinats;
        }
    }
}