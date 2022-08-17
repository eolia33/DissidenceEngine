using QbBridge;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Configuration;

namespace Server
{
    public class BridgeQBCore : BaseScript
    {
        public Dictionary<string, PlayerNoSql> playerNoSql { get; set; }

        public BridgeQBCore()
        {
            var _playerNoSql = new Dictionary<string, PlayerNoSql>();
            playerNoSql = _playerNoSql;
        }

        private PlayerNoSql insertGlobalNoSql(QbCore playerData)
        {
            var newPlayerClient = new PlayerNoSql
            {
                name       = playerData.PlayerData.Name,
                id         = playerData.PlayerData.Id.ToString(),
                license    = playerData.PlayerData.License,
                gangName   = playerData.PlayerData.Gang.Name,
                gangIsboss = playerData.PlayerData.Gang.Isboss.ToString(),
                gangLabel  = playerData.PlayerData.Gang.Label,
                gangGrade  = playerData.PlayerData.Gang.Grade.Name,
                citizenid  = playerData.PlayerData.Citizenid,
                birthdate  = playerData.PlayerData.Charinfo.Birthdate.ToString(),
                phone      = playerData.PlayerData.Charinfo.Phone.ToString(),
                cid        = playerData.PlayerData.Charinfo.Cid.ToString(),
                firstname  = playerData.PlayerData.Charinfo.Firstname,
                lastname   = playerData.PlayerData.Charinfo.Lastname,
                gender     = playerData.PlayerData.Charinfo.Gender.ToString(),
                account    = playerData.PlayerData.Charinfo.Account,
                jobOnDuty  = playerData.PlayerData.Job.Onduty.ToString(),
                jobName    = playerData.PlayerData.Job.Name,
                jobGrade   = playerData.PlayerData.Job.Grade.Name
            };
            return newPlayerClient;
        }

        private void updateGlobalNoSql(QbCore playerData)
        {
            var linq = playerNoSql.Where(x => x.Value.license == playerData.PlayerData.License).First();

            playerNoSql[linq.Key].name       = playerData.PlayerData.Name;
            playerNoSql[linq.Key].id         = playerData.PlayerData.Id.ToString();
            playerNoSql[linq.Key].license    = playerData.PlayerData.License;
            playerNoSql[linq.Key].gangName   = playerData.PlayerData.Gang.Name;
            playerNoSql[linq.Key].gangIsboss = playerData.PlayerData.Gang.Isboss.ToString();
            playerNoSql[linq.Key].gangLabel  = playerData.PlayerData.Gang.Label;
            playerNoSql[linq.Key].gangGrade  = playerData.PlayerData.Gang.Grade.Name;
            playerNoSql[linq.Key].citizenid  = playerData.PlayerData.Citizenid;
            playerNoSql[linq.Key].birthdate  = playerData.PlayerData.Charinfo.Birthdate.ToString();
            playerNoSql[linq.Key].phone      = playerData.PlayerData.Charinfo.Phone.ToString();
            playerNoSql[linq.Key].cid        = playerData.PlayerData.Charinfo.Cid.ToString();
            playerNoSql[linq.Key].firstname  = playerData.PlayerData.Charinfo.Firstname;
            playerNoSql[linq.Key].lastname   = playerData.PlayerData.Charinfo.Lastname;
            playerNoSql[linq.Key].gender     = playerData.PlayerData.Charinfo.Gender.ToString();
            playerNoSql[linq.Key].account    = playerData.PlayerData.Charinfo.Account;
            playerNoSql[linq.Key].jobOnDuty  = playerData.PlayerData.Job.Onduty.ToString();
            playerNoSql[linq.Key].jobName    = playerData.PlayerData.Job.Name;
            playerNoSql[linq.Key].jobGrade   = playerData.PlayerData.Job.Grade.Name;
        }


        public void getDataFromQbCore(string id, string json)
        {
            Debug.WriteLine("\x1b[36m"+"[Receiving from BRIDGE(" +id.ToString()+")]");

            var playerData = QbCore.FromJson(json);

            if (!playerNoSql.ContainsKey(playerData.PlayerData.License)) 
                playerNoSql.Add(playerData.PlayerData.License, insertGlobalNoSql(playerData));

            else
                updateGlobalNoSql(playerData);

        }

        public void sendDataToClient(QbCore playerData, string playerid)
        {
            var listToJson = new List<PlayerNoSql>();

            listToJson.Add(new PlayerNoSql()
            {
                id         = playerData.PlayerData.Id.ToString(),
                gangName   = playerData.PlayerData.Gang.Name,
                gangIsboss = playerData.PlayerData.Gang.Isboss.ToString(),
                gangLabel  = playerData.PlayerData.Gang.Label,
                citizenid  = playerData.PlayerData.Citizenid.ToString(),
                phone      = playerData.PlayerData.Charinfo.Phone.ToString(),
                cid        = playerData.PlayerData.Charinfo.Cid.ToString(),
                firstname  = playerData.PlayerData.Charinfo.Firstname,
                lastname   = playerData.PlayerData.Charinfo.Lastname,
                gender     = playerData.PlayerData.Charinfo.Gender.ToString(),
                jobOnDuty  = playerData.PlayerData.Job.Onduty.ToString(),
                jobName    = playerData.PlayerData.Job.Name,
                jobGrade   = playerData.PlayerData.Job.Grade.Name
            });

            Players[playerid].TriggerEvent("c#ServerUpdate", JsonConvert.SerializeObject(listToJson));
        }
    }
}