namespace RestaurantReview.DB.Migrations
{
    using RestaurantReview.Core.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RestaurantReview.DB.DataContext.ReviewsDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "RestaurantReview.DB.DataContext.ReviewsDb";
        }

        protected override void Seed(RestaurantReview.DB.DataContext.ReviewsDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //for (int i = 0; i < 101; i++) {
            //    context.Restaurants.AddOrUpdate(r => r.Name, new Restaurant() {
            //        Name = "Restaurant " + i,
            //        Address = i+" Test Ave.",
            //        City = "Philadelphia",
            //        State = "PA",
            //        Zip = "19115",
            //        Phone = string.Format("(215) 450-{0}", string.Join("", Enumerable.Repeat(i,4).ToArray()).Substring(0,4)),
            //        Web = $"http://r-{i}.test.com",
            //        Country = "USA"         


            //    });
            //}
        }
    }
}
