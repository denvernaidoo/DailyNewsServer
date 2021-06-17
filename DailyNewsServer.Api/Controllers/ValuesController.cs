using DailyNewsServer.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyNewsServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseController
    {
        [HttpGet]
        [Route("[action]")]
        public ActionResult<IEnumerable<string>> PublicValues()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<string>> AdminValues()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "Agent")]
        public ActionResult<IEnumerable<string>> AgentValues()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
