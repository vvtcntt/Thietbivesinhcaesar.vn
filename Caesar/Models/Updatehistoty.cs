using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caesar.Models;
namespace Caesar.Models
{
    public class Updatehistoty
    {
        public CaesarContext db = new CaesarContext();
        public static void UpdateHistory(string task,string FullName,string UserID)
        {

            CaesarContext db = new CaesarContext();
            tblHistoryLogin tblhistorylogin = new tblHistoryLogin();
            tblhistorylogin.FullName = FullName;
            tblhistorylogin.Task = task;
            tblhistorylogin.idUser = int.Parse(UserID);
            tblhistorylogin.DateCreate = DateTime.Now;
            tblhistorylogin.Active = true;
            
            db.tblHistoryLogins.Add(tblhistorylogin);
            db.SaveChanges();
           
        }
    }
}