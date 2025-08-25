namespace LimbooCards.UnitTests
{
    public class NoteTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateNote()
        {
            var text = "My first note";
            var createdBy = Guid.NewGuid();
            var createdAt = DateTime.UtcNow;

            var note = new Note(text, createdBy, createdAt);

            Assert.Equal(text, note.Text);
            Assert.Equal(createdBy, note.CreatedBy);
            Assert.Equal(createdAt, note.CreatedAt);
            Assert.Null(note.EditedAt);
        }

        [Fact]
        public void Constructor_EmptyText_ShouldThrowArgumentException()
        {
            var createdBy = Guid.NewGuid();

            var exception = Assert.Throws<ArgumentException>(() => new Note("", createdBy));
            Assert.Contains("Text cannot be empty", exception.Message);
        }

        [Fact]
        public void Constructor_EmptyGuidCreatedBy_ShouldThrowArgumentException()
        {
            var text = "Valid text";

            var exception = Assert.Throws<ArgumentException>(() => new Note(text, Guid.Empty));
            Assert.Contains("CreatedBy must be set", exception.Message);
        }

        [Fact]
        public void Edit_WithValidText_ShouldUpdateTextAndEditedAt()
        {
            var note = new Note("Original text", Guid.NewGuid());

            note.Edit("Updated text");

            Assert.Equal("Updated text", note.Text);
            Assert.NotNull(note.EditedAt);
        }

        [Fact]
        public void Edit_WithEmptyText_ShouldThrowArgumentException()
        {
            var note = new Note("Original text", Guid.NewGuid());

            var exception = Assert.Throws<ArgumentException>(() => note.Edit(""));
            Assert.Contains("Text cannot be empty", exception.Message);
        }
    }
}
