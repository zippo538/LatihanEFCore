using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;
using LatihanEFCore.DTO.Responses;
using LatihanEFCore.DTO.Responses.DTOs;

namespace LatihanEFCore.Mapping
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
    {
        //get
        CreateMap<Course, CourseDTO>()
            .ForMember(
                destination => destination.Title,
                options => options.MapFrom(source => source.Title ?? string.Empty))
            .ForMember(
                destination => destination.Description,
                options => options.MapFrom(source => source.Description ?? string.Empty));
    }
    }
}