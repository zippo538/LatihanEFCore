using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Models;
using LatihanEFCore.DTO.Responses.DTOs;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.DTO.Responses.Mapping
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            // Mapping Organization Tuition Course ActivityPoints
            CreateMap<Organization, OrganizeDTO>();
            CreateMap<Tuition, TuitionDTO>();
            CreateMap<Course, CourseDTO>();
            CreateMap<ActivityPoints, ActivityPointDTO>();

            // Entity Student -> StudentDTO
            CreateMap<Student, StudentDTO>()
                .ForMember(
                    destination => destination.Address,
                    options => options.MapFrom(
                        source => source.Address ?? string.Empty))
                .ForMember(
                    destination => destination.ActivityPoints,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.Tuitions,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.Courses,
                    options => options.Ignore());

            // Create DTO -> Student
            CreateMap<CreateStudentDTO, Student>()
                .ForMember(
        destination => destination.IdStudent,
        options => options.Ignore())
    .ForMember(
        destination => destination.EnrollmentDate,
        options => options.Ignore())
    .ForMember(
        destination => destination.GPA,
        options => options.Ignore())
    .ForMember(
        destination => destination.IdOrganization,
        options => options.MapFrom(source => source.IdOrganization))
    .ForMember(
        destination => destination.ActivityPoints,
        options => options.Ignore())
    .ForMember(
        destination => destination.Tuitions,
        options => options.Ignore())
    .ForMember(
        destination => destination.Courses,
        options => options.Ignore());

            // Update DTO -> Student
            CreateMap<UpdateStudentDTO, Student>()
                .ForMember(
                    destination => destination.IdStudent,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.IdOrganization,
                    options => options.MapFrom(source => source.IdOrganization))
                .ForMember(
                    destination => destination.ActivityPoints,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.Tuitions,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.Courses,
                    options => options.Ignore());
        }
    }
}