using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.ReservationControlServices
{
    public interface IReservationControlService
    {
        Task<IActionResult> Refuse(int? id, Controller controller);
        Task Accept(int? id, Controller controller);
        Task CreateReserv(int? id, Controller controller);
    }
}
