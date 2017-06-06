using System;
using System.Runtime.Serialization;

namespace Console.Client.Models
{
    [DataContract]
    class EventApiModel
    {
        [DataMember(Name = "correlationId")]
        public Guid CorrelationId { get; set; }
    }
}
