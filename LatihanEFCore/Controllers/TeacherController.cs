using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Data;
using LatihanEFCore.DTOs;
using LatihanEFCore.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using LatihanEFCore.Commons;


namespace home.mahindra.RiderProjects.LatihanEFCore.LatihanEFCore.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly IValidator<CreateTeacherDto> _createValidator;
        private readonly IValidator<UpdateTeacherDto> _updateValidator;

        public TeacherController(
            ITeacherService teacherService,
            IValidator<CreateTeacherDto> createValidator,
            IValidator<UpdateTeacherDto> updateValidator)
        {
            _teacherService = teacherService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _teacherService.GetAllTeacher();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _teacherService.GetTeacher(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [FromBody] CreateTeacherDto request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ServiceResult<TeacherDTO>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var result = await _teacherService.CreateTeacher(request);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateTeacherDto request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                var errorResponse = ServiceResult<TeacherDTO>.ErrorResult("Validation failed", errors);
                return BadRequest(errorResponse);
            }

            var result = await _teacherService.UpdateTeacher(id, request);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _teacherService.DeleteTeacher(id);
            return Ok(result);
        }
    }
}
