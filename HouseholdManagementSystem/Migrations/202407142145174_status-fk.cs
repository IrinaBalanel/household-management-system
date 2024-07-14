namespace HouseholdManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class statusfk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodoItems", "StatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.TodoItems", "StatusId");
            AddForeignKey("dbo.TodoItems", "StatusId", "dbo.TodoItemStatus", "StatusId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TodoItems", "StatusId", "dbo.TodoItemStatus");
            DropIndex("dbo.TodoItems", new[] { "StatusId" });
            DropColumn("dbo.TodoItems", "StatusId");
        }
    }
}
