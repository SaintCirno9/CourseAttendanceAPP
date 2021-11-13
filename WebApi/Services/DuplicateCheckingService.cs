using System.Linq;
using WebApi.DbContext;

namespace WebApi.Services
{
    public class DuplicateCheckingService
    {
        private readonly AppDbContext _context;

        public DuplicateCheckingService(AppDbContext context)
        {
            _context = context;
        }

        public bool CheckEmail(string email)
        {
            return _context.Teachers.Select(teacher => teacher.Email)
                .Concat(_context.Students.Select(student => student.Email)).All(email1 => email1 != email);
        }

        public bool CheckPhone(string phone)
        {
            return _context.Teachers.Select(teacher => teacher.PhoneNum)
                .Concat(_context.Students.Select(student => student.PhoneNum)).All(phone1 => phone1 != phone);
        }
    }
}