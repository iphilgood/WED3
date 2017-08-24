using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pizza.Data;
using Pizza.Models;

namespace Pizza.Services
{
    public class DataService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager  ) {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async void EnsureData(string adminPwd){
            var role = await _roleManager.FindByNameAsync("Administrator");
            if (role == null){
                role = new IdentityRole() {Name = "Administrator"};
                await _roleManager.CreateAsync(role);
            }
            if (await _userManager.FindByNameAsync("admin@admin.ch") == null) {
                var user = new ApplicationUser() {UserName = "admin@admin.ch" };
                await _userManager.CreateAsync(user, adminPwd);
                await _userManager.AddToRoleAsync(user, "Administrator");
            }
        }
    }
}
