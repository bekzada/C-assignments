using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoLoginRegistration.Models;
using DemoLoginRegistration.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;


namespace DemoLoginRegistration.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext dbContext;          //access to database 
        public  HomeController(HomeContext context)
        {
            dbContext =context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("register")]

        public IActionResult Register(User register)
        {
            if(ModelState.IsValid)
            {
                   if(dbContext.Users.Any(u => u.Email == register.Email))
                   {
                       ModelState.AddModelError("Email","That email already exists.");
                   }
                   else
                   {
                      PasswordHasher<User> hash = new PasswordHasher<User>();
                      register.Password =hash.HashPassword(register,register.Password);

                      dbContext.Users.Add(register); 
                      dbContext.SaveChanges();
                      return RedirectToAction("Login");
                    
                   }
            }
            return View("Index");
        }
        [HttpPost("signin")]

        public IActionResult SignIn(LoginUser log)
    {
        if(ModelState.IsValid)
        {
            User check = dbContext.Users.FirstOrDefault(u => u.Email ==log.LoginEmail);
            if(check ==null)
            {
                ModelState.AddModelError("LoginEmail","Invalid Email/Password");
                return View("Login");
            }
            else
            {
                PasswordHasher<LoginUser> compare = new PasswordHasher<LoginUser>();
                var result = compare.VerifyHashedPassword(log,check.Password,log.LoginPassword);
                if(result == 0)
                {
                   ModelState.AddModelError("LoginEmail","Invalid Email/Password");
                   return View("Login");

                }
                else 
                {
                    
                    Console.WriteLine(check.UserId);
                    HttpContext.Session.SetInt32("UserId",check.UserId);
                    Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
                    return RedirectToAction("Success");
                } 
            }
        }
        else
        {
            return View("Login");
        }

    }

    private User LoggedIn()
    {
        return dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
    }


    [HttpGet("success")]
    public IActionResult Success()
    {
        User userInDb = LoggedIn();
        if(userInDb == null)
        {
            Console.WriteLine("####################### LOGGING USER OUT #########################");
            return  RedirectToAction("LogOut");
        }
        ViewBag.User = userInDb;
        return View();

    }

    [HttpGet("logout")]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
 

        //////////////////////////////////////////////////////////////

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
}
