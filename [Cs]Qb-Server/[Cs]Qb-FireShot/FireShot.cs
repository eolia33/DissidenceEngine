using System;
using System.Linq;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;

namespace Server
{
    public class FireShot : BaseScript
    {
        public BridgeQbCore Bridge { get; }
        public Server       Server { get; }

        public FireShot(BridgeQbCore bridge, Server server)
        {
            Bridge = bridge;
            Server = server;
            
            Server.C("Fireshot :: Initialisation de l'objet FireShot");
        }

        public void getActiveCops(string json)
        {
            Server.C("getActiveCops :: starting");
            var alert = JsonConvert.DeserializeObject<Policealert>(json);
            var isAny = false;
            
            foreach (var item in alert.jobtotriger)
            {
                Server.C("Fireshot :: getActiveCops :: Foreach #1");
                var test = Bridge.PlayerData.First();
                var linq = Bridge.PlayerData.Where(x => x.Value.jobName == item && x.Value.jobOnDuty == "True");

                if (linq.Any())
                {
                    foreach (var cop in linq)
                        Players[Convert.ToInt32(cop.Value.id)].TriggerEvent(
                            "cs:engine:client:fireshot:alert", Convert.ToInt32(alert.circleDuration),
                            Convert.ToInt32(alert.circleSize),
                            Convert.ToInt32(alert.x),
                            Convert.ToInt32(alert.y),
                            Convert.ToBoolean(alert.displayStreetName),
                            Convert.ToString(alert.streetName));
                    isAny = true;
                }
            }

            if (!isAny && alert.defaultSwitchJob)
                foreach (var item in alert.jobToSwitch)
                { 
                    Server.C("Fireshot :: getActiveCops :: Foreach #2");
                    var linq = Bridge.PlayerData.Where(x => x.Value.jobName == item && x.Value.jobOnDuty == "True");

                    if (linq.Any())
                        foreach (var cop in linq)
                            Players[Convert.ToInt32(cop.Value.id)].TriggerEvent(
                                "cs:engine:client:fireshot:alert", Convert.ToInt32(alert.circleDuration),
                                Convert.ToInt32(alert.circleSize),
                                Convert.ToInt32(alert.x),
                                Convert.ToInt32(alert.y),
                                Convert.ToBoolean(alert.displayStreetName),
                                Convert.ToString(alert.streetName));
                }
            
            Server.C("Fireshot :: getActiveCops :: Ending");
        }

    }
}