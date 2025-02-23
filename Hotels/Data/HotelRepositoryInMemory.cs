using Hotels.Data.Models;

namespace Hotels.Data
{
    public class HotelRepositoryInMemory : IHotelRepository
    {
        private static List<Hotel> _hotels = new List<Hotel>();  // In-memory storage

        public IQueryable<Hotel> GetAll()
        {
            return _hotels.AsQueryable();
        }

        public Hotel? GetById(int id)
        {
            return _hotels.FirstOrDefault(h => h.Id == id);
        }

        public void Add(Hotel hotel)
        {
            hotel.Id = _hotels.Count > 0 ? _hotels.Max(h => h.Id) + 1 : 1;  // Auto-generate ID
            _hotels.Add(hotel);
        }

        public void Update(Hotel hotel)
        {
            var existingHotel = _hotels.FirstOrDefault(h => h.Id == hotel.Id);
            if (existingHotel != null)
            {
                existingHotel.Name = hotel.Name;
                existingHotel.Price = hotel.Price;
                existingHotel.Latitude = hotel.Latitude;
                existingHotel.Longitude = hotel.Longitude;
                return;
            }

            throw new KeyNotFoundException("Hotel not found");
        }

        public void Delete(int id)
        {
            var hotel = _hotels.FirstOrDefault(h => h.Id == id);
            if (hotel != null)
            {
                _hotels.Remove(hotel);
                return;
            }

            throw new KeyNotFoundException("Hotel not found");
        }
    }
}
