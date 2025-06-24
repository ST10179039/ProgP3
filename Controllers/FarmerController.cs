using AgriEnergyP2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AgriEnergyP2.Models;
using Microsoft.EntityFrameworkCore;
using AgriEnergyP2.Models.ViewModel;

namespace AgriEnergyP2.Controllers
{
    // Restrict access to users in the "Farmer" role
    [Authorize(Roles = "Farmer")]
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        // Constructor injects ApplicationDbContext and UserManager services
        public FarmerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //GET: Farmer/Index
        // Displays a list of products uploaded by the currently logged-in farmer
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            
            // Retrieve the corresponding farmer profile
            var farmer = await _context.Farmers
               .FirstOrDefaultAsync(f => f.ApplicationUserId == user.Id);
            // If the farmer profile does not exist, return a 404 error
            if (farmer == null)
            {

                return NotFound("Farmer Profile not found. ");
            }
            // Fetch all products associated with this farmer
            var products = await _context.Products
                .Where(p => p.FarmerId ==farmer.FarmerId)
                .ToListAsync();
            // Return the product list to the view
            return View(products);
        }
        // GET: Farmer/Create
        // Displays the form for creating a new product listing
        public IActionResult Create()
        {
            return View();
        }
        // POST: Farmer/Create
        // Handles the form submission to create a new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            // Retrieve the corresponding farmer profile
            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(f => f.ApplicationUserId == user.Id);
            // If the farmer profile is not found, return an error
            if (farmer == null)
                return NotFound("Farmer profile not found.");
            // Validate and process the form input
            if (ModelState.IsValid)
            {
                // Create a new Product entity from the view model
                var product = new Product
                {
                    Name = model.Name,
                    Type = model.Type,
                    ProductionDate = model.ProductionDate,
                    FarmerId = farmer.FarmerId
                };
                // Add the product to the database
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                // Notify success and redirect to index
                TempData["Success"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            // If model validation fails, redisplay the form with validation messages
            return View(model);
        }

    }
}