using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RestaurantReview.Web.Helpers {
    public static class HtmlHelperExtensions {
        public static HtmlString Ratings(this HtmlHelper helper, double rating) {
            if (rating > 0) {
                StringBuilder sb = new StringBuilder();
                rating = Math.Round(rating * 2, MidpointRounding.AwayFromZero);
                sb.AppendFormat("<img src='/images/stars{0}.gif' />", (rating / 2) * 10);
                return new HtmlString(sb.ToString()); 
            }
            else {
                return new HtmlString("");
            }
        }
    }
}