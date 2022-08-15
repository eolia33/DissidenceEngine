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


namespace Client
{
    public class Tracker : BaseScript
    {

        public SharedConfig configuration { get; set; }
        public Player player { get; set; }

        public NuiState nuiState { get; set; }



        public Tracker(SharedConfig _configuration, Bridge _bridge, Player _player, NuiState _nuiState)
        {
            configuration = _configuration;
            player = _player;
            nuiState = _nuiState;
        }

        public void trackerOpen()
        {
            string jsonString = "{\"type\":\"Open\",\"enable\":true}";
            nuiState.visible = true;
            nuiState.mouse = true;
            API.SendNuiMessage(jsonString);
        }

        public void trackerOff()
        {
            string jsonString = "{\"type\":\"Off\",\"enable\":true}";
            nuiState.visible = true;
            nuiState.mouse = true;
            Debug.WriteLine("bloque");
            API.SendNuiMessage(jsonString);
        }
        public void trackerClose()
        {
            Debug.WriteLine("clse");
            string jsonString = "{\"type\":\"Close\",\"enable\":true}";
            nuiState.visible = false;
            nuiState.mouse = false;
            API.SendNuiMessage(jsonString);
        }
        public void trackerJoin(IDictionary<string, object> json)
        {
            if (json["channel"] == "")
            {
                TriggerEvent("QBCore:Notify", "Le channel de la balise est vide");
                trackerOff();
                return;
            }

            if (json["name"] == "")
            {
                trackerOff();
                TriggerEvent("QBCore:Notify", "Le nom doit être renseigné");
                return;
            }

            var color = "";

            TriggerServerEvent("cs:engine:server:tracker:on", json["channel"].ToString(), json["name"].ToString(), color);
        }


        private void gpsPositionsFromServer(string json, int pollingRate, int blipSprite)
        {
             Dictionary<string, GpsNetworkClient> gps = JsonConvert.DeserializeObject<Dictionary<string, GpsNetworkClient>>(json);
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
        public void MakeBlip(Vector3 location)
        {
            var blip = AddBlipForCoord(location[0], location[1], location[2]);
            SetBlipSprite(blip, 1);
            SetBlipColour(blip, 3);
        }
        public async void removeBlip(int blip, int time)
        {
            await Delay(time); //
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

    }
}
