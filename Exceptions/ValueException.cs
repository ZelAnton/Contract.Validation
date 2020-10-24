using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "Value cannot be empty."</summary>
    [Serializable]
    public class ValueException : Exception, ISerializable
    {
        [CanBeNull]
        public string ValueName { get; }

        public ValueException ()
        { }

        public ValueException (
            [CanBeNull, CanBeEmpty, InvokerParameterName] string valueName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(message)
            => ValueName = valueName;

        protected ValueException ([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
            => ValueName = info.GetString(nameof(ValueName));

        public override void GetObjectData([NotNull] SerializationInfo info, StreamingContext context) //-V3099
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(ValueName), ValueName ?? string.Empty);
        }

        [CanBeNull] // ReSharper disable once RedundantBaseQualifier
        protected string OriginalMessage
            => base.Message;

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ValueName)
                    ? $"{ValueName} cannot be empty."
                    : "Value cannot be empty."
                : OriginalMessage;
    }
}