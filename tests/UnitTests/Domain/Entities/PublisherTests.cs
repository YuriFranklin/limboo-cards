namespace LimbooCards.UnitTests.Domain.Entities
{
    using LimbooCards.Domain.Entities;

    public class PublisherTests
    {
        [Fact]
        public void Publisher_ShouldInitializeProperties()
        {
            var id = Guid.CreateVersion7();
            var publisher = new Publisher(id, "OReilly");

            Assert.Equal(id, publisher.Id);
            Assert.Equal("OReilly", publisher.Name);
        }

        [Fact]
        public void Publisher_ShouldThrow_WhenIdIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var publisher = new Publisher(Guid.Empty, "Packt");
            });

            Assert.Contains("Id must be set.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Publisher_ShouldThrow_WhenNameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var publisher = new Publisher(Guid.CreateVersion7(), "");
            });

            Assert.Contains("Name cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Publisher_ShouldThrow_WhenNameIsWhitespace()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var publisher = new Publisher(Guid.CreateVersion7(), "   ");
            });

            Assert.Contains("Name cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Publisher_ShouldThrow_WhenNameIsNull()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var publisher = new Publisher(Guid.CreateVersion7(), null!);
            });

            Assert.Contains("Name cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }
    }
}
