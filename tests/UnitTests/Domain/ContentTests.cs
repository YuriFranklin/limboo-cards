namespace LimbooCards.UnitTests.Domain
{
    public class ContentTests
    {
        [Fact]
        public void Content_ShouldInitialize_WithStatus()
        {
            var content = new Content(
                name: "Content Name",
                checklistItemTitle: "Checklist Item",
                status: ContentStatus.OK
            );

            Assert.Equal("Content Name", content.Name);
            Assert.Equal("Checklist Item", content.ChecklistItemTitle);
            Assert.Equal(ContentStatus.OK, content.ContentStatus);
        }

        [Fact]
        public void Content_ShouldInitialize_WithoutStatus()
        {
            var content = new Content(
                name: "Content Name",
                checklistItemTitle: "Checklist Item"
            );

            Assert.Equal("Content Name", content.Name);
            Assert.Equal("Checklist Item", content.ChecklistItemTitle);
            Assert.Null(content.ContentStatus);
        }

        [Fact]
        public void Content_ShouldThrow_WhenNameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var content = new Content(
                    name: "",
                    checklistItemTitle: "Checklist Item"
                );
            });

            Assert.Contains("Name cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Content_ShouldThrow_WhenChecklistItemTitleIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var content = new Content(
                    name: "Content Name",
                    checklistItemTitle: ""
                );
            });

            Assert.Contains("ChecklistItemTitle cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }
    }
}
