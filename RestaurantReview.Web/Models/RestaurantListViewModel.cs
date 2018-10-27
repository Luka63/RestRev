using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Web.Models
{
    public class RestaurantListViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }

        [Required]
        [Range(1, 5)]
        public string Zip { get; set; }

        [Required]
        [Range(10, 12)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    
        public string Web { get; set; }

        [Required]
        public string Country { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public int CountOfReviews { get; set; }        
        public double AverageScore { get; set; }

    }
}