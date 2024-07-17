namespace HouseholdManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactionType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionTypes",
                c => new
                    {
                        TransactionTypeId = c.Int(nullable: false, identity: true),
                        TransactionTypeName = c.String(),
                    })
                .PrimaryKey(t => t.TransactionTypeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransactionTypes");
        }
    }
}
