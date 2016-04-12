namespace com.pharmscription.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Runtime.Serialization;

    [DataContract]
    public class ErrorMessage
    { 
        [DataMember(Name = "message")]
        public string Message { get; set; }

        public ErrorMessage(string message)
        {
            Message = message;
        }
    }
}