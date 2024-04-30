using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces.Authentication
{
    public interface IPasswordHashService
    {
        string HashPassword(string password);
        bool VerifyHashPassword(string password, string hashedPassword);
    }
}