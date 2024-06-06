using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DBconn;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private readonly Dbconn _db;

        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public ImageController(Dbconn db)
        {
            if (!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(_imagePath);
            }

            _db = db;
        }

        [HttpPost]
        [Route("upload{id:int}")]
        public async Task<HttpResponseMessage> UploadImage(IFormFile e, int id)
        {
            if (e != null || e.Length != 0)
            {

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(e.FileName);


                Employee emp = await _db.Employees.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (emp != null)
                {
                    ImageEmp x = await _db.Images.FirstOrDefaultAsync(a => a.Employee.Id == id);

                    if (x != null)
                    {
                        var existingImagePath = Path.Combine(_imagePath, $"{x.Imagepath}");

                        using (var stream = new FileStream(existingImagePath, FileMode.Create))
                        {
                            await e.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        var filePath = Path.Combine(_imagePath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await e.CopyToAsync(stream);
                        }

                    }

                    await _db.Database.ExecuteSqlAsync($"AddIMG {id},{fileName}");

                }

            }

            HttpResponseMessage responseMessage = new HttpResponseMessage();

            return responseMessage;/* (new { FileName = fileName, FilePath = $"/images/{fileName}" })*/

        }

        [HttpDelete]
        [Route("delete{id:int}")]
        public async Task<HttpResponseMessage> DeleteImage(int id)
        {
            ImageEmp x = await _db.Images.FirstOrDefaultAsync(a => a.Employee.Id == id);

            if (x != null)
            {
                var existingImagePath = Path.Combine(_imagePath, $"{x.Imagepath}");


                //check if image file exist.
                if (System.IO.File.Exists(existingImagePath))
                {
                    // Delete the image file
                    System.IO.File.Delete(existingImagePath);

                    // Update database record (if applicable)

                    await _db.Database.ExecuteSqlRawAsync($"DeleteIMG {id}");

                    HttpResponseMessage responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    return responseMessage;
                }
                else
                {

                    HttpResponseMessage responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);

                    return responseMessage;
                }

            }
            else
            {

                HttpResponseMessage responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);

                return responseMessage;
            }

        }

        [HttpGet]
        [Route("ReadImage{id:int}")]
        public async Task<IActionResult> ReadImage(int id)
        {
            ImageEmp x = await _db.Images.FirstOrDefaultAsync(a => a.Employee.Id == id);

            if (x != null)
            {
                var imagePath = Path.Combine(_imagePath, $"{x.Imagepath}");

                // Check if the image file exists
                if (System.IO.File.Exists(imagePath))
                {
                    // Read the image file
                    var Image = System.IO.File.ReadAllBytes(imagePath);

                    // Return the image as a file response
                    // return the image file
                    return File(Image, "image/jpeg");
                }
                else
                {
                    return NotFound("Image not found");
                }


            }
            else
            {

                return NotFound("Employee detail not found");
            }



        }

    }


}
