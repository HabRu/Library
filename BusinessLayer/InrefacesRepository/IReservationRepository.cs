using System.Threading.Tasks;

namespace Library.Services.ReservationControlServices
{
    public interface IReservationRepository
    {
        Task DeleteReserv(int? id, string email, bool hasAccess);
        Task AcceptReserv(int? id);
        Task CreateReserv(int? id, string userId);
    }
}
