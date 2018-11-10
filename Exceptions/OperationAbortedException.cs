using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Contract.Exceptions
{
    /// <summary>(Serializable) Операция отменена в силу описанных причин.</summary>
    [Serializable]
    public class OperationAbortedException : SystemException, ISerializable
    {
        /// <summary>Описание отменённой операции</summary>
        [CanBeNull, CanBeEmpty] public string OperationName { get; }

        /// <summary>Причина отмены операции</summary>
        [CanBeNull, CanBeEmpty] public string Reason { get; }

        public OperationAbortedException() { }

        [NotNull, NotEmpty]
        private static string GetMessage([CanBeNull, CanBeEmpty] string operationName, [CanBeNull, CanBeEmpty] string reason = null) =>
            string.IsNullOrWhiteSpace(operationName)
                ? string.IsNullOrWhiteSpace(reason)
                    ? "Операция отменена"
                    : $"Операция отменена: {reason}"
                : string.IsNullOrWhiteSpace(reason)
                    ? $"Операция \"{operationName}\" отменена"
                    : $"Операция \"{operationName}\" отменена: {reason}";


        public OperationAbortedException(
            [CanBeNull, CanBeEmpty] string operationName,
            [CanBeNull, CanBeEmpty] string reason = null,
            [CanBeNull] Exception innerException = null)
            : base(GetMessage(operationName, reason), innerException)
        {
            OperationName = operationName;
            Reason = reason;
        }

        public OperationAbortedException([CanBeNull, CanBeEmpty] string operationName, [CanBeNull] Exception innerException)
            : base(GetMessage(operationName), innerException) => OperationName = operationName;

        protected OperationAbortedException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            OperationName = info.GetString(nameof(OperationName));
            Reason = info.GetString(nameof(Reason));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(OperationName), OperationName);
            info.AddValue(nameof(Reason), Reason);
        }

        [CanBeNull] protected string OriginalMessage => base.Message;
    }
}