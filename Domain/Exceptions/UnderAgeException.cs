using Domain.Exceptions.App;

namespace Domain.Exceptions
{
    [System.Serializable]
    public class UnderAgeException : AppException
    {
        public UnderAgeException() { }
        public UnderAgeException(string message) : base(message) { }
        public UnderAgeException(string message, System.Exception inner) : base(message, inner) { }
        protected UnderAgeException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
