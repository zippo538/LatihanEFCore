using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LatihanEFCore.DTOs;
using LatihanEFCore.DTO.Responses;

namespace LatihanEFCore.Mapping
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
    {
        //get
        CreateMap<Course, CourseDto>()
            .ForMember(
                destination => destination.Title,
                options => options.MapFrom(source => source.Title ?? string.Empty))
            .ForMember(
                destination => destination.Description,
                options => options.MapFrom(source => source.Description ?? string.Empty));
    }
    }
}