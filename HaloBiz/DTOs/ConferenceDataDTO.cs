using System;

namespace HaloBiz.DTOs
{
    public class ConferenceDataDto
    {
        public string RequestId { get; set; }
        public ConferenceSolution ConferenceSolutionKey  { get; set; }
    }

    public class ConferenceSolution
    {
        public string Type { get; set; }
    }
}