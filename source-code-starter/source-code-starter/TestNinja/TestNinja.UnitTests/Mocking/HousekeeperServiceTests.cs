

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

            _houseKeeperList = new List<Housekeeper>
            {
                new Housekeeper { Oid = 1, Email="abc@gmail.com", FullName="Mithun Das", StatementEmailBody = "Test - 1" }
            
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
            var houseKeeper = _houseKeeperList.FirstOrDefault();

            _mailService.Setup(x => x.EmailFile(houseKeeper.Email, houseKeeper.StatementEmailBody, _fileName,
                        string.Format("Sandpiper Statement {0:yyyy-MM} {1}", _statementDate, houseKeeper.FullName))).Throws(new Exception());

            var result = _housekeeperHelper.SendStatementEmails(_statementDate);

            Assert.That(result, Is.False);
        }

        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatements()
        {
            var houseKeeper = _houseKeeperList.FirstOrDefault();

            var result = _housekeeperHelper.SendStatementEmails(_statementDate);

            _statementSaver.Verify(x => x.SaveStatement(houseKeeper.Oid, houseKeeper.FullName, _statementDate));
        }
    }
}
