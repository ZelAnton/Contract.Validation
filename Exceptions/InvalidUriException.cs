using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "The format of the URI = null could not be determined."</summary>
    [Serializable]
    public class InvalidUriException : ValueException, ISerializable
    {
        [CanBeNull] public string Uri { get; }

        public InvalidUriException()
        { }

        public InvalidUriException(
            [NotNull, NotWhitespace] string uri,
            [CanBeNull, CanBeEmpty, InvokerParameterName] string valueName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(valueName, message)
            => Uri = uri;

        protected InvalidUriException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ValueName)
                    ? Uri != null
                        ? $"The format of the URI {ValueName} \"{Uri}\" could not be determined."
                        : $"The format of the URI {ValueName} = null could not be determined."
                    : Uri != null
                        ? $"The format of the URI \"{Uri}\" could not be determined."
                        : "The format of the URI = null could not be determined."
                : OriginalMessage;
    }
}