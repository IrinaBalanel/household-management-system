namespace HouseholdManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ownerassignedfk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodoItems", "AssignedToOwnerId", c => c.Int(nullable: false));
            CreateIndex("dbo.TodoItems", "AssignedToOwnerId");
            //by setting cascadeDelete: false, the owner cannot be deleted if they have an assigned a TodoItem.
            AddForeignKey("dbo.TodoItems", "AssignedToOwnerId", "dbo.Owners", "OwnerId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TodoItems", "AssignedToOwnerId", "dbo.Owners");
            DropIndex("dbo.TodoItems", new[] { "AssignedToOwnerId" });
            DropColumn("dbo.TodoItems", "AssignedToOwnerId");
        }
    }
}
