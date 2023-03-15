
using Moq;
using NUnit.Framework;
using System.Net;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        private InstallerHelper _installerHelper;
        private Mock<IFileDownloader> _fileDownloader;

        [SetUp]
        public void Setup() 
        {
            _fileDownloader= new Mock<IFileDownloader>();
            _installerHelper = new InstallerHelper(_fileDownloader.Object, @"D:\");
        }

        [Test]
        [TestCase("fef1706276640fa2f99a5a4", "file-sample_150kB.pdf")]
        public void DownloadInstaller_DownloadComplete_ReturnTrue(string customerName, string installerName)
        {
            var result = _installerHelper.DownloadInstaller(customerName, installerName);

            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase("fefjhyuiyui671706276640fa2f99a5a4", "file-sample_150kB.pdf")]
        public void DownloadInstaller_DownloadFails_ReturnFalse(string customerName, string installerName)
        {
            // _fileDownloader.Setup(x => x.DownloadFile("", "")).Throws<WebException>();

            /*            _fileDownloader.Setup(
                            x => x.DownloadFile("https://file-examples.com/storage/fefjhyuiyui671706276640fa2f99a5a4/2017/10/file-sample_150kB.pdf", @"D:\")).Throws<WebException>();
            */

            _fileDownloader.Setup(x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>()))
                                        .Throws<WebException>();

            var result = _installerHelper.DownloadInstaller(customerName, installerName);

            Assert.That(result, Is.False);
        }
    }
}
