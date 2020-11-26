namespace MetopeMVCApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newPassordExpiryidentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastPasswordChangedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastPasswordChangedDate");
        }
    }
}
