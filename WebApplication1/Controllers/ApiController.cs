using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebApplication1.DBconn;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmpController : ControllerBase
    {
        private readonly Dbconn _db;

        public EmpController(Dbconn db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("Home/ReadData")]
        public async Task<List<Employee>> ReadData()
        {
            
            var response = await _db.Employees.FromSqlRaw($"GetData").ToListAsync();


            if (response == null)
            {
                return null;
            }
            else
            {
                return response;
            }

        }

        [HttpPost]
        [Route("Home/AddEmp")]
        public async Task<HttpResponseMessage> AddData([Bind("Id,Name,Age")] Employee emp)
        {
            if (ModelState.IsValid)
            {
                await _db.Database.ExecuteSqlRawAsync($"AddEmp {emp.Id},{emp.Name},{emp.Age}");
            }



            HttpResponseMessage Add = new HttpResponseMessage();

            return Add;
        }

        [HttpPut]
        [Route("Home/Update")]
        public async Task<HttpResponseMessage> UpdateData([Bind("Id,Name,Age")] Employee emp)
        {
            if (ModelState.IsValid)
            {
                await _db.Database.ExecuteSqlRawAsync($"UpdateEmp {emp.Id},{emp.Name},{emp.Age}");
            }
            HttpResponseMessage Update = new HttpResponseMessage();

            return Update;
        }

        [HttpDelete]
        [Route("Home/Delete")]
        public async Task<HttpResponseMessage> DeleteData(int id)
        {
            await _db.Database.ExecuteSqlRawAsync($"DeleteEmp {id}");
            HttpResponseMessage Delete = new HttpResponseMessage();
            return Delete;
        }

        [HttpGet]
        [Route("Home/GetEmp{id:int}")]
        public async Task<ActionResult<Employee>> GetEmp(int id)
        {

            var Result = await _db.Employees.FindAsync(id);
            if (Result != null)
            {
                if (ModelState.IsValid)
                {
                    return Result;
                }

            }

            return NotFound("No data Available");
        }


        


    }

    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
