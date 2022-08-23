using Newtonsoft.Json;
using Configuration;

namespace Client
{
    public class Bridge 
    {
        public SharedConfig Configuration { get; set; }
        public PlayerData Player { get; set; }

        public Bridge(SharedConfig configuration)
        {
            Configuration = configuration;
            PlayerData player = new PlayerData();
            Player = player;
        }

#pragma warning disable CS1998
        public async void decodingData(string playerData)
#pragma warning restore CS1998
        {
            Player = JsonConvert.DeserializeObject<PlayerData>(playerData);
        }
    }
}