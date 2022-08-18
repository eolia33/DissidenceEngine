using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using System.Drawing;
using Newtonsoft.Json.Linq;
using Configuration;
using Client;


namespace Client
{
    public class Tracker : BaseScript
    {
        public SharedConfig config { get; set; }
        public Player player { get; set; }
        public string color { get; set; }
        public NuiState nuiState { get; set; }


        public Tracker(SharedConfig _configuration, Bridge _bridge, Player _player, NuiState _nuiState)
        {
            config   = _configuration;
            player   = _player;
            nuiState = _nuiState;
            color    = "1";
        }

        public void trackerOpen()
        {
            string jsonString = "{\"type\":\"Open\",\"enable\":true}";
            nuiState.visible = true;
            nuiState.mouse   = true;
            SendNuiMessage(jsonString);
        }

        public void trackerClose()
        {
            string jsonString = "{\"type\":\"Close\",\"enable\":true}";
            nuiState.visible = false;
            nuiState.mouse   = false;
            SendNuiMessage(jsonString);
        }

        public void trackerLeave(int type)
        {
            Debug.WriteLine("client off command type " + type);
            string jsonString = "{\"type\":\"Off\",\"enable\":true}";
            if (type == 2)
            {
                nuiState.visible = false;
                nuiState.mouse = false;
            }
            else
            {
                nuiState.visible = true;
                nuiState.mouse = true;
            }
            SendNuiMessage(jsonString);

            if(type == 2)
                TriggerServerEvent("cs:engine:server:tracker:leave", "1");
        }


        public void trackerOff()
        {
            string jsonString = "{\"type\":\"Off\",\"enable\":true}";
            nuiState.visible = true;
            nuiState.mouse   = true;
            SendNuiMessage(jsonString);
        }

        public void trackerOk()
        {
            string jsonString = "{\"type\":\"On\",\"enable\":true}";
            nuiState.visible = true;
            nuiState.mouse   = true;
            SendNuiMessage(jsonString);
        }

        public void trackerSetColor(IDictionary<string, object> json)
        {
            var math = Convert.ToInt32(json["x"]) + 1;
            color = math.ToString();
        }

        public void trackerJoin(IDictionary<string, object> json)
        {
            if (string.IsNullOrEmpty(json["channel"].ToString()))
            {
                nuiNotify(config.msg_selfUserErrorFrequency, "");
                trackerOff();
                return;
            }

            if (string.IsNullOrEmpty(json["name"].ToString()))
            {
                trackerOff();
                nuiNotify(config.msg_selfUserNameFrequency, "");
                return;
            }

            if (string.IsNullOrEmpty(color))
                color = "1";

            TriggerServerEvent("cs:engine:server:tracker:on", json["channel"].ToString(), json["name"].ToString(),
                               color);
        }

        public void trackerServerPing(string json, int pollingRate, int blipSprite)
        {
            Dictionary<string, GpsNetworkClient> gps =
                JsonConvert.DeserializeObject<Dictionary<string, GpsNetworkClient>>(json);
            foreach (var v in gps)
            {
                if (v.Value.PedId != player.ServerId.ToString())
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

        public void MakeBlip(Vector3 location)
        {
            var blip = AddBlipForCoord(location[0], location[1], location[2]);
            SetBlipSprite(blip, 1);
            SetBlipColour(blip, 3);
        }

        public async void removeBlip(int blip, int time)
        {
            await Delay(time);
            RemoveBlip(ref blip);
        }


        public class GpsNetworkClient
        {
            public string PedId;
            public string PedName;
            public string PedColor;
            public float PedDirection;
            public Vector3 PedCoordinats;
        }

        public void nuiNotify(string[] msg, string replace = null)
        {
            TriggerEvent(config.notificationEngine, msg[0], msg[1], msg[2], msg[3]);
        }
    }
}