namespace MetopeMVCApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityIdScope : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EntityIdScope", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EntityIdScope");
        }
    }
}
