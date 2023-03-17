
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;
using System;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelper_OverlappingBookingsExistTests
    {
        private Mock<IBookingRepository> _bookingRepository;

        [SetUp] 
        public void SetUp() 
        {
            _bookingRepository = new Mock<IBookingRepository>();
        }

        [Test]
        public void BookingStartsAndFinishesBeforeAnExistingBooking_ReturnEmptyString()
        {
            var booking = new Booking { Id = 1, ArrivalDate = new DateTime(2023, 3, 17, 14, 0, 0), DepartureDate = new DateTime(2023, 3, 22, 10, 0, 0) };
            var overlappedBookings = new List<Booking>
            {
                new Booking{ Id = 2, ArrivalDate=new DateTime(2023, 3, 21, 14, 0, 0), DepartureDate=new DateTime(2023, 3, 25, 10, 0, 0), Reference = "ID-2"},
                new Booking{ Id = 3, ArrivalDate=new DateTime(2023, 3, 24, 14, 0, 0), DepartureDate=new DateTime(2023, 3, 27, 10, 0, 0), Reference = "ID-3"}
            }.AsQueryable();

            _bookingRepository.Setup(x => x.GetActiveBookings(booking.Id)).Returns(overlappedBookings);

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }
}
