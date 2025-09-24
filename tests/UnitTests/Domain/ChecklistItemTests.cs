namespace LimbooCards.UnitTests.Domain
{
    public class ChecklistItemTests
    {
        [Fact]
        public void ChecklistItem_ShouldInitializeProperties()
        {
            var updatedBy = Guid.CreateVersion7().ToString();
            var updatedAt = DateTime.UtcNow;

            var item = new ChecklistItem(
                id: "1",
                title: "Item 1",
                isChecked: false,
                orderHint: "A",
                updatedAt: updatedAt,
                updatedBy: updatedBy
            );

            Assert.NotNull(item);
            Assert.Equal("Item 1", item.Title);
            Assert.False(item.IsChecked);
            Assert.Equal("A", item.OrderHint);
            Assert.Equal(updatedAt, item.UpdatedAt);
            Assert.Equal(updatedBy, item.UpdatedBy);
        }

        [Fact]
        public void ChecklistItem_ShouldThrow_WhenTitleIsEmpty()
        {
            var updatedBy = Guid.CreateVersion7().ToString();
            var updatedAt = DateTime.UtcNow;

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var item = new ChecklistItem(
                    id: "1",
                    title: "",
                    isChecked: false,
                    orderHint: "A",
                    updatedAt: updatedAt,
                    updatedBy: updatedBy
                );
            });

            Assert.Contains("Title cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void ChecklistItem_ShouldThrow_WhenOrderHintIsEmpty()
        {
            var updatedBy = Guid.CreateVersion7().ToString();
            var updatedAt = DateTime.UtcNow;

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var item = new ChecklistItem(
                    id: "1",
                    title: "Item 1",
                    isChecked: false,
                    orderHint: "",
                    updatedAt: updatedAt,
                    updatedBy: updatedBy
                );
            });

            Assert.Contains("OrderHint cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void ChecklistItem_ShouldThrow_WhenUpdatedAtIsDefault()
        {
            var updatedBy = Guid.CreateVersion7().ToString();

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var item = new ChecklistItem(
                    id: "1",
                    title: "Item 1",
                    isChecked: false,
                    orderHint: "A",
                    updatedAt: default,
                    updatedBy: updatedBy
                );
            });

            Assert.Contains("UpdatedAt must be set.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void ChecklistItem_ShouldThrow_WhenUpdatedByIsEmpty()
        {
            var updatedAt = DateTime.UtcNow;

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var item = new ChecklistItem(
                    id: "1",
                    title: "Item 1",
                    isChecked: false,
                    orderHint: "A",
                    updatedAt: updatedAt,
                    updatedBy: string.Empty
                );
            });

            Assert.Contains("UpdatedBy must be set.", ex.Message.Split(Environment.NewLine)[0]);
        }
    }
}
