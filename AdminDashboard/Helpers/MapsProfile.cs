using AdminDashboard.Models;
using AutoMapper;
using Ecommerce.Core.Entities;

namespace AdminDashboard.Helpers
{
    public class MapsProfile : Profile
    {
        public MapsProfile()
        {
            CreateMap<Product , ProductViewModel>().ReverseMap();
        }
    }
}
