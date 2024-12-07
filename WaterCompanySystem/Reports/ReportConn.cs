using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WaterCompanySystem.Reports
{
    public class ReportConn
    {
        public static CrystalDecisions.Shared.ConnectionInfo getConnection()
        {
            
            CrystalDecisions.Shared.ConnectionInfo infocon = new CrystalDecisions.Shared.ConnectionInfo();
            string efConnectionString = ConfigurationManager.ConnectionStrings["WaterComponySystemEntities"].ConnectionString;
            var entityBuilder = new EntityConnectionStringBuilder(efConnectionString);
            string sqlConnectionString = entityBuilder.ProviderConnectionString;
            var strcon = new SqlConnectionStringBuilder(sqlConnectionString);
            infocon.ServerName = strcon.DataSource;
            infocon.DatabaseName = strcon.InitialCatalog;
            infocon.IntegratedSecurity = strcon.IntegratedSecurity;

            return infocon;
        }
    }
}

//var strcon = new System.Data.SqlClient.SqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings["WaterComponySystemEntities"].ConnectionString);
// CrystalDecisions.Shared.ConnectionInfo infocon = new CrystalDecisions.Shared.ConnectionInfo();
// infocon.ServerName = strcon.DataSource; ;
// infocon.DatabaseName = strcon.InitialCatalog;
// infocon.IntegratedSecurity = strcon.IntegratedSecurity;
// CrystalDecisions.Shared.ConnectionInfo infocon = new CrystalDecisions.Shared.ConnectionInfo();
//infocon.ServerName = @"DESKTOP-DBNE7B5\SQLEXPRESS";
// infocon.DatabaseName = "WaterComponySystem";
// infocon.IntegratedSecurity = true;