namespace HouseholdManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ownercreatedfk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodoItems", "CreatedByOwnerId", c => c.Int(nullable: false));
            CreateIndex("dbo.TodoItems", "CreatedByOwnerId");
            AddForeignKey("dbo.TodoItems", "CreatedByOwnerId", "dbo.Owners", "OwnerId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TodoItems", "CreatedByOwnerId", "dbo.Owners");
            DropIndex("dbo.TodoItems", new[] { "CreatedByOwnerId" });
            DropColumn("dbo.TodoItems", "CreatedByOwnerId");
        }
    }
}
