using noSql;
using QbBridge;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class QbSql : BaseScript
    {
        public int _serverId;
        public int _playerId;
        public Dictionary<string, PlayerNoSql> playersNoSql { get; set; }

        public QbSql(Dictionary<string, PlayerNoSql> _playersNoSql)
        {
            playersNoSql = _playersNoSql;
        }

        private PlayerNoSql insertGlobalNoSql(QbCore playerData)
        {
            var newPlayerClient = new PlayerNoSql
            {
                name = playerData.PlayerData.Name,
                id = playerData.PlayerData.Id,
                license = playerData.PlayerData.License,
                gangName = playerData.PlayerData.Gang.Name,
                gangIsboss = playerData.PlayerData.Gang.Isboss,
                gangLabel = playerData.PlayerData.Gang.Label,
                gangGrade = playerData.PlayerData.Gang.Grade.Name,
                citizenid = playerData.PlayerData.Citizenid,
                birthdate = playerData.PlayerData.Charinfo.Birthdate.ToString(),
                phone = playerData.PlayerData.Charinfo.Phone,
                cid = playerData.PlayerData.Charinfo.Cid,
                firstname = playerData.PlayerData.Charinfo.Firstname,
                lastname = playerData.PlayerData.Charinfo.Lastname,
                gender = playerData.PlayerData.Charinfo.Gender,
                account = playerData.PlayerData.Charinfo.Account,
                jobOnDuty = playerData.PlayerData.Job.Onduty,
                jobName = playerData.PlayerData.Job.Name,
                jobGrade = playerData.PlayerData.Job.Grade.Name,
            };

         return newPlayerClient;
        }

        private void updateGlobalNoSql(QbCore playerData)
        {
            var linq = playersNoSql.Where(x => x.Value.license == playerData.PlayerData.License).First();
            playersNoSql[linq.Key].name = playerData.PlayerData.Name;
            playersNoSql[linq.Key].id = playerData.PlayerData.Id;
            playersNoSql[linq.Key].license = playerData.PlayerData.License;
            playersNoSql[linq.Key].gangName = playerData.PlayerData.Gang.Name;
            playersNoSql[linq.Key].gangIsboss = playerData.PlayerData.Gang.Isboss;
            playersNoSql[linq.Key].gangLabel = playerData.PlayerData.Gang.Label;
            playersNoSql[linq.Key].gangGrade = playerData.PlayerData.Gang.Grade.Name;
            playersNoSql[linq.Key].citizenid = playerData.PlayerData.Citizenid;
            playersNoSql[linq.Key].birthdate = playerData.PlayerData.Charinfo.Birthdate.ToString();
            playersNoSql[linq.Key].phone = playerData.PlayerData.Charinfo.Phone;
            playersNoSql[linq.Key].cid = playerData.PlayerData.Charinfo.Cid;
            playersNoSql[linq.Key].firstname = playerData.PlayerData.Charinfo.Firstname;
            playersNoSql[linq.Key].lastname = playerData.PlayerData.Charinfo.Lastname;
            playersNoSql[linq.Key].gender = playerData.PlayerData.Charinfo.Gender;
            playersNoSql[linq.Key].account = playerData.PlayerData.Charinfo.Account;
            playersNoSql[linq.Key].jobOnDuty = playerData.PlayerData.Job.Onduty;
            playersNoSql[linq.Key].jobName = playerData.PlayerData.Job.Name;
            playersNoSql[linq.Key].jobGrade = playerData.PlayerData.Job.Grade.Name;
        }

        public async void playerDataManagement(string dataFromQbCore)
        {
            var playerData = QbBridge.QbCore.FromJson(dataFromQbCore);
            bool isInside = playersNoSql.ContainsKey(playerData.PlayerData.License);

            if (!isInside)
            {
                playersNoSql.Add(playerData.PlayerData.License, insertGlobalNoSql(playerData));
            }
            else
            {
                updateGlobalNoSql(playerData);
            }

        }
    }
}
