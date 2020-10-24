using System;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "Collection cannot be empty."</summary>
    [Serializable]
    public class CollectionIsEmptyException : ValueException, ISerializable
    {
        [CanBeNull, CanBeEmpty]
        public string CollectionName {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ValueName;
        }

        public CollectionIsEmptyException()
        { }

        public CollectionIsEmptyException(
            [CanBeNull, CanBeEmpty, InvokerParameterName] string collectionName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(collectionName, message)
        { }

        protected CollectionIsEmptyException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(CollectionName)
                    ? $"{CollectionName} collection cannot be empty."
                    : "Collection cannot be empty."
                : OriginalMessage;
    }

    /// <summary>Exception "Argument collection cannot be empty."</summary>
    [Serializable]
    public class ArgumentCollectionIsEmptyException : ArgumentException, ISerializable
    {
        [CanBeNull, CanBeEmpty]
        public string CollectionName {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ParamName;
        }

        public ArgumentCollectionIsEmptyException()
        { }

        public ArgumentCollectionIsEmptyException(
            [CanBeNull, CanBeEmpty, InvokerParameterName] string collectionName,
            [CanBeNull, CanBeEmpty] string message = null)
            : base(message, collectionName)
        { }

        protected ArgumentCollectionIsEmptyException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [CanBeNull, CanBeEmpty] protected string OriginalMessage => base.Message;

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(CollectionName)
                    ? $"Argument {CollectionName} collection cannot be empty."
                    : "Argument collection cannot be empty."
                : OriginalMessage;
    }
}