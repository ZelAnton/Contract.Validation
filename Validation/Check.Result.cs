using System;
using System.IO;
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

// ReSharper disable ArrangeAttributes
// ReSharper disable InvalidXmlDocComment
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable LoopCanBePartlyConvertedToQuery
// ReSharper disable MemberHidesStaticFromOuterClass
// ReSharper disable RedundantTypeSpecificationInDefaultExpression

namespace Contract.Validation
{
    /// <summary>Runtime валидация условий</summary>
    public abstract partial class Check
    {
        /// <summary>Валидация условий для возвращаемых методами значений. В DEBUG билде проверки осуществляются, иначе - нет</summary>
        public abstract class Result
        {
            /// <summary>Производить ли проверку значений, возвращаемых методами</summary>
            public static bool FullCheck
#if DEBUG || FULL_CHECK
                = true;
#else
                = false;
#endif

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="Exception">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static Guid GuidNotEmpty(
                Guid guid,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.GuidNotEmpty(guid, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : guid;

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T NotNull<T>(
                [NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class
                => FullCheck
                    ? Check.NotNull(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T NotNull<T>(
                [NoEnumeration] T? value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => FullCheck
                    // ReSharper disable once AssignNullToNotNullAttribute
                    ? Check.NotNull(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : (T)value;

            /// <summary>Проверка значения на значение по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ValueNotEmpty<T>(
                T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => FullCheck
                    ? Check.ValueNotEmpty(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="values">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ValuesNotEmpty<T>(
                [NoEnumeration] IEnumerable<T> values,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => FullCheck
                    ? Check.ValuesNotEmpty(values, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : values;

            /// <summary>Проверка строки на null и на равенство string.Empty</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static string NotNullOrEmpty(
                string value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.NotNullOrEmpty(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка строки на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из пробелов</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, NotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static string NotNullOrWhitespace(
                [NotNull, NotWhitespace] string value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.NotNullOrWhitespace(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка что объект не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если объект == null или DBNull</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T NotNullNotDbNull<T>(
                [NotNull, NotEmpty] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class
                => FullCheck
                    ? Check.NotNullNotDbNull(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T NotNull<T, TException>(
                [NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class
                where TException : NullReferenceException
                => FullCheck
                    ? Check.NotNull<T, TException>(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T NotNull<T, TException>(
                [NoEnumeration] T? value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                where TException : NullReferenceException
                => FullCheck
                    // ReSharper disable once AssignNullToNotNullAttribute
                    ? Check.NotNull<T, TException>(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : (T)value;

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ItemsNotNull<T>(
                [NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class, IEnumerable
                => FullCheck
                    ? Check.ItemsNotNull(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ItemsNotNull<T>(
                [NoEnumeration] IEnumerable<T?> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => FullCheck
                    ? Check.ItemsNotNull(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value.ConvertAll<T>();

            /// <summary>Проверка что все строки не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, ItemNotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<string> ItemsNotEmpty(
                [NotNull, ItemNotNull, ItemNotEmpty, NoEnumeration] IEnumerable<string> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.ItemsNotEmpty(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего кроме пробелов</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, ItemNotNull, ItemNotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<string> ItemsNotWhitespace(
                [NotNull, ItemNotNull, ItemNotWhitespace, NoEnumeration] IEnumerable<string> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.ItemsNotWhitespace(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если условие не выполняется</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, ItemNotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ItemsNotNullNotDbNull<T>(
                [NotNull, ItemNotNull, ItemNotEmpty, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class, IEnumerable
                => FullCheck
                    ? Check.ItemsNotNullNotDbNull(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка условия</summary>
            /// <exception cref="Exception">Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение, если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Assert<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.Assert(value, condition, string.IsNullOrEmpty(message) ? $"Return value of {callerMemberName} assertion" : message)
                    : value;

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение, если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
            [Pure, ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [NotNull] object[] exceptionParams)
                where TException : Exception
                => FullCheck
                    ? Check.Assert<T, TException>(value, condition, exceptionParams)
                    : value;

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение, если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where TException : Exception
                => FullCheck
                    ? Check.Assert<T, TException>(value, condition, string.IsNullOrEmpty(message) ? $"Return value of {callerMemberName} assertion" : message)
                    : value;

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ItemsIs<T>(
                [NoEnumeration] IEnumerable value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.ItemsIs<T>(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value.ConvertAll<T>();

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ItemsIs<T>(
                [NoEnumeration] IEnumerable value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.ItemsIs<T>(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, messageFactory)
                    : value.ConvertAll<T>();

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> All<T>(
                [NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.All(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, predicate, message)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> All<T>(
                [NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.All(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, predicate, messageFactory)
                    : value;

            /// <summary>Проверка, что перечисление не пусто</summary>
            /// <exception cref="NullReferenceException">Если перечисление равно null</exception>
            /// <exception cref="CollectionIsEmptyException">Если перечисление пусто</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static TEnumerable NotNullNotEmpty<TEnumerable>(
                [NoEnumeration] TEnumerable value,
                [CanBeNull, CanBeEmpty] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where TEnumerable : class, IEnumerable
                => FullCheck
                    ? Check.NotNullNotEmpty(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="NullReferenceException">Если value == null</exception>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, ContractAnnotation("condition:false => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ObjectState<T>(
                [NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.ObjectState(
                        value,
                        callerMemberName != null ? $"Return value of {callerMemberName}" : null,
                        condition,
                        string.IsNullOrWhiteSpace(message) ? $"Return value of {callerMemberName} invalid operation" : message)
                    : value;

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="enumType">Тип перечня (enum)</param>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T EnumInRange<T>(
                [NotNull] Type enumType,
                T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => FullCheck
                    ? Check.EnumInRange(enumType, value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="enumType">Тип перечня (enum)</param>
            /// <param name="value">Значение</param>
            /// <param name="getExceptionFunc">Метод-конструктор исключительной ситуации</param>
            [Pure, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T EnumInRangeCustom<T>(
                [NotNull] Type enumType,
                T value,
                [NotNull] EnumInRangeCustomExceptionFactory<T> getExceptionFunc)
                where T : struct
                => FullCheck
                    ? Check.EnumInRangeCustom(enumType, value, getExceptionFunc)
                    : value;

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="enumType">Тип перечня (enum)</param>
            /// <param name="value">Список значений</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> AllEnumInRange<T>(
                [NotNull] Type enumType,
                [NoEnumeration] IEnumerable<T> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => FullCheck
                    ? Check.AllEnumInRange(enumType, value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : value;

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Is<T>(
                [NoEnumeration] object value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.Is<T>(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : (T)value;

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Is<T>(
                [NoEnumeration] object value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.Is<T>(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, messageFactory)
                    : (T)value;

            /// <summary>Проверка того, что файл по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="FileNotFoundException">Если файл отсутствует на диске</exception>
            /// <param name="fileName">Путь к файлу</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            /// <returns>Путь к файлу</returns>
            [Pure, NotNull, FileExists, NotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static string FileExists(
                [NotNull, NotWhitespace, FileExists] string fileName,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.FileExists(fileName, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : fileName;

            /// <summary>Проверка того, что папка по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="DirectoryNotFoundException">Если папка отсутствует на диске</exception>
            /// <param name="path">Путь к папке</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            /// <returns>Путь к папке</returns>
            [Pure, NotNull, DirectoryExists, NotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static string DirectoryExists(
                [NotNull, NotWhitespace, DirectoryExists] string path,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.DirectoryExists(path, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : path;

            /// <summary>Проверка что стрим не равен null, что имеет ненулевую длину и текущая позиция не находится в конце стрима</summary>
            /// <exception cref="ArgumentNullException">Если переданный стрим == null</exception>
            /// <exception cref="Exception">Если длина стрима равна 0</exception>
            /// <exception cref="EndOfStreamException">Если позиция в преданном стриме находится в его конце</exception>
            /// <param name="stream">Стрим</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, чей результат проверяется</param>
            /// <returns>Стрим</returns>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static Stream StreamNotEmpty(
                [NotNull] Stream stream,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.StreamNotEmpty(stream, callerMemberName != null ? $"Return value of {callerMemberName}" : null, message)
                    : stream;

            /// <summary>Проверка что строка содержит корректный Uri</summary>
            /// <exception cref="ArgumentNullException">Если строка описывающая Uri == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка описывающая Uri == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка описывающая Uri состоит только из пробелов</exception>
            /// <exception cref="InvalidUriException">Если Uri некорректен</exception>
            /// <param name="value">Строка, содержащая Uri</param>
            /// <param name="scheme">Схема Uri которой должен соответствовать адрес. Например UriScheme.Http для Http адреса. Если null - схема не проверяется</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">Наименование строки</param>
            /// <returns>Строка, содержащая Uri</returns>
            [Pure, NotNull, NotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static string UriCorrect(
                [NotNull, NotWhitespace] string value,
                UriScheme scheme = UriScheme.Any,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => FullCheck
                    ? Check.UriCorrect(value, callerMemberName != null ? $"Return value of {callerMemberName}" : null, scheme, message)
                    : value;
        }
    }
}