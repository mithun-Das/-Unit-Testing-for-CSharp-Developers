using System.Net;

namespace TestNinja.Mocking
{
    public class InstallerHelper
    {
        private string _setupDestinationFile;
        public readonly IFileDownloader _fileDownloader;

        public InstallerHelper(IFileDownloader fileDownloader, string setupDestinationFile)
        {
            _setupDestinationFile = setupDestinationFile;
            _fileDownloader = fileDownloader;
        }

        public bool DownloadInstaller(string customerName, string installerName)
        {
            try
            {
                var url = string.Format("https://file-examples.com/storage/{0}/2017/10/{1}", customerName, installerName);
                _fileDownloader.DownloadFile(url, _setupDestinationFile);

                return true;
           }
            catch (WebException)
            {
                return false;
            }
        }
    }
}