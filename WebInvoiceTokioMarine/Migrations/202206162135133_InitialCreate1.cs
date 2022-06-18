namespace WebInvoiceTokioMarine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderDetails", "CustomerId", c => c.String(maxLength: 128));
            AddColumn("dbo.OrderDetails", "Total", c => c.Single(nullable: false));
            CreateIndex("dbo.OrderDetails", "CustomerId");
            AddForeignKey("dbo.OrderDetails", "CustomerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "CustomerId", "dbo.AspNetUsers");
            DropIndex("dbo.OrderDetails", new[] { "CustomerId" });
            DropColumn("dbo.OrderDetails", "Total");
            DropColumn("dbo.OrderDetails", "CustomerId");
            DropColumn("dbo.OrderDetails", "CreatedDate");
        }
    }
}
