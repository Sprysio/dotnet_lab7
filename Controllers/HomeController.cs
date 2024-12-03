using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using lab7.Models;
using lab7.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using lab7.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace lab7.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ChinookDbContext _chinook;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ChinookDbContext chinook, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _chinook = chinook;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View(_chinook.Customers.ToList());
    }
    [Authorize]
    public async Task<IActionResult> MyOrders()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        var customerId = user.CustomerId;
        return View(await _chinook.Invoices.Where(x => x.CustomerId == customerId).ToListAsync());
    }

    [Authorize]
    public async Task<IActionResult> OrderDetails(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        var customerId = user.CustomerId;
        var invoice = await _chinook.Invoices
            .Include(x => x.InvoiceLines)
            .ThenInclude(x => x.Track)
            .Where(x => x.CustomerId == customerId && x.InvoiceId == id)
            .FirstOrDefaultAsync();

        if (invoice == null)
        {
            return NotFound("Invoice not found.");
        }
        return View(invoice);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
