using AutoMapper;
using RestaurantReview.Core.Model;
using RestaurantReview.DB.DataContext;
using RestaurantReview.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace RestaurantReview.Web.Controllers {

    public class HomeController : Controller {


        public ActionResult Index(string Search) {
            using (ReviewsDb db = new ReviewsDb()) {

                // var Restaurant = db.Restaurants.ToList();

                var rest = db.Restaurants.Where(p=>p.Name.Contains(Search) || Search ==null).Include(r => r.Reviews).ToList();


                var vm = Mapper.Map<List<Restaurant>, List<RestaurantListViewModel>>(rest);

                if (Request.IsAjaxRequest()) {
                    return PartialView("_Restaurants",vm); //(View,Data)
                }
                return View(vm);

                #region 1000 restaurants
                /*
                 List<RestaurantListViewModel> RestaurantVm = new List<RestaurantListViewModel>();
                 
                      foreach (var item in Restaurants) {
                      RestaurantVm.Add(new RestaurantListViewModel {
                          Name = item.Name,
                          Id = item.Id,
                          ImageUrl = item.ImageUrl,
                          Address = item.Address,
                          City = item.City,
                          State = item.State,
                          Zip = item.Zip,
                          Phone = item.Phone,
                          Web = item.Web,
                          AverageScore = item.Reviews.Any() ? item.Reviews.Average(p => p.Rating) : 0,
                          CountOfReviews = item.Reviews.Count()

                      }); //WORKS!!!! do for all props
                  }*/
                #endregion
            }


        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}