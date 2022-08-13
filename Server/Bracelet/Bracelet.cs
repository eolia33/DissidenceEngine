using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Server
{
    internal class Bracelet:BaseScript
    {
        public void getSecurityBraceletCallFromClient(string playerId, string targetId)
        {
            Players[Convert.ToInt32(targetId)].TriggerEvent("QBCore:Notify", "Votre bracelet electronique vient d'être activé");

            Vector3 playerCoords = GetEntityCoords(GetPlayerPed(targetId));

            Players[Convert.ToInt32(playerId)].TriggerEvent("securtyBraceletRespFromServ", playerId, targetId, playerCoords);
        }
        public void getSecurityBraceletNotificationForPlolice(string playerId, string message)
        {
            Players[Convert.ToInt32(playerId)].TriggerEvent("QBCore:Notify", message);
        }


    }


}
