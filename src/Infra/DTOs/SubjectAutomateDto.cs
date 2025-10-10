using System.Text.Json.Serialization;

namespace LimbooCards.Infra.DTOs
{
    public class SubjectAutomateDto
    {
        [JsonPropertyName("ID")]
        public string? Id { get; set; }

        [JsonPropertyName("DISCIPLINA")]
        public string? Disciplina { get; set; }

        [JsonPropertyName("EQUIVALENCIA")]
        public string? Equivalencia { get; set; }

        [JsonPropertyName("EDITORA_MASTER")]
        public string? EditoraMaster { get; set; }

        [JsonPropertyName("EDITORA_NASA")]
        public string? EditoraNasa { get; set; }

        [JsonPropertyName("OFERTAS")]
        public string? Ofertas { get; set; }

        [JsonPropertyName("PRATICA")]
        public string? Pratica { get; set; }

        [JsonPropertyName("AGENTES")]
        public string? Agentes { get; set; }

        [JsonPropertyName("STATUS")]
        public string? Status { get; set; }

        [JsonPropertyName("OBS")]
        public string? Obs { get; set; }

        [JsonPropertyName("PDFS")]
        public string? Pdfs { get; set; }

        [JsonPropertyName("SCORM_1")]
        public string? Scorm1 { get; set; }

        [JsonPropertyName("SCORM_2")]
        public string? Scorm2 { get; set; }

        [JsonPropertyName("SCORM_3")]
        public string? Scorm3 { get; set; }

        [JsonPropertyName("SCORM_4")]
        public string? Scorm4 { get; set; }

        [JsonPropertyName("VIDEO_1")]
        public string? Video1 { get; set; }

        [JsonPropertyName("VIDEO_2")]
        public string? Video2 { get; set; }

        [JsonPropertyName("VIDEO_3")]
        public string? Video3 { get; set; }

        [JsonPropertyName("VIDEO_4")]
        public string? Video4 { get; set; }

        [JsonPropertyName("PLANO_AULA")]
        public string? PlanoAula { get; set; }

        [JsonPropertyName("UUID")]
        public string? Uuid { get; set; }

        [JsonPropertyName("OWNERS")]
        public List<UserAutomateDto>? Owners { get; set; }

        [JsonPropertyName("PUBLISHERS")]
        public List<SubjectPublisherAutomateDto>? Publishers { get; set; }

        [JsonPropertyName("EXTRA_PROPS")]
        public string? ExtraProps { get; set; }
    }
}