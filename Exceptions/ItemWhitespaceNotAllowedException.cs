using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "Strings collection cannot contains a whitespace strings."</summary>
    [Serializable]
    public class ItemWhitespaceNotAllowedException : ItemEmptyStringNotAllowedException, ISerializable
    {
        public ItemWhitespaceNotAllowedException()
        { }

        public ItemWhitespaceNotAllowedException(
            [CanBeNull, NoEnumeration] IEnumerable<string> strings,
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : base(strings, collectionName, message)
        { }

        public ItemWhitespaceNotAllowedException(
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string customMessage = null)
            : base(collectionName, customMessage)
        { }

        protected ItemWhitespaceNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(CollectionName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Strings collection {CollectionName} of type {CollectionClassName} cannot contains a whitespace strings."
                        : $"Strings collection {CollectionName} cannot contains a whitespace strings."
                    : "Strings collection cannot contains a whitespace strings."
                : OriginalMessage;
    }

    /// <summary>Exception "Strings collection cannot contains a whitespace strings."</summary>
    [Serializable]
    public class ArgumentItemWhitespaceNotAllowedException : ArgumentItemEmptyStringNotAllowedException, ISerializable
    {
        public ArgumentItemWhitespaceNotAllowedException()
        { }

        public ArgumentItemWhitespaceNotAllowedException(
            [CanBeNull, NoEnumeration] IEnumerable<string> strings,
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : base(strings, collectionName, message)
        { }

        public ArgumentItemWhitespaceNotAllowedException(
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : base(collectionName, message)
        { }

        protected ArgumentItemWhitespaceNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Argument strings collection {ParamName} of type {CollectionClassName} cannot contains a whitespace strings."
                        : $"Argument strings collection {ParamName} cannot contains a whitespace strings."
                    : "Argument strings collection cannot contains a whitespace strings."
                : OriginalMessage;
    }
}