using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Windows;
using Configuration;
using CsCoreServer.Scaffold;
using Newtonsoft.Json;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class Billing : BaseScript
    {
        private serverContext Sql { get; set; }
        Billing(serverContext sql)
        {
            Sql = sql;
        }


        public void pay(int id, int citizenId, string serverId, int amount, int bankMoney, int cashMoney, int paymentType)
        {
            var sql = Sql.Okokbilling.First(x => x.Id == id);

            switch (paymentType)
            {
                case 1 :
                    if (canPay(bankMoney, amount))
                    { 
                        TriggerEvent("qbcore:cs:internal:billing:remove:bank", citizenId, "bank", amount);
                    }
                    break;
                case 2 :
                    if (canPay(cashMoney, amount))
                    {
                        TriggerEvent("qbcore:cs:internal:billing:remove:cash", citizenId, "cash", amount);
                    }
                    break;
            }
            
            
            if(sql.Value < bankMoney)
            {
            }
        }
        
        public void cancel(int id)
        {
            var sql = Sql.Okokbilling.First(x => x.Id == id);
            sql.Status = 3;
            Sql.SaveChanges();
        }


        private bool canPay(int playerMoney, int bill)
        {
            if (bill < playerMoney)
                return true;

            return false;
        }
        
        private bool newBill(int playerMoney, int bill)
        {
       

            return false;
        }

        public void billingList(int id)
        {
            var sql = Sql.Okokbilling.Where(x => x.Id == id).OrderByDescending(x => x.Status);
                
                List<sqlBilling> jsonList  = new List<sqlBilling>();

            foreach (var item in sql)
            {
                jsonList.Add(new sqlBilling()
                {
                    Id = item.Id,
                    ToId = item.ToId,
                    ToName = item.ToName,
                    FromId = item.FromId,
                    FromName = item.FromName,
                    Society = item.Society,
                    SocietyName = item.SocietyName,
                    Item = item.Item,
                    Value = item.Value,
                    TaxValue = item.TaxValue,
                    Status = item.Status,
                    Notes = item.Notes,
                    SentDate = item.SentDate,
                    LimitPayDate = item.LimitPayDate,
                    PaidDate = item.PaidDate
               });
            }
           
        }

        public partial class sqlBilling
    {
        public int                   Id           { get; set; }
        public string                ToId         { get; set; }
        public string                ToName       { get; set; }
        public string                FromId       { get; set; }
        public string                FromName     { get; set; }
        public string                Society      { get; set; }
        public string                SocietyName  { get; set; }
        public string                 Item         { get; set; }
        public int                   Value        { get; set; }
        public int                   TaxValue     { get; set; }
        public int                   Status       { get; set; }
        public string                Notes        { get; set; }
        public string                SentDate     { get; set; }
        public string                LimitPayDate { get; set; }
        public string                PaidDate     { get; set; }
    }
        
    }
}