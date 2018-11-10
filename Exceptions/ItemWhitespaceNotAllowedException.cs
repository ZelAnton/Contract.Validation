using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Исключительная ситуация вида "Все строки коллекции должны содержать хотя бы один отличный от пробела символ".</summary>
    [Serializable]
    public class ItemWhitespaceNotAllowedException : ItemEmptyStringNotAllowedException, ISerializable
    {
        public ItemWhitespaceNotAllowedException() { }

        public ItemWhitespaceNotAllowedException(
            [CanBeNull, NoEnumeration] IEnumerable<string> strings,
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : base(strings, collectionName, message) { }

        public ItemWhitespaceNotAllowedException(
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string customMessage = null)
            : base(collectionName, customMessage) { }

        public ItemWhitespaceNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(CollectionName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Все строки коллекции {CollectionName} типа {CollectionClassName} должны содержать хотя бы один отличный от пробела символ"
                        : $"Все строки коллекции {CollectionName} должны содержать хотя бы один отличный от пробела символ"
                    : "Все строки коллекции должны содержать хотя бы один отличный от пробела символ"
                : OriginalMessage;
    }

    /// <summary>Исключительная ситуация вида "Все строки коллекции должны содержать хотя бы один отличный от пробела символ".</summary>
    [Serializable]
    public class ArgumentItemWhitespaceNotAllowedException : ArgumentItemEmptyStringNotAllowedException, ISerializable
    {
        public ArgumentItemWhitespaceNotAllowedException() { }

        public ArgumentItemWhitespaceNotAllowedException(
            [CanBeNull, NoEnumeration] IEnumerable<string> strings,
            [CanBeNull, InvokerParameterName] string collectionName,
            [CanBeNull] string message = null)
            : base(strings, collectionName, message) { }

        public ArgumentItemWhitespaceNotAllowedException([CanBeNull, InvokerParameterName] string collectionName, [CanBeNull] string message = null)
            : base(collectionName, message) { }

        public ArgumentItemWhitespaceNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Все строки коллекции {ParamName} типа {CollectionClassName} должны содержать хотя бы один отличный от пробела символ"
                        : $"Все строки коллекции {ParamName} должны содержать хотя бы один отличный от пробела символ"
                    : "Все строки коллекции должны содержать хотя бы один отличный от пробела символ"
                : OriginalMessage;
    }
}