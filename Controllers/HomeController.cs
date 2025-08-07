using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication2.Models;
using PESEL;
using PESEL.Models;
using PESEL.Validators.Impl;
using System.Reflection.Metadata.Ecma335;

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
                klient.P³ec = (genderDigit % 2 == 0) ? 0 : 1;
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
                klient.P³ec = (genderDigit % 2 == 0) ? 0 : 1;
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
