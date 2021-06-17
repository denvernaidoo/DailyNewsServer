using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyNewsServer.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public string Username { 
            get 
            {
                return User.Identity.Name;
            }
        }
    }
}
