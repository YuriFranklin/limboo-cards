namespace LimbooCards.Domain.Services
{
    using LimbooCards.Domain.Entities;
    public class CardTitleNormalizeService
    {
        public static string Normalize(Subject subject)
        {
            return $"[PENDÊNCIA - {subject.ModelId}] {subject.Name.ToUpper()}";
        }
    }

}