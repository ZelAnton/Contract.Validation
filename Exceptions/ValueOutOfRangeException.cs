using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "Value is out of the range of valid values."</summary>
    [Serializable]
    public class ValueOutOfRangeException : ValueException, ISerializable
    {
        public ValueOutOfRangeException()
        { }

        public ValueOutOfRangeException(
            [CanBeNull, CanBeEmpty] string valueName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(valueName, message)
        { }

        protected ValueOutOfRangeException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [NotNull]
        public override string Message
            => !string.IsNullOrWhiteSpace(OriginalMessage)
                ? OriginalMessage
                : !string.IsNullOrWhiteSpace(ValueName)
                    ? $"{ValueName} value is out of the range of valid values."
                    : "Value is out of the range of valid values.";
    }
}