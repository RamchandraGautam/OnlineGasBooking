using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace OnlineGasBooking.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Reports
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> StocksReport()
        {
            try
            {
                string connectionString = @"Data Source=.;Initial Catalog=OnlineGasDB;Integrated Security=True";
                using (var con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    var command = new SqlCommand("SELECT * FROM Products p INNER JOIN Categories c ON c.CategoryID = p.CategoryID INNER JOIN Suppliers s ON s.SupplierID = p.SupplierID", con);
                    var da = new SqlDataAdapter(command);
                    var ds = new DataSet();
                    da.Fill(ds, "Products");

                    var rptPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "Stocks.rpt");
                    using (var rd = new ReportDocument())
                    {
                        rd.Load(rptPath);
                        rd.SetDataSource(ds);

                        using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
                        {
                            return File(stream, "application/pdf");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Content($"<h2>Error: {ex.Message}</h2>", "text/html");
            }
        }

        public async Task<IActionResult> CustomersReport()
        {
            try
            {
                string connectionString = @"Data Source=.;Initial Catalog=OnlineGasDB;Integrated Security=True";
                using (var con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    var command = new SqlCommand("SELECT * FROM Customers", con);
                    var da = new SqlDataAdapter(command);
                    var ds = new DataSet();
                    da.Fill(ds, "Customers");

                    var rptPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "Customers.rpt");
                    using (var rd = new ReportDocument())
                    {
                        rd.Load(rptPath);
                        rd.SetDataSource(ds);

                        using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
                        {
                            return File(stream, "application/pdf");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Content($"<h2>Error: {ex.Message}</h2>", "text/html");
            }
        }

        public async Task<IActionResult> SalesReport()
        {
            try
            {
                string connectionString = @"Data Source=.;Initial Catalog=OnlineGasDB;Integrated Security=True";
                using (var con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();
                    var command = new SqlCommand("SELECT * FROM OrderDetails od INNER JOIN Orders o ON o.OrderID = od.OrderID INNER JOIN Products p ON p.ProductID = od.ProductID", con);
                    var da = new SqlDataAdapter(command);
                    var ds = new DataSet();
                    da.Fill(ds, "OrderDetails");

                    var rptPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "Sales.rpt");
                    using (var rd = new ReportDocument())
                    {
                        rd.Load(rptPath);
                        rd.SetDataSource(ds);

                        using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
                        {
                            return File(stream, "application/pdf");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Content($"<h2>Error: {ex.Message}</h2>", "text/html");
            }
        }
    }
}
