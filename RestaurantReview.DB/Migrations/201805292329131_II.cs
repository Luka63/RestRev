namespace RestaurantReview.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class II : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Restaurants", "Name", c => c.String());
            DropColumn("dbo.Restaurants", "Nmae");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Restaurants", "Nmae", c => c.String());
            DropColumn("dbo.Restaurants", "Name");
        }
    }
}
