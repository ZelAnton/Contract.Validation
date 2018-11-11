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
            /// <param name="argument">Объект, который не должен быть равен null</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNull<T>(
                [NotNull, NoEnumeration] T argument,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
                where T : class
                => Check.ArgumentNotNull(argument, argumentName, message);

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="argument">Объект, который не должен быть равен null</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNull<T>(
                [NotNull, NoEnumeration] T? argument,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
                where T : struct
                // ReSharper disable once AssignNullToNotNullAttribute
                => Check.ArgumentNotNull(argument, argumentName, message);

            /// <summary>Проверка аргумента на значение по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentValueNotEmpty<T>(
                [NotEmpty, NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentValueNotEmpty(value, argumentName, message);

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="values">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentValuesNotEmpty<T>(
                [NotNull, ItemNotEmpty, NoEnumeration] IEnumerable<T> values,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentValuesNotEmpty(values, valueName, message);

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="collection">Коллекция</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentItemsNotNull<T>(
                [NotNull, ItemNotNull, NoEnumeration] T collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Check.ArgumentItemsNotNull(collection, collectionName, message);

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="collection">Коллекция</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentItemsNotNull<T>(
                [NotNull, ItemNotNull, NoEnumeration] IEnumerable<T?> collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentItemsNotNull(collection, collectionName, message);

            /// <summary>Проверка что все строки в коллекции не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="collection">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentItemsNotEmpty(
                [NotNull, ItemNotNull, ItemNotEmpty] IEnumerable<string> collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                => Check.ArgumentItemsNotEmpty(collection, collectionName, message);

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ArgumentItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего кроме пробелов</exception>
            /// <param name="collection">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentItemsNotWhitespace(
                [NotNull, ItemNotNull, ItemNotWhitespace] IEnumerable<string> collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                => Check.ArgumentItemsNotWhitespace(collection, collectionName, message);

            /// <summary>Проверка строкового аргумента на null и на равенство string.Empty</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="argument">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNullOrEmpty(
                [NotNull, NotEmpty] string argument,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
                => Check.ArgumentNotNullOrEmpty(argument, argumentName, message);

            /// <summary>Проверка строкового аргумента на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="argument">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из пробелов</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNullOrWhitespace(
                [NotNull, NotWhitespace] string argument,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
                => Check.ArgumentNotNullOrWhitespace(argument, argumentName, message);

            /// <summary>Проверка аргумента на null и DBNull</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null или == DBNull</exception>
            /// <param name="argument">Объект, который не должен быть равен null</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNullNotDbNull<T>(
                [NotNull, NotEmpty] T argument,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
                where T : class
                => Check.ArgumentNotNullNotDbNull(argument, argumentName, message);

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Argument(
                bool condition,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
            {
                if (FullCheck)
                    Check.Argument(condition, argumentName, message);
            }

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="argument">Возвращаемое значение если проверка будет выполнена</param>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Argument<T>(
                [CanBeNull] T argument,
                [NotNull] Func<T, bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
                => Check.Argument(argument, condition, argumentName, message);

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Argument(
                [InstantHandle] Func<bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
            {
                if (FullCheck)
                    Check.Argument(condition, argumentName, message);
            }

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentInRange(
                bool condition,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
            {
                if (FullCheck)
                    Check.ArgumentInRange(condition, argumentName, message);
            }

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="argument">Возвращаемое значение если проверка будет выполнена</param>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentInRange<T>(
                [CanBeNull] T argument,
                [NotNull] Func<T, bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
                => Check.ArgumentInRange(argument, condition, argumentName, message);

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="argumentName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentGuidNotEmpty(
                [NotEmpty] Guid guid,
                [NotNull, NotWhitespace, InvokerParameterName] string argumentName,
                [CanBeNull] string message = null)
                => Check.ArgumentGuidNotEmpty(guid, argumentName, message);

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
                [NotEmpty] Guid guid,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.GuidNotEmpty(guid, valueName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNull<T>(
                [NotNull, NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                => Check.NotNull(value, valueName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNull<T>(
                [NotNull, NoEnumeration] T? value,
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
                [NotEmpty] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ValueNotEmpty(value, valueName, message);

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="values">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ValuesNotEmpty<T>(
                [NotNull, ItemNotEmpty, NoEnumeration] IEnumerable<T> values,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ValuesNotEmpty(values, valueName, message);

            /// <summary>Проверка строки на null и на равенство string.Empty</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNullOrEmpty(
                [NotNull, NotEmpty] string value,
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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNullOrWhitespace(
                [NotNull, NotWhitespace] string value,
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
                [NotNull, NotEmpty, NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                => Check.NotNullNotDbNull(value, valueName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNull<T, TException>(
                [NotNull, NoEnumeration] T value,
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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
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
            /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="collection">Коллекция, элементы которой должен быть не null</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsNotNull<T>(
                [NotNull, ItemNotNull, NoEnumeration] T collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Check.ItemsNotNull(collection, collectionName, message);

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="collection">Коллекция, элементы которой должен быть не null</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsNotNull<T>(
                [NotNull, ItemNotNull, NoEnumeration] IEnumerable<T?> collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ItemsNotNull(collection, collectionName, message);

            /// <summary>Проверка что все строки не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="collection">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsNotEmpty(
                [NotNull, ItemNotNull, ItemNotEmpty, NoEnumeration] IEnumerable<string> collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                => Check.ItemsNotEmpty(collection, collectionName, message);

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="collection"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего кроме пробелов</exception>
            /// <param name="collection">Коллекция строк, которые быть не должны быть равны string.Empty</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsNotWhitespace(
                [NotNull, ItemNotNull, ItemNotWhitespace, NoEnumeration] IEnumerable<string> collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                => Check.ItemsNotWhitespace(collection, collectionName, message);

            /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если условие не выполняется</exception>
            /// <param name="collection">Коллекция, элементы которой должен быть не null</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsNotNullNotDbNull<T>(
                [NotNull, ItemNotNull, ItemNotEmpty, NoEnumeration] T collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Check.ItemsNotNullNotDbNull(collection, collectionName, message);

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
                [CanBeNull] T value,
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
            /// <param name="collection">Коллекция</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsIs<T>(
                [NotNull, ItemNotNull, NoEnumeration] IEnumerable collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                => Check.ItemsIs<T>(collection, collectionName, message);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="collection">Коллекция</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsIs<T>(
                [NotNull, ItemNotNull, NoEnumeration] IEnumerable collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.ItemsIs<T>(collection, collectionName, messageFactory);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="collection">Коллекция</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ItemsIs<T>(
                [NotNull, ItemNotNull, NoEnumeration] IEnumerable collection,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.ItemsIs<T>(collection, UnknownValueName, messageFactory);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="collection">Коллекция</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void All<T>(
                [NotNull, NoEnumeration] IEnumerable<T> collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => Check.All(collection, collectionName, predicate, message);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="collection">Коллекция</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void All<T>(
                [NotNull, NoEnumeration] IEnumerable<T> collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Check.All(collection, collectionName, predicate, messageFactory);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="Exception">Если для какого-нибудь элемента перечисления не выполнится переданное условие</exception>
            /// <param name="collection">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void All<T>(
                [NotNull, NoEnumeration] IEnumerable<T> collection,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Check.All(collection, UnknownValueName, predicate, messageFactory);

            /// <summary>Проверка, что перечисление не пусто</summary>
            /// <exception cref="NullReferenceException">Если перечисление равно null</exception>
            /// <exception cref="CollectionIsEmptyException">Если перечисление пусто</exception>
            /// <param name="collection">Коллекция</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void NotNullNotEmpty<TEnumerable>(
                [NotNull, NotEmpty, NoEnumeration] TEnumerable collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull, CanBeEmpty] string message = null)
                where TEnumerable : class, IEnumerable
                => Check.NotNullNotEmpty(collection, collectionName, message);

            /// <summary>Проверка, что коллекция не пуста</summary>
            /// <exception cref="ArgumentNullException">Если коллекция равна null</exception>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="collection">Коллекция</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentNotNullNotEmpty<TEnumerable>(
                [NotNull, NotEmpty, NoEnumeration] TEnumerable collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull, CanBeEmpty] string message = null)
                where TEnumerable : class, IEnumerable
                => Check.ArgumentNotNullNotEmpty(collection, collectionName, message);

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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ObjectState<T>(
                [NotNull, NoEnumeration] T value,
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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ObjectState<T>(
                [NotNull, NoEnumeration] T value,
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
            /// <param name="collection">Список значений</param>
            /// <param name="collectionName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void AllEnumInRange<T>(
                [NotNull] Type enumType,
                [NotNull, NoEnumeration] IEnumerable<T> collection,
                [NotNull, NotWhitespace, InvokerParameterName] string collectionName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.AllEnumInRange(enumType, collection, collectionName, message);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="valueName">Наименование переданного объекта</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Is<T>(
                [NotNull, NoEnumeration] object value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.Is<T>(value, valueName, message);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="valueName">Наименование переданного объекта</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Is<T>(
                [NotNull, NoEnumeration] object value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.Is<T>(value, valueName, messageFactory);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void Is<T>(
                [NotNull, NoEnumeration] object value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.Is<T>(value, UnknownValueName, messageFactory);

            /// <summary>Проверка того, что файл по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="FileNotFoundException">Если файл отсутствует на диске</exception>
            /// <param name="fileName">Путь к файлу</param>
            /// <param name="valueName">Наименование переданного значения</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Путь к файлу</returns>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static void FileExists(
                [NotNull, NotWhitespace, FileExists] string fileName,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.FileExists(fileName, valueName, message);

            /// <summary>Проверка того, что папка по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="DirectoryNotFoundException">Если папка отсутствует на диске</exception>
            /// <param name="path">Путь к папке</param>
            /// <param name="valueName">Наименование переданного значения</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Путь к папке</returns>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static void DirectoryExists(
                [NotNull, NotWhitespace, DirectoryExists] string path,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.DirectoryExists(path, valueName, message);

            /// <summary>Проверка что стрим не равен null, что имеет ненулевую длину и текущая позиция не находится в конце стрима</summary>
            /// <exception cref="ArgumentNullException">Если переданный стрим == null</exception>
            /// <exception cref="Exception">Если длина стрима равна 0</exception>
            /// <exception cref="EndOfStreamException">Если позиция в преданном стриме находится в его конце</exception>
            /// <param name="stream">Стрим</param>
            /// <param name="streamName">Наименование стрима</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Стрим</returns>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
            public static void StreamNotEmpty(
                [NotNull, NotEmpty] Stream stream,
                [NotNull, NotWhitespace, InvokerParameterName] string streamName,
                [CanBeNull] string message = null)
                => Check.StreamNotEmpty(stream, streamName, message);

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
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void UriCorrect(
                [NotNull, NotWhitespace] string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                UriScheme scheme = UriScheme.Any,
                [CanBeNull] string message = null)
                => Check.UriCorrect(value, valueName, scheme, message);
        }
    }
}