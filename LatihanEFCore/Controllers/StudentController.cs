using LatihanEFCore.DTO.Responses.DTOs;
using LatihanEFCore.DTOs;
using LatihanEFCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Controllers
{

    [ApiController]
    [Route("api/students")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
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
        public async Task<IActionResult> Create([FromBody] CreateStudentDTO request)
        {
            var student = await _studentService.CreateStudent(request);

            return student.Success
                ? StatusCode(201, student)
                : Conflict(student);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateStudentDTO request)
        {
            var student = await _studentService.UpdateStudent(id, request);

            return student.Success
                ? Ok(student)
                : BadRequest(student);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _studentService.DeleteStudent(id);

            return response.Success
                ? Ok(response)
                : NotFound(response);
        }
    }
}
