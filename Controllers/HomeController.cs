using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EatInOslo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace EatInOslo.Controllers
{
    public class HomeController : Controller
    {
        private readonly EatInOsloContext _context;
        private readonly IHostingEnvironment _env;

        public HomeController(EatInOsloContext context, IHostingEnvironment env) 
        {
            _context = context;
            _env = env;
        }

        // #######################################
        //             ROUTING
        // #######################################

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try 
            {
                return View(await _context.Restaurant.ToListAsync());
            } 
                catch (Exception ex) 
            {
                return View(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Restaurant(int? id) 
        {
            ViewData["login"] = HttpContext.Session.GetString("login");

            try
            {
                return View(await _context.Restaurant
                    .Include("Review")
                    .Include("Image")
                    .SingleOrDefaultAsync(r => r.ID == id));
            } 
                catch (Exception ex)
            {
                return View(ex);
            }
        }

        // #######################################
        //             CREATE/Review
        // #######################################

        [HttpGet]
        public IActionResult NewReview(int? RestaurantID)
        {
            ViewData["Restaurantid"] = RestaurantID;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> NewReview([Bind("ID, RestaurantID, title, text, stars")] Review review)
        {
            if (ModelState.IsValid)
            {
                try {
                    string loginname = HttpContext.Session.GetString("login");
                    var user = _context.User.SingleOrDefault(u => u.name == loginname);
            
                    review.UserID = user.ID;

                    _context.Review.Add(review);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Restaurant", new { id = review.RestaurantID });
                } catch (Exception e) {
                    return View(e);
                }
            } else {
                ViewData["Restaurantid"] = review.RestaurantID;
                return View(review);
            }
        }

        // #######################################
        //               LOGIN/Guest (logout)
        // #######################################

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["login"] = HttpContext.Session.GetString("login");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("name, password")] User user)
        {
            if (ModelState.IsValid) {
                try {
                    List<User> guest = await _context.User.Where(i => i.name == user.name).ToListAsync();

                    if (guest[0].name == user.name && guest[0].password == user.password)
                    {
                        HttpContext.Session.SetString("login", user.name);
                        return RedirectToAction(nameof(Index));
                    }

                    return View(user);

                } catch (Exception e) {
                    return View(e);
                }
            } else {
                ViewData["login"] = HttpContext.Session.GetString("login");
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult CreateLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLogin([Bind("ID, name, password, email")] User user)
        {
            if (ModelState.IsValid) {
                try {
                    _context.User.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));
                } catch (Exception e) {
                    return View(e);
                }
            } else {
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult Logout() 
        {
            HttpContext.Session.Remove("login");
            return RedirectToAction(nameof(Index));
        }

        // #######################################
        //             Image_Upload
        // #######################################

        [HttpGet]
        public IActionResult Upload(int? RestaurantID)
        {   
            ViewData["RestaurantID"] = RestaurantID;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int? RestaurantID, IFormFile file, [Bind("ID, imgurl, description, RestaurantID")] Image image)
        {
            if (ModelState.IsValid)
            {
                if (file == null || file.Length == 0)
                {
                    return Content("file not selected");
                }
                try {
                    string path = _env.WebRootPath + "\\image\\uploads\\" + file.FileName;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    image.imgurl = file.FileName;
                    _context.Image.Add(image);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
                } catch (Exception e) {
                    return View(e);
                }
            } else {
                ViewData["RestaurantID"] = RestaurantID;
                return View(image);
            }
        }
    }
}