namespace LimbooCards.Domain.Shared
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents the status of content.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContentStatus
    {
        /// <summary>The content is OK.</summary>
        OK,

        /// <summary>The content is Missing.</summary>
        Missing
    }
}