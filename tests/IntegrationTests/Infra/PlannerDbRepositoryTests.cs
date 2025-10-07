namespace LimbooCards.IntegrationTests.Infra
{
    public class PlannerDbRepositoryTests
    {
        private readonly PlannerDbRepository _repository;

        public PlannerDbRepositoryTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            { });

            var mapper = mapperConfig.CreateMapper();

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            var context = new AppDbContext(options);

            _repository = new PlannerDbRepository(context, mapper);
        }

        [Fact]
        public async Task GetPlannerById_ShouldReturnPlanner()
        {
            var plannerId = "r69txn4te023WTwwj8jHL2QAFVIN";

            var planner = await _repository.GetPlannerByIdAsync(plannerId);

            Assert.NotNull(planner);
            Assert.Equal(plannerId, planner.Id);
        }
    }
}