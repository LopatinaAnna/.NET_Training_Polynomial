using System;
using System.Runtime.Serialization;

namespace PolynomialObject.Exceptions
{
    [Serializable]
    public class PolynomialArgumentNullException : Exception
    {
        public PolynomialArgumentNullException() { }

        public PolynomialArgumentNullException(string message) : base(message) { }

        protected PolynomialArgumentNullException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
