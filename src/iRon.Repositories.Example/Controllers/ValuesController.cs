using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iRon.Core.Interfaces;
using iRon.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace iRon.Repositories.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IiRonRepository<UserEntity> repository;
        public ValuesController(IiRonRepository<UserEntity> repository)
        {
            this.repository = repository;
        }

        // GET api/values
        [HttpGet]
        public  IEnumerable<UserEntity> Get()
        {
            //repository.SaveAsync(new UserEntity() {
            //    Name="WWWWWW",
            //    Password="XXXXXX"
            //});

            return repository.GetAllAsync().Result;

        }

   
    }
}
