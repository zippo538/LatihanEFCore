using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.Mapping
{
    public class TeacherMappingProfile : Profile
    {
        public TeacherMappingProfile()
        {
             CreateMap<Teacher, TeacherDTO>()
            .ForMember(
                dest => dest.IdCourse,
                opt => opt.MapFrom(src => src.idCourse));

        CreateMap<CreateTeacherDto, Teacher>();

        CreateMap<UpdateTeacherDto, Teacher>();
        }
    }
}