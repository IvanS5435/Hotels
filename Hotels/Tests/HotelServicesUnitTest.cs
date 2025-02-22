using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Hotels.Data;
using Hotels.Data.Models;
using Hotels.ViewData;
using Hotels.Services;
using NUnit.Framework.Legacy;

namespace Hotels.Tests
{
    [TestFixture]
    public class HotelServicesUnitTest
    {
        private Mock<IHotelRepository> _hotelRepositoryMock;
        private HotelService _hotelService;

        [SetUp]
        public void Setup()
        {
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _hotelService = new HotelService(_hotelRepositoryMock.Object);
        }

        [Test]
        public void GetAll_ShouldReturnAllHotels()
        {
            var hotels = new List<Data.Models.Hotel>
        {
            new Data.Models.Hotel { Id = 1, Name = "Hotel A", Latitude = 40.7128, Longitude = -74.0060, Price = 150 },
            new Data.Models.Hotel { Id = 2, Name = "Hotel B", Latitude = 34.0522, Longitude = -118.2437, Price = 200 }
        };

            _hotelRepositoryMock.Setup(repo => repo.GetAll()).Returns(hotels.AsQueryable());

            var result = _hotelService.GetAll().ToList();

            Assert.That(2 == result.Count);
            CollectionAssert.AreEqual("Hotel A", result[0].Name);
            CollectionAssert.AreEqual("Hotel B", result[1].Name);
        }

        [Test]
        public void GetById_ExistingHotel_ShouldReturnHotel()
        {
            var hotel = new Data.Models.Hotel { Id = 1, Name = "Hotel A", Latitude = 40.7128, Longitude = -74.0060, Price = 150 };
            _hotelRepositoryMock.Setup(repo => repo.GetById(1)).Returns(hotel);

            var result = _hotelService.GetById(1);

            ClassicAssert.IsNotNull(result);
            CollectionAssert.AreEqual("Hotel A", result.Name);
        }

        [Test]
        public void GetById_NonExistingHotel_ShouldThrowException()
        {
            _hotelRepositoryMock.Setup(repo => repo.GetById(1)).Returns((Data.Models.Hotel)null);

            Assert.Throws<KeyNotFoundException>(() => _hotelService.GetById(1));
        }

        [Test]
        public void Add_ShouldAddHotel()
        {
            var hotelCreate = new HotelCreate { Name = "Hotel C", Latitude = 51.5074, Longitude = -0.1278, Price = 180 };
            var hotel = new Data.Models.Hotel { Id = 3, Name = "Hotel C", Latitude = 51.5074, Longitude = -0.1278, Price = 180 };

            _hotelRepositoryMock.Setup(repo => repo.Add(It.IsAny<Data.Models.Hotel>())).Callback<Data.Models.Hotel>(h => h.Id = 3);

            var result = _hotelService.Add(hotelCreate);

            ClassicAssert.IsNotNull(result);
            Assert.That(3 == result.Id);
            CollectionAssert.AreEqual("Hotel C", result.Name);
        }

        [Test]
        public void Delete_ExistingHotel_ShouldReturnTrue()
        {
            var hotel = new Data.Models.Hotel { Id = 1, Name = "Hotel A", Latitude = 40.7128, Longitude = -74.0060, Price = 150 };
            _hotelRepositoryMock.Setup(repo => repo.GetById(1)).Returns(hotel);
            _hotelRepositoryMock.Setup(repo => repo.Delete(1)).Verifiable();

            var result = _hotelService.Delete(1);

            Assert.That(result,Is.True);
            _hotelRepositoryMock.Verify(repo => repo.Delete(1), Times.Once);
        }

        [Test]
        public void Delete_NonExistingHotel_ShouldReturnFalse()
        {
            _hotelRepositoryMock.Setup(repo => repo.GetById(1)).Returns((Data.Models.Hotel) null);

            var result = _hotelService.Delete(1);

            Assert.That(result, Is.False);
        }
    }
}
