using API_NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_NET.Services
{
    public interface IUserApiServices
    {
        UserApi UserSeek(LoginApi loginApi);
    }
}
