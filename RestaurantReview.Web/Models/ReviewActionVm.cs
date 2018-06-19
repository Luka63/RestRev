using RestaurantReview.Core.Model;
using RestaurantReview.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Web.Models {
    public class ReviewActionVm {

        public int? RestaurantId { get; set; }
        public string Title { get; set; }
        public Enums.ReviewDisplayType Type { get; set; }
        public Review Review { get; set; }
    }
}