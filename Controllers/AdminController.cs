using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EatInOslo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace EatInOslo.Controllers
{
    // [Authorize]
    public class AdminController : Controller
    {

        // ########################################
        //           SETUP & CONSTRUCTOR
        // ########################################
        

        private readonly EatInOsloContext _context;
        private IHostingEnvironment _env;

        public AdminController(EatInOsloContext context, IHostingEnvironment env) 
        {
            _context = context;
            _env = env;
        }

        // ########################################
        //                 ROUTING
        // ########################################

        // Admin login page
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("adminlogin") == "Admin") 
            {
                return RedirectToAction(nameof(Admin));
            }
            return View();
        }

        // Main Admin page
        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            if (HttpContext.Session.GetString("adminlogin") != "Admin") 
            {
                return RedirectToAction(nameof(Index));
            }

            try {
                return View(await _context.Resturant
                    .Include("Review")
                    .ToListAsync());

            } catch (Exception e) {
                return View(e);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Resturants()
        {
            if (HttpContext.Session.GetString("adminlogin") != "Admin") 
            {
                return RedirectToAction(nameof(Index));
            }

            try {
                return View(await _context.Resturant.ToListAsync());
            } catch (Exception e) {
                return View(e);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            if (HttpContext.Session.GetString("adminlogin") != "Admin") 
            {
                return RedirectToAction(nameof(Index));
            }

            try {
                return View(await _context.Review.ToListAsync());
            } catch (Exception e) {
                return View(e);
            }
        }

        // ########################################
        //                 LOGIN
        // ########################################

        // Post request for admin login
        [HttpPost]
        public async Task<IActionResult> Index([Bind("name, password")] User user)
        {
            try {
                List<User> us = await _context.User.Where(i => i.name == user.name).ToListAsync();

                if (us[0].name == "Admin" && us[0].password == user.password)
                {
                    HttpContext.Session.SetString("adminlogin", user.name);
                    return RedirectToAction(nameof(Admin));
                }

                return View();

            } catch (Exception e) {
                 return View(e);
            }
        }

        // #######################################
        //              EDIT/Resturant 
        // #######################################

        [HttpGet]
        public async Task<IActionResult> EditResturant(int? id)
        {
            if (HttpContext.Session.GetString("adminlogin") != "Admin") 
            {
                return RedirectToAction(nameof(Index));
            }

            try {
                return View(await _context.Resturant
                    .SingleOrDefaultAsync(r => r.ID == id));
            } catch (Exception e) {
                return View(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResturant(int id, IFormFile file, [Bind("ID, name, type, description, imgurl")] Resturant resturant)
        {
            try {
                if (file != null) 
                {
                    resturant.imgurl = file.FileName;

                    string path = _env.WebRootPath + "\\image\\resturants\\" + file.FileName;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                _context.Resturant.Update(resturant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Admin));
            } catch (Exception e) {
                return View(e);
            }
        }

        // #######################################
        //             DELETE/Resturant
        // #######################################

        [HttpGet]
        public async Task<IActionResult> DeleteResturant(int? id)
        {
            if (HttpContext.Session.GetString("adminlogin") != "Admin") 
            {
                return RedirectToAction(nameof(Index));
            }
            try {
                return View(await _context.Resturant
                    .SingleOrDefaultAsync(r => r.ID == id));
            } catch (Exception e) {
                return View(e);
            }
        }

        [HttpPost, ActionName("ConfirmDeleteResturant")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteResturant(int id)
        {
            _context.Resturant.Remove(await _context.Resturant
                .SingleOrDefaultAsync(c => c.ID == id));

            var rev = await _context.Review.Where(_review => _review.ResturantID == id).ToListAsync();
            foreach (var r in rev)
            {
                 _context.Review.Remove(r);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }

        // #######################################
        //             CREATE/Resturant
        // #######################################

        [HttpGet]
        public IActionResult NewResturant()
        {
            if (HttpContext.Session.GetString("adminlogin") != "Admin") 
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // Save resturant with Image upload
        [HttpPost]
        public async Task<IActionResult> NewResturant(IFormFile file, [Bind("ID,name,type,description,imgurl")] Resturant resturant) 
        { 
            if (file == null || file.Length == 0)
            {
                return Content("file not selected");
            }

            try {
                string path = _env.WebRootPath + "\\image\\resturants\\" + file.FileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                resturant.imgurl = file.FileName;
                _context.Resturant.Add(resturant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Admin));
            } catch (Exception e) {
                return View(e);
            }
        }

        // #######################################
        //             EDIT/Review
        // #######################################

        [HttpGet]
        public async Task<IActionResult> EditReview(int? id)
        {
            if (HttpContext.Session.GetString("adminlogin") != "Admin") 
            {
                return RedirectToAction(nameof(Index));
            }

            try {
                return View(await _context.Review.SingleOrDefaultAsync(r => r.ID == id));
            } catch (Exception e) {
                return View(e);
            }
            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(int id, [Bind("ID, text, title, UserID, ResturantID")] Review review)
        {
            _context.Review.Update(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }

        // #######################################
        //             Delete/Review
        // #######################################

        public async Task<IActionResult> RemoveReview(int? id)
        {
            if (HttpContext.Session.GetString("adminlogin") != "Admin") 
            {
                return RedirectToAction(nameof(Index));
            }

            return View(await _context.Review.SingleOrDefaultAsync(r => r.ID == id));
        }

        [HttpPost, ActionName("ConfirmDeleteReview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteReview(int id)
        {
            try {
                _context.Review.Remove(await _context.Review.SingleOrDefaultAsync(r => r.ID == id));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Admin));
            } catch (Exception e) {
                return View(e);
            }
        }

    }
}