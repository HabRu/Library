using Library.Models;
using System.Threading.Tasks;

namespace Library.Services.ReservationControlServices
{
    public interface IReservationRepository
    {
        Task DeleteReserv(int? id, string email, bool hasAccess);
        Task AcceptReserv(int? id);
        Task<Reservation> CreateReserv(int? id, string userId, string userName);
    }
}
