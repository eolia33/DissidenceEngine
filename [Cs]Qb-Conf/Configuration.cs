﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;

namespace Configuration
{   
    public class SharedConfig
    {
        public int s1 { get; set; }
        public int s2 { get; set; }
        public int s3 { get; set; }
        public int s4 { get; set; }
        public int s5 { get; set; }
        public string[] m1 { get; set; }
        public string[] m2 { get; set; }
        public string[] m3 { get; set; }
        public string[] m4 { get; set; }
        public string[] m5 { get; set; }
        public bool cerbere { get; set; }
        public string radioAcessDeniedMsg { get; set; }
        public string notificationEngine { get; set; }

        public string[] msg_selfUserLeaveTracker { get; set; } = new string[3];
        public string[] msg_otherUserLeaveTracker { get; set; } = new string[3];
        public string[] msg_otherUserLeaveTrackerByForce { get; set; } = new string[3];
        public string[] msg_selfUserJoinTracker { get; set; } = new string[3];
        public string[] msg_otherUserJoinTracker { get; set; } = new string[3];
        public string[] msg_selfUserErrorFrequency { get; set; } = new string[3];
        public string[] msg_selfUserNameFrequency { get; set; } = new string[3];
        public string[] msg_selfUserNameFrequencyRestricted { get; set; } = new string[3];
        public int pollingRate { get; set; }
        public int blipSprite { get; set; }
    }

    public class GpsDic
    {
        public string PedLicence;
        public string PedId;
        public string PedName;
        public string PedFrequency;
        public string PedColor;
        public float PedDirection;
        public Vector3 PedCoordinats;
    }

    public class GpsNetWork
    {
        public string PedLicence;
        public string PedId;
        public string PedName;
        public string PedFrequency;
        public string PedColor;
        public float PedDirection;
        public Vector3 PedCoordinats;
    }

    public class PlayerNoSql
    {
        public string name;
        public long id;
        public string license;
        public string gangName;
        public bool gangIsboss;
        public string gangLabel;
        public string gangGrade;
        public string job;
        public string citizenid;
        public string birthdate;
        public long phone;
        public long cid;
        public string firstname;
        public string lastname;
        public long gender;
        public string account;
        public bool jobOnDuty;
        public string jobName;
        public string jobGrade;
    }
    public class NuiState
    {
        public bool visible;
        public bool mouse;
    }
}
