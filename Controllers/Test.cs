//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;
//using WebApplication2.Models;

//namespace WebApplication2.Controllers
//{
//	public class Test : Controller
//	{
//		private readonly ILogger<Test> _logger;
//		private readonly AppDbContext _context;

//		public Test(ILogger<Test> logger, AppDbContext context)
//		{
//			_logger = logger;
//			_context = context;
//		}


//		public async Task<IActionResult> Index()
//		{
//			try
//			{ 
//			var Klienci = await _context.Klienci.ToListAsync();
//			return View(Klienci);
//			}
//			catch (Exception ex)
//			{
//				_logger.LogError(ex, "Error while retriving data from database");
//				return StatusCode(500, "Internal server error.");
//			}
//		}

//		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//		public IActionResult Error()
//		{
//			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//		}


//	}
//}



