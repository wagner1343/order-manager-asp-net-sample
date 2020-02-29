using System.Data.Entity.Migrations;

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