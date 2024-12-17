namespace Mako.Services.Shared
{
    public partial class SharedService
    {
        MakoDbContext _dbContext;

        public SharedService(MakoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
