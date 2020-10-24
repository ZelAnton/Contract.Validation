using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Contract.Exceptions;

using JetBrains.Annotations;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Contract.Validation
{
    /// <summary>Runtime валидация условий</summary>
    public abstract partial class Check
    {
        /// <summary>Условная валидация условий. Все методы работают только если у класса установлен статический флаг Enabled,
        ///          иначе значения возвращаются прозрачно без проверки.</summary>
        public abstract class Optional
        {
            /// <summary>Производить ли опциональные проверки</summary>
            public static bool Enabled
#if DEBUG || FULL_CHECK
                = true;
#else
                = false;
#endif

            /// <summary>Запуск действия только при активном заданном дефайне DEBUG и/или FULL_CHECK</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Invoke([NotNull] Action action)
            {
                Debug.ArgumentNotNull(action, nameof(action));

                if (Enabled)
                    action();
            }

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                => Enabled
                    ? Check.ArgumentNotNull(value, valueName, message)
                    : value;

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static T ArgumentNotNull<T>(
                [CanBeNull, NoEnumeration] T? value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ArgumentNotNull(value, valueName, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : value.Value;

            /// <summary>Проверка что аргумент не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentNotNull<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                where TException : ArgumentNullException
                => Enabled
                    ? Check.ArgumentNotNull<T, TException>(value, valueName, message)
                    : value;

            /// <summary>Проверка что аргумент не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static T ArgumentNotNull<T, TException>(
                [CanBeNull, NoEnumeration] T? value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                where TException : ArgumentNullException
                => Enabled
                    ? Check.ArgumentNotNull<T, TException>(value, valueName, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : value.Value;

            /// <summary>Проверка аргумента на null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt; => NotNull"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static T ArgumentGenericNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentGenericNotNull(value, valueName, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : value;

            /// <summary>Проверка, что коллекция не пуста</summary>
            /// <exception cref="ArgumentNullException">Если коллекция равна null</exception>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static TEnumerable ArgumentNotNullNotEmpty<TEnumerable>(
                [CanBeNull, NoEnumeration] TEnumerable value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull, CanBeEmpty] string message = null)
                where TEnumerable : class, IEnumerable
                => Enabled
                    ? Check.ArgumentNotNullNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка аргумента на значение по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentValueNotEmpty<T>(
                T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ArgumentValueNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что аргумент не пуст (пустое значение отличается от default)</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue">Пустое значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentValueNotEmpty<T>(
                T value,
                T emptyValue,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ArgumentValueNotEmpty(value, emptyValue, valueName, message)
                    : value;

            /// <summary>Проверка что аргумент не пуст (пустое значение отличается от default)</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue1">Пустое значение</param>
            /// <param name="emptyValue2">Пустое значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentValueNotEmpty<T>(
                T value,
                T emptyValue1,
                T emptyValue2,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ArgumentValueNotEmpty(value, emptyValue1, emptyValue2, valueName, message)
                    : value;

            /// <summary>Проверка что аргумент не пусто (не равно IntPtr.Zero)</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == IntPtr.Zero</exception>
            /// <param name="value">Значение, которое не должно быть равно IntPtr.Zero</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IntPtr ArgumentValueNotEmpty(
                IntPtr value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentValueNotEmpty(value, valueName, message)
                    : value;


            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="value">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ArgumentValuesNotEmpty<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ArgumentValuesNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                            переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ArgumentAll<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentAll(value, valueName, predicate, message)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                            переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ArgumentAll<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentAll(value, predicate, message)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                            переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ArgumentAll<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Enabled
                    ? Check.ArgumentAll(value, valueName, predicate, messageFactory)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                            переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ArgumentAll<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Enabled
                    ? Check.ArgumentAll(value, predicate, messageFactory)
                    : value;

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentItemsNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Enabled
                    ? Check.ArgumentItemsNotNull(value, valueName, message)
                    : value;

            /// <summary>Проверка что элементы коллекции не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ArgumentItemsNotNull<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T?> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ArgumentItemsNotNull(value, valueName, message)
                    : value.Cast<T>();

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentCollectionNotEmpty<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, ICollection
                => Enabled
                    ? Check.ArgumentCollectionNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IReadOnlyCollection<T> ArgumentReadOnlyCollectionNotEmpty<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IReadOnlyCollection<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentReadOnlyCollectionNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static TCollection ArgumentReadOnlyCollectionNotEmpty<TCollection, T>(
                [CanBeNull, NoEnumeration] TCollection value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where TCollection : class, IReadOnlyCollection<T>
                => Enabled
                    ? Check.ArgumentReadOnlyCollectionNotEmpty<TCollection, T>(value, valueName, message)
                    : value;

            /// <summary>Проверка что последовательность не пусто</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если последовательность пусто</exception>
            /// <param name="value">Последовательность, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование последовательности</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentEnumerationNotEmpty<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Enabled
                    ? Check.ArgumentEnumerationNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что все строки в последовательности не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Коллекция строк, которые быть не должны быть равны null или string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<string> ArgumentStringsNotEmpty(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<string> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentStringsNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что все строки последовательности не null, не пустые строки и не строки состоящие из одних пробелов</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ArgumentItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не
            ///                                                             содержащие ничего кроме пробелов</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null, string.Empty или заполнены
            ///                     одними только пробелами</param>
            /// <param name="valueName">(Optional) Наименование коллекции</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotWhitespace,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<string> ArgumentStringsNotWhitespace(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<string> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentStringsNotWhitespace(value, valueName, message)
                    : value;

            /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если коллекция содержит null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если коллекция содержит DBNull</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentItemsNotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Enabled
                    ? Check.ArgumentItemsNotNullNotDbNull(value, valueName, message)
                    : value;

            /// <summary>Проверка строкового аргумента на null и на равенство string.Empty</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string ArgumentNotNullOrEmpty(
                [CanBeNull] string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentNotNullOrEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка строкового аргумента на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из
            ///                     пробелов</param>
            /// <param name="valueName">(Optional) Наименование проверяемого параметра</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotWhitespace,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string ArgumentNotNullOrWhitespace(
                [CanBeNull] string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentNotNullOrWhitespace(value, valueName, message)
                    : value;

            /// <summary>Проверка аргумента на null и DBNull</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null или == DBNull</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentNotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                => Enabled
                    ? Check.ArgumentNotNullNotDbNull(value, valueName, message)
                    : value;

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void Argument(
                bool condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (Enabled)
                    Check.Argument(condition, valueName, message);
            }

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка будет выполнена</param>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => null"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Argument<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.Argument(value, condition, valueName, message)
                    : value;

            /// <summary>Проверка условия</summary>
            /// <exception><cref>ArgumentException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
            [ContractAnnotation("condition:false => halt; value:null => null"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Argument<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [NotNull, ItemNotNull] object[] exceptionParams)
                where TException : ArgumentException
                => Enabled
                    ? Check.Argument<T, TException>(value, condition, exceptionParams)
                    : value;

            /// <summary>Проверка условия</summary>
            /// <exception><cref>ArgumentException</cref>: Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void Argument<TException>(
                bool condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where TException : ArgumentException
            {
                if (Enabled)
                    Check.Argument<TException>(condition, valueName, message);
            }

            /// <summary>Проверка условия</summary>
            /// <exception><cref>ArgumentException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => null"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Argument<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where TException : ArgumentException
                => Enabled
                    ? Check.Argument<T, TException>(value, condition, valueName, message)
                    : value;

            /// <summary>Проверка аргумента</summary>
            /// <exception cref="ArgumentException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void Argument(
                [NotNull, InstantHandle] Func<bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (Enabled)
                    Check.Argument(condition, valueName, message);
            }

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void ArgumentInRange(
                bool condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (Enabled)
                    Check.ArgumentInRange(condition, valueName, message);
            }

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void ArgumentInRange(
                [NotNull, InstantHandle] Func<bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
            {
                if (Enabled)
                    Check.ArgumentInRange(condition(), valueName, message);
            }

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка будет выполнена</param>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => null"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentInRange<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentInRange(value, condition, valueName, message)
                    : value;

            /// <summary>Проверка попадания значения аргумента в список допустимых значений</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка будет выполнена</param>
            /// <param name="condition">Условие проверки значения аргумента</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => null"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ArgumentInRange<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentInRange(value, condition, valueName, message)
                    : value;

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int ArgumentIndexInRange(
                int index,
                int count,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIndexInRange(index, count, valueName, message)
                    : index;

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long ArgumentIndexInRange(
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
            [NotNull, GuidStr, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string ArgumentIsGuid(
                [CanBeNull] string guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsGuid(guid, valueName, message)
                    : guid;

            /// <summary>Проверка что строка содержит непустой guid</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="ArgumentException">Если строка не содержит GUID</exception>
            /// <exception cref="ArgumentValueEmptyException">Если GUID пуст</exception>
            /// <param name="guid">Строка, которая должна содержать непустой Guid</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotNull, NotEmptyGuid, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string ArgumentGuidNotEmpty(
                [CanBeNull] string guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentGuidNotEmpty(guid, valueName, message)
                    : guid;

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="ArgumentValueEmptyException">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Guid ArgumentGuidNotEmpty(
                Guid guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentGuidNotEmpty(guid, valueName, message)
                    : guid;

            /// <summary>Проверка что в словаре присутствует запись с переданным ключом</summary>
            /// <exception cref="ArgumentItemNotFoundException">Если ключ не найден</exception>
            /// <param name="dictionary">Словарь</param>
            /// <param name="key">Ключ, который должен присутствовать в словаре</param>
            /// <param name="dictionaryName">Наименование словаря</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("dictionary:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IReadOnlyDictionary<TKey, TValue> ArgumentContainsKey<TKey, TValue>(
                [CanBeNull, NoEnumeration] IReadOnlyDictionary<TKey, TValue> dictionary,
                [NotNull, NoEnumeration] TKey key,
                [CanBeNull, NotWhitespace, InvokerParameterName] string dictionaryName,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentContainsKey(dictionary, key, dictionaryName, message)
                    : dictionary;

            /*
            /// <summary>Проверка что элементы диапазона удовлетворяют переданному условию</summary>
            /// <exception cref="ArgumentItemValidationExceptionException">Если в диапазоне присутствуют элементы не удовлетворяющие
            ///                                                            переданному условию</exception>
            /// <param name="span">Диапазон</param>
            /// <param name="spanName">Наименование диапазона</param>
            /// <param name="predicate">Условие, которому должен соответствовать каждый элемент диапазона</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Span<T> ArgumentSpanAll<T>(
                Span<T> span,
                [CanBeNull, NotWhitespace, InvokerParameterName] string spanName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ArgumentSpanAll(span, spanName, predicate, message)
                    : span;

            /// <summary>Проверка что диапазон не пуст</summary>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если диапазон пуст</exception>
            /// <param name="span">Диапазон, который не должен быть пуст</param>
            /// <param name="spanName">Наименование диапазона</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Span<T> ArgumentSpanNotEmpty<T>(
                [NoEnumeration] Span<T> span,
                [CanBeNull, NotWhitespace, InvokerParameterName] string spanName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ArgumentSpanNotEmpty(span, spanName, message)
                    : span;
            */

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int ArgumentIsPositive(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long ArgumentIsPositive(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static float ArgumentIsPositive(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static double ArgumentIsPositive(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int ArgumentIsZeroOrPositive(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsZeroOrPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long ArgumentIsZeroOrPositive(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsZeroOrPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static float ArgumentIsZeroOrPositive(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsZeroOrPositive(value, valueName, message)
                    : value;


            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static double ArgumentIsZeroOrPositive(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsZeroOrPositive(value, valueName, message)
                    : value;
            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int ArgumentIsNegative(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsNegative(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long ArgumentIsNegative(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsNegative(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static float ArgumentIsNegative(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsNegative(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static double ArgumentIsNegative(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentIsNegative(value, valueName, message)
                    : value;

            /// <summary>Проверка что строка содержит guid</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="FormatException">Если строка не содержит GUID</exception>
            /// <param name="guid">Строка, которая должна содержать Guid</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotNull, GuidStr, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string IsGuid(
                [CanBeNull] string guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsGuid(guid, valueName, message)
                    : guid;

            /// <summary>Проверка что строка содержит непустой guid</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="FormatException">Если строка не содержит GUID</exception>
            /// <exception cref="ValueEmptyException">Если GUID пуст</exception>
            /// <param name="guid">Строка, которая должна содержать непустой Guid</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotNull, NotEmptyGuid, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string GuidNotEmpty(
                [CanBeNull] string guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ArgumentGuidNotEmpty(guid, valueName, message)
                    : guid;

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="ValueEmptyException">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Guid GuidNotEmpty(
                Guid guid,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.GuidNotEmpty(guid, valueName, message)
                    : guid;

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T NotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                => Enabled
                    ? Check.NotNull(value, valueName, message)
                    : value;

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static T NotNull<T>(
                [CanBeNull, NoEnumeration] T? value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.NotNull(value, valueName, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : value.Value;

            /// <summary>Проверка объект не null</summary>
            /// <exception cref="NullReferenceException">Если объект == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt; => NotNull"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static T GenericNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.GenericNotNull(value, valueName, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : value;

            /// <summary>Проверка значения на значение по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ValueNotEmpty<T>(
                T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ValueNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что значение не пусто (пустое значение отличается от default)</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue">Пустое значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ValueNotEmpty<T>(
                T value,
                T emptyValue,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ValueNotEmpty(value, emptyValue, valueName, message)
                    : value;

            /// <summary>Проверка что значение не пусто (пустое значение отличается от default)</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue1">Пустое значение</param>
            /// <param name="emptyValue2">Пустое значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ValueNotEmpty<T>(
                T value,
                T emptyValue1,
                T emptyValue2,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ValueNotEmpty(value, emptyValue1, emptyValue2, valueName, message)
                    : value;

            /// <summary>Проверка попадания значения в допустимый диапазон</summary>
            /// <exception cref="ValueOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
            /// <param name="value">Значение, которое должно попадать в допустимый диапазон</param>
            /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition: false => halt; value:null => null"),
             NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T InRange<T>(
                [CanBeNull] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.InRange(value, condition, valueName, message)
                    : value;

            /// <summary>Проверка попадания значения в допустимый диапазон</summary>
            /// <exception cref="ValueOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
            /// <param name="value">Значение, которое должно попадать в допустимый диапазон</param>
            /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition: false => halt; value:null => null"),
             NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T InRange<T>(
                [CanBeNull] T value,
                [NotNull, InstantHandle] Func<bool> condition,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.InRange(value, condition, valueName, message)
                    : value;

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ValueOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int IndexInRange(
                int index,
                int count,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IndexInRange(index, count, valueName, message)
                    : index;

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ValueOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long IndexInRange(
                long index,
                long count,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IndexInRange(index, count, valueName, message)
                    : index;

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="value">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ValuesNotEmpty<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ValuesNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка строки на null и на равенство string.Empty</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string NotNullOrEmpty(
                [CanBeNull] string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.NotNullOrEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка строки на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из
            ///                     пробелов</param>
            /// <param name="valueName">(Optional) Наименование проверяемого параметра</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotWhitespace,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string NotNullOrWhitespace(
                [CanBeNull] string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.NotNullOrWhitespace(value, valueName, message)
                    : value;

            /// <summary>Проверка что объект не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если объект == null или DBNull</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T NotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                => Enabled
                    ? Check.NotNullNotDbNull(value, valueName, message)
                    : value;

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T NotNull<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class
                where TException : NullReferenceException
                => Enabled
                    ? Check.NotNull<T, TException>(value, valueName, message)
                    : value;

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static T NotNull<T, TException>(
                [CanBeNull, NoEnumeration] T? value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                where TException : NullReferenceException
                => Enabled
                    ? Check.NotNull<T, TException>(value, valueName, message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : value.Value;

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ItemsNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Enabled
                    ? Check.ItemsNotNull(value, valueName, message)
                    : value;

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ItemsNotNull<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T?> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.ItemsNotNull(value, valueName, message)
                    : value.Cast<T>();

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T CollectionNotEmpty<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, ICollection
                => Enabled
                    ? Check.CollectionNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IReadOnlyCollection<T> ReadOnlyCollectionNotEmpty<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IReadOnlyCollection<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ReadOnlyCollectionNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static TCollection ReadOnlyCollectionNotEmpty<TCollection, T>(
                [CanBeNull, NoEnumeration] TCollection value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where TCollection : class, IReadOnlyCollection<T>
                => Enabled
                    ? Check.ReadOnlyCollectionNotEmpty<TCollection, T>(value, valueName, message)
                    : value;

            /// <summary>Проверка что последовательность не пусто</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если последовательность пусто</exception>
            /// <param name="value">Последовательность, которая не должна быть пуста</param>
            /// <param name="valueName">Наименование последовательности</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T EnumerationNotEmpty<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Enabled
                    ? Check.EnumerationNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что все строки последовательности не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null или string.Empty</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<string> StringsNotEmpty(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<string> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.StringsNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что все строки последовательности не null, не пустые строки и не строки состоящие из одних пробелов</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего
            ///                                                     кроме пробелов</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null, string.Empty или заполнены
            ///                     одними только пробелами</param>
            /// <param name="valueName">(Optional) Наименование коллекции</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotWhitespace,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<string> StringsNotWhitespace(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<string> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.StringsNotWhitespace(value, valueName, message)
                    : value;

            /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
            /// <exception cref="ItemNullsNotAllowedException">Если коллекция содержит null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если коллекция содержит DBNull</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ItemsNotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : class, IEnumerable
                => Enabled
                    ? Check.ItemsNotNullNotDbNull(value, valueName, message)
                    : value;

            /// <summary>Проверка условия</summary>
            /// <exception cref="Exception">Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void Assert(
                bool condition,
                [CanBeNull, NotEmpty, InvokerParameterName] string message = null)
            {
                if (Enabled)
                    Check.Assert(condition, message);
            }

            /// <summary>Проверка условия</summary>
            /// <exception cref="Exception">Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => null"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Assert<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, NotEmpty, InvokerParameterName] string message = null)
                => Enabled
                    ? Check.Assert(value, condition, message)
                    : value;

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void Assert<TException>(
                bool condition,
                [NotNull, ItemCanBeNull] object[] exceptionParams)
                where TException : Exception
            {
                if (Enabled)
                    Check.Assert<TException>(condition, exceptionParams);
            }

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
            [ContractAnnotation("condition:false => halt; value:null => null"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [NotNull, ItemCanBeNull] object[] exceptionParams)
                where TException : Exception
                => Enabled
                    ? Check.Assert<T, TException>(value, condition, exceptionParams)
                    : value;

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void Assert<TException>(
                bool condition,
                [CanBeNull] string message = null)
                where TException : Exception
            {
                if (Enabled)
                    Check.Assert<TException>(condition, message);
            }

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => null"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull] string message = null)
                where TException : Exception
                => Enabled
                    ? Check.Assert<T, TException>(value, condition, message)
                    : value;

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ItemsIs<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ItemsIs<T>(value, valueName, message)
                    : ConvertAll<T>(value);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ItemsIs<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Enabled
                    ? Check.ItemsIs<T>(value, valueName, messageFactory)
                    : ConvertAll<T>(value);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ItemsIs<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Enabled
                    ? Check.ItemsIs<T>(value, null, messageFactory)
                    : ConvertAll<T>(value);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                    переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> All<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.All(value, valueName, predicate, message)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                    переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> All<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.All(value, predicate, message)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                    переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> All<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T> value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Enabled
                    ? Check.All(value, valueName, predicate, messageFactory)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                    переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> All<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory)
                => Enabled
                    ? Check.All(value, null, predicate, messageFactory)
                    : value;

            /// <summary>Проверка, что перечисление не пусто</summary>
            /// <exception cref="NullReferenceException">Если перечисление равно null</exception>
            /// <exception cref="CollectionIsEmptyException">Если перечисление пусто</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static TEnumerable NotNullNotEmpty<TEnumerable>(
                [CanBeNull, NoEnumeration] TEnumerable value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull, CanBeEmpty] string message = null)
                where TEnumerable : class, IEnumerable
                => Enabled
                    ? Check.NotNullNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void ObjectState(
                bool condition,
                [CanBeNull, InvokerParameterName] string message = null)
            {
                if (Enabled)
                    Check.ObjectState(condition, message);
            }

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="NullReferenceException">Если value == null</exception>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ObjectState<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null)
                => Enabled
                    ? Check.ObjectState(value, valueName, condition, message)
                    : value;

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="NullReferenceException">Если value == null</exception>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("condition:false => halt; value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ObjectState<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null)
                => Enabled
                    ? Check.ObjectState(value, null, condition, message)
                    : value;

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="type">Тип значения</param>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Obsolete("Удалите передачу типа Enum, начиная c C# 7.2 он не требуется"), Pure,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T EnumInRange<T>(
                [NotNull] Type type,
                T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct, Enum
                => Enabled
                    ? Check.EnumInRange(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T EnumInRange<T>(
                T value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct, Enum
                => Enabled
                    ? Check.EnumInRange(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="value">Значение</param>
            /// <param name="getExceptionFunc">Метод-конструктор исключительной ситуации</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T EnumInRangeCustom<T>(
                T value,
                [NotNull] EnumInRangeCustomExceptionFactory<T> getExceptionFunc)
                where T : struct, Enum
                => Enabled
                    ? Check.EnumInRangeCustom(value, getExceptionFunc)
                    : value;

            /*
            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="type">Тип значения</param>
            /// <param name="values">Список значений</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Obsolete("Удалите передачу типа Enum, начиная c C# 7.2 он не требуется"), Pure,
             ContractAnnotation("values:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> AllEnumInRange<T>(
                [NotNull] Type type,
                [CanBeNull, NoEnumeration] IEnumerable<T> values,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct, Enum
                => Enabled
                    ? Check.AllEnumInRange(values, valueName, message)
                    : values;
            */

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="values">Список значений</param>
            /// <param name="valueName">Наименование коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("values:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> AllEnumInRange<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> values,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                where T : struct, Enum
                => Enabled
                    ? Check.AllEnumInRange(values, valueName, message)
                    : values;

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="InvalidCastException">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="valueName">Наименование переданного объекта</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Is<T>(
                [CanBeNull, NoEnumeration] object value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.Is<T>(value, valueName, message)
                    : (T) value;

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="InvalidCastException">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="valueName">Наименование переданного объекта</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Is<T>(
                [CanBeNull, NoEnumeration] object value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Enabled
                    ? Check.Is<T>(value, valueName, messageFactory)
                    : (T) value;

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Is<T>(
                [CanBeNull, NoEnumeration] object value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
                => Enabled
                    ? Check.Is<T>(value, null, messageFactory)
                    : (T) value;

            /// <summary>Проверка того, что файл по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="FileNotFoundException">Если файл отсутствует на диске</exception>
            /// <param name="value">Путь к файлу</param>
            /// <param name="valueName">Наименование переданного значения</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Путь к файлу</returns>
            [ContractAnnotation("value:null => halt"), NotNull, NotWhitespace, FileExists,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string FileExists(
                [CanBeNull] string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.FileExists(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что папка по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="DirectoryNotFoundException">Если папка отсутствует на диске</exception>
            /// <param name="value">Путь к папке</param>
            /// <param name="valueName">Наименование переданного значения</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Путь к папке</returns>
            [ContractAnnotation("value:null => halt"), NotNull, NotWhitespace, DirectoryExists,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string DirectoryExists(
                [CanBeNull] string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.DirectoryExists(value, valueName, message)
                    : value;

            /// <summary>Проверка что стрим не равен null, что имеет ненулевую длину и текущая позиция не находится в конце стрима</summary>
            /// <exception cref="ArgumentNullException">Если переданный стрим == null</exception>
            /// <exception cref="StreamNotEmpty">Если длина стрима равна 0</exception>
            /// <exception cref="EndOfStreamException">Если позиция в преданном стриме находится в его конце</exception>
            /// <param name="value">Стрим</param>
            /// <param name="valueName">Наименование стрима</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Стрим</returns>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Stream StreamNotEmpty(
                [CanBeNull] Stream value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.StreamNotEmpty(value, valueName, message)
                    : value;

            /// <summary>Проверка что строка содержит корректный Uri</summary>
            /// <exception cref="ArgumentNullException">Если строка описывающая Uri == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка описывающая Uri == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка описывающая Uri состоит только из пробелов</exception>
            /// <exception cref="InvalidUriException">Если Uri некорректен</exception>
            /// <param name="value">Строка, содержащая Uri</param>
            /// <param name="valueName">(Optional) Наименование строки</param>
            /// <param name="scheme">(Optional) Схема Uri которой должен соответствовать адрес. Например UriScheme.Http для Http адреса. Если
            ///                      null - схема не проверяется</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <returns>Строка, содержащая Uri</returns>
            [ContractAnnotation("value:null => halt"), NotNull, NotWhitespace,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string UriCorrect(
                [CanBeNull] string value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                UriScheme scheme = UriScheme.Any,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.UriCorrect(value, valueName, scheme, message)
                    : value;

            /// <summary>Проверка что в словаре присутствует запись с переданным ключом</summary>
            /// <exception cref="ArgumentItemNotFoundException">Если ключ не найден</exception>
            /// <param name="dictionary">Словарь</param>
            /// <param name="key">Ключ, который должен присутствовать в словаре</param>
            /// <param name="message">Сообщение об ошибке</param>
            [ContractAnnotation("dictionary:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IReadOnlyDictionary<TKey, TValue> ContainsKey<TKey, TValue>(
                [CanBeNull, NoEnumeration] IReadOnlyDictionary<TKey, TValue> dictionary,
                [NotNull, NoEnumeration] TKey key,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.ContainsKey(dictionary, key, message)
                    : dictionary;

            /*
            /// <summary>Проверка что элементы диапазона удовлетворяют переданному условию</summary>
            /// <exception cref="ItemValidationExceptionException">Если в диапазоне присутствуют элементы не удовлетворяющие
            ///                                                    переданному условию</exception>
            /// <param name="span">Диапазон</param>
            /// <param name="spanName">Наименование диапазона</param>
            /// <param name="predicate">Условие, которому должен соответствовать каждый элемент диапазона</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Span<T> SpanAll<T>(
                Span<T> span,
                [CanBeNull, NotWhitespace, InvokerParameterName] string spanName,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.SpanAll(span, spanName, predicate, message)
                    : span;

            /// <summary>Проверка что элементы диапазона удовлетворяют переданному условию</summary>
            /// <exception cref="ItemValidationExceptionException">Если в диапазоне присутствуют элементы не удовлетворяющие
            ///                                                    переданному условию</exception>
            /// <param name="span">Диапазон</param>
            /// <param name="predicate">Условие, которому должен соответствовать каждый элемент диапазона</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Span<T> SpanAll<T>(
                Span<T> span,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.SpanAll(span, null, predicate, message)
                    : span;

            /// <summary>Проверка что диапазон не пуст</summary>
            /// <exception cref="CollectionIsEmptyException">Если диапазон пуст</exception>
            /// <param name="span">Диапазон, который не должен быть пуст</param>
            /// <param name="spanName">Наименование диапазона</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Span<T> SpanNotEmpty<T>(
                [NoEnumeration] Span<T> span,
                [CanBeNull, NotWhitespace, InvokerParameterName] string spanName = null,
                [CanBeNull] string message = null)
                where T : struct
                => Enabled
                    ? Check.SpanNotEmpty(span, spanName, message)
                    : span;
            */

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int IsPositive(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long IsPositive(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static float IsPositive(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static double IsPositive(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int IsZeroOrPositive(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsZeroOrPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long IsZeroOrPositive(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsZeroOrPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static float IsZeroOrPositive(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsZeroOrPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static double IsZeroOrPositive(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsZeroOrPositive(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int IsNegative(
                int value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsNegative(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long IsNegative(
                long value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsNegative(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static float IsNegative(
                float value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsNegative(value, valueName, message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="valueName">Наименование проверяемого параметра</param>
            /// <param name="message">Сообщение об ошибке</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static double IsNegative(
                double value,
                [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
                [CanBeNull] string message = null)
                => Enabled
                    ? Check.IsNegative(value, valueName, message)
                    : value;

            /// <summary>Проверка, что Dispose у объекта ещё не вызывался.
            ///          Для контроля используется ссылка на объект, которая в Dispose устанавливается в null</summary>
            /// <exception cref="ObjectDisposedException">Если Dispose уже был вызван и notNullRef == null</exception>
            /// <param name="notNullRef">Контрольная ссылка, которая становится равной null после вызова Dispose</param>
            /// <param name="objectName">(Optional) Имя объекта</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("notNullRef:null => halt"), DebuggerHidden,
             MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotDisposed(
                [CanBeNull] object notNullRef,
                [CanBeNull, NotWhitespace] string objectName = null,
                [CanBeNull] string message = null)
            {
                if (Enabled)
                    Check.NotDisposed(notNullRef, objectName, message);
            }

            /// <summary>Проверка, что Dispose у объекта ещё не вызывался.
            ///          Для контроля используется ссылка на объект, которая в Dispose устанавливается в null</summary>
            /// <exception cref="ObjectDisposedException">Если Dispose уже был вызван и notNullRef == null</exception>
            /// <param name="notNullRef">Контрольная ссылка, которая становится равной null после вызова Dispose</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("notNullRef:null => halt"), DebuggerHidden,
             MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotDisposed<T>(
                [CanBeNull] object notNullRef,
                [CanBeNull] string message = null)
                where T : IDisposable
            {
                if (Enabled)
                    Check.NotDisposed<T>(notNullRef, message);
            }

            /// <summary>Проверка, что Dispose у объекта ещё не вызывался.
            ///          Для контроля флаг, который устанавливается в True в самом начале Dispose</summary>
            /// <exception cref="ObjectDisposedException">Если Dispose уже был вызван и disposedFlag == true</exception>
            /// <param name="disposedFlag">Флаг, который устанавливается в True в самом начале Dispose</param>
            /// <param name="objectName">(Optional) Имя объекта</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("disposedFlag:true => halt"), DebuggerHidden,
             MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotDisposed(
                bool disposedFlag,
                [CanBeNull, NotWhitespace] string objectName = null,
                [CanBeNull] string message = null)
            {
                if (Enabled)
                    Check.NotDisposed(disposedFlag, objectName, message);
            }

            /// <summary>Проверка, что Dispose у объекта ещё не вызывался.
            ///          Для контроля флаг, который устанавливается в True в самом начале Dispose</summary>
            /// <exception cref="ObjectDisposedException">Если Dispose уже был вызван и disposedFlag == true</exception>
            /// <param name="disposedFlag">Флаг, который устанавливается в True в самом начале Dispose</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            [ContractAnnotation("disposedFlag:true => halt"), DebuggerHidden,
             MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void NotDisposed<T>(
                bool disposedFlag,
                [CanBeNull] string message = null)
                where T : IDisposable
            {
                if (Enabled)
                    Check.NotDisposed<T>(disposedFlag, message);
            }
        }
    }
}