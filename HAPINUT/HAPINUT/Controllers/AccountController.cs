using HAPINUT.DTO;
using HAPINUT.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HAPINUT.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            string userName = User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
                return RedirectToAction("user-profile");
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUserViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            // Check if the user isValid
            bool isValid = false;

            using (Db db = new Db())
            {
                if (db.Users.Any(x => x.UserName == userModel.Username && x.PassWord == userModel.Password))
                    isValid = true;
                if (!isValid)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(userModel);
                }
                else
                {
                    var userId = db.Users.FirstOrDefault(x => x.UserName == userModel.Username);
                    var userRole = db.UserRoles.FirstOrDefault(x => x.UserId == userId.Id);
                    FormsAuthentication.SetAuthCookie(userModel.Username, userModel.RememberMe);
                    if (userRole.RoleId == 1)
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                    else
                        return Redirect("/Shop");
                    //return Redirect(FormsAuthentication.GetRedirectUrl(userModel.Username, userModel.RememberMe));
                }
            }
        }

        // GET: /account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        // POST: /account/create-account
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserViewModel model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", model);
            }

            // Check if passwords match
            if (!model.PassWord.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View("CreateAccount", model);
            }

            using (Db db = new Db())
            {
                // Make sure username is unique
                if (db.Users.Any(x => x.UserName.Equals(model.UserName)))
                {
                    ModelState.AddModelError("", "Username " + model.UserName + " is taken.");
                    model.UserName = "";
                    return View("CreateAccount", model);
                }

                // Create userDTO
                User userDTO = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    UserName = model.UserName,
                    PassWord = model.PassWord
                };

                // Add the DTO
                db.Users.Add(userDTO);

                // Save
                db.SaveChanges();

                // Add to UserRolesDTO
                int id = userDTO.Id;

                UserRole userRolesDTO = new UserRole()
                {
                    UserId = id,
                    RoleId = 2
                };

                db.UserRoles.Add(userRolesDTO);
                db.SaveChanges();
            }

            // Create a TempData message
            TempData["SM"] = "You are now registered and can login.";

            // Redirect
            return Redirect("~/account/login");
        }

        // GET: /account/Logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/account/login");
        }

        // GET: /account/user-profile
        [HttpGet]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile()
        {
            // Get username
            string username = User.Identity.Name;

            // Declare model
            UserViewModel model;

            using (Db db = new Db())
            {
                // Get user
                User dto = db.Users.FirstOrDefault(x => x.UserName == username);

                // Build model
                model = new UserViewModel(dto);
            }

            // Return view with model
            return View("UserProfile", model);
        }

        // POST: /account/user-profile
        [HttpPost]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile(UserViewModel model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            // Check if passwords match if need be
            if (!string.IsNullOrWhiteSpace(model.PassWord))
            {
                if (!model.PassWord.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                    return View("UserProfile", model);
                }
            }

            using (Db db = new Db())
            {
                // Get username
                string username = User.Identity.Name;

                // Make sure username is unique
                if (db.Users.Where(x => x.Id != model.Id).Any(x => x.UserName == username))
                {
                    ModelState.AddModelError("", "Username " + model.UserName + " already exists.");
                    model.UserName = "";
                    return View("UserProfile", model);
                }

                // Edit DTO
                User dto = db.Users.Find(model.Id);

                dto.FirstName = model.FirstName;
                dto.LastName = model.LastName;
                dto.EmailAddress = model.EmailAddress;
                dto.UserName = model.UserName;

                if (!string.IsNullOrWhiteSpace(model.PassWord))
                {
                    dto.PassWord = model.PassWord;
                }

                FormsAuthentication.SetAuthCookie(dto.UserName, false);
                // Save
                db.SaveChanges();
            }

            // Set TempData message
            TempData["SM"] = "You have edited your profile!";

            // Redirect
            return Redirect("~/account/user-profile");
        }
    }
}