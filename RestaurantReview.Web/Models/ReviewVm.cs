using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantReview.Web.Models
{
    public class ReviewVm
    {

        public int Id { get; set; }
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(512)]
        public string Body { get; set; }

        public string RestaurantName { get; set; }

        [Display(Name = "User Name")]
        [DisplayFormat(NullDisplayText = "anonymus")]
        public string ReviewerName { get; set; }

        public int RestaurantId { get; set; }


       // public DateTime ReviewDate { get; set; }
       // public Restaurant Restaurant { get; set; }
    }
}