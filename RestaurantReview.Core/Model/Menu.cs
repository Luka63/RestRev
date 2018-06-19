using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReview.Core.Model {
    public class Menu {
        public int MenuId { get; set; }
        public List<string> FoodName { get; set; }
        public List<decimal> price { get; set; }
        public int RestaurantId { get; set; }
    }
}
