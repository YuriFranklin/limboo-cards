namespace LimbooCards.UnitTests
{
    using OneOf;
    public class ContentTests
    {
        [Fact]
        public void Content_ShouldInitializeWithStringValue()
        {
            var content = new Content(
                name: "Content Name",
                checklistItemTitle: "Checklist Item",
                value: OneOf<string, bool>.FromT0("Some value")
            );

            Assert.Equal("Content Name", content.Name);
            Assert.Equal("Checklist Item", content.ChecklistItemTitle);
            Assert.True(content.Value.IsT0);
            Assert.Equal("Some value", content.Value.AsT0);
        }

        [Fact]
        public void Content_ShouldInitializeWithBoolValue()
        {
            var content = new Content(
                name: "Content Name",
                checklistItemTitle: "Checklist Item",
                value: OneOf<string, bool>.FromT1(true)
            );

            Assert.Equal("Content Name", content.Name);
            Assert.Equal("Checklist Item", content.ChecklistItemTitle);
            Assert.True(content.Value.IsT1);
            Assert.True(content.Value.AsT1);
        }

        [Fact]
        public void Content_ShouldThrow_WhenNameIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var content = new Content(
                    name: "",
                    checklistItemTitle: "Checklist Item",
                    value: OneOf<string, bool>.FromT0("Value")
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
                    checklistItemTitle: "",
                    value: OneOf<string, bool>.FromT0("Value")
                );
            });

            Assert.Contains("ChecklistItemTitle cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }

        [Fact]
        public void Content_ShouldThrow_WhenStringValueIsEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var content = new Content(
                    name: "Content Name",
                    checklistItemTitle: "Checklist Item",
                    value: OneOf<string, bool>.FromT0("")
                );
            });

            Assert.Contains("Value (string) cannot be empty.", ex.Message.Split(Environment.NewLine)[0]);
        }
    }
}
