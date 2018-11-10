using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Исключительная ситуация, относящаяся к именованному значению.</summary>
    [Serializable]
    public class ValueException : Exception, ISerializable
    {
        [CanBeNull]
        public string ValueName { get; }

        public ValueException () { }

        public ValueException (
            [CanBeNull, CanBeEmpty, InvokerParameterName] string valueName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(message) => ValueName = valueName;

        protected ValueException ([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) => ValueName = info.GetString(nameof(ValueName));

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(ValueName), ValueName ?? string.Empty);
        }

        [CanBeNull] protected string OriginalMessage => base.Message;
    }
}