using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Значение не может быть пустым.</summary>
    [Serializable]
    public class CollectionIsEmptyException : ValueException, ISerializable
    {
        [CanBeNull, CanBeEmpty] public string CollectionName => ValueName;

        public CollectionIsEmptyException() { }

        public CollectionIsEmptyException(
            [CanBeNull, CanBeEmpty, InvokerParameterName] string collectionName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(collectionName, message) { }

        protected CollectionIsEmptyException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(CollectionName)
                    ? $"Коллекция {CollectionName} пуста, что недопустимо в данном контексте"
                    : "Коллекция пуста, что недопустимо в данном контексте"
                : OriginalMessage;
    }

    /// <summary>Значение не может быть пустым.</summary>
    [Serializable]
    public class ArgumentCollectionIsEmptyException : ArgumentException, ISerializable
    {
        [CanBeNull, CanBeEmpty] public string CollectionName => ParamName;

        public ArgumentCollectionIsEmptyException() { }

        public ArgumentCollectionIsEmptyException(
            [CanBeNull, CanBeEmpty, InvokerParameterName] string collectionName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(message, collectionName) { }

        protected ArgumentCollectionIsEmptyException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) {}

        [CanBeNull, CanBeEmpty]
        protected string OriginalMessage => base.Message;

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(CollectionName)
                    ? $"Коллекция {CollectionName} пуста, что недопустимо в данном контексте"
                    : "Коллекция пуста, что недопустимо в данном контексте"
                : OriginalMessage;
    }
}