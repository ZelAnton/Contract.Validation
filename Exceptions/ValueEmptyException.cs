using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Значение не может быть пустым.</summary>
    [Serializable]
    public class ValueEmptyException : ValueException, ISerializable
    {
        public ValueEmptyException() { }

        public ValueEmptyException(
            [CanBeNull, CanBeEmpty, InvokerParameterName] string valueName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(valueName, message) { }

        protected ValueEmptyException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ValueName)
                    ? $"Значение {ValueName} пусто, что недопустимо в данном контексте"
                    : "Значение пусто, что недопустимо в данном контексте"
                : OriginalMessage;
    }

    /// <summary>Аргумент не может быть пуст.</summary>
    [Serializable]
    public class ArgumentValueEmptyException : ArgumentException, ISerializable
    {
        public ArgumentValueEmptyException() { }

        public ArgumentValueEmptyException(
            [CanBeNull, CanBeEmpty] string argumentName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(message, argumentName) { }

        protected ArgumentValueEmptyException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context) { }

        [CanBeNull] protected string OriginalMessage => base.Message;

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? $"Значение {ParamName} пусто, что недопустимо в данном контексте"
                    : "Значение пусто, что недопустимо в данном контексте"
                : OriginalMessage;
    }
}