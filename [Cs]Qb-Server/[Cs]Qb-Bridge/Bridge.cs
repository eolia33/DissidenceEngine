using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Newtonsoft.Json;
using Configuration;


namespace Server
{
    public class BridgeQBCore : BaseScript
    {
        public Dictionary<string, PlayerNoSql> playerNoSql { get; set; }
        public Server server { get; set; }

        public BridgeQBCore(Server _server)
        {
            var _playerNoSql = new Dictionary<string, PlayerNoSql>();
            playerNoSql = _playerNoSql;
            server      = _server;
        }

        private PlayerNoSql insertGlobalNoSql(dynamic playerData)
        {
            var newPlayerClient = new PlayerNoSql
            {
                name       = playerData["name"].ToString(),
                id         = playerData["id"].ToString(),
                license    = playerData["license"].ToString(),
                gangName   = playerData["gang"]["name"].ToString(),
                gangIsboss = playerData["gang"]["isboss"].ToString(),
                gangLabel  = playerData["gang"]["label"].ToString(),
                gangGrade  = playerData["gang"]["grade"]["name"].ToString(),
                citizenid  = playerData["citizenid"].ToString(),
                birthdate  = playerData["charinfo"]["birthdate"].ToString(),
                phone      = playerData["charinfo"]["phone"].ToString(),
                cid        = playerData["cid"].ToString(),
                firstname  = playerData["charinfo"]["firstname"].ToString(),
                lastname   = playerData["charinfo"]["lastname"].ToString(),
                gender     = playerData["charinfo"]["gender"].ToString(),
                account    = playerData["charinfo"]["account"].ToString(),
                jobOnDuty  = playerData["job"]["onduty"].ToString(),
                jobName    = playerData["job"]["name"].ToString(),
                jobGrade   = playerData["job"]["grade"]["name"].ToString()
            };

            //sendDataToClient(newPlayerClient, playerData["cid"].ToString());
            return newPlayerClient;
        }

        private void updateGlobalNoSql(dynamic playerData)
        {
            var linq = playerNoSql.First(x => x.Value.license == playerData["license"].ToString());

            playerNoSql[linq.Key].name       = playerData["name"].ToString();
            playerNoSql[linq.Key].id         = playerData["id"].ToString();
            playerNoSql[linq.Key].license    = playerData["license"].ToString();
            playerNoSql[linq.Key].gangName   = playerData["gang"]["name"].ToString();
            playerNoSql[linq.Key].gangIsboss = playerData["gang"]["isboss"].ToString();
            playerNoSql[linq.Key].gangLabel  = playerData["gang"]["label"].ToString();
            playerNoSql[linq.Key].gangGrade  = playerData["gang"]["name"].ToString();
            playerNoSql[linq.Key].citizenid  = playerData["citizenid"].ToString();
            playerNoSql[linq.Key].birthdate  = playerData["charinfo"]["birthdate"].ToString();
            playerNoSql[linq.Key].phone      = playerData["charinfo"]["phone"].ToString();
            playerNoSql[linq.Key].cid        = playerData["charinfo"]["firstname"].ToString();
            playerNoSql[linq.Key].lastname   = playerData["charinfo"]["lastname"].ToString();
            playerNoSql[linq.Key].gender     = playerData["charinfo"]["gender"].ToString();
            playerNoSql[linq.Key].account    = playerData["charinfo"]["account"].ToString();
            playerNoSql[linq.Key].jobOnDuty  = playerData["job"]["onduty"].ToString();
            playerNoSql[linq.Key].jobName    = playerData["job"]["name"].ToString();
            playerNoSql[linq.Key].jobGrade   = playerData["job"]["grade"]["name"].ToString();
        }

        public void x(string msg)
        {
            Debug.WriteLine("\x1b[36m" + msg);
        }

        public async void getDataFromQbCore(string id, string json)
        {

            var playerData = JsonConvert.DeserializeObject<dynamic>(json);

            if (!playerNoSql.ContainsKey(playerData["license"].ToString()))
                playerNoSql.Add(playerData["license"].ToString(), insertGlobalNoSql(playerData));

            else
                updateGlobalNoSql(playerData);

            if (!playerNoSql.ContainsKey(playerData["license"].ToString()))
                playerNoSql.Add(playerData["license"].ToString(), insertGlobalNoSql(playerData));

            else
                updateGlobalNoSql(playerData);
        }

        public void sendDataToClient(PlayerNoSql playerData, string playerid)
        {
            Players[playerid]
                .TriggerEvent("cs:engine:client:playerdata:update", JsonConvert.SerializeObject(playerData));
        }
    }
}