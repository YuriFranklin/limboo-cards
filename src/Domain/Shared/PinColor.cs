namespace LimbooCards.Domain.Shared
{
    using System.Runtime.Serialization;
    public enum PinColor
    {
        [EnumMember(Value = "blue")]
        Blue,

        [EnumMember(Value = "red")]
        Red,

        [EnumMember(Value = "green")]
        Green,

        [EnumMember(Value = "yellow")]
        Yellow,

        [EnumMember(Value = "purple")]
        Purple,

        [EnumMember(Value = "orange")]
        Orange,

        [EnumMember(Value = "gray")]
        Gray
    }
}
