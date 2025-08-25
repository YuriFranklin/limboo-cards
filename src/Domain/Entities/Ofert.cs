namespace LimbooCards.Domain.Entities
{
    public class Ofert
    {
        public Ofert(string project, string module)
        {
            this.Project = project;
            this.Module = module;

            this.Validate();
        }

        public string Project { get; private set; }
        public string Module { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Project) || this.Project.Length > 3)
            {
                throw new ArgumentException("Project must be non-empty and contain at most 3 characters.", nameof(this.Project));
            }

            if (string.IsNullOrWhiteSpace(this.Module) || this.Module.Length > 2)
            {
                throw new ArgumentException("Module must be non-empty and contain at most 2 characters.", nameof(this.Module));
            }
        }
    }
}
