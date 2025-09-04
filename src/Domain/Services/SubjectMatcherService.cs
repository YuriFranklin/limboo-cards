using LimbooCards.Domain.Entities;

namespace LimbooCards.Domain.Services
{
    public class SubjectMatcherService
    {
        public static Subject? MatchSubjectForCard(Card card, IEnumerable<Subject> subjects)
        {
            return subjects.FirstOrDefault(s => card.Title.Contains(s.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}