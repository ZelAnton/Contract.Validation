using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    [Serializable]
    public class InvalidUriException : ValueException, ISerializable
    {
        [CanBeNull] public string Uri { get; }

        public InvalidUriException() { }

        public InvalidUriException(
            [NotNull, NotWhitespace] string uri,
            [CanBeNull, CanBeEmpty, InvokerParameterName] string valueName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(valueName, message)
            => Uri = uri;

        protected InvalidUriException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context) { }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? string.IsNullOrWhiteSpace(Uri)
                    ? "Адрес пуст, что недопустимо в данном контексте"
                    : $"Адрес {Uri} некорректен"
                : OriginalMessage;
    }
}