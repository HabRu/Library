using BusinessLayer.InrefacesRepository;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.ImplementationsRepository
{
    public class EFTrackingsRepository : ITrackingsRepository
    {
        private readonly IRepository<Tracking> trackRep;

        public EFTrackingsRepository(IRepository<Tracking> trackRep)
        {
            this.trackRep = trackRep;
        }

        public async void Add(int bookId, string userId)
        {
            var tracking = new Tracking
            {
                BookId = bookId,
                UserId = userId
            };
            await trackRep.CreateAsync(tracking);
        }

        public async Task Delete(int bookId, string userId)
        {
            var tracking = trackRep.GetAll().FirstOrDefault((t) => t.BookId == bookId && t.UserId == userId);
            await trackRep.DeleteAsync(tracking);
        }

        public IEnumerable<Tracking> GetTrackingsByUserId(string userId)
        {
            return trackRep.GetAll().Where(t => t.UserId == userId);
        }
    }
}
