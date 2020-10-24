using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "String collection cannot contains empty strings."</summary>
    [Serializable]
    public class ItemEmptyStringNotAllowedException : EmptyStringNotAllowedException, ISerializable
    {
        [CanBeNull] public string CollectionClassName { get; }

        [CanBeNull] public string CollectionName
            => ValueName;

        public ItemEmptyStringNotAllowedException()
            : this(null, null, null)
        { }

        public ItemEmptyStringNotAllowedException(
            [CanBeNull, NoEnumeration] IEnumerable<string> strings,
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : base(collectionName, message)
            => CollectionClassName = strings?.GetType().FullName;

        public ItemEmptyStringNotAllowedException(
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : this(null, collectionName, message)
        { }

        protected ItemEmptyStringNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
            => CollectionClassName = info.GetString(nameof(CollectionClassName));

        public override void GetObjectData([NotNull] SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(CollectionClassName), CollectionClassName);
        }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(CollectionName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Strings collection {CollectionName} of type {CollectionClassName} cannot contains empty strings."
                        : $"Strings collection {CollectionName} cannot contains empty strings."
                    : "String collection cannot contains empty strings."
                : OriginalMessage;
    }

    /// <summary>Exception "Argument strings collection cannot contains empty strings."</summary>
    [Serializable]
    public class ArgumentItemEmptyStringNotAllowedException : ArgumentEmptyStringNotAllowedException, ISerializable
    {
        [CanBeNull] public string CollectionClassName { get; }

        public ArgumentItemEmptyStringNotAllowedException()
            : this(null, null, null)
        { }

        public ArgumentItemEmptyStringNotAllowedException(
            [CanBeNull, NoEnumeration] IEnumerable<string> strings,
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : base(collectionName, message)
                => CollectionClassName = strings?.GetType().FullName;

        public ArgumentItemEmptyStringNotAllowedException(
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : this(null, collectionName, message)
        { }

        protected ArgumentItemEmptyStringNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
            => CollectionClassName = info.GetString(nameof(CollectionClassName));

        public override void GetObjectData([NotNull] SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(CollectionClassName), CollectionClassName);
        }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Argument strings collection {ParamName} of type {CollectionClassName} cannot contains empty strings."
                        : $"Argument strings collection {ParamName} cannot contains empty strings."
                    : "Argument strings collection cannot contains empty strings."
                : OriginalMessage;
    }
}