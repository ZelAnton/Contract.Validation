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
// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable LoopCanBePartlyConvertedToQuery
// ReSharper disable MemberHidesStaticFromOuterClass
// ReSharper disable RedundantTypeSpecificationInDefaultExpression

namespace Contract.Validation
{
    /// <summary>Runtime валидация условий</summary>
    public abstract partial class Check
    {
        /// <summary>Условная валидация условий. Все методы работают только если у класса установлен статический флаг FullCheck, иначе значения возвращаются прозрачно без проверки.</summary>
        public abstract class Optional
        {
            /// <summary>Производить ли Debug-only проверки</summary>
            public static bool FullCheck
#if DEBUG || FULL_CHECK
                = true;
#else
                = false;
#endif

            /// <summary>Запуск действия только при активном заданном дефайне DEBUG и/или FULL_CHECK</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static void Invoke([NotNull] Action action)
            {
                Debug.ArgumentNotNull(action, nameof(action));

                if (FullCheck)
                    action();
            }

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ArgumentNotNull<T>(
                [NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                => FullCheck
                    ? Check.ArgumentNotNull(value, valueName, message)
                    : value;

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ArgumentNotNull<T>(
                [NoEnumeration] T? value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => FullCheck
                    // ReSharper disable once AssignNullToNotNullAttribute
                    ? Check.ArgumentNotNull(value, valueName, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : (T)value;

            /// <summary>Проверка аргумента на значение по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ArgumentValueNotEmpty<T>(
                [NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => FullCheck
                    ? Check.ArgumentValueNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="values">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ArgumentValuesNotEmpty<T>(
                [NoEnumeration] IEnumerable<T> values,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => FullCheck
                    ? Check.ArgumentValuesNotEmpty(values, valueName, message)
                    : values;

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ArgumentItemsNotNull<T>(
                [NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => FullCheck
                    ? Check.ArgumentItemsNotNull(value, valueName, message)
                    : value;

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ArgumentItemsNotNull<T>(
                [NoEnumeration] IEnumerable<T?> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => FullCheck
                    ? Check.ArgumentItemsNotNull(value, valueName, message)
                    : value.Cast<T>();

            /// <summary>Проверка что все строки в коллекции не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, ItemNotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<string> ArgumentItemsNotEmpty(
                [NoEnumeration] IEnumerable<string> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.ArgumentItemsNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ArgumentItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего кроме пробелов</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, ItemNotNull, ItemNotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<string> ArgumentItemsNotWhitespace(
                IEnumerable<string> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.ArgumentItemsNotWhitespace(value, valueName, message)
                    : value;

            /// <summary>Проверка строкового аргумента на null и на равенство string.Empty</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static string ArgumentNotNullOrEmpty(
                string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.ArgumentNotNullOrEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка строкового аргумента на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из пробелов</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, NotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static string ArgumentNotNullOrWhitespace(
                string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.ArgumentNotNullOrWhitespace(value, valueName, message)
                    : value;

            /// <summary>Проверка аргумента на null и DBNull</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null или == DBNull</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ArgumentNotNullNotDbNull<T>(
                T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                => FullCheck
                    ? Check.ArgumentNotNullNotDbNull(value, valueName, message)
                    : value;

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Argument(
                bool condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (FullCheck)
                    Check.Argument(condition, valueName, message);
            }

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка будет выполнена</param>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Argument<T>(
                [CanBeNull] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.Argument(value, condition, valueName, message)
                    : value;

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Argument(
                [NotNull, InstantHandle] Func<bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (FullCheck)
                    Check.Argument(condition, valueName, message);
            }

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentInRange(
                bool condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (FullCheck)
                    Check.ArgumentInRange(condition, valueName, message);
            }

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка будет выполнена</param>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ArgumentInRange<T>(
                [CanBeNull] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.ArgumentInRange(value, condition, valueName, message)
                    : value;

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static Guid ArgumentGuidNotEmpty(
                Guid guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.ArgumentGuidNotEmpty(guid, valueName, message)
                    : guid;

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="Exception">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static Guid GuidNotEmpty(
                Guid guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.GuidNotEmpty(guid, valueName, message)
                    : guid;

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T NotNull<T>(
                [NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                => FullCheck
                    ? Check.NotNull(value, valueName, message)
                    : value;

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T NotNull<T>(
                [NoEnumeration] T? value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => FullCheck
                    // ReSharper disable once AssignNullToNotNullAttribute
                    ? Check.NotNull(value, valueName, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : (T)value;

            /// <summary>Проверка значения на значение по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ValueNotEmpty<T>(
                T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => FullCheck
                    ? Check.ValueNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="values">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ValuesNotEmpty<T>(
                [NoEnumeration] IEnumerable<T> values,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => FullCheck
                    ? Check.ValuesNotEmpty(values, valueName, message)
                    : values;

            /// <summary>Проверка строки на null и на равенство string.Empty</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static string NotNullOrEmpty(
                string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.NotNullOrEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка строки на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из пробелов</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, NotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static string NotNullOrWhitespace(
                [NotNull, NotWhitespace] string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.NotNullOrWhitespace(value, valueName, message)
                    : value;

            /// <summary>Проверка что объект не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если объект == null или DBNull</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T NotNullNotDbNull<T>(
                [NotNull, NotEmpty, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                => FullCheck
                    ? Check.NotNullNotDbNull(value, valueName, message)
                    : value;

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T NotNull<T, TException>(
                [NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                where TException : NullReferenceException
                => FullCheck
                    ? Check.NotNull<T, TException>(value, valueName, message)
                    : value;

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T NotNull<T, TException>(
                [NoEnumeration] T? value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                where TException : NullReferenceException
                => FullCheck
                    // ReSharper disable once AssignNullToNotNullAttribute
                    ? Check.NotNull<T, TException>(value, valueName, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : (T)value;

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ItemsNotNull<T>(
                [NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => FullCheck
                    ? Check.ItemsNotNull(value, valueName, message)
                    : value;

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ItemsNotNull<T>(
                [NoEnumeration] IEnumerable<T?> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => FullCheck
                    ? Check.ItemsNotNull(value, valueName, message)
                    : value.Cast<T>();

            /// <summary>Проверка что все строки не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, ItemNotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<string> ItemsNotEmpty(
                [NotNull, ItemNotNull, ItemNotEmpty, NoEnumeration] IEnumerable<string> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.ItemsNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего кроме пробелов</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, ItemNotNull, ItemNotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<string> ItemsNotWhitespace(
                [NotNull, ItemNotNull, ItemNotWhitespace, NoEnumeration] IEnumerable<string> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.ItemsNotWhitespace(value, valueName, message)
                    : value;

            /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если условие не выполняется</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, ItemNotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ItemsNotNullNotDbNull<T>(
                [NotNull, ItemNotNull, ItemNotEmpty, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => FullCheck
                    ? Check.ItemsNotNullNotDbNull(value, valueName, message)
                    : value;

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
                if (FullCheck)
                    Check.Assert(condition, message);
            }

            /// <summary>Проверка условия</summary>
            /// <exception cref="Exception">Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Assert<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull, NotEmpty, InvokerParameterName] string message = null)
                => FullCheck
                    ? Check.Assert(value, condition, message)
                    : value;

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
                if (FullCheck)
                    Check.Assert<TException>(condition, exceptionParams);
            }

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
            [Pure, ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Assert<T, TException>(
                [CanBeNull] T value,
                [NotNull] Func<T, bool> condition,
                [NotNull] object[] exceptionParams)
                where TException : Exception
                => FullCheck
                    ? Check.Assert<T, TException>(value, condition, exceptionParams)
                    : value;

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
                if (FullCheck)
                    Check.Assert<TException>(condition, message);
            }

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, ContractAnnotation("condition:false => halt; value:null => null"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull] string message = null)
                where TException : Exception
                => FullCheck
                    ? Check.Assert<T, TException>(value, condition, message)
                    : value;

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ItemsIs<T>(
                [NoEnumeration] IEnumerable value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.ItemsIs<T>(value, valueName, message)
                    : value.ConvertAll<T>();

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            [Pure, NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ItemsIs<T>(
                [NoEnumeration] IEnumerable value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => FullCheck
                    ? Check.ItemsIs<T>(value, valueName, messageFactory)
                    : value.ConvertAll<T>();

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            [Pure, NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> ItemsIs<T>(
                [NoEnumeration] IEnumerable value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => FullCheck
                    ? Check.ItemsIs<T>(value, UnknownValueName, messageFactory)
                    : value.ConvertAll<T>();

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> All<T>(
                [NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.All(value, valueName, predicate, message)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> All<T>(
                [NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => FullCheck
                    ? Check.All(value, valueName, predicate, messageFactory)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> All<T>(
                [NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => FullCheck
                    ? Check.All(value, UnknownValueName, predicate, messageFactory)
                    : value;

            /// <summary>Проверка, что перечисление не пусто</summary>
            /// <exception cref="NullReferenceException">Если перечисление равно null</exception>
            /// <exception cref="CollectionIsEmptyException">Если перечисление пусто</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static TEnumerable NotNullNotEmpty<TEnumerable>(
                [NoEnumeration] TEnumerable value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull, CanBeEmpty] string message = null)
                where TEnumerable : class, IEnumerable
                => FullCheck
                    ? Check.NotNullNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка, что коллекция не пуста</summary>
            /// <exception cref="ArgumentNullException">Если коллекция равна null</exception>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static TEnumerable ArgumentNotNullNotEmpty<TEnumerable>(
                [NoEnumeration] TEnumerable value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull, CanBeEmpty] string message = null)
                where TEnumerable : class, IEnumerable
                => FullCheck
                    ? Check.ArgumentNotNullNotEmpty(value, valueName, message)
                    : value;

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
                if (FullCheck)
                    Check.ObjectState(condition, message);
            }

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="NullReferenceException">Если value == null</exception>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, ContractAnnotation("condition:false => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ObjectState<T>(
                [NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null)
                => FullCheck
                    ? Check.ObjectState(value, valueName, condition, message)
                    : value;

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="NullReferenceException">Если value == null</exception>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, ContractAnnotation("condition:false => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T ObjectState<T>(
                [NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null)
                => FullCheck
                    ? Check.ObjectState(value, UnknownValueName, condition, message)
                    : value;

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="enumType">Тип перечня (enum)</param>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T EnumInRange<T>(
                [NotNull] Type enumType,
                T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => FullCheck
                    ? Check.EnumInRange(enumType, value, valueName, message)
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
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static IEnumerable<T> AllEnumInRange<T>(
                [NotNull] Type enumType,
                [NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => FullCheck
                    ? Check.AllEnumInRange(enumType, value, valueName, message)
                    : value;

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="valueName">Наименование переданного объекта</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Is<T>(
                [NoEnumeration] object value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.Is<T>(value, valueName, message)
                    : (T)value;

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="valueName">Наименование переданного объекта</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Is<T>(
                [NoEnumeration] object value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => FullCheck
                    ? Check.Is<T>(value, valueName, messageFactory)
                    : (T)value;

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            [Pure, NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static T Is<T>(
                [NoEnumeration] object value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => FullCheck
                    ? Check.Is<T>(value, UnknownValueName, messageFactory)
                    : (T)value;

            /// <summary>Проверка того, что файл по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="FileNotFoundException">Если файл отсутствует на диске</exception>
            /// <param name="fileName">Путь к файлу</param>
            /// <param name="valueName">Наименование переданного значения</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Путь к файлу</returns>
            [Pure, NotNull, NotWhitespace, FileExists, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static string FileExists(
                [NotNull, NotWhitespace, FileExists] string fileName,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.FileExists(fileName, valueName, message)
                    : fileName;

            /// <summary>Проверка того, что папка по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="DirectoryNotFoundException">Если папка отсутствует на диске</exception>
            /// <param name="path">Путь к папке</param>
            /// <param name="valueName">Наименование переданного значения</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Путь к папке</returns>
            [Pure, NotNull, NotWhitespace, DirectoryExists, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static string DirectoryExists(
                [NotNull, NotWhitespace, DirectoryExists] string path,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.DirectoryExists(path, valueName, message)
                    : path;

            /// <summary>Проверка что стрим не равен null, что имеет ненулевую длину и текущая позиция не находится в конце стрима</summary>
            /// <exception cref="ArgumentNullException">Если переданный стрим == null</exception>
            /// <exception cref="Exception">Если длина стрима равна 0</exception>
            /// <exception cref="EndOfStreamException">Если позиция в преданном стриме находится в его конце</exception>
            /// <param name="stream">Стрим</param>
            /// <param name="streamName">Наименование стрима</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Стрим</returns>
            [Pure, NotNull, NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static Stream StreamNotEmpty(
                [NotNull] Stream stream,
                [CanBeNull, NotWhitespace, InvokerParameterName] string streamName,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.StreamNotEmpty(stream, streamName, message)
                    : stream;

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
            [Pure, NotNull, NotWhitespace, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static string UriCorrect(
                [NotNull, NotWhitespace] string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                UriScheme scheme = UriScheme.Any,
                [CanBeNull] string message = null)
                => FullCheck
                    ? Check.UriCorrect(value, valueName, scheme, message)
                    : value;
        }
    }
}