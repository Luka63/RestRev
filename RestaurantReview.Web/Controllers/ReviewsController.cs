using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RestaurantReview.Core.Model;
using RestaurantReview.DB.DataContext;
using RestaurantReview.Web.Helpers;
using RestaurantReview.Web.Models;

namespace RestaurantReview.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private ReviewsDb db = new ReviewsDb();

        // GET: Reviews
        public ActionResult Index(int Id)
        {
            //ViewBag.Name = db.Restaurants.Find(Id).Name.ToString();
            var restaurant = db.Restaurants.Include(r => r.Reviews).FirstOrDefault(r => r.Id == Id);
            return View(restaurant);
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Review review = db.Reviews.Include(p => p.Restaurant).FirstOrDefault(p => p.Id == id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ReviewVm vm = new ReviewVm();
            vm.Rating = review.Rating;
            vm.Body = review.Body;
            vm.ReviewerName = review.ReviewerName;
            vm.RestaurantName = review.Restaurant.Name;

            return View(vm);
        }

        // GET: Reviews/Create
        public ActionResult Create(int Id)
        {
            var r = db.Restaurants.Find(Id);

            var review = new ReviewVm();
            review.RestaurantId = r.Id;
            review.RestaurantName = r.Name;
            return View(review);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;
                if (Request.IsAuthenticated)
                {
                    review.ReviewerName = User.Identity.GetUserName();
                }

                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = review.RestaurantId });
            }

            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Manage(int? id, int? restaurantId)
        {
            var review = new ReviewVm();
            if (restaurantId.HasValue)
            {
                var r = db.Restaurants.Find(restaurantId);
                review.RestaurantId = r.Id;
                review.RestaurantName = r.Name;
            }
            else
            {
                var entity = db.Reviews.Include(p => p.Restaurant).Where(p => p.Id == id).FirstOrDefault();
                review.RestaurantId = entity.Restaurant.Id;
                review.RestaurantName = entity.Restaurant.Name;
                review.Id = entity.Id;
                review.Body = entity.Body;
                review.Rating = entity.Rating;
                review.ReviewerName = entity.ReviewerName;
                
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(Review review) // merge create and edit
        {
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;
                if (Request.IsAuthenticated)
                {
                    review.ReviewerName = User.Identity.GetUserName();
                }
                if (review.Id == 0)
                {
                    db.Reviews.Add(review);
                }
                else
                {
                    var entity = db.Reviews.Find(review.Id);
                    entity.Body = review.Body;
                    entity.Rating = review.Rating;
                }

               
                db.SaveChanges();
                return RedirectToAction("Index", new { id = review.RestaurantId } );
            }
            return View(review);
        }

        [ChildActionOnly]
        public ActionResult Review(ReviewActionVm model) {
            IQueryable<Review> review = db.Reviews;
            if (model.RestaurantId.HasValue) {
                review = review.Where(p => p.RestaurantId == model.RestaurantId.Value);
            }
            if (model.Type == Enums.ReviewDisplayType.Best) {
                model.Review = review.OrderByDescending(p => p.Rating).FirstOrDefault();
                model.Title = "best review";
            }
            else if (model.Type == Enums.ReviewDisplayType.Worse) {
                model.Review = review.OrderBy(p => p.Rating).ThenByDescending(p => p.ReviewDate).FirstOrDefault();
                model.Title = "worst rev";
            }
            else {
                model.Review = review.OrderByDescending(p => p.ReviewDate).FirstOrDefault();
                model.Title = "latest rev";
            }
            return PartialView("_Review", model);
        }
        
        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
