using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Salon.Data;
using Salon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Group = Salon.Models.Group;

namespace Salon.Controllers
{
    public class OtchetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OtchetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OtchetController
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(IFormFile fileExcel)
        {
            using (XLWorkbook workbook = new XLWorkbook(fileExcel.OpenReadStream()))
            {
                List<Group_ImpExp> Group_ImpExps = new List<Group_ImpExp>();
                List<Service_ImpExp> Service_ImpExps = new List<Service_ImpExp>();

                foreach (IXLWorksheet worksheet in workbook.Worksheets)
                {
                    if (worksheet.Name == "Group")
                    {
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            Group groups = new Group();
                            var range = worksheet.RangeUsed();

                            var table = range.AsTable();

                            groups.GroupName = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "GroupName").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            groups.Description = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Description").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            groups.Services_Count = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Services_Count").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            _context.Groups.Add(groups);

                            _context.SaveChanges();

                            Group_ImpExps.Add(new Group_ImpExp { GroupSubd = groups.GroupId, GroupExcel = int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "GroupId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString()) }); ;
                        }
                    }

                    if (worksheet.Name == "Service")
                    {
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            Service services = new Service();
                            var range = worksheet.RangeUsed();

                            var table = range.AsTable();

                            services.ServiceName = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "ServiceName").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            services.GroupId = Group_ImpExps.FirstOrDefault(c => c.GroupExcel == int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "GroupId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString())).GroupSubd;
                            services.ProductionCost = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "ProductionCost").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            services.Price = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Price").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            services.Description = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Description").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();

                            _context.Services.Add(services);

                            _context.SaveChanges();

                            Service_ImpExps.Add(new Service_ImpExp { ServiceSubd = services.ServiceId, ServiceExcel = int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "ServiceId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString()) }); ;
                        }
                    }

                    if (worksheet.Name == "Visit")
                    {
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            Visit visits = new Visit();
                            var range = worksheet.RangeUsed();

                            var table = range.AsTable();

                            visits.CustomerId = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "CustomerId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            visits.ServiceId = Service_ImpExps.FirstOrDefault(c => c.ServiceExcel == int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "ServiceId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString())).ServiceSubd;
                            visits.EmployeeId = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "EmployeeId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            visits.VisitDate = DateTime.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "VisitDate").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());

                            _context.Visits.Add(visits);

                            _context.SaveChanges();
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: OtchetController/Details/5
        public ActionResult Export(int? id)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Otchet");
                worksheet.Cell(1, 1).Value = "Услуга";
                worksheet.Cell(1, 2).Value = "Количество посещений";

                worksheet.Row(1).Style.Font.Bold = true;

                var otch = _context.Set<Visit_CountOtchet>().FromSqlInterpolated($"EXEC Otchet").ToList();
                int i = 2;
                foreach (Visit_CountOtchet item in otch)
                {
                    worksheet.Cell(i, 1).Value = item.nm;
                    worksheet.Cell(i, 2).Value = item.kol;
                    i++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreedsheetml.sheet")
                    {
                        FileDownloadName = $"Otchet_{DateTime.UtcNow.ToLongDateString()}.xlsx"
                    };
                }
            }
        }

        public ActionResult Export2()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet1 = workbook.Worksheets.Add("Group");
                worksheet1.Cell(1, 1).Value = "GroupId";
                worksheet1.Cell(1, 2).Value = "GroupName";
                worksheet1.Cell(1, 3).Value = "Description";
                worksheet1.Cell(1, 4).Value = "Services_Count";

                int i1 = 2;

                worksheet1.Row(1).Style.Font.Bold = true;

                foreach (Group item in _context.Groups)
                {
                    worksheet1.Cell(i1, 1).Value = item.GroupId;
                    worksheet1.Cell(i1, 2).Value = item.GroupName;
                    worksheet1.Cell(i1, 3).Value = item.Description;
                    worksheet1.Cell(i1, 4).Value = item.Services_Count;

                    i1++;
                }

                var worksheet2 = workbook.Worksheets.Add("Service");
                worksheet2.Cell(1, 1).Value = "ServiceId";
                worksheet2.Cell(1, 2).Value = "ServiceName";
                worksheet2.Cell(1, 3).Value = "GroupId";
                worksheet2.Cell(1, 4).Value = "ProductionCost";
                worksheet2.Cell(1, 5).Value = "Price";
                worksheet2.Cell(1, 6).Value = "Description";

                int i2 = 2;

                worksheet2.Row(1).Style.Font.Bold = true;

                foreach (Service item in _context.Services)
                {
                    worksheet2.Cell(i2, 1).Value = item.ServiceId;
                    worksheet2.Cell(i2, 2).Value = item.ServiceName;
                    worksheet2.Cell(i2, 3).Value = item.GroupId;
                    worksheet2.Cell(i2, 4).Value = item.ProductionCost;
                    worksheet2.Cell(i2, 5).Value = item.Price;
                    worksheet2.Cell(i2, 6).Value = item.Description;

                    i2++;
                }

                var worksheet = workbook.Worksheets.Add("Visit");
                worksheet.Cell(1, 1).Value = "VisitId";
                worksheet.Cell(1, 2).Value = "CustomerId";
                worksheet.Cell(1, 3).Value = "ServiceId";
                worksheet.Cell(1, 4).Value = "EmployeeId";
                worksheet.Cell(1, 5).Value = "VisitDate";

                worksheet.Row(1).Style.Font.Bold = true;

                int i = 2;
                foreach (Visit item in _context.Visits)
                {
                    worksheet.Cell(i, 1).Value = item.VisitId;
                    worksheet.Cell(i, 2).Value = item.CustomerId;
                    worksheet.Cell(i, 3).Value = item.ServiceId;
                    worksheet.Cell(i, 4).Value = item.EmployeeId;
                    worksheet.Cell(i, 5).Value = item.VisitDate;

                    i++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreedsheetml.sheet")
                    {
                        FileDownloadName = $"Export_{DateTime.UtcNow.ToLongDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
