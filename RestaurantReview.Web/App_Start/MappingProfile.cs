using RestaurantReview.Core.Model;
using RestaurantReview.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantReview.Web.App_Start {
    public class MappingProfile : AutoMapper.Profile {
        public MappingProfile() {

      //      CreateMap<Restaurant, RestaurantListViewModel>();

            CreateMap<Restaurant, RestaurantListViewModel>()
                .ForMember(p => p.CountOfReviews, ex => ex.MapFrom(p => p.Reviews.Count))
                .ForMember(p => p.AverageScore, ex => ex.MapFrom(r => r.Reviews.Any() ? Math.Round(r.Reviews.Average(rv => rv.Rating), 1) : 0));

            CreateMap<Review, ReviewVm>().ForMember(source=>source.ReviewerName,dest=>dest.MapFrom(source=>source.ReviewerName.ToUpper()) );
        }
    }
}