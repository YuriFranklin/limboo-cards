namespace LimbooCards.Infra.DTOs
{
#pragma warning disable IDE1006
    public class ChecklistItemAutomateDto
    {
        public string id { get; set; } = string.Empty;
        public ChecklistValue value { get; set; } = null!;
    }

    public class ChecklistValue
    {
        public bool isChecked { get; set; }
        public string title { get; set; } = string.Empty;
        public string orderHint { get; set; } = string.Empty;
        public DateTime lastModifiedDateTime { get; set; }
        public LastModifiedBy lastModifiedBy { get; set; } = null!;
    }

    public class LastModifiedBy
    {
        public LastModifiedUser user { get; set; } = null!;
    }

    public class LastModifiedUser
    {
        public string? displayName { get; set; }
        public Guid id { get; set; }
    }
}