namespace LimbooCards.Domain.Shared
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents the status of a subject.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubjectStatus
    {
        /// <summary>The subject is missing.</summary>
        Missing,

        /// <summary>The subject is incomplete.</summary>
        Incomplete,

        /// <summary>The subject is complete.</summary>
        Complete,
    }
}