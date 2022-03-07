using HalobizMigrations.Models.Halobiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class SuspectQualificationReceivingDTO
    {
        public EmotionalDisposition? EmotionalDisposition { get; set; }
        public bool? OwnersExists { get; set; }
        public bool? GateKeeperExists { get; set; }
        public bool? InfluencerExists { get; set; }
        public bool? DecisionMakerExists { get; set; }
        public string UpcomingEvents { get; set; }
        public string ProductSuggestion { get; set; }
        public ICollection<ServiceQualificationReceivingDTO> ServiceQualifications { get; set; }
        public long? Priority { get; set; }
        public bool IsPriority { get; set; }
        public string Plans { get; set; }
        public bool ToBeAddressed { get; set; }
        public string ProblemStatement { get; set; }
        public string Plan { get; set; }
        public string Goal { get; set; }
        public long SuspectId { get; set; }
        public bool CanBeSolved { get; set; }
        public long AuthorityScore { get; set; }
        public long BudgetScore { get; set; }
        public long TimingScore { get; set; }
        public long ChallengeScore { get; set; }
        public bool AuthorityCompleted { get; set; }
        public bool BudgetCompleted { get; set; }
        public bool TimingCompleted { get; set; }
        public bool ChallengeCompleted { get; set; }
        public bool IsActive { get; set; }
    }
}
