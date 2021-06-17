using DailyNewsServer.Core.Interfaces;
using DailyNewsServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyNewsServer.Core.Services
{   

    public class StudentService : IStudentService
    {
        private IGenericRepository<Student> _studentRepository;

        public StudentService(IGenericRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public List<Student> GetAll()
        {
            return _studentRepository.All().ToList();
        }

        public async Task<List<Student>> GetAllAsync()
        {
            var students =  await _studentRepository.AllAsync();
            return students.ToList();
        }

        public Student Get(int id)
        {
            return _studentRepository.FindByKey(id);
        }

        public async Task<Student> GetAsync(int id)
        {
            return await _studentRepository.FindByKeyAsync(id);
        }
    }
}
