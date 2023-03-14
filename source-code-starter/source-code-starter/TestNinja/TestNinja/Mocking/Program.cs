

namespace TestNinja.Mocking
{
    public class Program
    {
        public static void Main()
        {
            var service = new VideoService();
            var videoTitle = service.ReadVideoTitle();
        }
    }
}
