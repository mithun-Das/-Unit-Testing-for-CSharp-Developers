

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace TestNinja.Mocking
{
    public class VideoContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
    }

    public interface IVideoRepository
    {
        IEnumerable<Video> GetUnprocessedVideos();
    }
    public class VideoRepository : IVideoRepository
    {
        public IEnumerable<Video> GetUnprocessedVideos()
        {
            using (var context = new VideoContext())
            {
                var videos = (from video in context.Videos
                         where !video.IsProcessed
                         select video).ToList();

                return videos;
            }

        }
    }
}
