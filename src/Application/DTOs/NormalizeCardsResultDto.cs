namespace LimbooCards.Application.DTOs
{
    public class NormalizeCardsResultDto
    {
        public List<CardDto>? Success { get; set; }
        public List<string>? Failed { get; set; }
    }

}