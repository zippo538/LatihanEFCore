using LatihanEFCore.DTOs;
using LatihanEFCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using LatihanEFCore.Commons;
using LatihanEFCore.DTO.Responses;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Controllers
{

    [ApiController]
    [Route("api/students")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IValidator<CreateStudentDto> _createValidator;
        private readonly IValidator<UpdateStudentDTO> _updateValidator;

        public StudentController(
            IStudentService studentService,
            IValidator<CreateStudentDto> createValidator,
            IValidator<UpdateStudentDTO> updateValidator
            )
        {
            _studentService = studentService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _studentService.GetAllStudents();

            return response.Success
                ? Ok(response)
                : StatusCode(500, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetStudent(id);

            return student.Success
                ? Ok(student)
                : NotFound(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ServiceResult<StudentDTO>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var student = await _studentService.CreateStudent(request);


            return student.Success
                ? StatusCode(201, student)
                : Conflict(student);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateStudentDTO request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ServiceResult<StudentDTO>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var student = await _studentService.UpdateStudent(id, request);

            return student.Success
                ? Ok(student)
                : BadRequest(student);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _studentService.DeleteStudent(id);

            return response.Success
                ? Ok(response)
                : NotFound(response);
        }
    }
}
