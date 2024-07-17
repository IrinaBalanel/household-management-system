namespace HouseholdManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class category : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        TransactionTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.TransactionTypes", t => t.TransactionTypeId, cascadeDelete: true)
                .Index(t => t.TransactionTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "TransactionTypeId", "dbo.TransactionTypes");
            DropIndex("dbo.Categories", new[] { "TransactionTypeId" });
            DropTable("dbo.Categories");
        }
    }
}
