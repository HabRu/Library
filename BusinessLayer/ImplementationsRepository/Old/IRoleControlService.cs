using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.RoleControlServices
{
    public interface IRoleControlService
    {
        Task<IActionResult> Create(string name, Controller controller);
        Task<IActionResult> Delete(string id, Controller controller);
        Task<IActionResult> Edit(string userId, Controller controller);
        Task<IActionResult> Edit(string userId, List<string> roles, Controller controller);
    }
}
