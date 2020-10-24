using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Contract.Exceptions;

using JetBrains.Annotations;

// ReSharper disable RedundantAnnotation
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Contract.Validation
{
    /// <summary>Runtime валидация условий</summary>
    public abstract partial class Check
    {
        /// <summary>Debug only валидация условий</summary>
        public abstract class Debug
        {
            /// <summary>Запуск действия только при активном заданном дефайне DEBUG и/или FULL_CHECK</summary>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Invoke([NotNull] Action action)
                => action();

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ArgumentNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                => Check.ArgumentNotNull(value, valueName, message);

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentNotNull<T>(
                [CanBeNull, NoEnumeration] T? value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentNotNull(value, valueName, message);

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentGenericNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentGenericNotNull(value, valueName, message);

            /// <summary>Проверка, что коллекция не пуста</summary>
            /// <exception cref="ArgumentNullException">Если коллекция равна null</exception>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentNotNullNotEmpty<TEnumerable>(
                [CanBeNull, NoEnumeration] TEnumerable value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull, CanBeEmpty] string message = null)
                where TEnumerable : class, IEnumerable
                => Check.ArgumentNotNullNotEmpty(value, valueName, message);

            /// <summary>Проверка аргумента на значение по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentValueNotEmpty<T>(
                T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentValueNotEmpty(value, valueName, message);

            /// <summary>Проверка что аргумент не пуст (пустое значение отличается от default)</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue">Пустое значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentValueNotEmpty<T>(
                [NoEnumeration] T value,
                T emptyValue,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentValueNotEmpty(value, emptyValue, valueName, message);

            /// <summary>Проверка что аргумент не пуст (пустое значение отличается от default)</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue1">Пустое значение</param>
            /// <param name="emptyValue2">Пустое значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentValueNotEmpty<T>(
                [NoEnumeration] T value,
                T emptyValue1,
                T emptyValue2,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentValueNotEmpty(value, emptyValue1, emptyValue2, valueName, message);

            /// <summary>Проверка что аргумент IntPtr не пуст (пустое значение отличается от IntPtr.Zero)</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == IntPtr.Zero</exception>
            /// <param name="value">Значение, которое не должно быть равно IntPtr.Zero</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentValueNotEmpty(
                IntPtr value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentValueNotEmpty(value, valueName, message);

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="value">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentValuesNotEmpty<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentValuesNotEmpty(value, valueName, message);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления
            ///                                                            не выполнится переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
#if FEATURE_RELIABILITY_CONTRACTS
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentAll<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => Check.ArgumentAll(value, valueName, predicate, message);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                            переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentAll<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => Check.ArgumentAll(value, predicate, message);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                            переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="action">Метод проверки условия</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentAll<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Action<T> action,
                [CanBeNull] string message = null)
                => Check.ArgumentAll(value, action, message);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                            переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentAll<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Check.ArgumentAll(value, valueName, predicate, messageFactory);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                            переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void ArgumentAll<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Check.ArgumentAll(value, predicate, messageFactory);

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
#if FEATURE_RELIABILITY_CONTRACTS
                [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
            public static void ArgumentItemsNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
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
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentItemsNotNull<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T?> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ArgumentItemsNotNull(value, valueName, message);

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пустой</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentCollectionNotEmpty<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class, ICollection
                => Check.ArgumentCollectionNotEmpty(value, valueName, message);

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пустой</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentReadOnlyCollectionNotEmpty<T>(
                [CanBeNull, NoEnumeration] IReadOnlyCollection<T> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentReadOnlyCollectionNotEmpty(value, valueName, message);

            /// <summary>Проверка что последовательность не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если последовательность пуста</exception>
            /// <param name="value">Последовательность, которая не должна быть пустой</param>
            /// <param name="valueName">Наименование последовательности</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentEnumerationNotEmpty(
                [CanBeNull, NoEnumeration] IEnumerable value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentEnumerationNotEmpty(value, valueName, message);

            /// <summary>Проверка что все строки в последовательности не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null или string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentStringsNotEmpty(
                [CanBeNull, NoEnumeration] IEnumerable<string> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentStringsNotEmpty(value, valueName, message);

            /// <summary>Проверка что все строки последовательности не null, не пустые строки и не строки состоящие из одних пробелов</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ArgumentItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не
            ///                                                             содержащие ничего кроме пробелов</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null, string.Empty или заполнены
            ///                     одними только пробелами</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentStringsNotWhitespace(
                [CanBeNull, NoEnumeration] IEnumerable<string> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentStringsNotWhitespace(value, valueName, message);

            /// <summary>Проверка строкового аргумента на null и на равенство string.Empty</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentNotNullOrEmpty(
                [CanBeNull] string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentNotNullOrEmpty(value, valueName, message);

            /// <summary>Проверка строкового аргумента на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из
            ///                     пробелов</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentNotNullOrWhitespace(
                [CanBeNull] string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentNotNullOrWhitespace(value, valueName, message);

            /// <summary>Проверка аргумента на null и DBNull</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null или == DBNull</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentNotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                => Check.ArgumentNotNullNotDbNull(value, valueName, message);

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Argument(
                bool condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.Argument(condition, valueName, message);

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка будет выполнена</param>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Argument<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.Argument(value, condition, valueName, message);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>ArgumentException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Argument<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [NotNull, ItemNotNull] object[] exceptionParams)
                where TException : ArgumentException
                => Check.Argument<T, TException>(value, condition, exceptionParams);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>ArgumentException</cref>: Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Argument<TException>(
                bool condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where TException : ArgumentException
                => Check.Argument<TException>(condition, valueName, message);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>ArgumentException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Argument<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where TException : ArgumentException
                => Check.Argument<T, TException>(value, condition, valueName, message);

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Argument(
                [NotNull, InstantHandle] Func<bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.Argument(condition, valueName, message);

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentInRange(
                bool condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentInRange(condition, valueName, message);

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка будет выполнена</param>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentInRange<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentInRange(value, condition, valueName, message);

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка будет выполнена</param>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentInRange<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentInRange(value, condition, valueName, message);

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentInRange(
                [NotNull, InstantHandle] Func<bool> condition,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentInRange(condition(), valueName, message);

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIndexInRange(
                int index,
                int count,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIndexInRange(index, count, valueName, message);

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIndexInRange(
                long index,
                long count,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIndexInRange(index, count, valueName, message);

            /// <summary>Проверка что строка содержит guid</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="ArgumentException">Если строка не содержит GUID</exception>
            /// <param name="guid">Строка, которая должна содержать Guid</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), GuidStr,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsGuid(
                [CanBeNull] string guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsGuid(guid, valueName, message);

            /// <summary>Проверка что строка содержит непустой guid</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="ArgumentException">Если строка не содержит GUID</exception>
            /// <exception cref="ArgumentValueEmptyException">Если GUID пуст</exception>
            /// <param name="guid">Строка, которая должна содержать непустой Guid</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentGuidNotEmpty(
                [CanBeNull] string guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentGuidNotEmpty(guid, valueName, message);

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="ArgumentValueEmptyException">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentGuidNotEmpty(
                Guid guid,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ArgumentGuidNotEmpty(guid, valueName, message);

            /// <summary>Проверка что в словаре присутствует запись с переданным ключом</summary>
            /// <exception cref="ArgumentItemNotFoundException">Если ключ не найден</exception>
            /// <param name="dictionary">Словарь</param>
            /// <param name="key">Ключ, который должен присутствовать в словаре</param>
            /// <param name="dictionaryName">Наименование словаря</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("dictionary:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentContainsKey<TKey, TValue>(
                [CanBeNull, NoEnumeration] IReadOnlyDictionary<TKey, TValue> dictionary,
                [NotNull, NoEnumeration] TKey key,
                [CanBeNull, NotWhitespace, InvokerParameterName] string dictionaryName,
                [CanBeNull] string message = null)
                => Check.ArgumentContainsKey(dictionary, key, dictionaryName, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsPositive(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsPositive(value, valueName, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsPositive(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsPositive(value, valueName, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsPositive(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsPositive(value, valueName, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsPositive(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsPositive(value, valueName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsZeroOrPositive(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsZeroOrPositive(value, valueName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsZeroOrPositive(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsZeroOrPositive(value, valueName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsZeroOrPositive(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsZeroOrPositive(value, valueName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsZeroOrPositive(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsZeroOrPositive(value, valueName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsNegative(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsNegative(value, valueName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsNegative(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsNegative(value, valueName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsNegative(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsNegative(value, valueName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentIsNegative(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentIsNegative(value, valueName, message);

            /// <summary>Проверка что тип T является ссылочным</summary>
            /// <param name="valueName">(Optional) Наименование проверяемого параметра</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ArgumentIsRefType<T>(
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (!typeof(T).IsByRef)
                    throw !string.IsNullOrWhiteSpace(message)
                        ? !string.IsNullOrWhiteSpace(valueName)
                            ? new ArgumentException(message, valueName)
                            : new ArgumentException(message)
                        : !string.IsNullOrWhiteSpace(valueName)
                            ? new ArgumentException($"Value {valueName} of type {typeof(T)} with is not reference type!", valueName)
                        : new ArgumentException($"Type {typeof(T)} is not reference type!");
            }

            /// <summary>Проверка что тип T является типом-значением</summary>
            /// <param name="valueName">(Optional) Наименование проверяемого параметра</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void ArgumentIsValueType<T>(
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (!typeof(T).IsValueType)
                    throw !string.IsNullOrWhiteSpace(message)
                        ? !string.IsNullOrWhiteSpace(valueName)
                            ? new ArgumentException(message, valueName)
                            : new ArgumentException(message)
                        : !string.IsNullOrWhiteSpace(valueName)
                            ? new ArgumentException($"Value {valueName} of type {typeof(T)} with is not value type!", valueName)
                            : new ArgumentException($"Type {typeof(T)} is not reference type!");
            }

            /// <summary>Проверка что строка содержит guid</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="FormatException">Если строка не содержит GUID</exception>
            /// <param name="guid">Строка, которая должна содержать Guid</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), GuidStr,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsGuid(
                [CanBeNull] string guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsGuid(guid, valueName, message);

            /// <summary>Проверка что строка содержит непустой guid</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="FormatException">Если строка не содержит GUID</exception>
            /// <exception cref="ValueEmptyException">Если GUID пуст</exception>
            /// <param name="guid">Строка, которая должна содержать непустой Guid</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void GuidNotEmpty(
                [CanBeNull] string guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.ArgumentGuidNotEmpty(guid, valueName, message);

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="ValueEmptyException">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
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
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                => Check.NotNull(value, valueName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNull<T>(
                [CanBeNull, NoEnumeration] T? value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.NotNull(value, valueName, message);

            /// <summary>Проверка объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void GenericNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.GenericNotNull(value, valueName, message);

            /// <summary>Проверка значения на значение по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ValueNotEmpty<T>(
                T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ValueNotEmpty(value, valueName, message);

            /// <summary>Проверка что значение не пусто (пустое значение отличается от default)</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue">Пустое значение параметра</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ValueNotEmpty<T>(
                [NoEnumeration] T value,
                T emptyValue,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ValueNotEmpty(value, emptyValue, valueName, message);

            /// <summary>Проверка что значение не пусто (пустое значение отличается от default)</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue1">Пустое значение параметра</param>
            /// <param name="emptyValue2">Пустое значение параметра</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ValueNotEmpty<T>(
                [NoEnumeration] T value,
                T emptyValue1,
                T emptyValue2,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ValueNotEmpty(value, emptyValue1, emptyValue2, valueName, message);

            /// <summary>Проверка что значение IntPtr не пусто (не равно IntPtr.Zero)</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == IntPtr.Zero</exception>
            /// <param name="value">Значение, которое не должно быть равно IntPtr.Zero</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ValueNotEmpty(
                IntPtr value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ValueNotEmpty(value, valueName, message);

            /// <summary>Проверка попадания значения в допустимый диапазон</summary>
            /// <exception cref="ValueOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
            /// <param name="value">Значение, которое должно попадать в допустимый диапазон</param>
            /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void InRange<T>(
                [CanBeNull] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.InRange(value, condition, valueName, message);

            /// <summary>Проверка попадания значения в допустимый диапазон</summary>
            /// <exception cref="ValueOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
            /// <param name="value">Значение, которое должно попадать в допустимый диапазон</param>
            /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void InRange<T>(
                [CanBeNull] T value,
                [NotNull, InstantHandle] Func<bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.InRange(value, condition, valueName, message);

            /// <summary>Проверка попадания значения в допустимый диапазон</summary>
            /// <exception cref="ValueOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
            /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void InRange(
                [NotNull, InstantHandle] Func<bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.InRange(condition(), valueName, message);

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ValueOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IndexInRange(
                int index,
                int count,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IndexInRange(index, count, valueName, message);

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ValueOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IndexInRange(
                long index,
                long count,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IndexInRange(index, count, valueName, message);

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="value">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ValuesNotEmpty<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
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
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNullOrEmpty(
                [CanBeNull] string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.NotNullOrEmpty(value, valueName, message);

            /// <summary>Проверка строки на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из
            ///                     пробелов</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNullOrWhitespace(
                [CanBeNull] string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.NotNullOrWhitespace(value, valueName, message);

            /// <summary>Проверка что объект не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если объект == null или DBNull</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class
                => Check.NotNullNotDbNull(value, valueName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNull<T, TException>(
                [CanBeNull, NoEnumeration] T value,
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
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNull<T, TException>(
                [CanBeNull, NoEnumeration] T? value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                where TException : NullReferenceException
                => Check.NotNull<T, TException>(value, valueName, message);

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
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
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsNotNull<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T?> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct
                => Check.ItemsNotNull(value, valueName, message);

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void CollectionNotEmpty(
                [CanBeNull, NoEnumeration] ICollection value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.CollectionNotEmpty(value, valueName, message);

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ReadOnlyCollectionNotEmpty<T>(
                [CanBeNull, NoEnumeration] IReadOnlyCollection<T> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ReadOnlyCollectionNotEmpty(value, valueName, message);

            /// <summary>Проверка что последовательность не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если последовательность пуста</exception>
            /// <param name="value">Последовательность, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование последовательности</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void EnumerationNotEmpty(
                [CanBeNull, NoEnumeration] IEnumerable value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.EnumerationNotEmpty(value, valueName, message);

            /// <summary>Проверка что все строки последовательности не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null или string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void StringsNotEmpty(
                [CanBeNull, NoEnumeration] IEnumerable<string> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.StringsNotEmpty(value, valueName, message);

            /// <summary>Проверка что все строки последовательности не null, не пустые строки и не строки состоящие из одних пробелов</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего
            ///                                                     кроме пробелов</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null, string.Empty или заполнены
            ///                     одними только пробелами</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void StringsNotWhitespace(
                [CanBeNull, NoEnumeration] IEnumerable<string> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.StringsNotWhitespace(value, valueName, message);

            /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если условие не выполняется</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsNotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Check.ItemsNotNullNotDbNull(value, valueName, message);

            /// <summary>Проверка условия</summary>
            /// <exception cref="Exception">Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Assert(
                bool condition,
                [CanBeNull, NotEmpty, InvokerParameterName] string message = null)
                => Check.Assert(condition, message);

            /// <summary>Проверка условия</summary>
            /// <exception cref="Exception">Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Assert<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, NotEmpty, InvokerParameterName] string message = null)
                => Check.Assert(value, condition, message);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Assert<TException>(
                bool condition,
                [NotNull, ItemCanBeNull] object[] exceptionParams)
                where TException : Exception
                => Check.Assert<TException>(condition, exceptionParams);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [NotNull, ItemCanBeNull] object[] exceptionParams)
                where TException : Exception
                => Check.Assert<T, TException>(value, condition, exceptionParams);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Assert<TException>(
                bool condition,
                [CanBeNull] string message = null)
                where TException : Exception
                => Check.Assert<TException>(condition, message);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull] string message = null)
                where TException : Exception
                => Check.Assert<T, TException>(value, condition, message);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsIs<T>(
                [CanBeNull, NoEnumeration] IEnumerable value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.ItemsIs<T>(value, valueName, message);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsIs<T>(
                [CanBeNull, NoEnumeration] IEnumerable value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.ItemsIs<T>(value, valueName, messageFactory);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsIs<T>(
                [CanBeNull, NoEnumeration] IEnumerable value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.ItemsIs<T>(value, null, messageFactory);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                    переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void All<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => Check.All(value, valueName, predicate, message);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                    переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void All<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => Check.All(value, predicate, message);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                    переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void All<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Check.All(value, valueName, predicate, messageFactory);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                    переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void All<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Check.All(value, null, predicate, messageFactory);

            /// <summary>Проверка, что перечисление не пусто</summary>
            /// <exception cref="NullReferenceException">Если перечисление равно null</exception>
            /// <exception cref="CollectionIsEmptyException">Если перечисление пусто</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNullNotEmpty<TEnumerable>(
                [CanBeNull, NoEnumeration] TEnumerable value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull, CanBeEmpty] string message = null)
                where TEnumerable : class, IEnumerable
                => Check.NotNullNotEmpty(value, valueName, message);

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ObjectState(
                bool condition,
                [CanBeNull, InvokerParameterName] string message = null)
                => Check.ObjectState(condition, message);

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="NullReferenceException">Если value == null</exception>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => halt"),
             ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void ObjectState<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null)
                => Check.ObjectState(value, valueName, condition, message);

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="NullReferenceException">Если value == null</exception>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => halt"), Conditional("DEBUG"),
             Conditional("FULL_CHECK"), ContractAnnotation("condition:false => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ObjectState<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null)
                => Check.ObjectState(value, null, condition, message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="type">Тип значения</param>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Obsolete("Удалите передачу типа Enum, начиная c C# 7.2 он не требуется"), Conditional("DEBUG"),
             Conditional("FULL_CHECK"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void EnumInRange<T>(
                [NotNull] Type type,
                T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct, Enum
                => Check.EnumInRange(value, valueName, message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void EnumInRange<T>(
                T value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct, Enum
                => Check.EnumInRange(value, valueName, message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void EnumInRange<T>(
                long value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct, Enum
                => Check.EnumInRange<T>(value, valueName, message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void EnumInRange<T>(
                int value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct, Enum
                => Check.EnumInRange<T>(value, valueName, message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="value">Значение</param>
            /// <param name="getExceptionFunc">Метод-конструктор исключительной ситуации</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void EnumInRangeCustom<T>(
                T value,
                [NotNull] EnumInRangeCustomExceptionFactory<T> getExceptionFunc)
                where T : struct, Enum
                => Check.EnumInRangeCustom(value, getExceptionFunc);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="type">Тип значения</param>
            /// <param name="values">Список значений</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Obsolete("Удалите передачу типа Enum, начиная c C# 7.2 он не требуется"),
             ContractAnnotation("values:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void AllEnumInRange<T>(
                [NotNull] Type type,
                [CanBeNull, NoEnumeration] IEnumerable<T> values,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct, Enum
                => Check.AllEnumInRange(values, valueName, message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="values">Список значений</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("values:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void AllEnumInRange<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> values,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                where T : struct, Enum
                => Check.AllEnumInRange(values, valueName, message);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="InvalidCastException">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="valueName">Наименование переданного объекта</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Is<T>(
                [CanBeNull, NoEnumeration] object value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.Is<T>(value, valueName, message);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="InvalidCastException">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="valueName">Наименование переданного объекта</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Is<T>(
                [CanBeNull, NoEnumeration] object value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.Is<T>(value, valueName, messageFactory);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Is<T>(
                [CanBeNull, NoEnumeration] object value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Check.Is<T>(value, null, messageFactory);

            /// <summary>Проверка того, что файл по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="FileNotFoundException">Если файл отсутствует на диске</exception>
            /// <param name="value">Путь к файлу</param>
            /// <param name="valueName">Наименование переданного значения</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void FileExists(
                [CanBeNull] string value,
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
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void DirectoryExists(
                [CanBeNull] string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Check.DirectoryExists(value, valueName, message);

            /// <summary>Проверка что стрим не равен null, что имеет ненулевую длину и текущая позиция не находится в конце стрима</summary>
            /// <exception cref="ArgumentNullException">Если переданный стрим == null</exception>
            /// <exception cref="StreamNotEmpty">Если длина стрима равна 0</exception>
            /// <exception cref="EndOfStreamException">Если позиция в преданном стриме находится в его конце</exception>
            /// <param name="value">Стрим</param>
            /// <param name="valueName">Наименование стрима</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void StreamNotEmpty(
                [CanBeNull] Stream value,
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
            /// <param name="scheme">(Optional) Схема Uri которой должен соответствовать адрес. Например UriScheme.Http для Http
            ///                      адреса. Если null - схема не проверяется</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void UriCorrect(
                [CanBeNull] string value,
                [NotNull, NotWhitespace, InvokerParameterName] string valueName,
                UriScheme scheme = UriScheme.Any,
                [CanBeNull] string message = null)
                => Check.UriCorrect(value, valueName, scheme, message);

            /// <summary>Проверка что в словаре присутствует запись с переданным ключом</summary>
            /// <exception cref="ArgumentItemNotFoundException">Если ключ не найден</exception>
            /// <param name="dictionary">Словарь</param>
            /// <param name="key">Ключ, который должен присутствовать в словаре</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("dictionary:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ContainsKey<TKey, TValue>(
                [CanBeNull, NoEnumeration] IReadOnlyDictionary<TKey, TValue> dictionary,
                [NotNull, NoEnumeration] TKey key,
                [CanBeNull] string message = null)
                => Check.ContainsKey(dictionary, key, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsPositive(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsPositive(value, valueName, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsPositive(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsPositive(value, valueName, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsPositive(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsPositive(value, valueName, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsPositive(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsPositive(value, valueName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsZeroOrPositive(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsZeroOrPositive(value, valueName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsZeroOrPositive(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsZeroOrPositive(value, valueName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsZeroOrPositive(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsZeroOrPositive(value, valueName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsZeroOrPositive(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsZeroOrPositive(value, valueName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsNegative(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsNegative(value, valueName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsNegative(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsNegative(value, valueName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsNegative(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsNegative(value, valueName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsNegative(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Check.IsNegative(value, valueName, message);

            /// <summary>Проверка, что Dispose у объекта ещё не вызывался.
            ///          Для контроля используется ссылка на объект, которая в Dispose устанавливается в null</summary>
            /// <exception cref="ObjectDisposedException">Если Dispose уже был вызван и notNullRef == null</exception>
            /// <param name="notNullRef">Контрольная ссылка, которая становится равной null после вызова Dispose</param>
            /// <param name="objectName">(Optional) Имя объекта</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             ContractAnnotation("notNullRef:null => halt"), DebuggerHidden,
             MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotDisposed(
                [CanBeNull] object notNullRef,
                [CanBeNull, NotWhitespace] string objectName = null,
                [CanBeNull] string message = null)
                => Check.NotDisposed(notNullRef, objectName, message);

            /// <summary>Проверка, что Dispose у объекта ещё не вызывался.
            ///          Для контроля используется ссылка на объект, которая в Dispose устанавливается в null</summary>
            /// <exception cref="ObjectDisposedException">Если Dispose уже был вызван и notNullRef == null</exception>
            /// <param name="notNullRef">Контрольная ссылка, которая становится равной null после вызова Dispose</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             ContractAnnotation("notNullRef:null => halt"), DebuggerHidden,
             MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotDisposed<T>(
                [CanBeNull] object notNullRef,
                [CanBeNull] string message = null)
                where T : IDisposable
                => Check.NotDisposed<T>(notNullRef, message);

            /// <summary>Проверка, что Dispose у объекта ещё не вызывался.
            ///          Для контроля флаг, который устанавливается в True в самом начале Dispose</summary>
            /// <exception cref="ObjectDisposedException">Если Dispose уже был вызван и disposedFlag == true</exception>
            /// <param name="disposedFlag">Флаг, который устанавливается в True в самом начале Dispose</param>
            /// <param name="objectName">(Optional) Имя объекта</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             ContractAnnotation("disposedFlag:true => halt"), DebuggerHidden,
             MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotDisposed(
                bool disposedFlag,
                [CanBeNull, NotWhitespace] string objectName = null,
                [CanBeNull] string message = null)
                => Check.NotDisposed(disposedFlag, objectName, message);

            /// <summary>Проверка, что Dispose у объекта ещё не вызывался.
            ///          Для контроля флаг, который устанавливается в True в самом начале Dispose</summary>
            /// <exception cref="ObjectDisposedException">Если Dispose уже был вызван и disposedFlag == true</exception>
            /// <param name="disposedFlag">Флаг, который устанавливается в True в самом начале Dispose</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             ContractAnnotation("disposedFlag:true => halt"), DebuggerHidden,
             MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotDisposed<T>(
                bool disposedFlag,
                [CanBeNull] string message = null)
                where T : IDisposable
                => Check.NotDisposed<T>(disposedFlag, message);

            /// <summary>Проверка что тип T является ссылочным</summary>
            /// <param name="valueName">(Optional) Наименование проверяемого параметра</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void IsRefType<T>(
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (!typeof(T).IsByRef)
                    throw new InvalidOperationException(
                        !string.IsNullOrWhiteSpace(message)
                            ? message
                            : !string.IsNullOrWhiteSpace(valueName)
                                ? $"Value {valueName} of type {typeof(T)} with is not reference type!"
                            : $"Type {typeof(T)} is not reference type!");
            }

            /// <summary>Проверка что тип T является типом-значением</summary>
            /// <param name="valueName">(Optional) Наименование проверяемого параметра</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void IsValueType<T>(
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (!typeof(T).IsValueType)
                    throw new InvalidOperationException(
                        !string.IsNullOrWhiteSpace(message)
                            ? message
                            : !string.IsNullOrWhiteSpace(valueName)
                                ? $"Value {valueName} of type {typeof(T)} with is not value type!"
                                : $"Type {typeof(T)} is not reference type!");
            }

            /// <summary>Проверка что объект заблокирован конструкцией lock. Служит для проверки того, что контекст вызова
            ///     потокобезопасен</summary>
            /// <exception cref="InvalidOperationException">Если объект не заблокирован конструкцией lock</exception>
            /// <param name="syncObject">Объект, который должен быть заблокирован конструкцией lock</param>
            /// <param name="syncObjectName">(Optional) Имя объекта, который используется для блокирования доступа к контексту вызова</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void SyncLocked<T>(
                [NotNull] T syncObject,
                [CanBeNull, NotWhitespace, InvokerParameterName] string syncObjectName = null,
                [CanBeNull] string message = null)
                where T : class
                => Check.SyncLocked(syncObject, syncObjectName, message);
        }
    }
}