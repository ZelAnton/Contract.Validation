using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "Item not found!"</summary>
    [Serializable]
    public class ItemNotFoundException : Exception, ISerializable
    {
        internal const string ConstKeyValueKnown = "KeyValueKnown";

        public ItemNotFoundException()
        { }

        public ItemNotFoundException([CanBeNull] string message)
            : base(message)
        { }

        public ItemNotFoundException(
            [CanBeNull] string message,
            [CanBeNull] Exception innerException)
            : base(message, innerException)
        { }

        protected ItemNotFoundException(
            [NotNull] SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        { }

        [CanBeNull]
        protected string OriginalMessage
            // ReSharper disable once RedundantBaseQualifier
            => base.Message;

        [NotNull]
        public override string Message
            => string.IsNullOrWhiteSpace(OriginalMessage)
                ? "Item not found."
                : OriginalMessage;
    }

    /// <summary>Exception "Item not found!"</summary>
    [Serializable]
    public class ItemNotFoundException<TKey> : ItemNotFoundException, ISerializable
    {
        [CanBeNull]
        public TKey KeyValue { get; }

        private bool _keyIsKnown;

        private ItemNotFoundException() { }

        public ItemNotFoundException([CanBeNull] TKey keyValue)
        {
            KeyValue = keyValue;
            _keyIsKnown = true;
        }

        public ItemNotFoundException(
            [CanBeNull] TKey keyValue,
            [CanBeNull] string message)
            : base(message)
        {
            KeyValue = keyValue;
            _keyIsKnown = true;
        }

        public ItemNotFoundException(
            [CanBeNull] TKey keyValue,
            [CanBeNull] string message,
            [CanBeNull] Exception innerException)
            : base(message, innerException)
        {
            KeyValue = keyValue;
            _keyIsKnown = true;
        }

        protected ItemNotFoundException(
            [NotNull] SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            _keyIsKnown = info.GetBoolean(ConstKeyValueKnown);
            if (_keyIsKnown)
                KeyValue = (TKey) info.GetValue(nameof(KeyValue), typeof(TKey));
        }

        public override void GetObjectData(
            [NotNull] SerializationInfo info,
            StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(ConstKeyValueKnown, _keyIsKnown);
            if (_keyIsKnown)
                info.AddValue(nameof(KeyValue), KeyValue);
        }

        public override string Message
            => !string.IsNullOrWhiteSpace(OriginalMessage)
                ? OriginalMessage
                : _keyIsKnown
                    ? $"Item with key \"{KeyValue}\" not found."
                    : "Item not found.";
    }

    /// <summary>Exception "Item not found in argument."</summary>
    [Serializable]
    public class ArgumentItemNotFoundException : ArgumentException, ISerializable
    {
        public ArgumentItemNotFoundException()
        { }

        public ArgumentItemNotFoundException([CanBeNull] string message)
            : base(message)
        { }

        public ArgumentItemNotFoundException(
            [CanBeNull] string message,
            [CanBeNull] Exception innerException)
            : base(message, innerException)
        { }

        public ArgumentItemNotFoundException(
            [NotNull, NotWhitespace] string paramName,
            [CanBeNull] string message)
            : base(message, paramName)
        { }

        public ArgumentItemNotFoundException(
            [NotNull, NotWhitespace] string paramName,
            [CanBeNull] string message,
            [CanBeNull] Exception innerException)
            : base(message, paramName, innerException)
        { }

        protected ArgumentItemNotFoundException(
            [NotNull] SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        { }

        [CanBeNull]
        protected string OriginalMessage
            // ReSharper disable once RedundantBaseQualifier
            => base.Message;

        [NotNull]
        public override string Message
            => !string.IsNullOrWhiteSpace(OriginalMessage)
                ? OriginalMessage
                : !string.IsNullOrWhiteSpace(ParamName)
                    ? $"Item not found in argument {ParamName}."
                    : "Item not found in argument.";
    }

    /// <summary>Exception "Item not found in argument."</summary>
    [Serializable]
    public class ArgumentItemNotFoundException<TKey> : ArgumentItemNotFoundException, ISerializable
    {
        [CanBeNull]
        public TKey KeyValue { get; }

        private bool _keyIsKnown;

        private ArgumentItemNotFoundException()
        { }

        public ArgumentItemNotFoundException(
            [CanBeNull] TKey keyValue,
            [NotNull, NotWhitespace] string paramName)
            : base(paramName, (string) null)
        {
            KeyValue = keyValue;
            _keyIsKnown = true;
        }

        public ArgumentItemNotFoundException(
            [CanBeNull] TKey keyValue,
            [NotNull, NotWhitespace] string paramName,
            [CanBeNull] string message)
            : base(paramName, message)
        {
            KeyValue = keyValue;
            _keyIsKnown = true;
        }

        public ArgumentItemNotFoundException(
            [CanBeNull] TKey keyValue,
            [NotNull, NotWhitespace] string paramName,
            [CanBeNull] string message,
            [CanBeNull] Exception innerException)
            : base(paramName, message, innerException)
        {
            KeyValue = keyValue;
            _keyIsKnown = true;
        }

        protected ArgumentItemNotFoundException(
            [NotNull] SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            _keyIsKnown = info.GetBoolean(ItemNotFoundException.ConstKeyValueKnown);
            if (_keyIsKnown)
                KeyValue = (TKey) info.GetValue(nameof(KeyValue), typeof(TKey));
        }

        public override void GetObjectData(
            [NotNull] SerializationInfo info,
            StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(ItemNotFoundException.ConstKeyValueKnown, _keyIsKnown);
            if (_keyIsKnown)
                info.AddValue(nameof(KeyValue), KeyValue);
        }

        public override string Message
            => !string.IsNullOrWhiteSpace(OriginalMessage)
                ? OriginalMessage
                : _keyIsKnown
                    ? !string.IsNullOrWhiteSpace(ParamName)
                        ? $"Item with key {KeyValue} not found in argument {ParamName}."
                        : $"Item with key {KeyValue} not found in argument."
                    : !string.IsNullOrWhiteSpace(ParamName)
                        ? $"Item not found in argument {ParamName}."
                        : "Item not found in argument.";
    }
}