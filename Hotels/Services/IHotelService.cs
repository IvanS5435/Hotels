using Hotels.ViewData;

namespace Hotels.Services
{
    public interface IHotelService
    {
        // Get hotels
        IEnumerable<Hotel> GetAll();
        // Get hotels on specific page
        HotelsTable GetAll(int pageId, int pageSize);
        // Get hotels sorted by distance 
        HotelsTable GetByDistance(int pageId, int pageSize, double latitude, double longitude);
        // Get a single hotel by ID
        Hotel? GetById(int id);
        // Add a new hotel
        Hotel Add(HotelCreate hotel);
        // Update an existing hotel
        Hotel? Update(int hotelId, HotelCreate hotelUpdate);
        // Delete a hotel by ID
        bool Delete(int id);                   
    }
}
