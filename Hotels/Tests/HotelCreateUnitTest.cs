using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Hotels.ViewData;
using NUnit.Framework.Legacy;

namespace Hotels.Tests
{
    [TestFixture]
    public class HotelCreateUnitTest
    {
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }

        [Test]
        public void HotelCreate_ValidModel()
        {
            var model = new HotelCreate
            {
                Name = "Valid Hotel",
                Price = 100.50,
                Latitude = 45.12,
                Longitude = -93.45
            };

            var results = ValidateModel(model);
            CollectionAssert.IsEmpty(results);
        }

        [Test]
        public void HotelCreate_InvalidName()
        {
            var model = new HotelCreate
            {
                Name = "This name is way too long for validation",
                Price = 100.50,
                Latitude = 45.12,
                Longitude = -93.45
            };

            var results = ValidateModel(model);
            CollectionAssert.IsNotEmpty(results);
            Assert.That(results.Any(v => v.ErrorMessage.Contains("Name must be less than 20 characters")));
        }

        [Test]
        public void HotelCreate_InvalidPrice()
        {
            var model = new HotelCreate
            {
                Name = "Valid Hotel",
                Price = -10.00,
                Latitude = 45.12,
                Longitude = -93.45
            };

            var results = ValidateModel(model);
            CollectionAssert.IsNotEmpty(results);
            Assert.That(results.Any(v => v.ErrorMessage.Contains("Price must be greater than 0")));
        }

        [Test]
        public void HotelCreate_InvalidPrice_DecimalPoints()
        {
            var model = new HotelCreate
            {
                Name = "Valid Hotel",
                Price = 10.00456,
                Latitude = 45.12,
                Longitude = -93.45
            };

            var results = ValidateModel(model);
            CollectionAssert.IsNotEmpty(results);
            Assert.That(results.Any(v => v.ErrorMessage.Contains("Price must have up to 2 decimal places.")));
        }

        [Test]
        public void HotelCreate_InvalidLatitude()
        {
            var model = new HotelCreate
            {
                Name = "Valid Hotel",
                Price = 100.50,
                Latitude = 100.00,
                Longitude = -93.45
            };

            var results = ValidateModel(model);
            CollectionAssert.IsNotEmpty(results);
            Assert.That(results.Any(v => v.ErrorMessage.Contains("Latitude must be between -90 and 90")));
        }

        [Test]
        public void HotelCreate_InvalidLatitude_LowValue()
        {
            var model = new HotelCreate
            {
                Name = "Valid Hotel",
                Price = 100.50,
                Latitude = -100.00,
                Longitude = -93.45
            };

            var results = ValidateModel(model);
            CollectionAssert.IsNotEmpty(results);
            Assert.That(results.Any(v => v.ErrorMessage.Contains("Latitude must be between -90 and 90")));
        }


        [Test]
        public void HotelCreate_InvalidLatitude_DecimalPoints()
        {
            var model = new HotelCreate
            {
                Name = "Valid Hotel",
                Price = 100.50,
                Latitude = -45.005,
                Longitude = -93.45
            };

            var results = ValidateModel(model);
            CollectionAssert.IsNotEmpty(results);
            Assert.That(results.Any(v => v.ErrorMessage.Contains("Latitude must have up to 2 decimal places.")));
        }

        [Test]
        public void HotelCreate_InvalidLongitude()
        {
            var model = new HotelCreate
            {
                Name = "Valid Hotel",
                Price = 100.50,
                Latitude = 45.12,
                Longitude = 200.00
            };

            var results = ValidateModel(model);
            CollectionAssert.IsNotEmpty(results);
            Assert.That(results.Any(v => v.ErrorMessage.Contains("Longitude must be between -180 and 180")));
        }

        [Test]
        public void HotelCreate_InvalidLongitude_LowValue()
        {
            var model = new HotelCreate
            {
                Name = "Valid Hotel",
                Price = 100.50,
                Latitude = 45.12,
                Longitude = -200.00
            };

            var results = ValidateModel(model);
            CollectionAssert.IsNotEmpty(results);
            Assert.That(results.Any(v => v.ErrorMessage.Contains("Longitude must be between -180 and 180")));
        }

        [Test]
        public void HotelCreate_InvalidLongitude_DecimalPoints()
        {
            var model = new HotelCreate
            {
                Name = "Valid Hotel",
                Price = 100.50,
                Latitude = 45.12,
                Longitude = 20.000001
            };

            var results = ValidateModel(model);
            CollectionAssert.IsNotEmpty(results);
            Assert.That(results.Any(v => v.ErrorMessage.Contains("Longitude must have up to 2 decimal places.")));
        }
    }
}
