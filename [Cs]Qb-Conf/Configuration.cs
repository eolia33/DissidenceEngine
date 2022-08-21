using System.Collections.Generic;
using CitizenFX.Core;
using System.Drawing;

namespace Configuration
{
    public class SharedConfig
    {
        public int                         zoneWaitTime                        { get; set; }
        public string                      zoneDefaultName                     { get; set; }
        
        
        public bool         zoneDefaultDisPlayStreetName { get; set; }
        public List<string> zoneDefaultJobToTriggerNorth { get; set; }
        public List<string> zoneDefaultJobToTriggerSouth { get; set; }
        public bool         zoneDefaultSwitchJob         { get; set; }
        public List<string> zoneDefaultJobToSwtich       { get; set; }
        public string       notificationEngine           { get; set; }
        public int          zoneDefaultDay               { get; set; }
        public int          zoneDefaultNight             { get; set; }
        public Circle       zoneDefaultCircleSize        { get; set; }
        public int          zoneDefaultCircleDuration    { get; set; }
        public Circle       zoneDefaultCircleError       { get; set; }
        public List<string> zoneDefaultJobToSwtichSouth  { get; set; }
        public List<string> zoneDefaultJobToSwtichNorth  { get; set; }
        public int          zoneSprite                   { get; set; }
        public int          zoneSpriteColor              { get; set; }

        public bool                        cerbere                             { get; set; }
        public string[]                    msg_selfUserLeaveTracker            { get; set; } = new string[3];
        public string[]                    msg_selfUserLeaveTrackerDuty        { get; set; } = new string[3];
        public string[]                    msg_otherUserLeaveTracker           { get; set; } = new string[3];
        public string[]                    msg_otherUserLeaveTrackerByForce    { get; set; } = new string[3];
        public string[]                    msg_otherUserLeaveTrackerByDuty     { get; set; } = new string[3];
        public string[]                    msg_selfUserJoinTracker             { get; set; } = new string[3];
        public string[]                    msg_otherUserJoinTracker            { get; set; } = new string[3];
        public string[]                    msg_selfUserErrorFrequency          { get; set; } = new string[3];
        public string[]                    msg_selfUserNameFrequency           { get; set; } = new string[3];
        public string[]                    msg_selfUserNameFrequencyRestricted { get; set; } = new string[3];
        public string[]                    msg_selfTrackerNotificationOn       { get; set; } = new string[3];
        public string[]                    msg_selfTrackerNotificationOff      { get; set; } = new string[3];
        public string[]                    msg_zoneNotification                { get; set; } = new string[3];
        public int                         trackerServerPollingRate            { get; set; }
        public int                         trackerBlipSprite                   { get; set; }
        public List<RestrictedFrequencies> restrictedFrequencies               { get; set; }
        public ShootingZone                ShootingZone                        { get; set; }
    }

    public partial class ShootingZone
    {
        public Zone[] Zone { get; set; }
    }

    public partial class Zone
    {
        public string       Name              { get; set; }
        public long         Y                 { get; set; }
        public bool         DisPlayStreetName { get; set; }
        public List<string> JobToTrigger      { get; set; }
        public bool         SwitchJob         { get; set; }
        public List<string> JobToSwtich       { get; set; }
        public int          Day               { get; set; }
        public int          Night             { get; set; }
        public Circle       CircleSize        { get; set; }
        public int          CircleDuration    { get; set; }
        public Circle       CircleError       { get; set; }
        public Point[]      ZonePoints        { get; set; }
    }

    public partial class Circle
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public class Policealert
    {
        public int          circleSize        { get; set; }
        public int          circleDuration    { get; set; }
        public int          circleError       { get; set; }
        public string       streetName        { get; set; }
        public bool         displayStreetName { get; set; }
        public List<string> jobtotriger       { get; set; }
        public List<string> jobToSwitch       { get; set; }
        public bool         defaultSwitchJob  { get; set; }
        public int          x                 { get; set; }
        public int          y                 { get; set; }
    }

    public class Alert
    {
        public Policealert policealert { get; set; }
    }


    public class RestrictedFrequencies
    {
        public string       frequency { get; set; }
        public bool         onduty    { get; set; }
        public List<string> jobs      { get; set; }
    }

    public class TrackerDic
    {
        public string  PedLicence;
        public string  PedId;
        public string  PedName;
        public string  PedFrequency;
        public string  PedColor;
        public int     PedNotification;
        public float   PedDirection;
        public Vector3 PedCoordinats;
    }

    public class GpsNetWork
    {
        public string  PedLicence;
        public string  PedId;
        public string  PedName;
        public string  PedFrequency;
        public string  PedColor;
        public int     PedNotification;
        public float   PedDirection;
        public Vector3 PedCoordinats;
    }

    public class PlayerData
    {
        public string name;
        public string id;
        public string gangName;
        public string gangIsboss;
        public string gangLabel;
        public string citizenid;
        public string phone;
        public string cid;
        public string firstname;
        public string lastname;
        public string gender;
        public string jobOnDuty;
        public string jobName;
        public string jobGrade;
        public string license;
        public string gangGrade;
        public string birthdate;
        public string account;
    }

    public class NuiState
    {
        public bool visible;
        public bool mouse;
    }
}