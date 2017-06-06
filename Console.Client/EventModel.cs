using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Console.Client.Models
{
    [DataContract]
    public class EventModel
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "eventId")]
        public string EventId { get; set; }

        [DataMember(Name = "culture")]
        public string Culture { get; set; }

        [DataMember(Name = "correlationId")]
        public Guid? CorrelationId { get; set; }

        [DataMember(Name = "values")]
        public Dictionary<string, object> Values { get; set; }
    }
}
