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

        public FireShot(BridgeQbCore bridge)
        {
            Bridge = bridge;
        }

        public void getActiveCops(string json)
        {
            var alert = JsonConvert.DeserializeObject<Policealert>(json);
            var isAny = false;

            foreach (var item in alert.jobtotriger)
            {
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
        }
    }
}