
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
        private IQueryable<Booking> _overlappedBookings;

        [SetUp] 
        public void SetUp() 
        {
            _bookingRepository = new Mock<IBookingRepository>();

            _overlappedBookings = new List<Booking>
            {
                new Booking{ Id = 2, ArrivalDate = ArriveOn(2023, 3, 22), DepartureDate = DepartedOn(2023, 3, 25), Reference = "ID-2"},
                new Booking{ Id = 3, ArrivalDate = ArriveOn(2023, 3, 24), DepartureDate = DepartedOn(2023, 3, 27), Reference = "ID-3"}
            
            }.AsQueryable();

            _bookingRepository.Setup(x => x.GetActiveBookings(1)).Returns(_overlappedBookings);
        }

        [Test]
        public void BookingStartsAndFinishesBeforeAnExistingBooking_ReturnEmptyString()
        {
            var booking = new Booking { Id = 1, ArrivalDate = ArriveOn(2023, 3, 17), DepartureDate = DepartedOn(2023, 3, 22) };

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void BookingStartsAndFinishesAfterExistingBooking_ReturnEmptyString()
        {
            var booking = new Booking { Id = 1, ArrivalDate = ArriveOn(2023, 3, 29), DepartureDate = DepartedOn(2023, 3, 30) };

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void BookingStartsBeforeAndFinishesInMiddleOfAnExistingBooking_ReturnReference()
        {
            var booking = new Booking { Id = 1, ArrivalDate = ArriveOn(2023, 3, 17), DepartureDate = DepartedOn(2023, 3, 23) };

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(_overlappedBookings.ElementAt(0).Reference));
        }

        [Test]
        public void BookingStartsBeforeAndFinishesAfterOfAnExistingBooking_ReturnReference()
        {
            var booking = new Booking { Id = 1, ArrivalDate = ArriveOn(2023, 3, 17), DepartureDate = DepartedOn(2023, 3, 28) };

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(_overlappedBookings.ElementAt(0).Reference));
        }

        [Test]
        public void BookingStartsInMiddleAndFinishesAfterOfAnExistingBooking_ReturnReference()
        {
            var booking = new Booking { Id = 1, ArrivalDate = ArriveOn(2023, 3, 20), DepartureDate = DepartedOn(2023, 3, 28) };

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(_overlappedBookings.ElementAt(0).Reference));
        }

        [Test]
        public void BookingStartsAndFinishesInMiddleOfAnExistingBooking_ReturnReference()
        {
            var booking = new Booking { Id = 1, ArrivalDate = ArriveOn(2023, 3, 23), DepartureDate = DepartedOn(2023, 3, 24) };

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(_overlappedBookings.ElementAt(0).Reference));
        }

        [Test]
        public void BookingCancelled_ReturnEmptyString()
        {
            var booking = new Booking { Id = 1, Status = "Cancelled" };

            var result = BookingHelper.OverlappingBookingsExist(booking, _bookingRepository.Object);

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        private DateTime ArriveOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }

        private DateTime DepartedOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 10, 0, 0);
        }
    }
}
