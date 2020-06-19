using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApp.Models;
using WebApp.Data;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Components.Forms;
using System.Runtime.InteropServices;

namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index() => View(_userManager.Users.ToList());


        [HttpGet]
        public async Task<ActionResult> BlockUser(string BlockCheck)
        {
            if(BlockCheck=="on")
            {
                var userById = _userManager.GetUserId(HttpContext.User);
                var user = await _userManager.FindByIdAsync(userById);
                user.IsBlocked = true;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Block()
        {
            var users = _signInManager.UserManager.Users;
            foreach (var user in users)
            {
                if (user != null)
                {
                    if (user.IsBlocked)
                    {
                        user.AccessFailedCount = 6;
                        user.LockoutEnd = DateTime.Now.AddYears(100);
                        await _userManager.AccessFailedAsync(user);
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}