namespace LimbooCards.UnitTests
{
    public class CardTests
    {
        [Fact]
        public void Card_ShouldInitializeProperties()
        {
            var createdBy = Guid.NewGuid();
            var card = new Card(
                title: "Test Card",
                description: "Some description",
                hasDescription: true,
                createdBy: createdBy
            );

            Assert.NotNull(card);
            Assert.Equal("Test Card", card.Title);
            Assert.Equal("Some description", card.Description);
            Assert.True(card.HasDescription);
            Assert.Equal(createdBy, card.CreatedBy);
            Assert.NotEqual(Guid.Empty, card.Id);
            Assert.True(card.CreatedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void Card_ShouldThrow_WhenTitleIsEmpty()
        {
            var createdBy = Guid.NewGuid();

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var card = new Card(
                    title: "",
                    description: "desc",
                    hasDescription: true,
                    createdBy: createdBy
                );
            });

            Assert.Contains("Title cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Card_ShouldThrow_WhenDueDateBeforeCreatedAt()
        {
            var createdBy = Guid.NewGuid();
            var createdAt = DateTime.UtcNow;
            var dueDate = createdAt.AddMinutes(-5);

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var card = new Card(
                    title: "Title",
                    description: "Desc",
                    hasDescription: true,
                    createdBy: createdBy,
                    createdAt: createdAt,
                    dueDateTime: dueDate
                );
            });

            Assert.Contains("Due date cannot be earlier than the creation date.", ex.Message.Split(Environment.NewLine)[0]);
        }
    }
}
