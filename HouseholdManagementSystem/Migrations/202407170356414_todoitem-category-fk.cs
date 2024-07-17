namespace HouseholdManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class todoitemcategoryfk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodoItems", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.TodoItems", "CategoryId");
            AddForeignKey("dbo.TodoItems", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TodoItems", "CategoryId", "dbo.Categories");
            DropIndex("dbo.TodoItems", new[] { "CategoryId" });
            DropColumn("dbo.TodoItems", "CategoryId");
        }
    }
}
