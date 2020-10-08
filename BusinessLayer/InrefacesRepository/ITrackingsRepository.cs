using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.InrefacesRepository
{
    public interface ITrackingsRepository
    {
        void Add(int bookId, string userId);
        Task Delete(int bookId, string userId);
        IEnumerable<Tracking> GetTrackingsByUserId(string userId);
    }
}
