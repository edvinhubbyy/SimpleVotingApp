using Microsoft.AspNetCore.Mvc;
using SimpleVotingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleVotingApp.Controllers
{
    public class AccountController : Controller
    {
        // A simple list of users (you can replace this with a database in a real system)
        private static List<User> Users = new List<User>
        {
            new User { Username = "admin", Password = "admin123" },
            new User { Username = "user", Password = "password" }
        };

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // You can set session/cookies here for authentication if necessary
                TempData["Username"] = username; // Store the username temporarily
                return RedirectToAction("Index", "Home"); // Redirect to homepage or dashboard
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            TempData.Remove("Username"); // Clear the username from session
            return RedirectToAction("Login");
        }
    }
}
