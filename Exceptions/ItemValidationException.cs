using System;
using System.Collections;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "Collection items does not satisfy the conditions."</summary>
    [Serializable]
    public class ItemValidationExceptionException : Exception, ISerializable
    {
        [CanBeNull] public string CollectionClassName { get; }

        [CanBeNull] public string CollectionName { get; }

        public ItemValidationExceptionException()
            : this(null, null, null)
        { }

        protected ItemValidationExceptionException(
            [CanBeNull, InvokerParameterName] string collectionClassName,
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message)
            : base(message)
        {
            CollectionClassName = collectionClassName;
            CollectionName = collectionName;
        }

        public ItemValidationExceptionException(
            [CanBeNull, NoEnumeration] IEnumerable collection,
            [CanBeNull, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
            : this(collection?.GetType().FullName, collectionName, message)
        { }

        public ItemValidationExceptionException(
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : this(null, collectionName, message)
        { }

        protected ItemValidationExceptionException(
            [NotNull] SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            CollectionClassName = info.GetString(nameof(CollectionClassName));
            CollectionName = info.GetString(nameof(CollectionName));
        }

        public override void GetObjectData(
            [NotNull] SerializationInfo info,
            StreamingContext context) //-V3099
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(CollectionClassName), CollectionClassName);
            info.AddValue(nameof(CollectionName), CollectionName);
        }

        [CanBeNull] protected string OriginalMessage
            => base.Message;

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(CollectionName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Collection {CollectionName} of type {CollectionClassName} contains item that does not satisfy the conditions."
                        : $"Collection {CollectionName} contains item that does not satisfy the conditions."
                    : !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Collection of type {CollectionClassName} contains item that does not satisfy the conditions."
                        : "Collection items does not satisfy the conditions."
                : OriginalMessage;
    }

    /// <summary>Exception "Argument collection items does not satisfy the conditions."</summary>
    [Serializable]
    public class ArgumentItemValidationExceptionException : ArgumentException, ISerializable
    {
        [CanBeNull] public string CollectionClassName { get; }

        public ArgumentItemValidationExceptionException()
            : this(null, null, null)
        { }

        protected ArgumentItemValidationExceptionException(
            [CanBeNull, NoEnumeration] string collectionClassName,
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message)
            : base(collectionName, message)
            => CollectionClassName = collectionClassName;

        public ArgumentItemValidationExceptionException(
            [CanBeNull, NoEnumeration] IEnumerable collection,
            [CanBeNull, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
            : this(collection?.GetType().FullName, collectionName, message)
        { }

        public ArgumentItemValidationExceptionException(
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : this(null, collectionName, message)
        { }

        protected ArgumentItemValidationExceptionException(
            [NotNull] SerializationInfo info,
            StreamingContext context)
            : base(info, context)
            => CollectionClassName = info.GetString(nameof(CollectionClassName));

        public override void GetObjectData(
            [NotNull] SerializationInfo info,
            StreamingContext context) //-V3099
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(CollectionClassName), CollectionClassName);
        }

        [CanBeNull] protected string OriginalMessage
            => base.Message;

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Argument collection {ParamName} of type {CollectionClassName} contains item that does not satisfy the conditions."
                        : $"Argument collection {ParamName} contains item that does not satisfy the conditions."
                    : !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Argument collection of type {CollectionClassName} contains item that does not satisfy the conditions."
                        : "Argument collection items does not satisfy the conditions."
                : OriginalMessage;
    }
}