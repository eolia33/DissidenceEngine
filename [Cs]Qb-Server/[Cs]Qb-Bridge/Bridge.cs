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

        private PlayerNoSql insertGlobalNoSql(PlayerData playerData)
        {
            var newPlayerClient = new PlayerNoSql
            {
                name       = playerData.Name,
                id         = playerData.Cid.ToString(),
                license    = playerData.License,
                gangName   = playerData.Gang.Name,
                gangIsboss = playerData.Gang.Isboss.ToString(),
                gangLabel  = playerData.Gang.Label,
                gangGrade  = playerData.Gang.Grade.Name,
                citizenid  = playerData.Citizenid,
                birthdate  = playerData.Charinfo.Birthdate.ToString(),
                phone      = playerData.Charinfo.Phone.ToString(),
                cid        = playerData.Cid.ToString(),
                firstname  = playerData.Charinfo.Firstname,
                lastname   = playerData.Charinfo.Lastname,
                gender     = playerData.Charinfo.Gender.ToString(),
                account    = playerData.Charinfo.Account,
                jobOnDuty  = playerData.Job.Onduty.ToString(),
                jobName    = playerData.Job.Name,
                jobGrade   = playerData.Job.Grade.Name
            };
            return newPlayerClient;
        }

        private void updateGlobalNoSql(PlayerData  playerData)
        {
            var linq = playerNoSql.Where(x => x.Value.license == playerData.License).First();

            playerNoSql[linq.Key].name       = playerData.Name;
            playerNoSql[linq.Key].id         = playerData.Cid.ToString();
            playerNoSql[linq.Key].license    = playerData.License;
            playerNoSql[linq.Key].gangName   = playerData.Gang.Name;
            playerNoSql[linq.Key].gangIsboss = playerData.Gang.Isboss.ToString();
            playerNoSql[linq.Key].gangLabel  = playerData.Gang.Label;
            playerNoSql[linq.Key].gangGrade  = playerData.Gang.Grade.Name;
            playerNoSql[linq.Key].citizenid  = playerData.Citizenid;
            playerNoSql[linq.Key].birthdate  = playerData.Charinfo.Birthdate.ToString();
            playerNoSql[linq.Key].phone      = playerData.Charinfo.Phone.ToString();
            playerNoSql[linq.Key].cid        = playerData.Cid.ToString();
            playerNoSql[linq.Key].firstname  = playerData.Charinfo.Firstname;
            playerNoSql[linq.Key].lastname   = playerData.Charinfo.Lastname;
            playerNoSql[linq.Key].gender     = playerData.Charinfo.Gender.ToString();
            playerNoSql[linq.Key].account    = playerData.Charinfo.Account;
            playerNoSql[linq.Key].jobOnDuty  = playerData.Job.Onduty.ToString();
            playerNoSql[linq.Key].jobName    = playerData.Job.Name;
            playerNoSql[linq.Key].jobGrade   = playerData.Job.Grade.Name;
        }


        public void getDataFromQbCore(string id, string json)
        {
            Debug.WriteLine("\x1b[36m"+"[Receiving from BRIDGE(" +id.ToString()+")]");

            var playerData = PlayerData.FromJson(json);

            if (!playerNoSql.ContainsKey(playerData.License))
            {
                playerNoSql.Add(playerData.License, insertGlobalNoSql(playerData));
            }

            else
            {
                updateGlobalNoSql(playerData);
            }

        }

        public void sendDataToClient(PlayerData playerData, string playerid)
        {
            var listToJson = new List<PlayerNoSql>();

            listToJson.Add(new PlayerNoSql()
            {
                id         = playerData.Cid.ToString(),
                gangName   = playerData.Gang.Name,
                gangIsboss = playerData.Gang.Isboss.ToString(),
                gangLabel  = playerData.Gang.Label,
                citizenid  = playerData.Citizenid.ToString(),
                phone      = playerData.Charinfo.Phone.ToString(),
                cid        = playerData.Cid.ToString(),
                firstname  = playerData.Charinfo.Firstname,
                lastname   = playerData.Charinfo.Lastname,
                gender     = playerData.Charinfo.Gender.ToString(),
                jobOnDuty  = playerData.Job.Onduty.ToString(),
                jobName    = playerData.Job.Name,
                jobGrade   = playerData.Job.Grade.Name
            });

            Players[playerid].TriggerEvent("c#ServerUpdate", JsonConvert.SerializeObject(listToJson));
        }
    }
}