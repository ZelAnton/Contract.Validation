using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Contract.Exceptions;

using JetBrains.Annotations;

#if FEATURE_RELIABILITY_CONTRACTS
using System.Runtime.ConstrainedExecution;
#endif

// ReSharper disable MemberHidesStaticFromOuterClass

namespace Contract.Validation
{
    /// <summary>Runtime валидация условий</summary>
    public abstract partial class Check
    {
        /// <summary>Debug only валидация условий</summary>
        public abstract class Debug
        {
            /// <summary>Производить ли Debug-only проверки</summary>
            public static bool FullCheck
#if DEBUG || FULL_CHECK
                = true;
#else
                = false;
#endif

            /// <summary>Запуск действия только при активном заданном дефайне DEBUG и/или FULL_CHECK</summary>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static void Invoke([NotNull] Action action)
                => action();

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNull<T>(
                [NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                => Check.ArgumentNotNull(value, valueName, message);

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNull<T>(
                [NoEnumeration] T? value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                // ReSharper disable once AssignNullToNotNullAttribute
                => Check.ArgumentNotNull(value, valueName, message);

            /// <summary>Проверка аргумента на значение по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentValueNotEmpty<T>(
                [NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentValueNotEmpty(value, valueName, message);

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="value">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentValuesNotEmpty<T>(
                [NoEnumeration] IEnumerable<T> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentValuesNotEmpty(value, valueName, message);

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentItemsNotNull<T>(
                [NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Check.ArgumentItemsNotNull(value, valueName, message);

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentItemsNotNull<T>(
                [NoEnumeration] IEnumerable<T?> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentItemsNotNull(value, valueName, message);

            /// <summary>Проверка что все строки в коллекции не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentItemsNotEmpty(
                [NoEnumeration] IEnumerable<string> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentItemsNotEmpty(value, valueName, message);

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ArgumentItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего кроме пробелов</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentItemsNotWhitespace(
                [NoEnumeration] IEnumerable<string> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentItemsNotWhitespace(value, valueName, message);

            /// <summary>Проверка строкового аргумента на null и на равенство string.Empty</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNullOrEmpty(
                string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentNotNullOrEmpty(value, valueName, message);

            /// <summary>Проверка строкового аргумента на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из пробелов</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNullOrWhitespace(
                string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentNotNullOrWhitespace(value, valueName, message);

            /// <summary>Проверка аргумента на null и DBNull</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null или == DBNull</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNullNotDbNull<T>(
                T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                => Check.ArgumentNotNullNotDbNull(value, valueName, message);

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Argument(
                bool condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Argument<T>(
                [CanBeNull] T value,
                [NotNull] Func<T, bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.Argument(value, condition, valueName, message);

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Argument(
                [InstantHandle] Func<bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentInRange(
                bool condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentInRange<T>(
                [NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentInRange(value, condition, valueName, message);

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentGuidNotEmpty(
                Guid guid,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentGuidNotEmpty(guid, valueName, message);

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="Exception">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void GuidNotEmpty(
                Guid guid,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.GuidNotEmpty(guid, valueName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNull<T>(
                [NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                => Check.NotNull(value, valueName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNull<T>(
                [NoEnumeration] T? value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                // ReSharper disable once AssignNullToNotNullAttribute
                => Check.NotNull(value, valueName, message);

            /// <summary>Проверка значения на значение по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ValueNotEmpty<T>(
                T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ValueNotEmpty(value, valueName, message);

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="value">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ValuesNotEmpty<T>(
                [NoEnumeration] IEnumerable<T> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ValuesNotEmpty(value, valueName, message);

            /// <summary>Проверка строки на null и на равенство string.Empty</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNullOrEmpty(
                string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.NotNullOrEmpty(value, valueName, message);

            /// <summary>Проверка строки на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из пробелов</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNullOrWhitespace(
                string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.NotNullOrWhitespace(value, valueName, message);

            /// <summary>Проверка что объект не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если объект == null или DBNull</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNullNotDbNull<T>(
                [NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                => Check.NotNullNotDbNull(value, valueName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNull<T, TException>(
                [NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                where TException : NullReferenceException
                => Check.NotNull<T, TException>(value, valueName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNull<T, TException>(
                [NoEnumeration] T? value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                where TException : NullReferenceException
                // ReSharper disable once AssignNullToNotNullAttribute
                => Check.NotNull<T, TException>(value, valueName, message);

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsNotNull<T>(
                [NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Check.ItemsNotNull(value, valueName, message);

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsNotNull<T>(
                [NoEnumeration] IEnumerable<T?> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ItemsNotNull(value, valueName, message);

            /// <summary>Проверка что все строки не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsNotEmpty(
                [NoEnumeration] IEnumerable<string> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ItemsNotEmpty(value, valueName, message);

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего кроме пробелов</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsNotWhitespace(
                [NoEnumeration] IEnumerable<string> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ItemsNotWhitespace(value, valueName, message);

            /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если условие не выполняется</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsNotNullNotDbNull<T>(
                [NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Check.ItemsNotNullNotDbNull(value, valueName, message);

            /// <summary>Проверка условия</summary>
            /// <exception cref="Exception">Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Assert<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull, NotEmpty, InvokerParameterName] string message = null)
                => Check.Assert(value, condition, message);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [NotNull] object[] exceptionParams)
                where TException : Exception
                => Check.Assert<T, TException>(value, condition, exceptionParams);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull] string message = null)
                where TException : Exception
                => Check.Assert<T, TException>(value, condition, message);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsIs<T>(
                [NoEnumeration] IEnumerable value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ItemsIs<T>(value, valueName, message);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsIs<T>(
                [NoEnumeration] IEnumerable value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.ItemsIs<T>(value, valueName, messageFactory);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsIs<T>(
                [NoEnumeration] IEnumerable value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.ItemsIs<T>(value, UnknownValueName, messageFactory);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void All<T>(
                [NoEnumeration] IEnumerable<T> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => Check.All(value, valueName, predicate, message);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void All<T>(
                [NoEnumeration] IEnumerable<T> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Check.All(value, valueName, predicate, messageFactory);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void All<T>(
                [NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Check.All(value, UnknownValueName, predicate, messageFactory);

            /// <summary>Проверка, что перечисление не пусто</summary>
            /// <exception cref="NullReferenceException">Если перечисление равно null</exception>
            /// <exception cref="CollectionIsEmptyException">Если перечисление пусто</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNullNotEmpty<TEnumerable>(
                [NoEnumeration] TEnumerable value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull, CanBeEmpty] string message = null)
                where TEnumerable : class, IEnumerable
                => Check.NotNullNotEmpty(value, valueName, message);

            /// <summary>Проверка, что коллекция не пуста</summary>
            /// <exception cref="ArgumentNullException">Если коллекция равна null</exception>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNullNotEmpty<TEnumerable>(
                [NoEnumeration] TEnumerable value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull, CanBeEmpty] string message = null)
                where TEnumerable : class, IEnumerable
                => Check.ArgumentNotNullNotEmpty(value, valueName, message);

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
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
            [ContractAnnotation("condition:false => halt; value:null => halt"), ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ObjectState<T>(
                [NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null)
                => Check.ObjectState(value, valueName, condition, message);

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="NullReferenceException">Если value == null</exception>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ObjectState<T>(
                [NoEnumeration] T value,
                [NotNull] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null)
                => Check.ObjectState(value, UnknownValueName, condition, message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="enumType">Тип перечня (enum)</param>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void EnumInRange<T>(
                [NotNull] Type enumType,
                T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.EnumInRange(enumType, value, valueName, message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="enumType">Тип перечня (enum)</param>
            /// <param name="value">Значение</param>
            /// <param name="getExceptionFunc">Метод-конструктор исключительной ситуации</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void EnumInRangeCustom<T>(
                [NotNull] Type enumType,
                T value,
                [NotNull] EnumInRangeCustomExceptionFactory<T> getExceptionFunc)
                where T : struct
                => Check.EnumInRangeCustom(enumType, value, getExceptionFunc);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="enumType">Тип перечня (enum)</param>
            /// <param name="value">Список значений</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void AllEnumInRange<T>(
                [NotNull] Type enumType,
                [NoEnumeration] IEnumerable<T> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.AllEnumInRange(enumType, value, valueName, message);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="valueName">Наименование переданного объекта</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Is<T>(
                [NoEnumeration] object value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.Is<T>(value, valueName, message);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="valueName">Наименование переданного объекта</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Is<T>(
                [NoEnumeration] object value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.Is<T>(value, valueName, messageFactory);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Is<T>(
                [NoEnumeration] object value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.Is<T>(value, UnknownValueName, messageFactory);

            /// <summary>Проверка того, что файл по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="FileNotFoundException">Если файл отсутствует на диске</exception>
            /// <param name="value">Путь к файлу</param>
            /// <param name="valueName">Наименование переданного значения</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Путь к файлу</returns>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static void FileExists(
                string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.FileExists(value, valueName, message);

            /// <summary>Проверка того, что папка по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="DirectoryNotFoundException">Если папка отсутствует на диске</exception>
            /// <param name="value">Путь к папке</param>
            /// <param name="valueName">Наименование переданного значения</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Путь к папке</returns>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static void DirectoryExists(
                string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.DirectoryExists(value, valueName, message);

            /// <summary>Проверка что стрим не равен null, что имеет ненулевую длину и текущая позиция не находится в конце стрима</summary>
            /// <exception cref="ArgumentNullException">Если переданный стрим == null</exception>
            /// <exception cref="Exception">Если длина стрима равна 0</exception>
            /// <exception cref="EndOfStreamException">Если позиция в преданном стриме находится в его конце</exception>
            /// <param name="value">Стрим</param>
            /// <param name="valueName">Наименование стрима</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Стрим</returns>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static void StreamNotEmpty(
                Stream value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.StreamNotEmpty(value, valueName, message);

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
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void UriCorrect(
                string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                UriScheme scheme = UriScheme.Any,
                [CanBeNull] string message = null)
                => Check.UriCorrect(value, valueName, scheme, message);
        }
    }
}