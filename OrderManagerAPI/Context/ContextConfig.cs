using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace OrderManagerAPI.Context
{
    public class ContextConfig : DbMigrationsConfiguration<OrderManagerDBContext>
    {
        public ContextConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AspNetWebApi";
        }
    }
}