namespace LimbooCards.Application.DTOs
{
    using LimbooCards.Domain.Entities;
    using System;
    using System.Collections.Generic;

    public class UpdateSubjectDto
    {
        public Guid Id { get; set; }

        // Dados básicos
        public string Name { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public SubjectStatus Status { get; set; }

        public List<OfertDto>? Oferts { get; set; }
        public List<string>? Equivalencies { get; set; }
        public List<ContentDto>? Contents { get; set; }

        public Guid? OwnerId { get; set; }
        public List<Guid>? CoOwnerIds { get; set; }

        public List<SubjectPublisherDto>? Publishers { get; set; }

        public bool? IsCurrent { get; set; }
        public bool? IsExpect { get; set; }
    }
}
