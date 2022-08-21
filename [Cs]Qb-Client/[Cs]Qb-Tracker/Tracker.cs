using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using Configuration;



namespace Client
{
    public class Tracker : BaseScript
    {
        public SharedConfig Config { get; set; }
        public Player Player { get; set; }
        public string Color { get; set; }
        public NuiState NuiState { get; set; }


        public Tracker(SharedConfig configuration, Bridge bridge, Player player, NuiState nuiState)
        {
            Config   = configuration;
            Player   = player;
            NuiState = nuiState;
            Color    = "1";
        }

        public async void trackerOpen()
        {
            string jsonString = "{\"type\":\"Open\",\"enable\":true}";
            NuiState.visible = true;
            NuiState.mouse   = true;
            SendNuiMessage(jsonString);
        }

        public async void trackerClose()
        {
            string jsonString = "{\"type\":\"Close\",\"enable\":true}";
            NuiState.visible = false;
            NuiState.mouse   = false;
            SendNuiMessage(jsonString);
        }

        public async void trackerLeave(int type)
        {
            Debug.WriteLine("client off command type " + type);
            string jsonString = "{\"type\":\"Off\",\"enable\":true}";
            if (type == 2)
            {
                NuiState.visible = false;
                NuiState.mouse = false;
            }
            else
            {
                NuiState.visible = true;
                NuiState.mouse = true;
            }
            SendNuiMessage(jsonString);

            if(type == 1)
                TriggerServerEvent("cs:engine:server:tracker:leave", "1");

            if (type == 2)
                TriggerServerEvent("cs:engine:server:tracker:leave:", "1");
        }


        public async void trackerOff()
        {
            string jsonString = "{\"type\":\"Off\",\"enable\":true}";
            NuiState.visible = true;
            NuiState.mouse   = true;
            SendNuiMessage(jsonString);
        }

        public async void trackerOk()
        {
            string jsonString = "{\"type\":\"On\",\"enable\":true}";
            NuiState.visible = true;
            NuiState.mouse   = true;
            SendNuiMessage(jsonString);
        }

        public async void trackerSetColor(IDictionary<string, object> json)
        {
            var math = Convert.ToInt32(json["x"]) + 1;
            Color = math.ToString();
        }

        public async void trackerJoin(IDictionary<string, object> json)
        {
            if (string.IsNullOrEmpty(json["channel"].ToString()))
            {
                nuiNotify(Config.msg_selfUserErrorFrequency, "");
                trackerOff();
                return;
            }

            if (string.IsNullOrEmpty(json["name"].ToString()))
            {
                trackerOff();
                nuiNotify(Config.msg_selfUserNameFrequency, "");
                return;
            }

            if (string.IsNullOrEmpty(Color))
                Color = "1";

            TriggerServerEvent("cs:engine:server:tracker:on", json["channel"].ToString(), json["name"].ToString(),
                               Color);
        }

        public async void trackerServerPing(string json, int pollingRate, int blipSprite)
        {
            Dictionary<string, TrackerJsonNetwork> trackerClients =
                JsonConvert.DeserializeObject<Dictionary<string, TrackerJsonNetwork>>(json);
            foreach (var v in trackerClients)
            {
                if (v.Value.PedId != Player.ServerId.ToString())
                {
                    var blip = AddBlipForCoord(v.Value.PedCoordinats.X, v.Value.PedCoordinats.Y,
                                                  v.Value.PedCoordinats.Z);
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

        public async void MakeBlip(Vector3 location)
        {
            var blip = AddBlipForCoord(location[0], location[1], location[2]);
            SetBlipSprite(blip, 1);
            SetBlipColour(blip, 3);
        }

        private async Task removeBlip(int blip, int time)
        {
            await Delay(time);
            RemoveBlip(ref blip);
        }

        public class TrackerJsonNetwork
        {
            public string PedId;
            public string PedName;
            public string PedColor;
            public float PedDirection;
            public Vector3 PedCoordinats;
        }

        public async void nuiNotify(string[] msg, string replace = null)
        {
            TriggerEvent(Config.notificationEngine, msg[0], msg[1], msg[2], msg[3]);
        }
    }
}