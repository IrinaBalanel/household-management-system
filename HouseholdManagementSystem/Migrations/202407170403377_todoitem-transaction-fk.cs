namespace HouseholdManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class todoitemtransactionfk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodoItems", "TransactionId", c => c.Int());
            CreateIndex("dbo.TodoItems", "TransactionId");
            AddForeignKey("dbo.TodoItems", "TransactionId", "dbo.Transactions", "TransactionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TodoItems", "TransactionId", "dbo.Transactions");
            DropIndex("dbo.TodoItems", new[] { "TransactionId" });
            DropColumn("dbo.TodoItems", "TransactionId");
        }
    }
}
