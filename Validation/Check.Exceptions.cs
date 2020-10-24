using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace Contract.Validation
{
    /// <summary>Runtime exception creation</summary>
    public abstract partial class Check
    {
        /// <exception cref="ArgumentException"/>
        /// <returns cref="ArgumentException"/>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static ArgumentException CreateArgumentException(
            [CanBeNull] string valueName,
            [CanBeNull] string message)
            => !string.IsNullOrWhiteSpace(message)
                ? !string.IsNullOrWhiteSpace(valueName)
                    ? new ArgumentException(message, valueName)
                    : new ArgumentException(message)
                : !string.IsNullOrWhiteSpace(valueName)
                    ? new ArgumentException($"Argument {valueName} does not satisfy the conditions.", valueName)
                    : new ArgumentException("Argument does not satisfy the conditions.");

        /// <exception cref="ArgumentException"/>
        /// <returns cref="ArgumentException"/>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static ArgumentException CreateArgumentException(
            [CanBeNull] string message)
            => string.IsNullOrWhiteSpace(message)
                ? new ArgumentException("Argument does not satisfy the conditions.")
                : new ArgumentException(message);

        /// <exception cref="ArgumentNullException"/>
        /// <returns cref="ArgumentNullException"/>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static ArgumentNullException CreateArgumentNullException(
            [CanBeNull] string valueName,
            [CanBeNull] string message)
            => !string.IsNullOrWhiteSpace(message)
                ? !string.IsNullOrWhiteSpace(valueName)
                    ? new ArgumentNullException(valueName, message)
                    : new ArgumentNullException(null, message)
                : !string.IsNullOrWhiteSpace(valueName)
                    ? new ArgumentNullException(valueName, $"Argument {valueName} cannot be null.")
                    : new ArgumentNullException(null, "Argument cannot be null.");

        /// <exception cref="ArgumentNullException"/>
        /// <returns cref="ArgumentNullException"/>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static ArgumentNullException CreateArgumentNullException(
            [CanBeNull] string message)
            => !string.IsNullOrWhiteSpace(message)
                ? new ArgumentNullException(null, message)
                : new ArgumentNullException(null, "Argument cannot be null.");

        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <returns cref="ArgumentOutOfRangeException"/>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(
            [CanBeNull] string valueName,
            [CanBeNull] string message)
            => !string.IsNullOrWhiteSpace(message)
                ? !string.IsNullOrWhiteSpace(valueName)
                    ? new ArgumentOutOfRangeException(valueName, message)
                    : new ArgumentOutOfRangeException(null, message)
                : !string.IsNullOrWhiteSpace(valueName)
                    ? new ArgumentOutOfRangeException(valueName, $"Argument {valueName} is out of the range of valid values.")
                    : new ArgumentOutOfRangeException(null, "Argument is out of the range of valid values.");

        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <returns cref="ArgumentOutOfRangeException"/>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(
            [CanBeNull] string message)
            => !string.IsNullOrWhiteSpace(message)
                ? new ArgumentOutOfRangeException(null, message)
                : new ArgumentOutOfRangeException(null, "Argument is out of the range of valid values.");

        /// <exception cref="NullReferenceException"/>
        /// <returns cref="NullReferenceException"/>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static NullReferenceException CreateNullReferenceException(
            [CanBeNull] string valueName,
            [CanBeNull] string message)
            => !string.IsNullOrWhiteSpace(message)
                ? new NullReferenceException(message)
                : !string.IsNullOrWhiteSpace(valueName)
                    ? new NullReferenceException($"Value {valueName} cannot be null.")
                    : new NullReferenceException("Value cannot be null.");

        /// <exception cref="NullReferenceException"/>
        /// <returns cref="NullReferenceException"/>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static NullReferenceException CreateNullReferenceException(
            [CanBeNull] string message)
            => !string.IsNullOrWhiteSpace(message)
                ? new NullReferenceException(message)
                : new NullReferenceException("Value cannot be null.");

        /// <exception cref="InvalidCastException"/>
        /// <returns cref="InvalidCastException"/>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static InvalidCastException CreateInvalidCastException(
            [NotNull] Type type,
            [CanBeNull] string valueName,
            [CanBeNull] string message)
        {
            message = string.IsNullOrWhiteSpace(message)
                ? !string.IsNullOrWhiteSpace(valueName)
                    ? $"Invalid cast of {valueName} value to {type} type"
                    : $"Invalid cast to {type} type"
                : message;
            return new InvalidCastException(message);
        }

        /// <exception><cref>TException</cref></exception>
        /// <returns><cref>TException</cref></returns>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static Exception CreateException<TException>(
            [CanBeNull] string message,
            [NotNull, InstantHandle] Func<string> defaultMessageConstructor)
            where TException : Exception
        {
            Debug.ArgumentNotNull(defaultMessageConstructor, nameof(defaultMessageConstructor));

            message = string.IsNullOrWhiteSpace(message)
                ? defaultMessageConstructor()
                : message;
            ConstructorInfo constructor =  typeof(TException).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new [] { typeof(string) },
                null);
            if (constructor != null) // ReSharper disable once AssignNullToNotNullAttribute
                return (TException) constructor.Invoke(new object[] {message});

            constructor =  typeof(TException).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new [] { typeof(string), typeof(Exception) },
                null);
            if (constructor != null) // ReSharper disable once AssignNullToNotNullAttribute
                return (TException) constructor.Invoke(new object[] {message, null});

            constructor =  typeof(TException).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                Type.EmptyTypes,
                null);
            if (constructor != null) // ReSharper disable once AssignNullToNotNullAttribute
                return (TException) constructor.Invoke(Array.Empty<object>());

            throw CreateArgumentException(null, $"{typeof(TException)} has no correct constructor!{Environment.NewLine}{message}");
        }

        /// <exception><cref>TException</cref></exception>
        /// <returns><cref>TException</cref></returns>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static Exception CreateException<TException>(
            [CanBeNull] string message)
            where TException : Exception
        {
            ConstructorInfo constructor =  typeof(TException).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new [] { typeof(string) },
                null);
            if (constructor != null) // ReSharper disable once AssignNullToNotNullAttribute
                return (TException) constructor.Invoke(new object[] {message});

            constructor =  typeof(TException).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new [] { typeof(string), typeof(Exception) },
                null);
            if (constructor != null) // ReSharper disable once AssignNullToNotNullAttribute
                return (TException) constructor.Invoke(new object[] {message, null});

            constructor =  typeof(TException).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                Type.EmptyTypes,
                null);
            if (constructor != null) // ReSharper disable once AssignNullToNotNullAttribute
                return (TException) constructor.Invoke(Array.Empty<object>());

            throw CreateArgumentException(null, $"{typeof(TException)} has no correct constructor!{Environment.NewLine}{message}");
        }

        /// <exception><cref>TException</cref></exception>
        /// <returns><cref>TException</cref></returns>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), MustUseReturnValue]
        private static Exception CreateExceptionWithParams<TException>(
            [NotNull, ItemNotNull] object[] exceptionParams)
            where TException : Exception
        {
            Debug.ItemsNotNull(exceptionParams, nameof(exceptionParams));

            Type[] argumentTypes;
            int paramsCount = exceptionParams.Length;
            if (paramsCount > 0)
            {
                argumentTypes = new Type[paramsCount];
                for (int i = paramsCount - 1; i >= 0; i--)
                    argumentTypes[i] = exceptionParams[i].GetType();
            }
            else
                argumentTypes = Array.Empty<Type>();
            ConstructorInfo constructor = typeof(TException).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                argumentTypes,
                null);
            if (constructor != null) // ReSharper disable once AssignNullToNotNullAttribute
                return (TException)constructor.Invoke(exceptionParams);

            constructor = typeof(TException).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new [] { typeof(string) },
                null);
            if (constructor != null) // ReSharper disable once PossibleNullReferenceException
                throw (TException) constructor.Invoke(new object[] { $"{typeof(TException)} has no correct constructor!"});

            throw CreateArgumentException(null, $"{typeof(TException)} has no correct constructor!");
        }
    }
}