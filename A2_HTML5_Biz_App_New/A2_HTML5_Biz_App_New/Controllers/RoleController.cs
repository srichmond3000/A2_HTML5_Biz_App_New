using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using A2_HTML5_Biz_App_New.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace A2_HTML5_Biz_App_New.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private ApplicationDbContext _ctx;

        public RoleController()
        {
            _ctx = new ApplicationDbContext();
        }
        
        // GET: Role
        public ActionResult Index()
        {
            var appRoles = _ctx.Roles.ToList();
            return View(appRoles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                _ctx.Roles.Add(new IdentityRole()
                {
                    Name = collection["RoleName"],
                });
                _ctx.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AssignUserToRole()
        {
            var list = _ctx.Roles.OrderBy(role => role.Name)
                .ToList().Select(role => new SelectListItem { Value = role.Name, Text = role.Name })
                .ToList();
            ViewBag.Roles = list;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserAddToRole(string userName, string roleName)
        {
            ApplicationUser user = _ctx.Users.Where(usr => usr
                .UserName
                .Equals(userName, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            // Display roles in dropdown
            var list = _ctx
                .Roles
                .OrderBy(role => role.Name)
                .ToList()
                .Select(role => new SelectListItem() { Value = role.Name, Text = role.Name })
                .ToList();

            if(user != null)
            {
                var account = new AccountController();
                account.UserManager.AddToRoleAsync(user.Id, roleName);
                ViewBag.ResultMessage = "Role created successfully";

                return View("AssignUserToRole");
            }
            else
            {
                ViewBag.ErrorMessage = "Sorry user is not available";
                return View("AssignUserToRole");
            }
        }
    }
}