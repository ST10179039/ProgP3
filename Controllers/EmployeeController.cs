using AgriEnergyP2.Data;
using AgriEnergyP2.Models;
using AgriEnergyP2.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyP2.Controllers
{
    // Only users with the "Employee" role are authorized to access this controller
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        // Constructor injects ApplicationDbContext and UserManager for database and identity operations
        public EmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Employee
        // Loads the default Employee dashboard or index page
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Employee/AddFarmer
        // Displays a form for adding a new farmer user and profile
        public IActionResult AddFarmer()
        {
            return View();
        }

        // POST: /Employee/AddFarmer
        // Handles creation of a new farmer account and profile after form submission
        [HttpPost]
        public async Task<IActionResult> AddFarmer(AddFarmerViewModel model)
        {
            // Check if a user with the given email already exists
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "A user with this email already exists.");
                    return View(model);
                }
                // Create a new ApplicationUser instance
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                // Attempt to create the user account with the given password
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {// Assign the user to the "Farmer" role
                    await _userManager.AddToRoleAsync(user, "Farmer");
                    // Create the Farmer profile and associate it with the ApplicationUser
                    var farmer = new Farmer
                    {
                        ApplicationUserId = user.Id,
                        FarmName = model.FarmName,
                        Location = model.Location,
                        DateJoined = DateTime.UtcNow
                    };

                    // Save the new farmer profile to the database
                    _context.Farmers.Add(farmer);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Farmer profile created successfully!";
                    return RedirectToAction("Index","Employee");
                }
                // If user creation fails, add errors to the model state
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            // If model validation fails, redisplay the form with validation messages
            return View(model);
        }

        // GET: /Employee/ListFarmers
        // Returns a list of all farmers, including linked user account information
        public async Task<IActionResult> ListFarmers()
        {
            var farmers = await _context.Farmers
                .Include(f => f.ApplicationUser)
                .ToListAsync();

            return View(farmers);
        }

        // GET: /Employee/ProductList
        // Displays a filterable list of products by type and/or production date range
        public async Task<IActionResult> ProductList(string productType, DateTime? fromDate, DateTime? toDate)
        {
            // Start building a query that includes farmer and user details
            var productsQuery = _context.Products
                .Include(p => p.Farmer)
                .ThenInclude(f => f.ApplicationUser)
                .AsQueryable();

            if (!string.IsNullOrEmpty(productType))
            {
                productsQuery = productsQuery.Where(p => p.Type == productType);
            }

            if (fromDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate <= toDate.Value);
            }
            // Execute the query and return the filtered list
            var products = await productsQuery.ToListAsync();
            return View(products);
        }
    }
}
