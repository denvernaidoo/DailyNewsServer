using DailyNewsServer.Core.Interfaces;
using DailyNewsServer.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyNewsServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            return await _studentService.GetAllAsync();
        }

        // GET api/students/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            return await _studentService.GetAsync(id);
        }
    }
}
