using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>Exception "Operation aborted"</summary>
    [Serializable]
    public class OperationAbortedException : SystemException, ISerializable
    {
        /// <summary>Aborted operation name</summary>
        [CanBeNull, CanBeEmpty] public string OperationName { get; }

        /// <summary>Reason for aborting the operation</summary>
        [CanBeNull, CanBeEmpty] public string Reason { get; }

        public OperationAbortedException()
        { }

        [NotNull, NotEmpty]
        private static string GetMessage(
            [CanBeNull, CanBeEmpty] string operationName,
            [CanBeNull, CanBeEmpty] string reason = null)
            => string.IsNullOrWhiteSpace(operationName)
                ? string.IsNullOrWhiteSpace(reason)
                    ? "Operation aborted."
                    : $"Operation aborted due to: {Environment.NewLine}{reason}"
                : string.IsNullOrWhiteSpace(reason)
                    ? $"Operation \"{operationName}\" aborted."
                    : $"Operation \"{operationName}\" aborted due to: {Environment.NewLine}{reason}";


        public OperationAbortedException(
            [CanBeNull, CanBeEmpty] string operationName,
            [CanBeNull, CanBeEmpty] string reason = null,
            [CanBeNull] Exception innerException = null)
            : base(GetMessage(operationName, reason), innerException)
        {
            OperationName = operationName;
            Reason = reason;
        }

        public OperationAbortedException(
            [CanBeNull, CanBeEmpty] string operationName,
            [CanBeNull] Exception innerException)
            : base(GetMessage(operationName), innerException)
            => OperationName = operationName;

        protected OperationAbortedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            OperationName = info.GetString(nameof(OperationName));
            Reason = info.GetString(nameof(Reason));
        }

        public override void GetObjectData([NotNull] SerializationInfo info, StreamingContext context) //-V3099
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(OperationName), OperationName);
            info.AddValue(nameof(Reason), Reason);
        }

        [CanBeNull]
        protected string OriginalMessage
            // ReSharper disable once RedundantBaseQualifier
            => base.Message;
    }
}