using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class Cmd
    {

        public Server       Server        { get; set; }
        public Tracker      Tracker       { get; set; }
        public BridgeQbCore Bridge        { get; set; }
        public FireShot     FireShot      { get; set; }
        public SharedConfig Configuration { get; set; }

        public Cmd(Server server, Tracker tracker, BridgeQbCore bridge, FireShot fireshot, SharedConfig configuration )
        {
            Server        = server;
            Tracker       = tracker;
            Bridge        = bridge;
            FireShot      = fireshot;
            Configuration = configuration;
        }

    }
}