

using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using TestNinja.Mocking;
using System.Collections.Generic;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HousekeeperServiceTests
    {
        private Mock<IHouseKeeperRepository> _houseKeeperRepository;
        private Mock<IEmailService> _mailService;
        private Mock<IStatementSaver> _statementSaver;
        private Mock<IXtraMessageBox> _xtraMessageBox;

        private IQueryable<Housekeeper> _houseKeeperList;
        private HousekeeperService _housekeeperHelper;
        private DateTime _statementDate = new DateTime(2023, 1, 1);
        private string _fileName = "File Testing - 1";
        private Housekeeper _housekeeper;

        [SetUp]
        public void Setup()
        {
            _houseKeeperRepository = new Mock<IHouseKeeperRepository>();
            _mailService = new Mock<IEmailService>();
            _statementSaver = new Mock<IStatementSaver>();
            _xtraMessageBox = new Mock<IXtraMessageBox>();

            _housekeeperHelper = new HousekeeperService(_houseKeeperRepository.Object,
                                            _statementSaver.Object,
                                            _mailService.Object,
                                            _xtraMessageBox.Object);

            _housekeeper = new Housekeeper { Oid = 1, Email = "abc@gmail.com", FullName = "Mithun Das", StatementEmailBody = "Test - 1" };
            _houseKeeperList = new List<Housekeeper>
            {
                _housekeeper
            }.AsQueryable();

            _houseKeeperRepository.Setup(x => x.GetHousekeepers()).Returns(_houseKeeperList);
            _statementSaver.Setup(x => x.SaveStatement(1, "Mithun Das", _statementDate)).Returns(_fileName);
        }

        [Test]
        public void SendStatementEmails_EmailFile_VerifyEmailCall() 
        {
            var houseKeeper = _houseKeeperList.FirstOrDefault();

            _housekeeperHelper.SendStatementEmails(_statementDate);

            _mailService.Verify(x => x.EmailFile(houseKeeper.Email, houseKeeper.StatementEmailBody, _fileName,
                        string.Format("Sandpiper Statement {0:yyyy-MM} {1}", _statementDate, houseKeeper.FullName)));
        }

        [Test]
        public void SendStatementEmails_EmailFile_ThrowException()
        {
            _mailService.Setup(x => x.EmailFile(_housekeeper.Email, _housekeeper.StatementEmailBody, _fileName,
                        string.Format("Sandpiper Statement {0:yyyy-MM} {1}", _statementDate, _housekeeper.FullName))).Throws(new Exception());

            var result = _housekeeperHelper.SendStatementEmails(_statementDate);

            Assert.That(result, Is.False);
        }

        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatements()
        {
            var result = _housekeeperHelper.SendStatementEmails(_statementDate);

            _statementSaver.Verify(x => x.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate));
        }

        [Test]
        public void SendStatementEmails_HouseKeeperEmailIsNull_ShouldNotGenerateStatements()
        {
            _housekeeper.Email = null;

            var result = _housekeeperHelper.SendStatementEmails(_statementDate);

            _statementSaver.Verify(x => x.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate),
                Times.Never);
        }

        [Test]
        public void SendStatementEmails_HouseKeeperEmailIsWhiteSpace_ShouldNotGenerateStatements()
        {
            _housekeeper.Email = " ";

            var result = _housekeeperHelper.SendStatementEmails(_statementDate);

            _statementSaver.Verify(x => x.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate),
                Times.Never);
        }

        [Test]
        public void SendStatementEmails_HouseKeeperEmailIsEmptyString_ShouldNotGenerateStatements()
        {
            _housekeeper.Email = "";

            var result = _housekeeperHelper.SendStatementEmails(_statementDate);

            _statementSaver.Verify(x => x.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate),
                Times.Never);
        }
    }
}
