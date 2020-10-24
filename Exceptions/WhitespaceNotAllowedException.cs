using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "String cannot be whitespace."</summary>
    [Serializable]
    public class WhitespaceNotAllowedException : EmptyStringNotAllowedException, ISerializable
    {
        public WhitespaceNotAllowedException()
        { }

        public WhitespaceNotAllowedException(
            [CanBeNull, InvokerParameterName] string stringName,
            [CanBeNull] string message = null)
            : base(stringName, message)
        { }

        protected WhitespaceNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ValueName)
                    ? $"{ValueName} string cannot be whitespace."
                    : "String cannot be whitespace."
                : OriginalMessage;
    }

    /// <summary>Exception "Argument string cannot be whitespace."</summary>
    [Serializable]
    public class ArgumentWhitespaceNotAllowedException : ArgumentEmptyStringNotAllowedException, ISerializable
    {
        public ArgumentWhitespaceNotAllowedException()
        { }

        public ArgumentWhitespaceNotAllowedException(
            [CanBeNull, CanBeEmpty, InvokerParameterName] string argumentName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(argumentName, message)
        { }

        protected ArgumentWhitespaceNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? $"Argument {ParamName} string cannot be whitespace."
                    : "Argument string cannot be whitespace."
                : OriginalMessage;
    }
}