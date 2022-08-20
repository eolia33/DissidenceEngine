using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;
using System.Linq;
using System.Threading;

namespace Client
{
    public class FireShot : BaseScript
    {
        private SharedConfig jsonContent    { get; set; }
        private Random       random         { get; set; }
        private SharedConfig config         { get; set; }
        private int          chance         { get; set; }
        private List<Zone>   zoneList       { get; set; }
        private  bool         canIcheckAgain { get; set; }

        public FireShot(SharedConfig _config)
        {
            var _jsonContent = JsonConvert.DeserializeObject<SharedConfig>(LoadResourceFile(GetCurrentResourceName(), "zone.json"));
            jsonContent      = _jsonContent;
            var _random      = new Random();
            random           = _random;
            config           = _config;
            canIcheckAgain   = true;
        }

        private async Task dexterPrescot(int buster)
        {
            await Delay(buster);
            canIcheckAgain = true;
        }

        public async Task checkZone(int id)
        {
            if (!canIcheckAgain)
                return;

            canIcheckAgain = false;
            dexterPrescot(config.zoneWaitTime);
            var playerCoords = GetEntityCoords(id, false);
            var playerPoint  = new Point(Convert.ToInt32(playerCoords.X), Convert.ToInt32(playerCoords.Y));
            var playerTime   = GetClockHours();
            var inLoop       = false;
            var filter       = 0;

            if (playerCoords.Y >= 1300)
                filter = 1300;


            var linq = jsonContent.ShootingZone.Zone.Where(x => x.Y == filter);
            foreach (var item in linq)
            {
                var zonePoints = item.ZonePoints;

                if (isInZone(zonePoints, playerPoint))
                {
                    triggerProcess(item, playerCoords, playerPoint, playerTime);
                    inLoop = true;
                    break;
                }
            }

            if (!inLoop)
            {
                var zoneToSwitch  = config.zoneDefaultJobToSwtichSouth;
                var zoneToTrigger = config.zoneDefaultJobToTriggerSouth;

                if (playerCoords.Y >= 1300)
                {
                    zoneToSwitch  = config.zoneDefaultJobToSwtichNorth;
                    zoneToTrigger = config.zoneDefaultJobToTriggerNorth;
                }

                var defaultZone = new Zone()
                {
                    Name              = config.zoneDefaultName,
                    Y                 = 0,
                    DisPlayStreetName = config.zoneDefaultDisPlayStreetName,
                    JobToTrigger      = zoneToTrigger,
                    SwitchJob         = config.zoneDefaultSwitchJob,
                    JobToSwtich       = zoneToSwitch,
                    Day               = config.zoneDefaultDay,
                    Night             = config.zoneDefaultNight,
                    CircleSize        = config.zoneDefaultCircleSize,
                    CircleDuration    = config.zoneDefaultCircleDuration,
                    CircleError       = config.zoneDefaultCircleError
                };


                triggerProcess(defaultZone, playerCoords, playerPoint, playerTime);
            }
        }

        private void triggerProcess(Zone item, Vector3 playerCoords, Point playerPoint, int playerTime)
        {
            chance = item.Night;

            if (playerTime > 5 && playerTime < 22)
                chance = item.Day;

            if (randomGenerator(chance, 0, 1) == 0)
                return;

            var circleDrawSize  = randomGenerator(item.CircleSize.Min,  item.CircleSize.Max,  2);
            var circleDrawError = randomGenerator(item.CircleError.Min, item.CircleError.Max, 2);

            var vector = new Vector3(playerCoords.X + circleDrawError, playerCoords.Y + circleDrawError, 0);

            var policeAlertContent = new Policealert()
            {
                circleDuration    = 20,
                circleSize        = circleDrawSize,
                circleError       = circleDrawError,
                streetName        = World.GetStreetName(vector),
                displayStreetName = item.DisPlayStreetName,
                jobtotriger       = item.JobToTrigger,
                defaultSwitchJob  = item.SwitchJob,
                jobToSwitch       = item.JobToSwtich,
                x                 = Convert.ToInt32(vector.X),
                y                 = Convert.ToInt32(vector.Y)
            };
            TriggerServerEvent("cs:server:shootingzone:new:policealert", policeAlertContent);
        }

        private int randomGenerator(int x, int y, int type)
        {
            switch (type)
            {
                case 1:

                    var rand = random.Next(0, 10000);

                    if (rand > 10000 - x * 100)
                        return 1;

                    return 0;
                    break;

                case 2:
                    return random.Next(x, y);
                    break;
            }

            return 0;
        }

        private static bool isInZone(Point[] poly, Point pnt)
        {
            int i, j;
            var nvert = poly.Length;
            var c = false;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
                if (poly[i].Y > pnt.Y != poly[j].Y > pnt.Y &&
                    pnt.X < (poly[j].X - poly[i].X) * (pnt.Y - poly[i].Y) / (poly[j].Y - poly[i].Y) + poly[i].X)
                    c = !c;

            return c;
        }
        
        public void fireShotServerResponse(int circleDuration,int circlesize, int x, int y, bool displayStreetName, string streetName )
        {
            var blip = AddBlipForCoord(x, y,0);
            SetBlipSprite(blip, 161);
            SetBlipColour(blip, 41);
            SetBlipScale(blip, circlesize);
            SetBlipAsShortRange(blip, true);
            _ = Task.Run(() => { removeBlip(blip, circleDuration); });
            
            if(displayStreetName)
                 nuiNotify(config.msg_zoneNotification, "Location :" + streetName);
            else              
                nuiNotify(config.msg_zoneNotification, "");
        }  
        private Task removeBlip(int blip, int time)
        {
            Thread.Sleep(time);
            RemoveBlip(ref blip);

            return Task.CompletedTask;
        }

        public async void nuiNotify(string[] msg, string replace = null)
        {
            TriggerEvent(config.notificationEngine, msg[0], msg[1], msg[2], msg[3]);
        }
    }
}