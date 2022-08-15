using System;
using CitizenFX.Core;
using Newtonsoft.Json;
using Configuration;

namespace Client
{
    public class Bridge : BaseScript
    {
        public SharedConfig configuration { get; set; }
        public PlayerNoSql player { get; set; }

        public Bridge(SharedConfig _configuration)
        {
            configuration = _configuration;
            PlayerNoSql _player = new PlayerNoSql();
            player = _player;
            EventHandlers["C#:Engine:Client:Bridge:Data"] += new Action<string>(getDataFromServer);

        }

        public void getDataFromServer(string playerData)
        {
            player = JsonConvert.DeserializeObject<PlayerNoSql>(playerData);
        }
    }
}

