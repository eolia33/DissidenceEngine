using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CitizenFX.Core;
using Configuration;
using Newtonsoft.Json;

namespace Server
{
    public class BridgeQbCore : BaseScript
    {
        public Dictionary<string, PlayerData> PlayerData { get; set; }
        public Server Server { get; set; }

        public BridgeQbCore(Server server)
        {
            var playerData = new Dictionary<string, PlayerData>();
            PlayerData = playerData;
            Server      = server;
            
            Server.C("(bridge) Initialisation");
        }
        

        public async void getDataFromQbCore(string id, string json)
        {            
            Server.C("(bridge) : getDataFromQbCore :: start");

            var playerData = JsonConvert.DeserializeObject<dynamic>(json);

            if (!PlayerData.ContainsKey(Server.buildKey(playerData["license"].ToString(), id)))
            {
                PlayerData.Add(Server.buildKey(playerData["license"].ToString(),id), globalSql(playerData, id, 1)); 
            }
            else
            {
                globalSql(playerData, id, 2);

            }

            Server.C("(bridge) : getDataFromQbCore :: end");
        }

        public PlayerData globalSql(dynamic playerData, string id, int action)
        {
            Server.C("(bridge) : globalSql :: start");
            var data = new PlayerData
            {
                name       = playerData["name"].ToString(),
                id         = id,
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

            Server.C("(bridge) : globalSql :: #1");
            switch (action)
            {
                case 1:
                    Server.C("(bridge) : globalSql :: case #1");
                    Players[Convert.ToInt32(id)]
                        .TriggerEvent("cs:engine:client:playerdata:update", JsonConvert.SerializeObject(data));
                    break;

                case 2: 
                    Server.C("(bridge) : globalSql :: case #2");
                    Players[Convert.ToInt32(id)]
                        .TriggerEvent("cs:engine:client:playerdata:update", JsonConvert.SerializeObject(data));
                    
                    var linq = PlayerData.First(x => x.Key == Server.buildKey(playerData["license"].ToString(),id));
                    PlayerData[linq.Key].name       = data.name;
                    PlayerData[linq.Key].id         = id;
                    PlayerData[linq.Key].license    = data.license;
                    PlayerData[linq.Key].gangName   = data.gangName;
                    PlayerData[linq.Key].gangIsboss = data.gangIsboss;
                    PlayerData[linq.Key].gangLabel  = data.gangLabel;
                    PlayerData[linq.Key].gangGrade  = data.gangGrade;
                    PlayerData[linq.Key].citizenid  = data.citizenid;
                    PlayerData[linq.Key].birthdate  = data.birthdate;
                    PlayerData[linq.Key].phone      = data.phone;                                                       
                    PlayerData[linq.Key].cid        = data.cid;
                    PlayerData[linq.Key].lastname   = data.lastname;
                    PlayerData[linq.Key].gender     = data.gender;
                    PlayerData[linq.Key].account    = data.account;
                    PlayerData[linq.Key].jobOnDuty  = data.jobOnDuty;
                    PlayerData[linq.Key].jobName    = data.jobName;
                    break;
            }           
            Server.C("(bridge) : globalSql :: end");
            return data;
        }   

    }
}