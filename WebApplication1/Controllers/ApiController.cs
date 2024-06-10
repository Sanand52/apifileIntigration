using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DBconn;
using WebApplication1.Models;
using WebApplication1.Models.NewFolder;


namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmpController : ControllerBase
    {
        private readonly Dbconn _db;

        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public EmpController(Dbconn db)
        {
            if (!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(_imagePath);
            }

            _db = db;
        }

        [HttpGet]
        [Route("Home/ReadData")]
        public async Task<ActionResult> ReadData()
        {
            List<EmployeeViewMdel> list = new List<EmployeeViewMdel>();

            var result = await _db.EmployeesViewMdels.FromSqlInterpolated($"GetData").ToListAsync();

            foreach (var i in result)
            {
                list.Add(new EmployeeViewMdel
                {
                    Id = i.Id, 
                    Name = i.Name,
                    Age = i.Age,
                    IMAGEId = i.IMAGEId,
                    Imagepath = i.Imagepath, 

                });

                if (i.Imagepath != null)
                {
                    var imagePath = Path.Combine(_imagePath, $"{i.Imagepath}");

                    // Check if the image file exists
                    if (System.IO.File.Exists(imagePath))
                    {
                        // Read the image file
                        var imgFile = System.IO.File.ReadAllBytes(imagePath);


                        //this code is for downloadable file.
                        //File = File(imgFile, "application/octet-stream", imagePath);//
                        
                    }
                }
            }

            

            if (list == null)
            {
                return null;
            }
            else
            {
                return Ok(list);
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
