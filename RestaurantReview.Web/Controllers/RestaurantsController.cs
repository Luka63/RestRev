using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantReview.Core.Model;
using RestaurantReview.DB.DataContext;

namespace RestaurantReview.Web.Controllers {
    [Authorize]
    public class RestaurantsController : Controller {
        private ReviewsDb db = new ReviewsDb();

        // GET: Restaurants
        public ActionResult Index() {
            return View(db.Restaurants.ToList());
        }

        // GET: Restaurants/Details/5
        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null) {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // GET: Restaurants/Create
        public ActionResult Create() {

            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,City,State,Zip,Phone,Web,Country,ImageUrl")] Restaurant restaurant) {
            if (ModelState.IsValid) {
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant.ImageUrl != null) {
                ViewBag.ImageUrl = $"{ConfigurationManager.AppSettings["RestaurantImageUploadBase"]}/{restaurant.Id}/{restaurant.ImageUrl}";
            }

            if (restaurant == null) {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Restaurant restaurant, HttpPostedFileBase restaurantImage) {
            var baseUnc = Server.MapPath("~" + ConfigurationManager.AppSettings["restaurantImageUploadBase"]);
            baseUnc = $"{baseUnc}{restaurant.Id}\\";

            if (!Directory.Exists(baseUnc)) {
                Directory.CreateDirectory(baseUnc);
            }



            string[] validPhotoFile = { ".jpg", ".png" };
            bool isValidUpload = false;
            if (restaurantImage != null && restaurantImage.ContentLength > 0) {
                isValidUpload = validPhotoFile.Any(item => restaurantImage.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
            }
            if (isValidUpload) {
                restaurant.ImageUrl = Path.GetFileName(restaurantImage.FileName);
                restaurantImage.SaveAs(baseUnc + Path.GetFileName(restaurantImage.FileName));

            }



            if (ModelState.IsValid) {
                // restaurant.ImageUrl = Path.GetFileName(restaurantImage.FileName);

                db.Entry(restaurant).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public ActionResult Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null) {
                return HttpNotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Restaurant restaurant = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
