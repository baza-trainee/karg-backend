using AutoMapper;
using karg.BLL.DTO.Animals;
using karg.DAL.Models;
using karg.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Profiles
{
    public class AnimalProfile : Profile
    {
        public AnimalProfile()
        {
            CreateMap<Animal, AnimalDTO>();
            CreateMap<CreateAndUpdateAnimalDTO, Animal>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse(typeof(AnimalCategory), src.Category)))
                .ForMember(dest => dest.Images, opt => opt.Ignore());
        }
    }
}
