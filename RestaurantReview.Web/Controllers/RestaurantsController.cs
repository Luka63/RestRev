using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using RestaurantReview.Core.Model;
using RestaurantReview.DB.DataContext;
using RestaurantReview.Web.Models;

namespace RestaurantReview.Web.Controllers {
    [Authorize] //prevents accesing this controller without autorization
    public class RestaurantsController : Controller {
        private ReviewsDb db = new ReviewsDb();

        // GET: Restaurants
        // [Route("Restaurants/Index/{Search}")]
        public ActionResult Index(string Search){
            // var Restaurant = db.Restaurants.ToList();
            var rest = db.Restaurants.Where(p => p.Name.Contains(Search) || Search == null).Include(r => r.Reviews).ToList();
            //   var rest = db.Restaurants.Where(p => p.Name.Contains(Search) || p.Reviews.Count > 0).Include(r => r.Reviews).ToList();
            // string number = "";
            //     var asdf = db.Restaurants.Where(p => p.Name.Contains(Search) || Search == null && p.Phone.Contains(number) || number == null).ToList();
            //                      Source              Destination
            var vm = Mapper.Map<List<Restaurant>, List<RestaurantListViewModel>>(rest); //mapping   

            if (Request.IsAjaxRequest()) {
                return PartialView("_Restaurants", vm); //(View,Data)
            }



            return View(vm);

        }
        public ActionResult MenuProfile(string tab) {
            var ProfileMenu = new ProfileMenu();
            switch (tab) {
                case "general":
                    ProfileMenu.Category = "_general";
                    break;
                case "general2":
                    ProfileMenu.Category = "_general";
                    return PartialView(ProfileMenu.Category, ProfileMenu);
                case "history": { ProfileMenu.Category = "_history"; ProfileMenu.name = "3333"; }
                    break;
                default:
                    ProfileMenu.Category = "_general";
                    break;
            }

            if (Request.IsAjaxRequest()) {
                return RedirectToAction("part", "Restaurants", new { ProfileMenu.Category, ProfileMenu });
            }
            return View(ProfileMenu);
        }
        public ActionResult part(string tab, string name) {

            var ProfileMenu = new ProfileMenu();
            switch (tab) {
                case "general":
                    ProfileMenu.Category = "_general";
                    break;
                case "history": { ProfileMenu.Category = "_history"; ProfileMenu.name = !string.IsNullOrEmpty(name) ? name : "333"; }
                    break;
                default:
                    ProfileMenu.Category = "_general";
                    break;
            }
            return PartialView(ProfileMenu.Category, ProfileMenu);
        }

        public ActionResult Movies() {
            var movies = new List<object>();

            movies.Add(new { Title = "Ghostbusters", Genre = "Comedy", Year = 1984 });
            movies.Add(new { Title = "Gone with Wind", Genre = "Drama", Year = 1939 });
            movies.Add(new { Title = "Star Wars", Genre = "Science Fiction", Year = 1977 });

            return Json(movies, JsonRequestBehavior.AllowGet);
        }

        // GET: Restaurants/Details/5
        [Route("Restaurants/Details/{id}")]
        public ActionResult Details(int? id, Boolean JSON = false) {



            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);

            if (restaurant == null) {
                return HttpNotFound();
            }
            if (JSON) {
                return Json(restaurant, JsonRequestBehavior.AllowGet);
            }


            HttpCookie aCookie = new HttpCookie("restaurant"); //creating cookie with name "restaurant"
            aCookie.Values["id"] = id.ToString(); //adding subkey "id" with value "id.ToString();"
            aCookie.Values["name"] = restaurant.Name; //adding subkey "name" with value "restaurant.name" 
            aCookie.Expires = DateTime.Now.AddDays(1); //adding expire date 
            Response.Cookies.Add(aCookie); // sending a cookie

            StringBuilder output = new System.Text.StringBuilder();
            for (int i = 0; i < Request.Cookies.Count; i++) {
                aCookie = Request.Cookies[i];
                output.Append("Cookie name = " + Server.HtmlEncode(aCookie.Name)
                    + "<br />");
                output.Append("Cookie value = " + Server.HtmlEncode(aCookie.Value)
                    + "<br /><br />");
            }
            string cookie = output.ToString();

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
