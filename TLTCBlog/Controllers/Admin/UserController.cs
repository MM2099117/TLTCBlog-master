using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TLTCBlog.Models;

namespace TLTCBlog.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {

        /// GET: User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        /// <summary>
        /// Gets a hashset of admin details
        /// </summary>
        /// <param name="users"></param>
        /// <param name="context"></param>
        /// <returns>A hashSet of Admins</returns>
        private HashSet<string> GetAdminUserNames(List<ApplicationUser> users, TLTCBlogDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var Admins = new HashSet<string>();

            foreach (var user in users)
            {
                if (UserManager.IsInRole(user.Id, "Admin"))
                {
                    Admins.Add(user.UserName);
                }
            }
            return Admins;
        }

        /// <summary>
        /// GET: User/List
        /// </summary>
        /// <returns>view with a list of users</returns>
        public ActionResult List()
        {
            using (var DB = new TLTCBlogDbContext())
            {
                var users = DB.Users.ToList();

                var admins = GetAdminUserNames(users, DB);
                ViewBag.Admins = admins;

                return View(users);
            }
        }

        /// <summary>
        /// Edits roles in the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View with edited roles</returns>
        public ActionResult Edit(string id)
        {
            using (var DB = new TLTCBlogDbContext())
            {
                ///validate Id
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ///gets user from the database
                var user = DB.Users.Where(u => u.Id == id).First();

                ///check if user exists already
                if (user == null)
                {
                    return HttpNotFound();
                }

                ///creates a new view model
                var ViewModel = new EditUserViewModel();
                ViewModel.User = user;
                ViewModel.Roles = GetUserRoles(user, DB);

                ///pass the model to the view
                return View(ViewModel);

            }
        }

        /// <summary>
        /// GET: User Roles from DB
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dB"></param>
        /// <returns>List of roles</returns>
        private List<Role> GetUserRoles(ApplicationUser user, TLTCBlogDbContext dB)
        {
            ///create user manager
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            ///get all application roles
            var roles = dB.Roles.Select(r => r.Name).OrderBy(r => r).ToList();
            ///for each application role, check if the user has that role
            var userRoles = new List<Role>();

            foreach (var roleName in roles)
            {
                var role = new Role { Name = roleName };

                if (userManager.IsInRole(user.Id, roleName))
                {
                    role.isSelected = true;
                }

                userRoles.Add(role);
            }

            ///returns a list with all roles
            return userRoles;


        }
        /// <summary>
        /// POST: User/Edit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns>View Model</returns>
        [HttpPost]
        public ActionResult Edit(string id, EditUserViewModel viewModel)
        {
            ///checks if modelState is valid
            if (ModelState.IsValid)
            {
                using (var DB = new TLTCBlogDbContext())
                {
                    ///get user from database
                    var user = DB.Users.FirstOrDefault(u => u.Id == id);

                    ///check if user exists already
                    if (user == null)
                    {
                        return HttpNotFound();
                    }

                    ///if password field is not empty change password to new value
                    if (!string.IsNullOrEmpty(viewModel.Password))
                    {
                        var hasher = new PasswordHasher();
                        var passwordHash = hasher.HashPassword(viewModel.Password);
                        user.PasswordHash = passwordHash;
                    }

                    ///set user property values
                    user.Email = viewModel.User.Email;
                    user.FullName = viewModel.User.FullName;
                    user.UserName = viewModel.User.Email;
                    this.SetUserRoles(viewModel, user, DB);

                    ///save changes
                    DB.Entry(user).State = EntityState.Modified;
                    DB.SaveChanges();

                    return RedirectToAction("List");
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// SET: user roles for admin, user or suspended
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <param name="dB"></param>
        private void SetUserRoles(EditUserViewModel model, ApplicationUser user, TLTCBlogDbContext dB)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            foreach (var role in model.Roles)
            {
                if (role.isSelected)
                {
                    userManager.AddToRole(user.Id, role.Name);
                }
                else if (!role.isSelected)
                {
                    userManager.RemoveFromRole(user.Id, role.Name);
                }
            }
        }

        /// <summary>
        /// GET: User/Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View with the user details</returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var DB = new TLTCBlogDbContext())
            {
                /// gets user from database
                var user = DB.Users.Where(u => u.Id.Equals(id)).First();

                /// check if user exists
                if (user == null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }
        }

        /// <summary>
        /// POST: delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>updated tutorial</returns>
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var DB = new TLTCBlogDbContext())
            {
                /// gets user from database
                var user = DB.Users.Where(u => u.Id.Equals(id)).First();


                /// check if user exists
                if (user == null)
                {
                    return HttpNotFound();
                }

                /// gets user articles from database
                var userArticles = DB.BlogArticles.Where(a => a.Creator.Id == user.Id);

                /// delete user articles
                foreach(var article in userArticles)
                {
                    DB.BlogArticles.Remove(article);
                }

                ///delete user and save changes
                DB.Users.Remove(user);
                DB.SaveChanges();

                return View(user);
            }
        }
    }
}