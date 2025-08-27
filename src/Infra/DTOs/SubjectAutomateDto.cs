namespace LimbooCards.Infra.DTOs
{
    public class SubjectAutomateDto
    {
        public string? ID { get; set; }
        public string? DISCIPLINA { get; set; }
        public string? EQUIVALENCIA { get; set; }
        public string? EDITORA_MASTER { get; set; }
        public string? EDITORA_NASA { get; set; }
        public string? OFERTAS { get; set; }
        public string? AGENTE_DIG { get; set; }
        public string? STATUS_DIG { get; set; }
        public string? OBS_DIG { get; set; }
        public string? BANNER { get; set; }
        public string? DC { get; set; }
        public string? AC { get; set; }
        public string? VIDEOTECA_1 { get; set; }
        public string? VIDEOTECA_2 { get; set; }
        public string? VIDEOTECA_3 { get; set; }
        public string? VIDEOTECA_4 { get; set; }
        public string? E_BOOK { get; set; }
        public string? MATERIAL_1 { get; set; }
        public string? MATERIAL_2 { get; set; }
        public string? MATERIAL_3 { get; set; }
        public string? MATERIAL_4 { get; set; }
        public string? BQ_1 { get; set; }
        public string? BQ_2 { get; set; }
        public string? BQ_3 { get; set; }
        public string? BQ_4 { get; set; }
        public string? UUID { get; set; }
        public List<UserAutomateDto>? OWNERS { get; set; }
        public List<SubjectPublisherAutomateDto>? PUBLISHERS { get; set; }
        public string? EXTRA_PROPS { get; set; }
    }
}
