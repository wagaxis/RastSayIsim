using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ElpoDockerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {



        //public TestData Get()
        //{
        //    var rng = new Random();
        //    return new TestData()
        //    {
        //        RamdomNumber = rng.Next(-20, 55),
        //        Date = DateTime.Now,
        //        RandomData = Guid.NewGuid()
        //    };
        //} 

        [HttpGet]
        public TestData Get()
        {
            var d = new TestDockerModel();
            var res = d.People.OrderBy(i => Guid.NewGuid()).FirstOrDefault();
            return new TestData()
            {
                RamdomNumber = new Random().Next(1, int.MaxValue),
                Data = res,
                Date = DateTime.Now,
                Server = Environment.MachineName + " / " + Environment.UserName,

            };
        }
    }
}
