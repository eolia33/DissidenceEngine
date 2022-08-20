using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Json
{
    public class CircleError
    {
        public int min { get; set; }
        public int max { get; set; }
    }

    public class CircleSize
    {
        public int min { get; set; }
        public int max { get; set; }
    }

    public class Root
    {
        public ShootingZone ShootingZone { get; set; }
    }

    public class ShootingZone
    {
        public List<Zone> Zone { get; set; }
    }

    public class Zone
    {
        public string       name              { get; set; }
        public int          Y                 { get; set; }
        public bool         disPlayStreetName { get; set; }
        public List<string> jobToTrigger      { get; set; }
        public bool         switchJob         { get; set; }
        public List<string> jobToSwtich       { get; set; }
        public int          day               { get; set; }
        public int          night             { get; set; }
        public CircleSize   circleSize        { get; set; }
        public int          circleDuration    { get; set; }
        public CircleError  circleError       { get; set; }
        public Point[]      zonePoints        { get; set; }
    }

    public class ZonePoint
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}