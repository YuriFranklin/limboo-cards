namespace Domain.Tests
{
    using LimbooCards.Domain.Entities;
    using Xunit;

    public class NoteTests
    {
        [Fact]
        public void Should_Create_Note_With_ValidData()
        {
            var createdBy = Guid.NewGuid();
            var note = new Note("This is a note", createdBy);

            Assert.Equal("This is a note", note.Text);
            Assert.Equal(createdBy, note.CreatedBy);
            Assert.True(note.CreatedAt <= DateTime.UtcNow);
            Assert.Null(note.EditedAt);
        }

        [Fact]
        public void Should_Throw_When_Text_IsEmpty()
        {
            var createdBy = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() =>
                new Note("", createdBy)
            );
        }

        [Fact]
        public void Should_Throw_When_CreatedBy_IsEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
                new Note("Some text", Guid.Empty)
            );
        }

        [Fact]
        public void Should_Edit_Text_Successfully()
        {
            var note = new Note("Original text", Guid.NewGuid());

            note.Edit("Updated text");

            Assert.Equal("Updated text", note.Text);
            Assert.NotNull(note.EditedAt);
            Assert.True(note.EditedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void Should_Throw_When_EditText_IsEmpty()
        {
            var note = new Note("Original text", Guid.NewGuid());

            Assert.Throws<ArgumentException>(() =>
                note.Edit("")
            );
        }
    }
}
