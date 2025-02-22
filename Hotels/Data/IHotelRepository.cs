using Hotels.Data.Models;

namespace Hotels.Data
{
    public interface IHotelRepository
    {
        IQueryable<Hotel> GetAll();
        Hotel? GetById(int id);
        void Add(Hotel hotel);
        void Update(Hotel hotel);
        void Delete(int id);
    }
}