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
        public Dictionary<string, PlayerNoSql> playersNoSql { get; set; }

        public BridgeQBCore(Dictionary<string, PlayerNoSql> _playersNoSql)
        {
            playersNoSql = _playersNoSql;
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
            var linq = playersNoSql.Where(x => x.Value.license == playerData.PlayerData.License).First();

            playersNoSql[linq.Key].name       = playerData.PlayerData.Name;
            playersNoSql[linq.Key].id         = playerData.PlayerData.Id.ToString();
            playersNoSql[linq.Key].license    = playerData.PlayerData.License;
            playersNoSql[linq.Key].gangName   = playerData.PlayerData.Gang.Name;
            playersNoSql[linq.Key].gangIsboss = playerData.PlayerData.Gang.Isboss.ToString();
            playersNoSql[linq.Key].gangLabel  = playerData.PlayerData.Gang.Label;
            playersNoSql[linq.Key].gangGrade  = playerData.PlayerData.Gang.Grade.Name;
            playersNoSql[linq.Key].citizenid  = playerData.PlayerData.Citizenid;
            playersNoSql[linq.Key].birthdate  = playerData.PlayerData.Charinfo.Birthdate.ToString();
            playersNoSql[linq.Key].phone      = playerData.PlayerData.Charinfo.Phone.ToString();
            playersNoSql[linq.Key].cid        = playerData.PlayerData.Charinfo.Cid.ToString();
            playersNoSql[linq.Key].firstname  = playerData.PlayerData.Charinfo.Firstname;
            playersNoSql[linq.Key].lastname   = playerData.PlayerData.Charinfo.Lastname;
            playersNoSql[linq.Key].gender     = playerData.PlayerData.Charinfo.Gender.ToString();
            playersNoSql[linq.Key].account    = playerData.PlayerData.Charinfo.Account;
            playersNoSql[linq.Key].jobOnDuty  = playerData.PlayerData.Job.Onduty.ToString();
            playersNoSql[linq.Key].jobName    = playerData.PlayerData.Job.Name;
            playersNoSql[linq.Key].jobGrade   = playerData.PlayerData.Job.Grade.Name;
        }


        public void getDataFromQbCore(string json)
        {
            var playerData = QbCore.FromJson(json);

            if (!playersNoSql.ContainsKey(playerData.PlayerData.License))
                playersNoSql.Add(playerData.PlayerData.License, insertGlobalNoSql(playerData));

            else
                updateGlobalNoSql(playerData);


            sendDataToClient(playerData, playerData.PlayerData.Id.ToString());
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