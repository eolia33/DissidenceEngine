using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading;

namespace Client
    {
        public class Billing : BaseScript
        {
            public SharedConfig Config   { get; set; }
            public Player       Player   { get; set; }
            public NuiState     NuiState { get; set; }
            public Bridge       Bridge   { get; set; }
            
            public bool Closest { get; set; }

            public Billing(SharedConfig configu, Bridge bridge, Player player, NuiState nuiState)
            {
                Config   = configu;
                Bridge   = bridge;
                NuiState = nuiState;
                Player   = player;
                Closest  = false;
                
                RegisterCommand("billing", new Action<string>((source) =>
                {
                    TriggerEvent("cs:engine:client:billing:open");
                }), false);
                
            }
            
            public void open(string invoice)
            {
                Debug.WriteLine("menu");
                string jsonString = "{\"action\":\"mainmenu\",\"society\":true,\"create\":true}";
                NuiState.visible = true;
                NuiState.mouse   = true;
                SendNuiMessage(jsonString);

                getBillingTarget(GetActivePlayers());

            }

            public dynamic  getPlayersFromCoords(Vector3 coords, int distance)
            {
               return GetActivePlayers();
            }
            
            public void invoice()
            {
                string jsonString = "{\"action\":\"myinvoices\",\"society\":true,\"create\":true}";
                NuiState.visible = true;
                NuiState.mouse   = true;
                SendNuiMessage(jsonString);
            }

            public async void close()
            {
                string jsonString = "{\"type\":\"Close\",\"enable\":true,\"create\":true}";
                NuiState.visible = false;
                NuiState.mouse   = false;
                SendNuiMessage(jsonString);
            }

            private void getBillingTarget( dynamic activePlayers)
            {
                Debug.WriteLine(activePlayers.Count());
    
                 
                   

            }
        }
    }