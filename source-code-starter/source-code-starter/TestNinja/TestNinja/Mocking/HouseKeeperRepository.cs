

using System.Linq;

namespace TestNinja.Mocking
{
    public interface IHouseKeeperRepository
    {
        IQueryable<Housekeeper> GetHousekeepers();
    }

    public class HouseKeeperRepository : IHouseKeeperRepository
    {
        private static readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public IQueryable<Housekeeper> GetHousekeepers()
        {
            return UnitOfWork.Query<Housekeeper>();
        }
    }
}
