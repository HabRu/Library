﻿using Library.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public class RoleInitializerApp
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "Admin_Admin_1";

            string userEmail = "user@gmail.com";
            string userPassword = "User_User_1";

            string librarianEmail = "librarian@gmail.com";
            string librarianPassword = "Librarian_Librarian_1";

            if (await roleManager.FindByNameAsync(RolesConfig.admin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RolesConfig.admin));
            }
            if (await roleManager.FindByNameAsync(RolesConfig.librarian) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RolesConfig.librarian));
            }
            if (await roleManager.FindByNameAsync(RolesConfig.user) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RolesConfig.user));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, RolesConfig.admin);
                }
            }
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                User user = new User { Email = userEmail, UserName = userEmail, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RolesConfig.user);
                }
            }
            if (await userManager.FindByNameAsync(librarianEmail) == null)
            {
                User librarian = new User { Email = librarianEmail, UserName = librarianEmail, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(librarian, librarianPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(librarian, RolesConfig.librarian);
                }
            }
        }
    }
}