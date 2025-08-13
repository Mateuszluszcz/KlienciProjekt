using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication2.Models;
using PESEL;
using PESEL.Models;
using PESEL.Validators.Impl;
using System.Reflection.Metadata.Ecma335;
using ClosedXML.Excel;
using System.Text;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var klienci = await _context.Klienci.OrderBy(k => k.ID).ToListAsync();
                return View(klienci);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving data from database");
                return StatusCode(500, "Internal server error.");
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Klienci klient)
        {
            if (!ModelState.IsValid)
            {
                return View(klient);
            }

            if (klient.PESEL.Length >= 11)
            {
                var yearPart = klient.PESEL.Substring(0, 2);
                var monthPart = klient.PESEL.Substring(2, 2);
                var genderDigit = int.Parse(klient.PESEL.Substring(9, 1));

                int year = int.Parse(yearPart);
                int month = int.Parse(monthPart);

                if (month >= 1 && month <= 12)
                    year += 1900;
                else if (month >= 21 && month <= 32)
                    year += 2000;


                klient.BirthYear = year;
                klient.Płec = (genderDigit % 2 == 0) ? 0 : 1;

               
            }
            else
            {

                ModelState.AddModelError("PESEL", "PESEL must be at least 11 characters long.");
                return View(klient);
            }




            _context.Klienci.Add(klient);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }






        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public async Task<IActionResult> Edit(Klienci klient)
        //{
        //	if (ModelState.IsValid)
        //	{
        //		_context.Klienci.Update(klient); 
        //		await _context.SaveChangesAsync();
        //		return RedirectToAction(nameof(Index));
        //	}
        //	return View(klient);
        //}
        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klient = await _context.Klienci.FindAsync(id);
            if (klient == null)
            {
                return NotFound();
            }
            return View(klient);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Klienci model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var klient = await _context.Klienci.FindAsync(id);

            if (klient == null)
            {
                return NotFound();
            }


            klient.Name = model.Name;
            klient.Surname = model.Surname;
            klient.PESEL = model.PESEL;
            if (klient.PESEL.Length >= 11)
            {
                var yearPart = klient.PESEL.Substring(0, 2);
                var monthPart = klient.PESEL.Substring(2, 2);
                var genderDigit = int.Parse(klient.PESEL.Substring(9, 1));

                int year = int.Parse(yearPart);
                int month = int.Parse(monthPart);

                if (month >= 1 && month <= 12)
                    year += 1900;
                else if (month >= 21 && month <= 32)
                    year += 2000;


                klient.BirthYear = year;
                klient.Płec = (genderDigit % 2 == 0) ? 0 : 1;
            }
            else
            {

                ModelState.AddModelError("PESEL", "PESEL must be at least 11 characters long.");
                return View(model);
            }



            _context.Klienci.Update(klient);
            _context.SaveChanges();

            //return RedirectToAction("Index");
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool KlientExists(int id)
        {
            return _context.Klienci.Any(e => e.ID == id);
        }
        public async Task<IActionResult> Delete(int id)
        {

            var klient = await _context.Klienci.FindAsync(id);
            if (klient != null)
            {
                _context.Klienci.Remove(klient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(klient);
        }

        public IActionResult ImportExport()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file, string fileType)
        {
            if (file == null || file.Length == 0)
                return BadRequest("select a file");

            var clients = new List<Klienci>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                if (fileType == "xlsx")
                {
                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheets.First();
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                    foreach (var row in rows)
                    {
                        clients.Add(new Klienci
                        {
                            Name = row.Cell(1).GetValue<string>(),
                            Surname = row.Cell(2).GetValue<string>(),
                            PESEL = row.Cell(3).GetValue<string>(),
                            BirthYear = row.Cell(4).GetValue<int>(),
                            Płec = row.Cell(5).GetValue<int>()
                        });
                    }
                }
                else if (fileType == "csv")
                {
                    stream.Position = 0;
                    using var reader = new StreamReader(stream, Encoding.UTF8);
                    var csvLines = reader.ReadToEnd().Split('\n').Skip(1);

                    foreach (var line in csvLines)
                    {
                        var values = line.Split(',');
                        if (values.Length >= 5 && !string.IsNullOrWhiteSpace(values[0]))
                        {
                            clients.Add(new Klienci
                            {
                                Name = values[0].Trim(),
                                Surname = values[1].Trim(),
                                PESEL = values[2].Trim(),
                                BirthYear = int.Parse(values[3]),
                                Płec = int.Parse(values[4])
                            });
                        }
                    }
                }
            }

            _context.Klienci.AddRange(clients);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Export(string fileType)
        {
            var clients = await _context.Klienci.ToListAsync();

            if (fileType == "xlsx")
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Klienci");

                ws.Cell(1, 1).Value = "Name";
                ws.Cell(1, 2).Value = "Surname";
                ws.Cell(1, 3).Value = "PESEL";
                ws.Cell(1, 4).Value = "BirthYear";
                ws.Cell(1, 5).Value = "Płec";

                for (int i = 0; i < clients.Count; i++)
                {
                    ws.Cell(i + 2, 1).Value = clients[i].Name;
                    ws.Cell(i + 2, 2).Value = clients[i].Surname;
                    ws.Cell(i + 2, 3).Value = clients[i].PESEL;
                    ws.Cell(i + 2, 4).Value = clients[i].BirthYear;
                    ws.Cell(i + 2, 5).Value = clients[i].Płec;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "klienci.xlsx");
            }
            else if (fileType == "csv")
            {
                var sb = new StringBuilder();
                sb.AppendLine("Name,Surname,PESEL,BirthYear,Płec");

                foreach (var c in clients)
                {
                    sb.AppendLine($"{c.Name},{c.Surname},{c.PESEL},{c.BirthYear},{c.Płec}");
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "klienci.csv");
            }

            return BadRequest("Bad file format");
        }


        //public async Task<IActionResult> Index()
        //{
        //    var klienci = await _context.Klienci.OrderBy(k =>k.ID).ToListAsync();
        //}
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}