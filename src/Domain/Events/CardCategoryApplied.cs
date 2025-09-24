namespace LimbooCards.Domain.Events
{
    using System;
    using System.Collections.Generic;
    public class CardCategoryApplied
    {
        public CardCategoryApplied(string cardId, Dictionary<string, bool>? appliedCategories = null)
        {
            CardId = cardId;
            AppliedCategories = appliedCategories ?? new Dictionary<string, bool>();

            Validate();
        }

        public string CardId { get; }
        public Dictionary<string, bool>? AppliedCategories { get; private set; }

        private void Validate()
        {
            if (CardId == null || CardId == string.Empty)
                throw new ArgumentException("CardId cannot be empty or null.", nameof(CardId));

            if (AppliedCategories == null)
                throw new ArgumentNullException(nameof(AppliedCategories));

            foreach (var key in AppliedCategories.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("AppliedCategories cannot contain null or empty keys.", nameof(AppliedCategories));
            }
        }
    }
}
