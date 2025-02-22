using Hotels.Data;
using Hotels.Data.Models;
using Hotels.ViewData;

namespace Hotels.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        // Get all hotels
        public IEnumerable<ViewData.Hotel> GetAll()
        {
            return _hotelRepository.GetAll().Select(h => new ViewData.Hotel { Id = h.Id, Name = h.Name, Latitude = h.Latitude, Longitude = h.Longitude, Price = h.Price });
        }

        // Get hotels on specific page (pagination)
        public HotelsTable GetAll(int pageId, int pageSize)
        {
            var hotels = _hotelRepository.GetAll()
                .Skip((pageId - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new HotelsTable
            {
                Hotels = hotels.Select(h => new ViewData.Hotel { Id = h.Id, Name = h.Name, Latitude = h.Latitude, Longitude = h.Longitude, Price = h.Price }).ToList(),
                TotalPages = TotalPagesCount(pageSize, _hotelRepository.GetAll().Count())
            };
        }

        // Get hotels sorted by distance from a specific latitude and longitude
        public HotelsTable GetByDistance(int pageId, int pageSize, double latitude, double longitude)
        {
            var hotels = _hotelRepository.GetAll()
                .OrderBy(h => GetDistance(h.Latitude, h.Longitude, latitude, longitude))  // Sort by distance
                .Skip((pageId - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new HotelsTable
            {
                Hotels = hotels.Select(h => new ViewData.Hotel { Id = h.Id, Name = h.Name, Latitude = h.Latitude, Longitude = h.Longitude, Price = h.Price, Distance = GetDistance(h.Latitude, h.Longitude, latitude, longitude) }).ToList(),
                TotalPages = TotalPagesCount(pageSize, _hotelRepository.GetAll().Count())
            };
        }

        // Get a single hotel by ID
        public ViewData.Hotel? GetById(int id)
        {
            var hotel = _hotelRepository.GetById(id);
            if (hotel == null) { 
                throw new KeyNotFoundException("Hotel not found");
            }
            return new ViewData.Hotel
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Price = hotel.Price,
                Latitude = hotel.Latitude,
                Longitude = hotel.Longitude
            };
        }

        // Add a new hotel
        public ViewData.Hotel Add(HotelCreate hotelCreate)
        {
            var hotel = new Data.Models.Hotel
            {
                Name = hotelCreate.Name,
                Price = hotelCreate.Price,
                Latitude = hotelCreate.Latitude,
                Longitude = hotelCreate.Longitude,
            };

            _hotelRepository.Add(hotel);
            return new ViewData.Hotel
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Price = hotel.Price,
                Latitude = hotel.Latitude,
                Longitude = hotel.Longitude
            };
        }

        // Update an existing hotel
        public ViewData.Hotel? Update(int hotelId, HotelCreate hotelUpdate)
        {
            var hotel = _hotelRepository.GetById(hotelId);
            if (hotel != null)
            {
                hotel.Name = hotelUpdate.Name;
                hotel.Price = hotelUpdate.Price;
                hotel.Latitude = hotelUpdate.Latitude;
                hotel.Longitude = hotelUpdate.Longitude;

                _hotelRepository.Update(hotel);

                return new ViewData.Hotel
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    Price = hotel.Price,
                    Latitude = hotel.Latitude,
                    Longitude = hotel.Longitude
                };
            }
           
            throw new KeyNotFoundException("Hotel not found");
        }

        // Delete a hotel by ID
        public bool Delete(int id)
        {
            var hotel = _hotelRepository.GetById(id);
            if (hotel != null)
            {
                _hotelRepository.Delete(id);
                return true;
            }
            return false;
        }

        // Helper method to calculate distance between two geographical coordinates
        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var r = 6371; // Radius of the Earth in kilometers
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = r * c; // Distance in kilometers
            return distance;
        }

        private double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        private int TotalPagesCount(int pageSize, int nuberOfElements)
        {
            return (int)Math.Ceiling((double)nuberOfElements / (double)pageSize);
        }
    }
}
