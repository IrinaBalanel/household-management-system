namespace HouseholdManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roomfk : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoomChores",
                c => new
                    {
                        Room_RoomId = c.Int(nullable: false),
                        Chore_ChoreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Room_RoomId, t.Chore_ChoreId })
                .ForeignKey("dbo.Rooms", t => t.Room_RoomId, cascadeDelete: true)
                .ForeignKey("dbo.Chores", t => t.Chore_ChoreId, cascadeDelete: true)
                .Index(t => t.Room_RoomId)
                .Index(t => t.Chore_ChoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoomChores", "Chore_ChoreId", "dbo.Chores");
            DropForeignKey("dbo.RoomChores", "Room_RoomId", "dbo.Rooms");
            DropIndex("dbo.RoomChores", new[] { "Chore_ChoreId" });
            DropIndex("dbo.RoomChores", new[] { "Room_RoomId" });
            DropTable("dbo.RoomChores");
        }
    }
}
