using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;
using System.Linq;

namespace Client
{
    public class FireShot : BaseScript
    {
        private SharedConfig jsonContent    { get; set; }
        private Random       random         { get; set; }
        private SharedConfig config         { get; set; }
        private int          chance         { get; set; }
        private List<Zone>   zoneList       { get; set; }
        private  bool        canIcheckAgain { get; set; }

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

        public bool parking(int id)
        {
            var playerCoords = GetEntityCoords(id, false);
            var playerPoint  = new Point(Convert.ToInt32(playerCoords.X), Convert.ToInt32(playerCoords.Y));

            if (playerCoords.Z < 3)
            {
                var linq = jsonContent.ShootingZone.Zone.Where(x => x.Name == "Parking");
                foreach (var item in linq)
                {
                    var zonePoints = item.ZonePoints;
                    if (isInZone(zonePoints, playerPoint))
                    {
                        return true;
                    }
                }
            }

            return false;
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
            TriggerServerEvent("cs:server:shootingzone:new:policealert", JsonConvert.SerializeObject(policeAlertContent));
        }

        private int randomGenerator(int x, int y, int type)
        {
            switch (type)
            {
                case 1:

                    var rand = random.Next(0, 100);

                    if (rand > (100 - x))
                        return 1;

                    return 0;
                    break;

                case 2:
                    return random.Next(x, y);
                    break;
            }

            return 0;
        }

        private static bool isInZone(Point[] poly, Point point)
        {
            int i, j;
            var nvert = poly.Length;
            var c = false;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
                if (poly[i].Y > point.Y != poly[j].Y > point.Y &&
                    point.X < (poly[j].X - poly[i].X) * (point.Y - poly[i].Y) / (poly[j].Y - poly[i].Y) + poly[i].X)
                    c = !c;

            return c;
        }
        

        public void fireShotServerResponse(int circleDuration,int circlesize, int x, int y, bool displayStreetName, string streetName )
        {
           
            makeBlip(circleDuration, circlesize, x, y);

            if(displayStreetName)
                nui(config.msg_zoneNotification, "Au niveau de : " + streetName);
            else 
                nui(config.msg_zoneNotification, "");

            
        }  

        private async Task makeBlip(int circleDuration, int circlesize, int x, int y)
        {
            var blip = AddBlipForRadius(x, y, 0, (circlesize));
            SetBlipSprite(blip, config.zoneSprite);
            SetBlipColour(blip, config.zoneSpriteColor);
            SetBlipAsShortRange(blip, true);
            await Delay(10000);
            RemoveBlip(ref blip);            
        }

        public void nui(string[] msg, string replace = null)
        {
            TriggerEvent(config.notificationEngine, msg[0], msg[1].Replace("{replace}",replace), msg[2], msg[3]);
        }
    }
}