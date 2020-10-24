using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "Value cannot be empty."</summary>
    [Serializable]
    public class ValueEmptyException : ValueException, ISerializable
    {
        public ValueEmptyException()
        { }

        public ValueEmptyException(
            [CanBeNull, CanBeEmpty, InvokerParameterName] string valueName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(valueName, message)
        { }

        protected ValueEmptyException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ValueName)
                    ? $"{ValueName} value cannot be empty."
                    : "Value cannot be empty."
                : OriginalMessage;
    }

    /// <summary>Exception "Argument value cannot be empty."</summary>
    [Serializable]
    public class ArgumentValueEmptyException : ArgumentException, ISerializable
    {
        public ArgumentValueEmptyException()
        { }

        public ArgumentValueEmptyException(
            [CanBeNull, CanBeEmpty] string argumentName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(message, argumentName)
        { }

        protected ArgumentValueEmptyException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [CanBeNull] protected string OriginalMessage
            => base.Message;

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? $"Argument {ParamName} value cannot be empty."
                    : "Argument value cannot be empty."
                : OriginalMessage;
    }
}