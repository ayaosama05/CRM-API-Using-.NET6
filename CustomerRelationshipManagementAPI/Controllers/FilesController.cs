
using ClosedXML.Excel;
using CustomerRelationshipManagementAPI.Core.Models;
using CustomerRelationshipManagementAPI.Data;
using CustomerRelationshipManagementAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerRelationshipManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =UserRoles.Admin)]
    public class FilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FilesController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private bool CheckIfExcelFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
                            //"." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".xlsx" || extension == ".xls");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            if (CheckIfExcelFile(file))
            {
                var result = await WriteToFileAsync(file, cancellationToken);
                return result.Status ? Ok(result.Message) : BadRequest(result.Message);
            }

            return BadRequest(new { message = "Invalid file extension" });
        }

        private async Task<UploadFile> SaveFileAsync(IFormFile file)
        {
            try
            {
                var fileextension = Path.GetExtension(file.FileName);
                //Create new file Name due to security reasons.
                var filename = Guid.NewGuid().ToString() + fileextension;
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Files");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filepath = Path.Combine(folderPath, filename);

                using (FileStream fileStream = new(filepath, FileMode.Create, FileAccess.ReadWrite))
                {
                    await file.CopyToAsync(fileStream);
                };

                return new UploadFile { Status = true , Message = "File Saved Successfully" , filePath = filepath};
            }
            catch
            {
                return new UploadFile { Status = false, Message = "Can't save file an error has been occured !"};
            }
        }
        private Customer ReadExcelRow(IXLRow row)
        {
            return new Customer
            {
                FirstName = row.Cell(1).Value.ToString() ?? "",
                LastName = row.Cell(2).Value.ToString() ?? "",
                PhoneNumber = row.Cell(3).Value.ToString() ?? "",
                Email = row.Cell(4).Value.ToString() ?? "",
                Birthdate = !string.IsNullOrEmpty(row.Cell(5).Value.ToString()) ? Convert.ToDateTime(row.Cell(5).Value.ToString()) : null,
            };
        }
        private async Task<UploadFile> ReadExcelFileAsync(string filepath)
        {
            try
            {
                int failedRows = 0;int totalRows = 0;
                var customersList = new List<Customer>();
                // Opening the stream and reading it back.
                using (FileStream fs = new(filepath, FileMode.Open, FileAccess.Read))
                {
                    int rowno = 1;

                    using (var workbook = new XLWorkbook(filepath))
                    {
                        var sheets = workbook.Worksheets.First();
                        var rows = sheets.Rows().ToList();
                        totalRows = rows.Count() -1; //exclude header

                        foreach (var row in rows)
                        {
                            if (rowno != 1)
                            {
                                var cell_fname = row.Cell(1).Value.ToString();
                                var cell_PhoneNumber = row.Cell(3).Value.ToString();
                                var cell_Email = row.Cell(4).Value.ToString();

                                string?[] cells = new string?[] { cell_fname, cell_Email, cell_PhoneNumber };
                                if(cells.Any(s => string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s)))
                                {
                                    failedRows++;
                                    continue;
                                }

                                try
                                {
                                    Customer newCustomer = ReadExcelRow(row);
                                    _context.Customers.Add(newCustomer);
                                }
                                catch
                                {
                                    failedRows++;
                                }
                            }
                            else
                            {
                                rowno = 2;
                            }
                        }

                    }
                }
                await _context.SaveChangesAsync();
                return new UploadFile { Status = true, Message = $"{totalRows - failedRows} has beeen uploaded successfully from total {totalRows} rows" };
            }
            catch (Exception ex)
            {
                return new UploadFile {  Status= false , Message = ex.Message};
            }
        }

        private async Task<UploadFile> WriteToFileAsync(IFormFile file, CancellationToken cancellationToken)
        {
            UploadFile result = new UploadFile { Status = false };

            if (!cancellationToken.IsCancellationRequested)
            {
                result = await SaveFileAsync(file);
                if (result.Status)
                    result = await ReadExcelFileAsync(result.filePath);
            }
            else
            {
                result.Message = "Request has been cancelled !";
            }
            return result;
        }
    }
}
