namespace AzureTestAppPerformanceTest.Exceptions
{
    public class GenericNullException : Exception
    {
        public GenericNullException() { }

        public GenericNullException(string message)
            : base(message) { }

        public GenericNullException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
