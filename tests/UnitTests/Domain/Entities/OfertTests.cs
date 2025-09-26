namespace LimbooCards.UnitTests.Domain.Entities
{
    using LimbooCards.Domain.Entities;
    using Xunit;

    public class OfertTests
    {
        [Fact]
        public void Ofert_ShouldInitializeProperties_WhenValid()
        {
            var ofert = new Ofert("DIG", "AE");

            Assert.Equal("DIG", ofert.Project);
            Assert.Equal("AE", ofert.Module);
        }

        [Theory]
        [InlineData("")]
        [InlineData("PRJT")]
        public void Ofert_ShouldThrow_WhenProjectInvalid(string invalidProject)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Ofert(invalidProject, "M1"));
            Assert.Contains("Project", ex.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("MOD")]
        public void Ofert_ShouldThrow_WhenModuleInvalid(string invalidModule)
        {
            var ex = Assert.Throws<ArgumentException>(() => new Ofert("P1", invalidModule));
            Assert.Contains("Module", ex.Message);
        }
    }
}
