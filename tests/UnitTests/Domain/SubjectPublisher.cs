namespace LimbooCards.UnitTests.Domain
{
    using LimbooCards.Domain.Entities;

    public class SubjectPublisherTests
    {
        [Fact]
        public void SubjectPublisher_ShouldInitializeProperties()
        {
            var publisher = new SubjectPublisher("PublisherA", isCurrent: true, isExpect: false);

            Assert.Equal("PublisherA", publisher.Name);
            Assert.True(publisher.IsCurrent);
            Assert.False(publisher.IsExpect);
        }

        [Fact]
        public void SubjectPublisher_ShouldDefaultFlagsToFalse()
        {
            var publisher = new SubjectPublisher("PublisherB");

            Assert.Equal("PublisherB", publisher.Name);
            Assert.False(publisher.IsCurrent ?? false);
            Assert.False(publisher.IsExpect ?? false);
        }

        [Fact]
        public void SubjectPublisher_ShouldThrow_WhenNameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var publisher = new SubjectPublisher("");
            });

            Assert.Contains("Name cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void SubjectPublisher_ShouldThrow_WhenNameIsWhitespace()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var publisher = new SubjectPublisher("   ");
            });

            Assert.Contains("Name cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void SubjectPublisher_ShouldAccept_WhenOnlyIsExpectIsTrue()
        {
            var publisher = new SubjectPublisher("PublisherC", isExpect: true);

            Assert.Equal("PublisherC", publisher.Name);
            Assert.True(publisher.IsExpect);
            Assert.False(publisher.IsCurrent ?? false);
        }
    }
}
