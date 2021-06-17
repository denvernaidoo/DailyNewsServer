using DailyNewsServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DailyNewsServer.Core.Interfaces
{
    public interface IStudentService
    {
        List<Student> GetAll();
        Task<List<Student>> GetAllAsync();
        Student Get(int id);
        Task<Student> GetAsync(int id);
    }
}
