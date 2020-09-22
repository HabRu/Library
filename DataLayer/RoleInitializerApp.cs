using Library.Models;
using Microsoft.AspNetCore.Identity;
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
                User admin = new User { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, RolesConfig.ADMIN);
                }
            }
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                User user = new User { Email = userEmail, UserName = userEmail, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RolesConfig.USER);
                }
            }
            if (await userManager.FindByNameAsync(librarianEmail) == null)
            {
                User librarian = new User { Email = librarianEmail, UserName = librarianEmail, EmailConfirmed = true };
                IdentityResult result = await userManager.CreateAsync(librarian, librarianPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(librarian, RolesConfig.LIBRARIAN);
                }
            }
        }
    }
}
