using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Contract.Internal;
using Contract.Exceptions;

using JetBrains.Annotations;

#if FEATURE_RELIABILITY_CONTRACTS
using System.Runtime.ConstrainedExecution;
#endif

// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable LoopCanBePartlyConvertedToQuery
// ReSharper disable RedundantTypeSpecificationInDefaultExpression
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable HeuristicUnreachableCode
// 
namespace Contract.Validation
{
    /// <summary>Runtime валидация условий</summary>
    public abstract partial class Check
    {
        private const string UnknownValueName = "unknown";

        /// <summary>Проверка аргумента на null</summary>
        /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
        /// <param name="argument">Объект, который не должен быть равен null</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ArgumentNotNull<T>(
            [NotNull, NoEnumeration] T argument,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
            where T : class
        {
            if (argument == null)
            {
                argumentName = argumentName ?? UnknownValueName;

                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is null" : message;
                throw new ArgumentNullException(argumentName, message);
            }

            return argument;
        }

        /// <summary>Проверка аргумента на null</summary>
        /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
        /// <param name="argument">Объект, который не должен быть равен null</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ArgumentNotNull<T>(
            [NotNull] T? argument,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (argument == null)
            {
                argumentName = argumentName ?? UnknownValueName;

                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is null" : message;
                throw new ArgumentNullException(argumentName, message);
            }

            return (T)argument;
        }

        /// <summary>Проверка аргумента на значение по-умолчанию</summary>
        /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
        /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ArgumentValueNotEmpty<T>(
            [NotEmpty, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (value.Equals(default(T)))
            {
                argumentName = argumentName ?? UnknownValueName;

                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is empty (equal to {default(T)})" : message;
                throw new ArgumentValueEmptyException(argumentName, message);
            }

            return value;
        }

        /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
        /// <exception cref="ArgumentValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
        /// <param name="values">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> ArgumentValuesNotEmpty<T>(
            [NotNull, ItemNotEmpty] IEnumerable<T> values,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            foreach (T value in ArgumentNotNull(values, valueName))
                if (value.Equals(default(T)))
                {
                    valueName = valueName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is empty (equal to {default(T)})" : message;
                    throw new ArgumentValueEmptyException(valueName, message);
                }

            return values;
        }

        /// <summary>Проверка что элементы коллекции не null</summary>
        /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
        /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ArgumentItemsNotNull<T>(
            [NotNull, ItemNotNull] T collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
            where T : class, IEnumerable
        {
            ArgumentNotNull(collection, collectionName);

            foreach (object item in collection)
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (item == null)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Argument collection {collectionName} has null element" : message;
                    throw new ArgumentItemNullsNotAllowedException(collection, collectionName, message);
                }

            return collection;
        }

        /// <summary>Проверка что элементы коллекции не null</summary>
        /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
        /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> ArgumentItemsNotNull<T>(
            [NotNull, ItemNotNull] IEnumerable<T?> collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            ArgumentNotNull(collection, collectionName);

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (T? item in collection)
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (item == null)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Argument collection {collectionName} has null element" : message;
                    throw new ArgumentItemNullsNotAllowedException(collection, collectionName, message);
                }

