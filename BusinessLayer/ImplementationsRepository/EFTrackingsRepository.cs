using BusinessLayer.InrefacesRepository;
using Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.ImplementationsRepository
{
    public class EFTrackingsRepository : ITrackingsRepository
    {
        private readonly IRepository db;

        public EFTrackingsRepository(IRepository db)
        {
            this.db = db;
        }
        public async void Add(int bookId, string userId)
        {
            Tracking tracking = new Tracking
            {
                BookId = bookId,
                UserId = userId
            };
            await db.Trackings.AddAsync(tracking);
            db.DbContext.SaveChanges();
        }

        public  void Delete(int bookId, string userId)
        {
            Tracking tracking = db.Trackings.FirstOrDefault((t) => t.BookId == bookId && t.UserId == userId);
            db.Trackings.Remove(tracking);
            db.DbContext.SaveChanges();
        }

        public IEnumerable<Tracking> GetTrackingsByUserId(string userId)
        {
            return db.Trackings.Where(t => t.UserId == userId);
        }
    }
}
