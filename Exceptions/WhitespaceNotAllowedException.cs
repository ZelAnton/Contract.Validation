using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Исключительная ситуация вида "строка должна содержать хотя бы один отличный от пробела символ".</summary>
    [Serializable]
    public class WhitespaceNotAllowedException : EmptyStringNotAllowedException, ISerializable
    {
        public WhitespaceNotAllowedException() {}

        public WhitespaceNotAllowedException(
            [CanBeNull, InvokerParameterName] string stringName,
            [CanBeNull] string message = null)
            : base(stringName, message) { }

        public WhitespaceNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ValueName)
                    ? $"Строка {ValueName} должна содержать хотя бы один отличный от пробела символ"
                    : "Строка должна содержать хотя бы один отличный от пробела символ"
                : OriginalMessage;
    }

    /// <summary>Исключительная ситуация вида "строка должна содержать хотя бы один отличный от пробела символ".</summary>
    [Serializable]
    public class ArgumentWhitespaceNotAllowedException : ArgumentEmptyStringNotAllowedException, ISerializable
    {
        public ArgumentWhitespaceNotAllowedException() { }

        public ArgumentWhitespaceNotAllowedException([CanBeNull, CanBeEmpty, InvokerParameterName] string argumentName, [CanBeNull, CanBeEmpty] string message = null)
            : base(argumentName, message) { }

        public ArgumentWhitespaceNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? $"Строка {ParamName} должна содержать хотя бы один отличный от пробела символ"
                    : "Строка должна содержать хотя бы один отличный от пробела символ"
                : OriginalMessage;
    }
}