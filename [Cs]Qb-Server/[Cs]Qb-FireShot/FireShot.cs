using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Newtonsoft.Json;
using Configuration;


namespace Server
{
    public class FireShot : BaseScript
    {
        public BridgeQBCore Bridge { get; }
        public FireShot(BridgeQBCore bridge)
        {
            Bridge = bridge;
        }

        public void getActiveCops(string json)
        {
            var alert = JsonConvert.DeserializeObject<Policealert>(json);
            var isAny = false;
            foreach (var item in alert.jobtotriger)
            {
                var linq = Bridge.playerNoSql.Where(x => x.Value.jobName == item && x.Value.jobOnDuty == "true");

                if (linq.Any())
                {
                    foreach (var cop in linq)
                    {
                        Players[Convert.ToInt32(cop.Value.cid)].TriggerEvent(
                            "cs:engine:client:fireshot:alert", alert.circleDuration, alert.circleSize,
                            alert.circleError, alert.streetName, alert.displayStreetName);
                    }

                    isAny = true;
                }
            }

            if (!isAny && alert.defaultSwitchJob)
            {
                foreach (var item in alert.jobToSwitch)
                {
                    var linq = Bridge.playerNoSql.Where(x => x.Value.jobName == item && x.Value.jobOnDuty == "true");

                    if (linq.Any())
                    {
                        foreach (var cop in linq)
                        {
                            Players[Convert.ToInt32(cop.Value.cid)].TriggerEvent(
                                "cs:engine:client:fireshot:alert", alert.circleDuration, alert.circleSize,
                                alert.circleError, alert.streetName, alert.displayStreetName);
                        }
                    }
                }
            }
        }
    }
}