            // ReSharper disable once PossibleMultipleEnumeration
            return collection.Cast<T>();
        }

        /// <summary>Проверка что все строки в коллекции не null и не пусты</summary>
        /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
        /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
        /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
        /// <param name="collection">Коллекция строк, которые быть не должны быть равны string.Empty</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, ItemNotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<string> ArgumentItemsNotEmpty(
            [NotNull, ItemNotNull, ItemNotEmpty] IEnumerable<string> collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
        {
            ArgumentNotNull(collection, collectionName);

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (string str in collection)
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (str == null)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Argument collection {collectionName} has null element" : message;
                    throw new ArgumentItemNullsNotAllowedException(collection, collectionName, message);
                }

                if (str == string.Empty)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Argument collection {collectionName} has empty string element" : message;
                    throw new ArgumentItemEmptyStringNotAllowedException(collection, collectionName, message);
                }
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return collection;
        }

        /// <summary>Проверка что элементы коллекции не null</summary>
        /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
        /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
        /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
        /// <exception cref="ArgumentItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего кроме пробелов</exception>
        /// <param name="collection">Коллекция строк, которые быть не должны быть равны string.Empty</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, ItemNotNull, ItemNotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<string> ArgumentItemsNotWhitespace(
            [NotNull, ItemNotNull, ItemNotWhitespace] IEnumerable<string> collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
        {
            ArgumentNotNull(collection, collectionName);

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (string str in collection)
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (str == null)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Argument collection {collectionName} has null element" : message;
                    throw new ArgumentItemNullsNotAllowedException(collection, collectionName, message);
                }

                if (str == string.Empty)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Argument collection {collectionName} has empty string element" : message;
                    throw new ArgumentItemEmptyStringNotAllowedException(collection, collectionName, message);
                }

                if (string.IsNullOrWhiteSpace(str))
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Argument collection {collectionName} has whitespace string element" : message;
                    throw new ArgumentItemWhitespaceNotAllowedException(collection, collectionName, message);
                }
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return collection;
        }

        /// <summary>Проверка строкового аргумента на null и на равенство string.Empty</summary>
        /// <exception cref="ArgumentNullException">Если строка == null</exception>
        /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
        /// <param name="argument">Строковый аргумент, который не должен быть равен null или string.Empty</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static string ArgumentNotNullOrEmpty(
            [NotNull, NotEmpty] string argument,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
        {
            if (argument == null)
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is null" : message;
                throw new ArgumentNullException(argumentName, message);
            }

            if (argument.Equals(string.Empty))
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is empty string" : message;
                throw new ArgumentEmptyStringNotAllowedException(argumentName, message);
            }

            return argument;
        }

        /// <summary>Проверка строкового аргумента на null и на равенство string.Empty или состоять только из пробелов</summary>
        /// <exception cref="ArgumentNullException">Если строка == null</exception>
        /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
        /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
        /// <param name="argument">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из пробелов</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, NotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static string ArgumentNotNullOrWhitespace(
            [NotNull, NotWhitespace] string argument,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
        {
            if (argument == null)
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is null" : message;
                throw new ArgumentNullException(argumentName, message);
            }

            if (argument == string.Empty)
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is empty string" : message;
                throw new ArgumentEmptyStringNotAllowedException(argumentName, message);
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is whitespace" : message;
                throw new ArgumentWhitespaceNotAllowedException(argumentName, message);
            }

            return argument;
        }

        /// <summary>Проверка аргумента на null и DBNull</summary>
        /// <exception cref="ArgumentNullException">Если аргумент == null или == DBNull</exception>
        /// <param name="argument">Объект, который не должен быть равен null</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ArgumentNotNullNotDbNull<T>(
            [NotNull, NotEmpty] T argument,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
            where T : class
        {
            if (argument == null)
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is null" : message;
                throw new ArgumentNullException(argumentName, message);
            }

            if (DBNull.Value.Equals(argument))
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is DBNull" : message;
                throw new ArgumentNullException(argumentName, message);
            }

            return argument;
        }

        /// <summary>Проверка аргумента</summary>
        /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
        /// <param name="condition">Условие проверки значения аргумента</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static void Argument(
            bool condition,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
        {
            if (!condition)
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} does not satisfy condition" : message;
                throw new ArgumentException(message, argumentName);
            }
        }

        /// <summary>Проверка аргумента</summary>
        /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
        /// <param name="argument">Возвращаемое значение если проверка будет выполнена</param>
        /// <param name="condition">Условие проверки значения аргумента</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt; argument:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T Argument<T>(
            [CanBeNull] T argument,
            [NotNull] Func<T, bool> condition,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
        {
            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(argument))
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} does not satisfy condition" : message;
                throw new ArgumentException(message, argumentName);
            }

            return argument;
        }

        /// <summary>Проверка аргумента</summary>
        /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
        /// <param name="condition">Условие проверки значения аргумента</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static void Argument(
            [NotNull, InstantHandle] Func<bool> condition,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
        {
            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition())
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} does not satisfy condition" : message;
                throw new ArgumentException(message, argumentName);
            }
        }

        /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
        /// <param name="condition">Условие проверки значения аргумента</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static void ArgumentInRange(
            bool condition,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
        {
            if (!condition)
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is out of range" : message;
                throw new ArgumentOutOfRangeException(argumentName, message);
            }
        }

        /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
        /// <param name="argument">Возвращаемое значение если проверка будет выполнена</param>
        /// <param name="condition">Условие проверки значения аргумента</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt; argument:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ArgumentInRange<T>(
            [CanBeNull] T argument,
            [NotNull] Func<T, bool> condition,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
        {
            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(argument))
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is out of range" : message;
                throw new ArgumentOutOfRangeException(argumentName, message);
            }

            return argument;
        }

        /// <summary>Проверка что guid не пуст</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если guid == Guid.Empty</exception>
        /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
        /// <param name="argumentName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static Guid ArgumentGuidNotEmpty(
            [NotEmpty] Guid guid,
            [CanBeNull, NotWhitespace, InvokerParameterName] string argumentName = null,
            [CanBeNull] string message = null)
        {
            if (guid == Guid.Empty)
            {
                argumentName = argumentName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument {argumentName} is empty Guid" : message;
                throw new ArgumentOutOfRangeException(argumentName, message);
            }

            return guid;
        }

        /// <summary>Проверка что guid не пуст</summary>
        /// <exception cref="Exception">Если guid == Guid.Empty</exception>
        /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static Guid GuidNotEmpty(
            [NotEmpty] Guid guid,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (guid == Guid.Empty)
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is empty Guid" : message;
                throw new Exception(message);
            }

            return guid;
        }

        /// <summary>Проверка что объект не null</summary>
        /// <exception cref="NullReferenceException">Если объект null</exception>
        /// <param name="value">Объект, который не должен быть равен null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T NotNull<T>(
            [NotNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class
        {
            if (value == null)
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is null" : message;
                throw new NullReferenceException(message);
            }

            return value;
        }

        /// <summary>Проверка что объект не null</summary>
        /// <exception cref="NullReferenceException">Если объект null</exception>
        /// <param name="value">Объект, который не должен быть равен null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T NotNull<T>(
            [NotNull, NoEnumeration] T? value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (value == null)
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is null" : message;
                throw new NullReferenceException(message);
            }

            return (T)value;
        }

        /// <summary>Проверка значения на значение по-умолчанию</summary>
        /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
        /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ValueNotEmpty<T>(
            [NotEmpty] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (value.Equals(default(T)))
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is empty (equal to {default(T)})" : message;
                throw new ValueEmptyException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
        /// <exception cref="ValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
        /// <param name="values">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> ValuesNotEmpty<T>(
            [NotNull, ItemNotEmpty] IEnumerable<T> values,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            foreach (T value in NotNull(values, valueName))
                if (value.Equals(default(T)))
                {
                    valueName = valueName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is empty (equal to {default(T)})" : message;
                    throw new ValueEmptyException(valueName, message);
                }

            return values;
        }

        /// <summary>Проверка строки на null и на равенство string.Empty</summary>
        /// <exception cref="NullReferenceException">Если строка == null</exception>
        /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
        /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static string NotNullOrEmpty(
            [NotNull, NotEmpty] string value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (value == null)
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is null" : message;
                throw new NullReferenceException(message);
            }

            if (value.Equals(string.Empty))
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is empty string" : message;
                throw new EmptyStringNotAllowedException(message);
            }

            return value;
        }

        /// <summary>Проверка строки на null и на равенство string.Empty или состоять только из пробелов</summary>
        /// <exception cref="NullReferenceException">Если строка == null</exception>
        /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
        /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
        /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из пробелов</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, NotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static string NotNullOrWhitespace(
            [NotNull, NotWhitespace] string value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (value == null)
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is null" : message;
                throw new NullReferenceException(message);
            }

            if (value == string.Empty)
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is empty string" : message;
                throw new EmptyStringNotAllowedException(message);
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is whitespace string" : message;
                throw new WhitespaceNotAllowedException(message);
            }

            return value;
        }

        /// <summary>Проверка что объект не null и не DBNull</summary>
        /// <exception cref="NullReferenceException">Если объект == null или DBNull</exception>
        /// <param name="value">Объект, который не должен быть равен null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T NotNullNotDbNull<T>(
            [NotNull, NotEmpty] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class
        {
            if (value == null)
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is null" : message;
                throw new NullReferenceException(message);
            }

            if (DBNull.Value.Equals(value))
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is DBNull" : message;
                throw new NullReferenceException(message);
            }

            return value;
        }

        /// <summary>Проверка что объект не null</summary>
        /// <exception><cref>TException</cref>Если условие не выполняется</exception>
        /// <param name="value">Объект, который должен быть не null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T NotNull<T, TException>(
            [NotNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class
            where TException : NullReferenceException
        {
            if (value == null)
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is null" : message;
                throw (TException) Activator.CreateInstance(typeof(TException), message);
            }

            return value;
        }

        /// <summary>Проверка что объект не null</summary>
        /// <exception><cref>TException</cref>Если условие не выполняется</exception>
        /// <param name="value">Объект, который должен быть не null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T NotNull<T, TException>(
            [NotNull, NoEnumeration] T? value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
            where TException : NullReferenceException
        {
            if (value == null)
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} is null" : message;
                throw (TException) Activator.CreateInstance(typeof(TException), message);
            }

            return (T)value;
        }

        /// <summary>Проверка что элементы последовательности не null</summary>
        /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
        /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
        /// <param name="collection">Коллекция, элементы которой должен быть не null</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ItemsNotNull<T>(
            [NotNull, ItemNotNull] T collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
            where T : class, IEnumerable
        {
            NotNull(collection, collectionName);

            foreach (object item in collection)
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (item == null)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has null item" : message;
                    throw new ItemNullsNotAllowedException(collection, collectionName, message);
                }

            return collection;
        }

        /// <summary>Проверка что элементы последовательности не null</summary>
        /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
        /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
        /// <param name="collection">Коллекция, элементы которой должен быть не null</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> ItemsNotNull<T>(
            [NotNull, ItemNotNull] IEnumerable<T?> collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            NotNull(collection, collectionName);

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (T? item in collection)
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (item == null)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has null item" : message;
                    throw new ItemNullsNotAllowedException(collection, collectionName, message);
                }

            // ReSharper disable once PossibleMultipleEnumeration
            return collection.Cast<T>();
        }

        /// <summary>Проверка что все строки не null и не пусты</summary>
        /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
        /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
        /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
        /// <param name="collection">Коллекция строк, которые быть не должны быть равны string.Empty</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, ItemNotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<string> ItemsNotEmpty(
            [NotNull, ItemNotNull, ItemNotEmpty] IEnumerable<string> collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
        {
            NotNull(collection, collectionName);

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (string str in collection)
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (str == null)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has null item" : message;
                    throw new ItemNullsNotAllowedException(collection, collectionName, message);
                }

                if (str == string.Empty)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has empty string item" : message;
                    throw new ItemEmptyStringNotAllowedException(collection, collectionName, message);
                }
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return collection;
        }

        /// <summary>Проверка что элементы коллекции не null</summary>
        /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
        /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
        /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
        /// <exception cref="ItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего кроме пробелов</exception>
        /// <param name="collection">Коллекция строк, которые быть не должны быть равны string.Empty</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, ItemNotNull, ItemNotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<string> ItemsNotWhitespace(
            [NotNull, ItemNotNull, ItemNotWhitespace] IEnumerable<string> collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
        {
            NotNull(collection, collectionName);

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (string str in collection)
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (str == null)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has null item" : message;
                    throw new ItemNullsNotAllowedException(collection, collectionName, message);
                }

                if (str == string.Empty)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has empty string item" : message;
                    throw new ItemEmptyStringNotAllowedException(collection, collectionName, message);
                }

                if (string.IsNullOrWhiteSpace(str))
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has whitespace string item" : message;
                    throw new ItemWhitespaceNotAllowedException(collection, collectionName, message);
                }
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return collection;
        }

        /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
        /// <exception cref="NullReferenceException">Если условие не выполняется</exception>
        /// <param name="collection">Коллекция, элементы которой должен быть не null</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, ItemNotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ItemsNotNullNotDbNull<T>(
            [NotNull, ItemNotNull, ItemNotEmpty] T collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
            where T : class, IEnumerable
        {
            NotNull(collection, collectionName);

            foreach (object item in collection)
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (item == null)
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has null item" : message;
                    throw new ItemNullsNotAllowedException(collection, collectionName, message);
                }

                if (DBNull.Value.Equals(item))
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has DBNull item" : message;
                    throw new ItemNullsNotAllowedException(collection, collectionName, message);
                }
            }

            return collection;
        }

        /// <summary>Проверка условия</summary>
        /// <exception cref="Exception">Если условие не выполняется</exception>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static void Assert(
            bool condition,
            [CanBeNull, NotEmpty, InvokerParameterName] string message = null)
        {
            if (!condition)
                throw new Exception(message);
        }

        /// <summary>Проверка условия</summary>
        /// <exception cref="Exception">Если условие не выполняется</exception>
        /// <param name="value">Возвращаемое значение если проверка пройдена</param>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T Assert<T>(
            [CanBeNull] T value,
            [NotNull] Func<T, bool> condition,
            [CanBeNull, NotEmpty, InvokerParameterName] string message = null)
        {
            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(value))
                throw new Exception(message);
            return value;
        }

        /// <summary>Проверка условия</summary>
        /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static void Assert<TException>(
            bool condition,
            [NotNull] object[] exceptionParams)
            where TException : Exception
        {
            if (!condition)
                throw (TException)Activator.CreateInstance(typeof(TException), exceptionParams);
        }

        /// <summary>Проверка условия</summary>
        /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
        /// <param name="value">Возвращаемое значение если проверка пройдена</param>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
        [ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T Assert<T, TException>(
            [CanBeNull] T value,
            [NotNull] Func<T, bool> condition,
            [NotNull] object[] exceptionParams)
            where TException : Exception
        {
            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(value))
                throw (TException)Activator.CreateInstance(typeof(TException), exceptionParams);
            return value;
        }

        /// <summary>Проверка условия</summary>
        /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static void Assert<TException>(
            bool condition,
            [CanBeNull] string message = null)
            where TException : Exception
        {
            if (!condition)
                throw (TException) Activator.CreateInstance(typeof(TException), message);
        }

        /// <summary>Проверка условия</summary>
        /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
        /// <param name="value">Возвращаемое значение если проверка пройдена</param>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T Assert<T, TException>(
            [CanBeNull] T value,
            [NotNull] Func<T, bool> condition,
            [CanBeNull] string message = null)
            where TException : Exception
        {
            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(value))
                throw (TException) Activator.CreateInstance(typeof(TException), message);
            return value;
        }

        /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
        /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
        /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> ItemsIs<T>(
            [NotNull, ItemNotNull] IEnumerable collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
        {
            foreach (object item in NotNull(collection, collectionName))
                if (item != null && !(item is T))
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has item \"{item}\", with is not {typeof(T)}" : message;
                    throw new Exception(message);
                }

            return collection.ConvertAll<T>();
        }

        /// <summary>Конструктор сообщения об ошибке для ненулевого объекта</summary>
        [NotNull] public delegate string ObjectMessageFactory([NotNull] object value);

        /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
        /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
        /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
        [NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> ItemsIs<T>(
            [NotNull, ItemNotNull] IEnumerable collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName,
            [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
        {
            Debug.ArgumentNotNull(messageFactory, nameof(messageFactory));

            foreach (object item in NotNull(collection, collectionName))
                if (item != null && !(item is T))
                {
                    string message = messageFactory(item);
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ?  $"Collection {collectionName} has item \"{item}\", with is not {typeof(T)}" : message;
                    throw new Exception(message);
                }

            return collection.ConvertAll<T>();
        }

        /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
        /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
        /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
        [NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> ItemsIs<T>(
            [NotNull, ItemNotNull] IEnumerable collection,
            [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
            => ItemsIs<T>(collection, null, messageFactory);

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="predicate">Условие</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> All<T>(
            [NotNull] IEnumerable<T> collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [CanBeNull] string message = null)
        {
            Debug.ArgumentNotNull(predicate, nameof(predicate));

            foreach (T item in NotNull(collection, collectionName))
                if (!predicate(item))
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has item \"{item}\", with is not satisfy the condition" : message;
                    throw new Exception(message);
                }

            return collection;
        }

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="predicate">Условие</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> All<T>(
            [NotNull] IEnumerable<T> collection,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [CanBeNull] string message = null)
            => All(collection, null, predicate, message);

        /// <summary>Конструктор сообщения об ошибке</summary>
        [NotNull] public delegate string TemplateMessageFactory<in T>(T value);

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="predicate">Условие</param>
        /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> All<T>(
            [NotNull] IEnumerable<T> collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [NotNull] TemplateMessageFactory<T> messageFactory)
        {
            Debug.ArgumentNotNull(predicate, nameof(predicate));
            Debug.ArgumentNotNull(messageFactory, nameof(messageFactory));

            foreach (T item in NotNull(collection, collectionName))
                if (!predicate(item))
                {
                    string message = messageFactory(item);
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} has item \"{item}\", with is not satisfy the condition" : message;
                    throw new Exception(message);
                }

            return collection;
        }

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="predicate">Условие</param>
        /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> All<T>(
            [NotNull] IEnumerable<T> collection,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [NotNull] TemplateMessageFactory<T> messageFactory)
            => All(collection, null, predicate, messageFactory);

        /// <summary>Проверка, что перечисление не пусто</summary>
        /// <exception cref="NullReferenceException">Если перечисление равно null</exception>
        /// <exception cref="CollectionIsEmptyException">Если перечисление пусто</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static TEnumerable NotNullNotEmpty<TEnumerable>(
            [NotNull] TEnumerable collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull, CanBeEmpty] string message = null)
            where TEnumerable : class, IEnumerable
        {
            NotNull(collection, collectionName);

            if (!collection.GetEnumerator().MoveNext())
            {
                collectionName = collectionName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Collection {collectionName} is empty" : message;
                throw new CollectionIsEmptyException(collectionName, message);
            }

            return collection;
        }

        /// <summary>Проверка, что коллекция не пуста</summary>
        /// <exception cref="ArgumentNullException">Если коллекция равна null</exception>
        /// <exception cref="ArgumentCollectionIsEmptyException">Если коллекция пуста</exception>
        /// <param name="collection">Коллекция</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static TEnumerable ArgumentNotNullNotEmpty<TEnumerable>(
            [NotNull] TEnumerable collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull, CanBeEmpty] string message = null)
            where TEnumerable : class, IEnumerable
        {
            ArgumentNotNull(collection, collectionName);

            if (!collection.GetEnumerator().MoveNext())
            {
                collectionName = collectionName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Argument collection {collectionName} is empty" : message;
                throw new ArgumentCollectionIsEmptyException(collectionName, message);
            }

            return collection;
        }

        /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
        /// <exception cref="InvalidOperationException">Если condition == false</exception>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static void ObjectState(
            bool condition,
            [CanBeNull, InvokerParameterName] string message = null)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }

        /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
        /// <exception cref="NullReferenceException">Если value == null</exception>
        /// <exception cref="InvalidOperationException">Если condition == false</exception>
        /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ObjectState<T>(
            [NotNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
            [NotNull] Func<T, bool> condition,
            [CanBeNull, InvokerParameterName] string message = null)
        {
            Debug.ArgumentNotNull(condition, nameof(condition));

            if (Equals(value, null))
                throw new NullReferenceException($"{valueName ?? UnknownValueName} is null");

            if (!condition(value))
                throw new InvalidOperationException(message);

            return value;
        }

        /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
        /// <exception cref="NullReferenceException">Если value == null</exception>
        /// <exception cref="InvalidOperationException">Если condition == false</exception>
        /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T ObjectState<T>(
            [NotNull, NoEnumeration] T value,
            [NotNull] Func<T, bool> condition,
            [CanBeNull, InvokerParameterName] string message = null)
            => ObjectState(value, null, condition, message);

        /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
        /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
        /// <param name="enumType">Тип перечня (enum)</param>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T EnumInRange<T>(
            [NotNull] Type enumType,
            T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            Debug.ArgumentNotNull(enumType, nameof(enumType));

            if (!Enum.IsDefined(enumType, value))
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"{valueName} == {Convert.ToInt64(value)} is out of enumeration range ({enumType})" : message;
                throw new InvalidEnumArgumentException(message);
            }

            return value;
        }

        [NotNull] public delegate Exception EnumInRangeCustomExceptionFactory<in T>(T value)
            where T : struct;

        /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
        /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
        /// <param name="enumType">Тип перечня (enum)</param>
        /// <param name="value">Значение</param>
        /// <param name="getExceptionFunc">Метод-конструктор исключительной ситуации</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T EnumInRangeCustom<T>(
            [NotNull] Type enumType,
            T value,
            [NotNull] EnumInRangeCustomExceptionFactory<T> getExceptionFunc)
            where T : struct
        {
            Debug.ArgumentNotNull(enumType, nameof(enumType));
            Debug.ArgumentNotNull(getExceptionFunc, nameof(getExceptionFunc));

            if (!Enum.IsDefined(enumType, value))
                // ReSharper disable once PossibleNullReferenceException
                throw getExceptionFunc(value);

            return value;
        }

        /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
        /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
        /// <param name="enumType">Тип перечня (enum)</param>
        /// <param name="collection">Список значений</param>
        /// <param name="collectionName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static IEnumerable<T> AllEnumInRange<T>(
            [NotNull] Type enumType,
            [NotNull] IEnumerable<T> collection,
            [CanBeNull, NotWhitespace, InvokerParameterName] string collectionName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            Debug.ArgumentNotNull(enumType, nameof(enumType));
            NotNull(collection, collectionName);

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (T value in collection)
                if (!Enum.IsDefined(enumType, value))
                {
                    collectionName = collectionName ?? UnknownValueName;
                    message = string.IsNullOrWhiteSpace(message)
                        ? $"Collection {collectionName} contains value {Convert.ToInt64(value)} with is out of enumeration range ({enumType})"
                        : message;
                    throw new InvalidEnumArgumentException(message);
                }

            // ReSharper disable once PossibleMultipleEnumeration
            return collection;
        }

        /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
        /// <exception cref="NullReferenceException">Если объект null</exception>
        /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
        /// <param name="value">Проверяемый объект</param>
        /// <param name="valueName">Наименование переданного объекта</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T Is<T>(
            [NotNull, NoEnumeration] object value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            NotNull(value, valueName, message);

            if (!(value is T))
            {
                valueName = valueName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"'{valueName}' is not of type '{typeof(T)}'" : message;
                throw new Exception(message);
            }

            return (T)value;
        }

        /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
        /// <exception cref="NullReferenceException">Если объект null</exception>
        /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
        /// <param name="value">Проверяемый объект</param>
        /// <param name="valueName">Наименование переданного объекта</param>
        /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T Is<T>(
            [NotNull, NoEnumeration] object value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
            [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
        {
            Debug.ArgumentNotNull(messageFactory, nameof(messageFactory));
            NotNull(value, valueName);

            if (!(value is T))
                throw new Exception(messageFactory(value));

            return (T)value;
        }

        /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
        /// <exception cref="NullReferenceException">Если объект null</exception>
        /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
        /// <param name="value">Проверяемый объект</param>
        /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static T Is<T>(
            [NotNull, NoEnumeration] object value,
            [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
            => Is<T>(value, null, messageFactory);

        /// <summary>Проверка того, что файл по указанному пути существует на диске</summary>
        /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
        /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
        /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
        /// <exception cref="FileNotFoundException">Если файл отсутствует на диске</exception>
        /// <param name="fileName">Путь к файлу</param>
        /// <param name="valueName">Наименование переданного значения</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        /// <returns>Путь к файлу</returns>
        [NotNull, NotWhitespace, FileExists, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        public static string FileExists(
            [NotNull, NotWhitespace, FileExists] string fileName,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            ArgumentNotNullOrWhitespace(fileName, valueName);

            if (!File.Exists(fileName))
            {
                message = string.IsNullOrWhiteSpace(message) ? $"File \"{fileName}\" not found" : message;
                throw new FileNotFoundException(message, fileName);
            }

            return fileName;
        }

        /// <summary>Проверка того, что папка по указанному пути существует на диске</summary>
        /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
        /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
        /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
        /// <exception cref="DirectoryNotFoundException">Если папка отсутствует на диске</exception>
        /// <param name="path">Путь к папке</param>
        /// <param name="valueName">Наименование переданного значения</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        /// <returns>Путь к папке</returns>
        [NotNull, NotWhitespace, DirectoryExists, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        public static string DirectoryExists(
            [NotNull, NotWhitespace, DirectoryExists] string path,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            ArgumentNotNullOrWhitespace(path, valueName);

            if (!Directory.Exists(path))
            {
                message = string.IsNullOrWhiteSpace(message) ? $"Folder \"{path}\" not found" : message;
                throw new DirectoryNotFoundException(message);
            }

            return path;
        }

        /// <summary>Проверка что стрим не равен null, что имеет ненулевую длину и текущая позиция не находится в конце стрима</summary>
        /// <exception cref="ArgumentNullException">Если переданный стрим == null</exception>
        /// <exception cref="Exception">Если длина стрима равна 0</exception>
        /// <exception cref="EndOfStreamException">Если позиция в преданном стриме находится в его конце</exception>
        /// <param name="stream">Стрим</param>
        /// <param name="streamName">Наименование стрима</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        /// <returns>Стрим</returns>
        [NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        public static Stream StreamNotEmpty(
            [NotNull, NotEmpty] Stream stream,
            [CanBeNull, NotWhitespace, InvokerParameterName] string streamName,
            [CanBeNull] string message = null)
        {
            ArgumentNotNull(stream, streamName);

            if (!stream.CanRead)
            {
                streamName = streamName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Stream {streamName} of type {stream.GetType()} can not be read" : message;
                throw new Exception(message);
            }

            if (stream.Length == 0)
            {
                streamName = streamName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Stream {streamName} has zero length" : message;
                throw new Exception(message);
            }

            if (stream.Position >= stream.Length - 1)
            {
                streamName = streamName ?? UnknownValueName;
                message = string.IsNullOrWhiteSpace(message) ? $"Stream {streamName} has zero length" : message;
                throw new EndOfStreamException(message);
            }

            return stream;
        }

        /// <summary>Проверка что строка содержит корректный Uri</summary>
        /// <exception cref="ArgumentNullException">Если строка описывающая Uri == null</exception>
        /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка описывающая Uri == string.Empty</exception>
        /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка описывающая Uri состоит только из пробелов</exception>
        /// <exception cref="InvalidUriException">Если Uri некорректен</exception>
        /// <param name="value">Строка, содержащая Uri</param>
        /// <param name="valueName">Наименование строки</param>
        /// <param name="scheme">Схема Uri которой должен соответствовать адрес. Например UriScheme.Http для Http адреса. Если null - схема не проверяется</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        /// <returns>Строка, содержащая Uri</returns>
        [NotNull, NotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        public static string UriCorrect(
            [NotNull, NotWhitespace] string value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            UriScheme scheme = UriScheme.Any,
            [CanBeNull] string message = null)
        {
            ArgumentNotNullOrWhitespace(value, valueName);

            if (!Uri.TryCreate(value, UriKind.Absolute, out Uri uri) || scheme != UriScheme.Any && scheme != UriScheme.None && UriSchemes.Name2Value[uri.Scheme] == scheme)
            {
                valueName = valueName ?? UnknownValueName;
                throw new InvalidUriException(value, valueName, string.IsNullOrWhiteSpace(message) ? $"Uri address {value} not correct" : message);
            }

            return value;
        }
    }
}