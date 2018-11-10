using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Исключительная ситуация вида "строка не должна быть пустой".</summary>
    [Serializable]
    public class EmptyStringNotAllowedException : ValueEmptyException, ISerializable
    {
        public EmptyStringNotAllowedException() {}

        public EmptyStringNotAllowedException(
            [CanBeNull, InvokerParameterName] string valueName,
            [CanBeNull] string message = null)
            : base(valueName, message) { }

        public EmptyStringNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ValueName)
                    ? $"Строка {ValueName} пуста, что недопустимо в данном контексте"
                    : "Строка пуста, что недопустимо в данном контексте"
                : OriginalMessage;
    }

    /// <summary>Исключительная ситуация вида "строка не должна быть пустой".</summary>
    [Serializable]
    public class ArgumentEmptyStringNotAllowedException : ArgumentValueEmptyException, ISerializable
    {
        public ArgumentEmptyStringNotAllowedException() {}

        public ArgumentEmptyStringNotAllowedException(
            [CanBeNull, InvokerParameterName] string argumentName,
            [CanBeNull] string message = null)
            : base(argumentName, message) { }

        public ArgumentEmptyStringNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? $"Строка {ParamName} пуста, что недопустимо в данном контексте"
                    : "Строка пуста, что недопустимо в данном контексте"
                : OriginalMessage;
    }
}