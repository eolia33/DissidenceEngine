using Newtonsoft.Json;
using Configuration;
using CitizenFX.Core;

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

        public async void decodingData(string playerData)
        {
            Player = JsonConvert.DeserializeObject<PlayerData>(playerData);
        }
    }
}