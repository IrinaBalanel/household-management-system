namespace HouseholdManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactiontodoitemremovefk : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TodoItems", "TransactionId", "dbo.Transactions");
            DropIndex("dbo.TodoItems", new[] { "TransactionId" });
            AddColumn("dbo.TodoItems", "Transaction_TransactionId", c => c.Int());
            AddColumn("dbo.Transactions", "TodoItemId", c => c.Int());
            CreateIndex("dbo.TodoItems", "Transaction_TransactionId");
            AddForeignKey("dbo.TodoItems", "Transaction_TransactionId", "dbo.Transactions", "TransactionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TodoItems", "Transaction_TransactionId", "dbo.Transactions");
            DropIndex("dbo.TodoItems", new[] { "Transaction_TransactionId" });
            DropColumn("dbo.Transactions", "TodoItemId");
            DropColumn("dbo.TodoItems", "Transaction_TransactionId");
            CreateIndex("dbo.TodoItems", "TransactionId");
            AddForeignKey("dbo.TodoItems", "TransactionId", "dbo.Transactions", "TransactionId");
        }
    }
}
