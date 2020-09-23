using Library.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Library
{
    public class RoleInitializerApp
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@gmail.com";
            var password = "Admin_Admin_1";

            var userEmail = "user@gmail.com";
            var userPassword = "User_User_1";

            var librarianEmail = "librarian@gmail.com";
            var librarianPassword = "Librarian_Librarian_1";

            if (await roleManager.FindByNameAsync(RolesConfig.ADMIN) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RolesConfig.ADMIN));
            }
            if (await roleManager.FindByNameAsync(RolesConfig.LIBRARIAN) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RolesConfig.LIBRARIAN));
            }
            if (await roleManager.FindByNameAsync(RolesConfig.USER) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RolesConfig.USER));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, RolesConfig.ADMIN);
                }
            }
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                var user = new User { Email = userEmail, UserName = userEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RolesConfig.USER);
                }
            }
            if (await userManager.FindByNameAsync(librarianEmail) == null)
            {
                var librarian = new User { Email = librarianEmail, UserName = librarianEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(librarian, librarianPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(librarian, RolesConfig.LIBRARIAN);
                }
            }
        }
    }
}
