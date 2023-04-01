using System;

namespace TestNinja.Mocking
{
    public class HousekeeperService
    {
        private readonly IStatementSaver _statementSaver;
        private readonly IEmailService _emailService;
        private readonly IHouseKeeperRepository _houseKeeperRepository;
        private readonly IXtraMessageBox _xtraMessageBox;

        public HousekeeperService(IHouseKeeperRepository houseKeeperRepository, 
                                IStatementSaver statementSaver, 
                                IEmailService emailService,
                                IXtraMessageBox xtraMessageBox
            ) 
        {
            _statementSaver = statementSaver;
            _emailService = emailService;
            _houseKeeperRepository = houseKeeperRepository;
            _xtraMessageBox = xtraMessageBox;
        }

        public bool SendStatementEmails(DateTime statementDate)
        {
            var housekeepers = _houseKeeperRepository.GetHousekeepers();

            foreach (var housekeeper in housekeepers)
            {
                if (string.IsNullOrWhiteSpace(housekeeper.Email))
                    continue;

                var statementFilename = _statementSaver.SaveStatement(housekeeper.Oid, housekeeper.FullName, statementDate);

                if (string.IsNullOrWhiteSpace(statementFilename))
                    continue;

                var emailAddress = housekeeper.Email;
                var emailBody = housekeeper.StatementEmailBody;

                try
                {
                    _emailService.EmailFile(emailAddress, emailBody, statementFilename,
                        string.Format("Sandpiper Statement {0:yyyy-MM} {1}", statementDate, housekeeper.FullName));
                }
                catch (Exception e)
                {
                    _xtraMessageBox.Show(e.Message, string.Format("Email failure: {0}", emailAddress),
                        MessageBoxButtons.OK);
                }
            }

            return true;
        }
    }

    public enum MessageBoxButtons
    {
        OK
    }

    public interface IXtraMessageBox
    {
        void Show(string s, string housekeeperStatements, MessageBoxButtons ok);
    }

    public class XtraMessageBox : IXtraMessageBox
    {
        public void Show(string s, string housekeeperStatements, MessageBoxButtons ok)
        {
        }
    }

    public class Housekeeper
    {
        public string Email { get; set; }
        public int Oid { get; set; }
        public string FullName { get; set; }
        public string StatementEmailBody { get; set; }
    }
}