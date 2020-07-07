using System;
using System.Runtime.Serialization;

namespace PolynomialObject.Exceptions
{
    [Serializable]
    public class PolynomialArgumentException : Exception
    {
        public PolynomialArgumentException() { }

        public PolynomialArgumentException(string message) : base(message) { }

        protected PolynomialArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
