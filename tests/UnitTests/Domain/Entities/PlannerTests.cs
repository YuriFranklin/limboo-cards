using LimbooCards.Domain.Entities;
using Xunit;

namespace LimbooCards.UnitTests.Domain.Entities
{
    public class PlannerTests
    {
        private static List<PlannerBucket> CreateValidBuckets()
        {
            return new List<PlannerBucket>
            {
                new(Guid.NewGuid().ToString(), "Default Bucket", isDefault: true),
                new(Guid.NewGuid().ToString(), "End Bucket", isEnd: true),
                new(Guid.NewGuid().ToString(), "History Bucket", isHistory: true)
            };
        }

        [Fact]
        public void Constructor_ShouldCreatePlanner_WhenValidData()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var buckets = CreateValidBuckets();

            // Act
            var planner = new Planner(id, "My Planner", buckets);

            // Assert
            Assert.Equal(id, planner.Id);
            Assert.Equal("My Planner", planner.Name);
            Assert.Equal(3, planner.Buckets.Count);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrow_WhenNameIsInvalid(string invalidName)
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var buckets = CreateValidBuckets();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Planner(id, invalidName, buckets));
            Assert.Contains("Planner name cannot be empty.", ex.Message);
            Assert.Equal("Name", ex.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenBucketsIsEmpty()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var emptyBuckets = new List<PlannerBucket>();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Planner(id, "Valid Name", emptyBuckets));
            Assert.Contains("Planner must contain at least one bucket.", ex.Message);
            Assert.Equal("Buckets", ex.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenNoDefaultBucketExists()
        {
            var id = Guid.NewGuid().ToString();
            var bucketsWithoutDefault = new List<PlannerBucket>
            {
                new(Guid.NewGuid().ToString(), "End Bucket", isEnd: true),
                new(Guid.NewGuid().ToString(), "History Bucket", isHistory: true)
            };

            var ex = Assert.Throws<ArgumentException>(() => new Planner(id, "Valid Name", bucketsWithoutDefault));
            Assert.Contains("Planner must contain at least one default bucket.", ex.Message);
            Assert.Equal("Buckets", ex.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenNoEndBucketExists()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var bucketsWithoutEnd = new List<PlannerBucket>
            {
                new(Guid.NewGuid().ToString(), "Default Bucket", isDefault: true),
                new(Guid.NewGuid().ToString(), "History Bucket", isHistory: true)
            };

            var ex = Assert.Throws<ArgumentException>(() => new Planner(id, "Valid Name", bucketsWithoutEnd));
            Assert.Contains("Planner must contain at least one end bucket.", ex.Message);
            Assert.Equal("Buckets", ex.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenNoHistoryBucketExists()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var bucketsWithoutHistory = new List<PlannerBucket>
            {
                new(Guid.NewGuid().ToString(), "Default Bucket", isDefault: true),
                new(Guid.NewGuid().ToString(), "End Bucket", isEnd: true)
            };

            var ex = Assert.Throws<ArgumentException>(() => new Planner(id, "Valid Name", bucketsWithoutHistory));
            Assert.Contains("Planner must contain at least one history bucket.", ex.Message);
            Assert.Equal("Buckets", ex.ParamName);
        }
    }
}