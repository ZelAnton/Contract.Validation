using System;
using System.Collections;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Исключительная ситуация вида "Коллекция не должна содержать null".</summary>
    [Serializable]
    public class ItemNullsNotAllowedException : NullReferenceException, ISerializable
    {
        [CanBeNull] public string CollectionClassName { get; }

        [CanBeNull] public string CollectionName { get; }

        public ItemNullsNotAllowedException()
            : this(null, null, null) { }

        public ItemNullsNotAllowedException([CanBeNull, NoEnumeration] IEnumerable collection, [CanBeNull, InvokerParameterName] string collectionName = null, [CanBeNull] string message = null)
            : base(message)
        {
            CollectionClassName = collection?.GetType().FullName;
            CollectionName = collectionName;
        }

        public ItemNullsNotAllowedException([CanBeNull, InvokerParameterName] string collectionName, [CanBeNull] string message = null)
            : this(null, collectionName, message) { }

        public ItemNullsNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            CollectionClassName = info.GetString(nameof(CollectionClassName));
            CollectionName = info.GetString(nameof(CollectionName));
        }

        public override void GetObjectData([NotNull] SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(CollectionClassName), CollectionClassName);
            info.AddValue(nameof(CollectionName), CollectionName);
        }

        [CanBeNull] protected string OriginalMessage => base.Message;

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(CollectionName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Коллекция {CollectionName} типа {CollectionClassName} содержит null, что недопустимо в данном контексте"
                        : $"Коллекция {CollectionName} содержит null, что недопустимо в данном контексте"
                    : "Коллекция содержит null, что недопустимо в данном контексте"
                : OriginalMessage;
    }

    /// <summary>Исключительная ситуация вида "Коллекция не должна содержать null".</summary>
    [Serializable]
    public class ArgumentItemNullsNotAllowedException : ArgumentNullException, ISerializable
    {
        [CanBeNull] public string CollectionClassName { get; }

        public ArgumentItemNullsNotAllowedException()
            : this(null, null, null) { }

        public ArgumentItemNullsNotAllowedException([CanBeNull, NoEnumeration] IEnumerable collection, [CanBeNull, InvokerParameterName] string collectionName = null, [CanBeNull] string message = null)
            : base(collectionName, message) => CollectionClassName = collection?.GetType().FullName;

        public ArgumentItemNullsNotAllowedException([CanBeNull, InvokerParameterName] string collectionName, [CanBeNull] string message = null)
            : this(null, collectionName, message) { }

        public ArgumentItemNullsNotAllowedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context) => CollectionClassName = info.GetString(nameof(CollectionClassName));

        public override void GetObjectData([NotNull] SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(CollectionClassName), CollectionClassName);
        }

        [CanBeNull] protected string OriginalMessage => base.Message;

        [NotNull]
        public override string Message =>
            string.IsNullOrWhiteSpace(OriginalMessage)
                ? !string.IsNullOrWhiteSpace(ParamName)
                    ? !string.IsNullOrWhiteSpace(CollectionClassName)
                        ? $"Коллекция {ParamName} типа {CollectionClassName} содержит null, что недопустимо в данном контексте"
                        : $"Коллекция {ParamName} содержит null, что недопустимо в данном контексте"
                    : "Коллекция содержит null, что недопустимо в данном контексте"
                : OriginalMessage;
    }
}