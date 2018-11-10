using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Исключительная ситуация вида "Коллекция строк не должна содержать пустые строки".</summary>
    [Serializable]
    public class ItemEmptyStringNotAllowedException : EmptyStringNotAllowedException, ISerializable
    {
        [CanBeNull] public string CollectionClassName { get; }

        [CanBeNull] public string CollectionName => ValueName;

        public ItemEmptyStringNotAllowedException()
            : this(null, null, null) { }

        public ItemEmptyStringNotAllowedException(
            [CanBeNull, NoEnumeration] IEnumerable<string> strings,
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : base(collectionName, message)
            => CollectionClassName = strings?.GetType().FullName;

        public ItemEmptyStringNotAllowedException(
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : this(null, collectionName, message) { }

        public ItemEmptyStringNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(CollectionName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Коллекция строк {CollectionName} типа {CollectionClassName} содержит пустые строки, что недопустимо в данном контексте"
                        : $"Коллекция строк {CollectionName} содержит пустые строки, что недопустимо в данном контексте"
                    : "Коллекция содержит пустые строки, что недопустимо в данном контексте"
                : OriginalMessage;
    }

    /// <summary>Исключительная ситуация вида "Коллекция строк не должна содержать пустые строки".</summary>
    [Serializable]
    public class ArgumentItemEmptyStringNotAllowedException : ArgumentEmptyStringNotAllowedException, ISerializable
    {
        [CanBeNull] public string CollectionClassName { get; }

        public ArgumentItemEmptyStringNotAllowedException()
            : this(null, null, null) { }

        public ArgumentItemEmptyStringNotAllowedException(
            [CanBeNull, NoEnumeration] IEnumerable<string> strings,
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : base(collectionName, message)
            => CollectionClassName = strings?.GetType().FullName;

        public ArgumentItemEmptyStringNotAllowedException([CanBeNull, InvokerParameterName] string collectionName, [CanBeNull] string message = null)
            : this(null, collectionName, message) { }

        public ArgumentItemEmptyStringNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Коллекция строк {ParamName} типа {CollectionClassName} содержит пустые строки, что недопустимо в данном контексте"
                        : $"Коллекция строк {ParamName} содержит пустые строки, что недопустимо в данном контексте"
                    : "Коллекция содержит пустые строки, что недопустимо в данном контексте"
                : OriginalMessage;
    }
}