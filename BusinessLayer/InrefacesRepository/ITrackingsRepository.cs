using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.InrefacesRepository
{
    public interface ITrackingsRepository
    {
        void Add(int bookId, string userId);
        void Delete(int bookId, string userId);
        IEnumerable<Tracking> GetTrackingsByUserId(string userId);
    }
}
