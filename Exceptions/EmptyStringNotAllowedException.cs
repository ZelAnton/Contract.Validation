using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "String cannot be empty."</summary>
    [Serializable]
    public class EmptyStringNotAllowedException : ValueEmptyException, ISerializable
    {
        public EmptyStringNotAllowedException()
        { }

        public EmptyStringNotAllowedException(
            [CanBeNull, InvokerParameterName] string valueName,
            [CanBeNull] string message = null)
            : base(valueName, message)
        { }

        protected EmptyStringNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ValueName)
                    ? $"{ValueName} string cannot be empty."
                    : "String cannot be empty."
                : OriginalMessage;
    }

    /// <summary>Exception "Argument string cannot be empty."</summary>
    [Serializable]
    public class ArgumentEmptyStringNotAllowedException : ArgumentValueEmptyException, ISerializable
    {
        public ArgumentEmptyStringNotAllowedException()
        { }

        public ArgumentEmptyStringNotAllowedException(
            [CanBeNull, InvokerParameterName] string argumentName,
            [CanBeNull] string message = null)
            : base(argumentName, message)
        { }

        protected ArgumentEmptyStringNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? $"Argument {ParamName} string cannot be empty."
                    : "Argument string cannot be empty."
                : OriginalMessage;
    }
}