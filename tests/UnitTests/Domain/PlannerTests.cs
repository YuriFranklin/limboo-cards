namespace LimbooCards.UnitTests.Domain
{
    public class PlannerTests
    {
        [Fact]
        public void Constructor_ShouldCreatePlanner_WhenValidData()
        {
            var id = Guid.NewGuid();
            var buckets = new List<PlannerBucket>
            {
                new PlannerBucket(Guid.NewGuid(), "Default Bucket", true)
            };

            var planner = new Planner(id, "My Planner", buckets);

            Assert.Equal(id, planner.Id);
            Assert.Equal("My Planner", planner.Name);
            Assert.Single(planner.Buckets);
            Assert.True(planner.Buckets[0].IsDefault);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrow_WhenNameIsInvalid(string? invalidName)
        {
            var id = Guid.NewGuid();
            var buckets = new List<PlannerBucket>
            {
                new PlannerBucket(Guid.NewGuid(), "Default Bucket", true)
            };

            var ex = Assert.Throws<ArgumentException>(() => new Planner(id, invalidName!, buckets));
            Assert.Contains("Planner name cannot be empty.", ex.Message);
            Assert.Equal("Name", ex.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenBucketsIsNull()
        {
            var id = Guid.NewGuid();

            var ex = Assert.Throws<ArgumentException>(() => new Planner(id, "Valid Name", null));
            Assert.Contains("Planner must contain at least one bucket.", ex.Message);
            Assert.Equal("Buckets", ex.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenBucketsIsEmpty()
        {
            var id = Guid.NewGuid();

            var ex = Assert.Throws<ArgumentException>(() => new Planner(id, "Valid Name", new List<PlannerBucket>()));
            Assert.Contains("Planner must contain at least one bucket.", ex.Message);
            Assert.Equal("Buckets", ex.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenNoDefaultBucketExists()
        {
            var id = Guid.NewGuid();
            var buckets = new List<PlannerBucket>
            {
                new PlannerBucket(Guid.NewGuid(), "Non Default", false)
            };

            var ex = Assert.Throws<ArgumentException>(() => new Planner(id, "Valid Name", buckets));
            Assert.Contains("Planner must contain at least one default bucket.", ex.Message);
            Assert.Equal("Buckets", ex.ParamName);
        }
    }
}
