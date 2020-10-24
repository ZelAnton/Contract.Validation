using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Contract.Exceptions;

using JetBrains.Annotations;

// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable AssignNullToNotNullAttribute

namespace Contract.Validation
{
    /// <summary>Runtime валидация условий</summary>
    abstract partial class Check
    {
        /// <summary>Производить ли проверки</summary>
        public static bool Enabled = true;

        /// <summary>Проверка аргумента на null</summary>
        /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
        /// <param name="value">Объект, который не должен быть равен null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentNotNull<T>(
            [CanBeNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class
        {
            if (!Enabled)
                return value;

            if (value is null)
                throw CreateArgumentNullException(valueName, message);

            return value;
        }

        /// <summary>Проверка аргумента на null</summary>
        /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
        /// <param name="value">Объект, который не должен быть равен null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentNotNull<T>(
            [CanBeNull, NoEnumeration] T? value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (!value.HasValue)
                throw CreateArgumentNullException(valueName, message);

            return value.Value;
        }

        /// <summary>Проверка аргумента на null</summary>
        /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
        /// <param name="value">Объект, который не должен быть равен null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt; => NotNull"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentGenericNotNull<T>(
            [CanBeNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (typeof(T).IsByRef &&
                ReferenceEquals(value, null))
                throw CreateArgumentNullException(valueName, message);

            return value;
        }

        /// <summary>Проверка что аргумент не null</summary>
        /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
        /// <param name="value">Объект, который должен быть не null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentNotNull<T, TException>(
            [CanBeNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class
            where TException : ArgumentNullException
        {
            if (!Enabled)
                return value;

            if (value is null)
                throw CreateException<TException>(
                    message,
                    () => !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} is null."
                        : "Argument is null.");

            return value;
        }

        /// <summary>Проверка что аргумент не null</summary>
        /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
        /// <param name="value">Объект, который должен быть не null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentNotNull<T, TException>(
            [CanBeNull, NoEnumeration] T? value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
            where TException : ArgumentNullException
        {
            if (!value.HasValue)
                throw CreateException<TException>(
                    message,
                    () => !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} is null."
                        : "Argument is null.");

            return (T) value;
        }

        /// <summary>Проверка аргумента на значение по-умолчанию</summary>
        /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
        /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentValueNotEmpty<T>(
            [NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (!Enabled)
                return value;

            if (Equals(value, default(T)))
                throw new ArgumentValueEmptyException(valueName, message);

            return value;
        }

        /// <summary>Проверка что аргумент не пуст (пустое значение отличается от default)</summary>
        /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
        /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
        /// <param name="emptyValue">Пустое значение параметра</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentValueNotEmpty<T>(
            [NoEnumeration] T value,
            T emptyValue,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (!Enabled)
                return value;

            if (Equals(value, emptyValue))
                throw new ArgumentValueEmptyException(valueName, message);

            return value;
        }

        /// <summary>Проверка что аргумент не пуст (пустое значение отличается от default)</summary>
        /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
        /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
        /// <param name="emptyValue1">Пустое значение параметра</param>
        /// <param name="emptyValue2">Пустое значение параметра</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentValueNotEmpty<T>(
            [NoEnumeration] T value,
            T emptyValue1,
            T emptyValue2,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (!Enabled)
                return value;

            if (Equals(value, emptyValue1) || Equals(value, emptyValue2))
                throw new ArgumentValueEmptyException(valueName, message);

            return value;
        }

        /// <summary>Проверка что аргумент IntPtr не пуст (пустое значение отличается от IntPtr.Zero)</summary>
        /// <exception cref="ArgumentValueEmptyException">Если аргумент == IntPtr.Zero</exception>
        /// <param name="value">Значение, которое не должно быть равно IntPtr.Zero</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IntPtr ArgumentValueNotEmpty(
            IntPtr value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (Equals(value, IntPtr.Zero))
                throw new ArgumentValueEmptyException(valueName, message);

            return value;
        }

        /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
        /// <exception cref="ArgumentValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
        /// <param name="value">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ArgumentValuesNotEmpty<T>(
            [CanBeNull] IEnumerable<T> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            foreach (T item in value) // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (Equals(item, default(T)))
                    throw new ArgumentValueEmptyException(valueName, message);

            return value;
        }

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
        ///                                                            переданное условие</exception>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="predicate">Условие</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ArgumentAll<T>(
            [CanBeNull] IEnumerable<T> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);
            Debug.ArgumentNotNull(predicate, nameof(predicate));

            foreach (T item in value)
                if (!predicate(item))
                    throw new ArgumentItemValidationExceptionException(message);

            return value;
        }

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="checkAction">Метод проверки условия</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ArgumentAll<T>(
            [CanBeNull] IEnumerable<T> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
            [NotNull, InstantHandle] Action<T> checkAction,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);
            Debug.ArgumentNotNull(checkAction, nameof(checkAction));

            foreach (T item in value)
                checkAction(item);

            return value;
        }

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
        ///                                                            переданное условие</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="predicate">Условие</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ArgumentAll<T>(
            [CanBeNull] IEnumerable<T> value,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [CanBeNull] string message = null)
            => ArgumentAll(value, null, predicate, message);

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
        ///                                                            переданное условие</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="checkAction">Метод проверки условия</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ArgumentAll<T>(
            [CanBeNull] IEnumerable<T> value,
            [NotNull, InstantHandle] Action<T> checkAction,
            [CanBeNull] string message = null)
            => ArgumentAll(value, null, checkAction, message);

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
        ///                                                            переданное условие</exception>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="predicate">Условие</param>
        /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ArgumentAll<T>(
            [CanBeNull] IEnumerable<T> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [NotNull] TemplateMessageFactory<T> messageFactory)
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);
            Debug.ArgumentNotNull(predicate, nameof(predicate));
            Debug.ArgumentNotNull(messageFactory, nameof(messageFactory));

            foreach (T item in value)
                if (!predicate(item))
                {
                    string message = messageFactory(item);
                    throw new ArgumentItemValidationExceptionException(message);
                }

            return value;
        }

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
        ///                                                            переданное условие</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="predicate">Условие</param>
        /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ArgumentAll<T>(
            [CanBeNull] IEnumerable<T> value,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [NotNull] TemplateMessageFactory<T> messageFactory)
            => ArgumentAll(value, null, predicate, messageFactory);

        /// <summary>Проверка что элементы коллекции не null</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentItemsNotNull<T>(
            [CanBeNull] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class, IEnumerable
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            foreach (object item in value)
                if (item is null)
                    throw new ArgumentItemNullsNotAllowedException(value, valueName, message);

            return value;
        }

        /// <summary>Проверка, что коллекция не пуста</summary>
        /// <exception cref="ArgumentNullException">Если коллекция равна null</exception>
        /// <exception cref="ArgumentCollectionIsEmptyException">Если коллекция пуста</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static TEnumerable ArgumentNotNullNotEmpty<TEnumerable>(
            [CanBeNull] TEnumerable value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull, CanBeEmpty] string message = null)
            where TEnumerable : class, IEnumerable
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            if (value is ICollection collection)
            {
                if (collection.Count == 0)
                    throw new ArgumentCollectionIsEmptyException(valueName, message);
            }
            else
            {
                IEnumerator enumerator = value.GetEnumerator();
                try
                {
                    if (enumerator.MoveNext() != true)
                        throw new ArgumentCollectionIsEmptyException(valueName, message);
                }
                finally
                {
                    if (enumerator is IDisposable enumeratorDisposable)
                        enumeratorDisposable.Dispose();
                }
            }

            return value;
        }

        /// <summary>Проверка что элементы коллекции не null</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ArgumentItemsNotNull<T>(
            [CanBeNull] IEnumerable<T?> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            ArgumentNotNull(value, valueName);

            foreach (T? item in value)
                if (item is null)
                    throw new ArgumentItemNullsNotAllowedException(value, valueName, message);

            return value.Cast<T>();
        }

        /// <summary>Проверка что коллекция не пуста</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
        /// <param name="value">Коллекция, которая не должна быть пустой</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentCollectionNotEmpty<T>(
            [CanBeNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class, ICollection
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            if (value.Count == 0)
                throw new ArgumentCollectionIsEmptyException(valueName, message);
            return value;
        }

        /// <summary>Проверка что коллекция не пуста</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
        /// <param name="value">Коллекция, которая не должна быть пустой</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IReadOnlyCollection<T> ArgumentReadOnlyCollectionNotEmpty<T>(
            [CanBeNull, NoEnumeration] IReadOnlyCollection<T> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            if (value.Count == 0)
                throw new ArgumentCollectionIsEmptyException(valueName, message);
            return value;
        }

        /// <summary>Проверка что коллекция не пуста</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
        /// <param name="value">Коллекция, которая не должна быть пустой</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static TCollection ArgumentReadOnlyCollectionNotEmpty<TCollection, T>(
            [CanBeNull, NoEnumeration] TCollection value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where TCollection : class, IReadOnlyCollection<T>
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            if (value.Count == 0)
                throw new ArgumentCollectionIsEmptyException(valueName, message);
            return value;
        }

        /// <summary>Проверка что последовательность не пусто</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="CollectionIsEmptyException">Если последовательность пусто</exception>
        /// <param name="value">Последовательность, которая не должна быть пустой</param>
        /// <param name="valueName">Наименование последовательности</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentEnumerationNotEmpty<T>(
            [CanBeNull] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class, IEnumerable
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            if (value is ICollection collection)
            {
                if (collection.Count == 0)
                    throw new ArgumentCollectionIsEmptyException(valueName, message);
            }
            else
            {
                IEnumerator enumerator = value.GetEnumerator();
                try
                {
                    if (enumerator.MoveNext() != true)
                        throw new ArgumentCollectionIsEmptyException(valueName, message);
                }
                finally
                {
                    if (enumerator is IDisposable enumeratorDisposable)
                        enumeratorDisposable.Dispose();
                }
            }
            return value;
        }

        /// <summary>Проверка что все строки в последовательности не null и не пусты</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
        /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
        /// <param name="value">Последовательность строк, которые быть не должны быть равны null или string.Empty</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotEmpty,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<string> ArgumentStringsNotEmpty(
            [CanBeNull] IEnumerable<string> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            foreach (string str in value)
            {
                if (str is null)
                    throw new ArgumentItemNullsNotAllowedException(value, valueName, message);

                if (str == string.Empty)
                    throw new ArgumentItemEmptyStringNotAllowedException(value, valueName, message);
            }

            return value;
        }

        /// <summary>Проверка что все строки последовательности не null, не пустые строки и не строки состоящие из одних пробелов</summary>
        /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
        /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
        /// <exception cref="ArgumentItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не
        ///     содержащие ничего кроме пробелов</exception>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <param name="value">Последовательность строк, которые быть не должны быть равны null, string.Empty или заполнены
        ///     одними только пробелами</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <returns>An enumerator that allows foreach to be used to process argument strings not whitespace in this collection.
        ///     This will never be null</returns>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotWhitespace,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<string> ArgumentStringsNotWhitespace(
            [CanBeNull] IEnumerable<string> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            foreach (string str in value)
            {
                if (str is null)
                    throw new ArgumentItemNullsNotAllowedException(value, valueName, message);

                if (str == string.Empty)
                    throw new ArgumentItemEmptyStringNotAllowedException(value, valueName, message);

                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentItemWhitespaceNotAllowedException(value, valueName, message);
            }

            return value;
        }

        /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
        /// <exception cref="ArgumentItemNullsNotAllowedException">Если коллекция содержит null</exception>
        /// <exception cref="ArgumentItemNullsNotAllowedException">Если коллекция содержит DBNull</exception>
        /// <param name="value">Коллекция, элементы которой должен быть не null</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotEmpty,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ArgumentItemsNotNullNotDbNull<T>(
            [CanBeNull] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class, IEnumerable
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            foreach (object item in value)
            {
                if (item is null)
                    throw new ArgumentItemNullsNotAllowedException(value, valueName, message);

                if (Equals(DBNull.Value, item))
                {
                    message = string.IsNullOrWhiteSpace(message)
                        ? !string.IsNullOrWhiteSpace(valueName)
                            ? $"Argument {valueName} collection cannot contains DBNull values."
                            : "Argument collection cannot contains DBNull values."
                        : message;
                    throw new ArgumentItemNullsNotAllowedException(value, valueName, message);
                }
            }

            return value;
        }

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
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            if (Equals(value, string.Empty))
                throw new ArgumentEmptyStringNotAllowedException(valueName, message);

            return value;
        }

        /// <summary>Проверка строкового аргумента на null и на равенство string.Empty или состоять только из пробелов</summary>
        /// <exception cref="ArgumentNullException">Если строка == null</exception>
        /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
        /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
        /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из пробелов</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, NotWhitespace,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static string ArgumentNotNullOrWhitespace(
            [CanBeNull] string value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value, valueName);

            if (value == string.Empty)
                throw new ArgumentEmptyStringNotAllowedException(valueName, message);

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentWhitespaceNotAllowedException(valueName, message);

            return value;
        }

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
        {
            if (!Enabled)
                return value;

            ArgumentNotNull(value);

            if (Equals(DBNull.Value, value))
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} collection cannot contains DBNull values."
                        : "Argument collection cannot contains DBNull values."
                    : message;
                throw CreateArgumentNullException(valueName, message);
            }

            return value;
        }

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
            if (!Enabled)
                return;

            if (!condition)
                throw CreateArgumentException(valueName, message);
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
        {
            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(value))
                throw CreateArgumentException(valueName, message);

            return value;
        }

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
        {
            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(condition, nameof(condition));
            Debug.ItemsNotNull(exceptionParams, nameof(exceptionParams));

            if (!condition(value))
                throw CreateExceptionWithParams<TException>(exceptionParams);

            return value;
        }

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
            if (!condition)
                throw CreateException<TException>(
                    message,
                    () => !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} does not satisfy the conditions."
                        : "Argument does not satisfy the conditions.");
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
        {
            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(value))
                throw CreateException<TException>(
                    message,
                    () => !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} does not satisfy the conditions."
                        : "Argument does not satisfy the conditions.");

            return value;
        }

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
            if (!Enabled)
                return;

            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition())
                throw CreateArgumentException(valueName, message);
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
            if (!Enabled)
                return;

            if (!condition)
                throw CreateArgumentOutOfRangeException(valueName, message);
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
        {
            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(value))
                throw CreateArgumentOutOfRangeException(valueName, message);

            return value;
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
            [NotNull, InstantHandle] Func<bool> condition,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition())
                throw CreateArgumentOutOfRangeException(valueName, message);

            return value;
        }

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
        {
            if (!Enabled)
                return index;

            Debug.IsZeroOrPositive(count, nameof(count));

            if (index < 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be zero or positive number."
                        : "Index must be zero or positive number."
                    : message;

                throw new ArgumentOutOfRangeException(valueName, message);
            }
            if (index >= count)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} goes beyond the collection items count ({count})."
                        : "Index in arguments goes beyond the collection items count ({count})."
                    : message;
                throw new ArgumentOutOfRangeException(valueName, message);
            }

            return index;
        }

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
        {
            if (!Enabled)
                return index;

            Debug.IsZeroOrPositive(count, nameof(count));

            if (index < 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be zero or positive number."
                        : "Index must be zero or positive number."
                    : message;

                throw new ArgumentOutOfRangeException(valueName, message);
            }
            if (index >= count)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} goes beyond the collection items count ({count})."
                        : "Index in arguments goes beyond the collection items count ({count})."
                    : message;
                throw new ArgumentOutOfRangeException(valueName, message);
            }

            return index;
        }

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
        {
            if (!Enabled)
                return guid;

            Check.ArgumentNotNullOrWhitespace(guid, valueName);
            if (!Guid.TryParse(guid, out Guid _))
                if (!string.IsNullOrWhiteSpace(valueName))
                    throw new ArgumentException(!string.IsNullOrWhiteSpace(message)
                                                    ? message
                                                    : $"Value {valueName} must contain GUID!",
                                                valueName);
                else
                    throw new ArgumentException(!string.IsNullOrWhiteSpace(message)
                                                    ? message
                                                    : "Must contain GUID!");

            return guid;
        }

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
        {
            if (!Enabled)
                return guid;

            Check.NotNullOrWhitespace(guid, valueName);
            if (!Guid.TryParse(guid, out Guid result))
                if (!string.IsNullOrWhiteSpace(valueName))
                    throw new ArgumentException(!string.IsNullOrWhiteSpace(message)
                                                    ? message
                                                    : $"Value {valueName} must contain GUID!",
                                                valueName);
                else
                    throw new ArgumentException(!string.IsNullOrWhiteSpace(message)
                                                    ? message
                                                    : "Must contain GUID!");

            if (result == Guid.Empty)
                throw new ArgumentValueEmptyException(!string.IsNullOrWhiteSpace(valueName) ? valueName : null,
                                                      !string.IsNullOrWhiteSpace(message)
                                                          ? message
                                                          : !string.IsNullOrWhiteSpace(valueName)
                                                              ? $"Value {valueName} must contain non-empty GUID!"
                                                              : "Must contain non-empty GUID!");

            return guid;
        }

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
        {
            if (!Enabled)
                return guid;

            if (guid == Guid.Empty)
                throw new ArgumentValueEmptyException(valueName, message);

            return guid;
        }

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
        {
            if (!Enabled)
                return dictionary;

            ArgumentNotNull(dictionary, nameof(dictionary));

            if (!dictionary.ContainsKey(key))
                throw new ArgumentItemNotFoundException<TKey>(
                    key, ArgumentNotNullOrWhitespace(dictionaryName, nameof(dictionaryName)), message);

            return dictionary;
        }

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
        {
            if (!Enabled)
                return span;

            Debug.ArgumentNotNull(predicate, nameof(predicate));

            foreach (T item in span)
                if (!predicate(item))
                    throw ArgumentItemValidationExceptionException.FromSpan(span, spanName, message);

            return span;
        }

        /// <summary>Проверка что элементы диапазона удовлетворяют переданному условию</summary>
        /// <exception cref="ArgumentItemValidationExceptionException">Если в диапазоне присутствуют элементы не удовлетворяющие
        ///                                                            переданному условию</exception>
        /// <param name="span">Диапазон</param>
        /// <param name="predicate">Условие, которому должен соответствовать каждый элемент диапазона</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static Span<T> ArgumentSpanAll<T>(
            Span<T> span,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [CanBeNull] string message = null)
            where T : struct
            => ArgumentSpanAll(span, null, predicate, message);

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
        {
            if (!Enabled)
                return span;

            if (span.Length == 0)
                throw new ArgumentCollectionIsEmptyException(spanName, message);

            return span;
        }
        */

        /// <summary>Проверка того, что значение больше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [PositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static int ArgumentIsPositive(
            int value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value <= 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a positive number."
                        : "Argument value must be a positive number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение больше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [PositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static long ArgumentIsPositive(
            long value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value <= 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a positive number."
                        : "Argument value must be a positive number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение больше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [PositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static float ArgumentIsPositive(
            float value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value <= 0.0f)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a positive number."
                        : "Argument value must be a positive number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение больше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [PositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static double ArgumentIsPositive(
            double value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value <= 0.0d)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a positive number."
                        : "Argument value must be a positive number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение равно или больше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static int ArgumentIsZeroOrPositive(
            int value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value < 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a zero or positive number."
                        : "Argument value must be a zero or positive number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение равно или больше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static long ArgumentIsZeroOrPositive(
            long value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value < 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a zero or positive number."
                        : "Argument value must be a zero or positive number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение равно или больше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static float ArgumentIsZeroOrPositive(
            float value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value < 0.0f)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a zero or positive number."
                        : "Argument value must be a zero or positive number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение равно или больше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение меньше нуля</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static double ArgumentIsZeroOrPositive(
            double value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value < 0.0d)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a zero or positive number."
                        : "Argument value must be a zero or positive number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение меньше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NegativeNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static int ArgumentIsNegative(
            int value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value >= 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a negative number."
                        : "Argument value be a negative number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение меньше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NegativeNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static long ArgumentIsNegative(
            long value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value >= 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a negative number."
                        : "Argument value must be a negative number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение меньше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NegativeNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static float ArgumentIsNegative(
            float value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value >= 0.0f)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a negative number."
                        : "Argument value must be a negative number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение меньше нуля</summary>
        /// <exception cref="ArgumentOutOfRangeException">Если переданное значение больше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NegativeNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static double ArgumentIsNegative(
            double value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value >= 0.0d)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Argument {valueName} must be a negative number."
                        : "Argument value must be a negative number."
                    : message;
                throw CreateArgumentOutOfRangeException(valueName, message);
            }

            return value;
        }

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
        {
            if (!Enabled)
                return guid;

            Check.NotNullOrWhitespace(guid, valueName);
            if (!Guid.TryParse(guid, out _))
                throw new FormatException(!string.IsNullOrWhiteSpace(message)
                                              ? message
                                              : !string.IsNullOrWhiteSpace(valueName)
                                                  ? $"Value {valueName} must contain GUID!"
                                                  : "Must contain GUID!");

            return guid;
        }

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
        {
            if (!Enabled)
                return guid;

            Check.NotNullOrWhitespace(guid, valueName);
            if (!Guid.TryParse(guid, out Guid result))
                throw new FormatException(!string.IsNullOrWhiteSpace(message)
                                              ? message
                                              : !string.IsNullOrWhiteSpace(valueName)
                                                  ? $"Value {valueName} must contain GUID!"
                                                  : "Must contain GUID!");

            if (result == Guid.Empty)
                throw new ValueEmptyException(!string.IsNullOrWhiteSpace(valueName) ? valueName : null,
                                              !string.IsNullOrWhiteSpace(message)
                                                  ? message
                                                  : !string.IsNullOrWhiteSpace(valueName)
                                                      ? $"Value {valueName} must contain non-empty GUID!"
                                                      : "Must contain non-empty GUID!");

            return guid;
        }

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
        {
            if (!Enabled)
                return guid;

            if (guid == Guid.Empty)
                throw new ValueEmptyException(message);

            return guid;
        }

        /// <summary>Проверка что объект не null</summary>
        /// <exception cref="NullReferenceException">Если объект null</exception>
        /// <param name="value">Объект, который не должен быть равен null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static T NotNull<T>(
            [CanBeNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class
        {
            if (!Enabled)
                return value;

            if (value is null)
                throw CreateNullReferenceException(valueName, message);

            return value;
        }

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
        {
            if (!value.HasValue)
                throw CreateNullReferenceException(valueName, message);

            return (T) value;
        }

        /// <summary>Проверка что объект не null</summary>
        /// <exception cref="NullReferenceException">Если аргумент == null</exception>
        /// <param name="value">Объект, который не должен быть равен null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value: null => halt; => NotNull"), MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static T GenericNotNull<T>(
            [CanBeNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (typeof(T).IsByRef &&
                ReferenceEquals(value, null))
                throw CreateNullReferenceException(valueName, message);

            return value;
        }

        /// <summary>Проверка значения на значение по-умолчанию</summary>
        /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
        /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ValueNotEmpty<T>(
            [NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (!Enabled)
                return value;

            if (Equals(value, default(T)))
                throw new ValueEmptyException(valueName, message);

            return value;
        }

        /// <summary>Проверка что значение не пусто (пустое значение отличается от default)</summary>
        /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
        /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
        /// <param name="emptyValue">Пустое значение параметра</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ValueNotEmpty<T>(
            [NoEnumeration] T value,
            T emptyValue,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (!Enabled)
                return value;

            if (Equals(value, emptyValue))
                throw new ValueEmptyException(valueName, message);

            return value;
        }

        /// <summary>Проверка что значение не пусто (пустое значение отличается от default)</summary>
        /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
        /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
        /// <param name="emptyValue1">Пустое значение параметра</param>
        /// <param name="emptyValue2">Пустое значение параметра</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ValueNotEmpty<T>(
            [NoEnumeration] T value,
            T emptyValue1,
            T emptyValue2,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (!Enabled)
                return value;

            if (Equals(value, emptyValue1) || Equals(value, emptyValue2))
                throw new ValueEmptyException(valueName, message);

            return value;
        }

        /// <summary>Проверка что значение IntPtr не пусто (пустое значение отличается от IntPtr.Zero)</summary>
        /// <exception cref="ValueEmptyException">Если аргумент == IntPtr.Zero</exception>
        /// <param name="value">Значение, которое не должно быть равно IntPtr.Zero</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IntPtr ValueNotEmpty(
            IntPtr value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (Equals(value, IntPtr.Zero))
                throw new ValueEmptyException(valueName, message);

            return value;
        }

        /// <summary>Проверка попадания значения в допустимый диапазон</summary>
        /// <exception cref="ValueOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
        /// <param name="value">Значение, которое должно попадать в допустимый диапазон</param>
        /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => null"), NotEmpty,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T InRange<T>(
            [CanBeNull] T value,
            [NotNull, InstantHandle] Func<T, bool> condition,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (!condition(value))
                throw new ValueOutOfRangeException(valueName, message);

            return value;
        }

        /// <summary>Проверка попадания значения в допустимый диапазон</summary>
        /// <exception cref="ValueOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
        /// <param name="value">Значение, которое должно попадать в допустимый диапазон</param>
        /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => null"),
         NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T InRange<T>(
            [CanBeNull] T value,
            [NotNull, InstantHandle] Func<bool> condition,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (!condition())
                throw new ValueOutOfRangeException(valueName, message);

            return value;
        }

        /// <summary>Проверка попадания значения в допустимый диапазон</summary>
        /// <exception cref="ValueOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
        /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static void InRange(
            bool condition,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return;

            if (!condition)
                throw new ValueOutOfRangeException(valueName, message);
        }

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
        {
            if (!Enabled)
                return index;

            Debug.IsZeroOrPositive(count, nameof(count));

            if (index < 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be zero or positive number."
                        : "Index must be zero or positive number."
                    : message;

                throw new ValueOutOfRangeException(valueName, message);
            }
            if (index >= count)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} goes beyond the collection items count ({count})."
                        : "Index goes beyond the collection items count ({count})."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return index;
        }

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
        {
            if (!Enabled)
                return index;

            Debug.IsZeroOrPositive(count, nameof(count));

            if (index < 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be zero or positive number."
                        : "Index must be zero or positive number."
                    : message;

                throw new ValueOutOfRangeException(valueName, message);
            }
            if (index >= count)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} goes beyond the collection items count ({count})."
                        : "Index goes beyond the collection items count ({count})."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return index;
        }

        /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
        /// <exception cref="ValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
        /// <param name="value">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotEmpty,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ValuesNotEmpty<T>(
            [CanBeNull] IEnumerable<T> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            foreach (T item in value) // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (Equals(item, default(T)))
                    throw new ValueEmptyException(valueName, message);

            return value;
        }

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
        {
            if (!Enabled)
                return value;

            if (value is null)
                throw CreateNullReferenceException(valueName, message);

            if (Equals(value, string.Empty))
                throw new EmptyStringNotAllowedException(message);

            return value;
        }

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
        {
            if (!Enabled)
                return value;

            if (value is null)
                throw CreateNullReferenceException(valueName, message);

            if (value == string.Empty)
                throw new EmptyStringNotAllowedException(message);

            if (string.IsNullOrWhiteSpace(value))
                throw new WhitespaceNotAllowedException(message);

            return value;
        }

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
        {
            if (!Enabled)
                return value;

            if (value is null)
                throw CreateNullReferenceException(valueName, message);

            if (Equals(DBNull.Value, value))
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"Value {valueName} cannot be DBNull value."
                        : "Value cannot be DBNull value."
                    : message;
                throw CreateNullReferenceException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка что объект не null</summary>
        /// <exception><cref>TException</cref>Если условие не выполняется</exception>
        /// <param name="value">Объект, который должен быть не null</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static T NotNull<T, TException>(
            [CanBeNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class
            where TException : NullReferenceException
        {
            if (!Enabled)
                return value;

            if (value is null)
                throw CreateException<TException>(
                    message,
                    () => !string.IsNullOrWhiteSpace(valueName)
                        ? $"Value {valueName} cannot be null."
                        : "Value cannot be null.");

            return value;
        }

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
        {
            if (!value.HasValue)
                throw CreateException<TException>(
                    message,
                    () => !string.IsNullOrWhiteSpace(valueName)
                        ? $"Value {valueName} cannot be null."
                        : "Value cannot be null.");

            return (T) value;
        }

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
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            foreach (object item in value)
                if (item is null)
                    throw new ItemNullsNotAllowedException(value, valueName, message);

            return value;
        }

        /// <summary>Проверка что элементы последовательности не null</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
        /// <param name="value">Коллекция, элементы которой должен быть не null</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static IEnumerable<T> ItemsNotNull<T>(
            [CanBeNull] IEnumerable<T?> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            NotNull(value, valueName);

            foreach (T? item in value)
                if (item is null)
                    throw new ItemNullsNotAllowedException(value, valueName, message);

            return value.Cast<T>();
        }

        /// <summary>Проверка что коллекция не пуста</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
        /// <param name="value">Коллекция, которая не должна быть пустой</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static T CollectionNotEmpty<T>(
            [CanBeNull] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class, ICollection
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            if (value.Count == 0)
                throw new CollectionIsEmptyException(valueName, message);
            return value;
        }

        /// <summary>Проверка что коллекция не пуста</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
        /// <param name="value">Коллекция, которая не должна быть пустой</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static IReadOnlyCollection<T> ReadOnlyCollectionNotEmpty<T>(
            [CanBeNull, NoEnumeration] IReadOnlyCollection<T> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            if (value.Count == 0)
                throw new CollectionIsEmptyException(valueName, message);
            return value;
        }

        /// <summary>Проверка что коллекция не пуста</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
        /// <param name="value">Коллекция, которая не должна быть пустой</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static TCollection ReadOnlyCollectionNotEmpty<TCollection, T>(
            [CanBeNull, NoEnumeration] TCollection value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where TCollection : class, IReadOnlyCollection<T>
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            if (value.Count == 0)
                throw new CollectionIsEmptyException(valueName, message);
            return value;
        }

        /// <summary>Проверка что последовательность не пусто</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="CollectionIsEmptyException">Если последовательность пусто</exception>
        /// <param name="value">Последовательность, которая не должна быть пустой</param>
        /// <param name="valueName">Наименование последовательности</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static T EnumerationNotEmpty<T>(
            [CanBeNull] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class, IEnumerable
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            if (value is ICollection collection)
            {
                if (collection.Count == 0)
                    throw new CollectionIsEmptyException(valueName, message);
            }
            else
            {
                IEnumerator enumerator = value.GetEnumerator();
                try
                {
                    if (enumerator.MoveNext() != true)
                        throw new CollectionIsEmptyException(valueName, message);
                }
                finally
                {
                    if (enumerator is IDisposable enumeratorDisposable)
                        enumeratorDisposable.Dispose();
                }
            }
            return value;
        }

        /// <summary>Проверка что все строки последовательности не null и не пусты</summary>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
        /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
        /// <param name="value">Последовательность строк, которые быть не должны быть равны null или string.Empty</param>
        /// <param name="valueName">Наименование последовательности строк</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotEmpty,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<string> StringsNotEmpty(
            [CanBeNull] IEnumerable<string> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            foreach (string str in value)
            {
                if (str is null)
                    throw new ItemNullsNotAllowedException(value, valueName, message);

                if (str == string.Empty)
                    throw new ItemEmptyStringNotAllowedException(value, valueName, message);
            }

            return value;
        }

        /// <summary>Проверка что все строки последовательности не null, не пустые строки и не строки состоящие из одних пробелов</summary>
        /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
        /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
        /// <exception cref="ItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие ничего
        ///                                                     кроме пробелов</exception>
        /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
        /// <param name="value">Последовательность строк, которые быть не должны быть равны null, string.Empty или заполнены
        ///                     одними только пробелами</param>
        /// <param name="valueName">(Optional) Наименование коллекции</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotWhitespace,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<string> StringsNotWhitespace(
            [CanBeNull] IEnumerable<string> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            foreach (string str in value)
            {
                if (str is null)
                    throw new ItemNullsNotAllowedException(value, valueName, message);

                if (str == string.Empty)
                    throw new ItemEmptyStringNotAllowedException(value, valueName, message);

                if (string.IsNullOrWhiteSpace(str))
                    throw new ItemWhitespaceNotAllowedException(value, valueName, message);
            }

            return value;
        }

        /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
        /// <exception cref="ItemNullsNotAllowedException">Если коллекция содержит null</exception>
        /// <exception cref="ItemNullsNotAllowedException">Если коллекция содержит DBNull</exception>
        /// <param name="value">Коллекция, элементы которой должен быть не null</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotEmpty,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ItemsNotNullNotDbNull<T>(
            [CanBeNull] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : class, IEnumerable
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            foreach (object item in value)
            {
                if (item is null)
                    throw new ItemNullsNotAllowedException(value, valueName, message);

                if (Equals(DBNull.Value, item))
                {
                    message = string.IsNullOrWhiteSpace(message)
                        ? !string.IsNullOrWhiteSpace(valueName)
                            ? $"Collection {valueName} cannot contains DBNull values."
                            : "Collection cannot contains DBNull values."
                        : message;
                    throw new ItemNullsNotAllowedException(value, valueName, message);
                }
            }

            return value;
        }

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
            if (!condition)
                throw new Exception(message);
        }

        /// <summary>Проверка условия</summary>
        /// <exception cref="Exception">Если условие не выполняется</exception>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="messageConstructor">Метод-конструктор сообщения об ошибке</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static void Assert(
            bool condition,
            [NotNull, NotEmpty] Func<string> messageConstructor)
        {
            if (!condition)
            {
                string message = messageConstructor();
                if (string.IsNullOrWhiteSpace(message))
                    throw new Exception(message);
                throw new Exception(message);
            }
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
        {
            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(value))
                throw new Exception(message);
            return value;
        }

        /// <summary>Проверка условия</summary>
        /// <exception cref="Exception">Если условие не выполняется</exception>
        /// <param name="value">Возвращаемое значение если проверка пройдена</param>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="messageConstructor">Метод-конструктор сообщения об ошибке</param>
        [ContractAnnotation("condition:false => halt; value:null => null"),
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T Assert<T>(
            [CanBeNull, NoEnumeration] T value,
            [NotNull, InstantHandle] Func<T, bool> condition,
            [NotNull, NotEmpty] Func<string> messageConstructor)
        {
            Debug.ArgumentNotNull(messageConstructor, nameof(messageConstructor));

            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(value))
            {
                string message = messageConstructor();
                if (string.IsNullOrWhiteSpace(message))
                    throw new Exception(message);
                throw new Exception(message);
            }

            return value;
        }

        /// <summary>Проверка условия</summary>
        /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static void Assert<TException>(
            bool condition,
            [NotNull, ItemNotNull] object[] exceptionParams)
            where TException : Exception
        {
            Debug.ItemsNotNull(exceptionParams, nameof(exceptionParams));

            if (!condition)
                throw CreateExceptionWithParams<TException>(exceptionParams);
        }

        /// <summary>Проверка условия</summary>
        /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
        /// <param name="condition">Условие, которое должно быть true</param>
        /// <param name="messageConstructor">Метод-конструктор сообщения об ошибке</param>
        [ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static void Assert<TException>(
            bool condition,
            [NotNull, NotEmpty] Func<string> messageConstructor)
            where TException : Exception
        {
            Debug.ArgumentNotNull(messageConstructor, nameof(messageConstructor));

            if (!condition)
            {
                string message = messageConstructor();
                throw CreateException<TException>(message);
            }
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
            [NotNull, ItemNotNull] object[] exceptionParams)
            where TException : Exception
        {
            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(condition, nameof(condition));
            Debug.ItemsNotNull(exceptionParams, nameof(exceptionParams));

            if (!condition(value))
                throw CreateExceptionWithParams<TException>(exceptionParams);

            return value;
        }

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
            if (!condition)
                throw CreateException<TException>(message);
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
        {
            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(condition, nameof(condition));

            if (!condition(value))
                throw CreateException<TException>(message);

            return value;
        }

        /// <summary>Безопасная конвертация, поддерживающая например конвертацию IEnumerable из decimal в IEnumerable из long
        ///          (обычный Cast выбрасывает exception). Критично для работы с СУБД, например Oracle числа возвращает в виде
        ///          decimal</summary>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        private static IEnumerable<T> ConvertAll<T>(
            [NotNull] IEnumerable enumeration,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            Debug.ArgumentNotNull(enumeration, nameof(enumeration));

            return enumeration as IEnumerable<T> ?? _ConvertAll<T>(enumeration, formatProvider);
        }

        [NotNull, ItemCanBeNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        private static IEnumerable<T> _ConvertAll<T>(
            [NotNull] IEnumerable enumeration,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            Debug.ArgumentNotNull(enumeration, nameof(enumeration));

            Type type = typeof(T);
            foreach (object item in enumeration)
                if (item == null || item is DBNull)
                    yield return default;
                else
                    yield return (T) Convert.ChangeType(item, type, formatProvider);
        }

        /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
        /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
        /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ItemsIs<T>(
            [CanBeNull] IEnumerable value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            NotNull(value, valueName);

            foreach (object item in value)
                if (item != null && !(item is T))
                {
                    message = string.IsNullOrWhiteSpace(message)
                        ? !string.IsNullOrWhiteSpace(valueName)
                            ? $"Collection {valueName} has item \"{item}\", with is not {typeof(T)} type."
                            : $"Collection has item \"{item}\", with is not {typeof(T)} type."
                        : message;
                    throw new InvalidCastException(message);
                }

            return ConvertAll<T>(value);
        }

        /// <summary>Конструктор сообщения об ошибке для ненулевого объекта</summary>
        [NotNull] public delegate string ObjectMessageFactory([NotNull] object value);

        /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
        /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
        /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ItemsIs<T>(
            [CanBeNull] IEnumerable value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
            [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
        {
            NotNull(value, valueName);
            Debug.ArgumentNotNull(messageFactory, nameof(messageFactory));

            foreach (object item in value)
                if (item != null && !(item is T))
                {
                    string message = messageFactory(item);
                    message = string.IsNullOrWhiteSpace(message)
                        ? !string.IsNullOrWhiteSpace(valueName)
                            ? $"Collection {valueName} has item \"{item}\", with is not {typeof(T)} type."
                            : $"Collection has item \"{item}\", with is not {typeof(T)} type."
                        : message;
                    throw new InvalidCastException(message);
                }

            return ConvertAll<T>(value);
        }

        /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
        /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
        /// <exception cref="Exception">Если обнаружен элемент не являющийся объектом нужного типа</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> ItemsIs<T>(
            [CanBeNull] IEnumerable value,
            [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
            => ItemsIs<T>(value, null, messageFactory);

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
        ///                                                    переданное условие</exception>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="predicate">Условие</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static IEnumerable<T> All<T>(
            [CanBeNull] IEnumerable<T> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);
            Debug.ArgumentNotNull(predicate, nameof(predicate));

            foreach (T item in value)
                if (!predicate(item))
                    throw new ItemValidationExceptionException(message);

            return value;
        }

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
        ///                                                    переданное условие</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="predicate">Условие</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static IEnumerable<T> All<T>(
            [CanBeNull] IEnumerable<T> value,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [CanBeNull] string message = null)
            => All(value, null, predicate, message);

        /// <summary>Конструктор сообщения об ошибке</summary>
        [NotNull] public delegate string TemplateMessageFactory<in T>([CanBeNull] T value);

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
        ///                                                    переданное условие</exception>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="predicate">Условие</param>
        /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static IEnumerable<T> All<T>(
            [CanBeNull] IEnumerable<T> value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [NotNull] TemplateMessageFactory<T> messageFactory)
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);
            Debug.ArgumentNotNull(predicate, nameof(predicate));
            Debug.ArgumentNotNull(messageFactory, nameof(messageFactory));

            foreach (T item in value)
                if (!predicate(item))
                {
                    string message = messageFactory(item);
                    throw new ItemValidationExceptionException(message);
                }

            return value;
        }

        /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
        /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
        /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
        ///                                                    переданное условие</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="predicate">Условие</param>
        /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static IEnumerable<T> All<T>(
            [CanBeNull] IEnumerable<T> value,
            [NotNull, InstantHandle] Func<T, bool> predicate,
            [NotNull] TemplateMessageFactory<T> messageFactory)
            => All(value, null, predicate, messageFactory);

        /// <summary>Проверка, что перечисление не пусто</summary>
        /// <exception cref="NullReferenceException">Если перечисление равно null</exception>
        /// <exception cref="CollectionIsEmptyException">Если перечисление пусто</exception>
        /// <param name="value">Коллекция</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static TEnumerable NotNullNotEmpty<TEnumerable>(
            [CanBeNull] TEnumerable value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull, CanBeEmpty] string message = null)
            where TEnumerable : class, IEnumerable
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            if (value is ICollection collection)
            {
                if (collection.Count == 0)
                    throw new CollectionIsEmptyException(valueName, message);
            }
            else
            {
                IEnumerator enumerator = value.GetEnumerator();
                try
                {
                    if (enumerator.MoveNext() != true)
                        throw new CollectionIsEmptyException(valueName, message);
                }
                finally
                {
                    if (enumerator is IDisposable enumeratorDisposable)
                        enumeratorDisposable.Dispose();
                }
            }

            return value;
        }

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
        [ContractAnnotation("condition:false => halt; value:null => halt"), NotNull,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ObjectState<T>(
            [CanBeNull, NoEnumeration] T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
            [NotNull, InstantHandle] Func<T, bool> condition,
            [CanBeNull, InvokerParameterName] string message = null)
        {
            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(condition, nameof(condition));

            if (Equals(value, null)) //-V3111
                throw CreateNullReferenceException(valueName, message);

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
        [ContractAnnotation("condition:false => halt; value:null => halt"), NotNull,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T ObjectState<T>(
            [CanBeNull, NoEnumeration] T value,
            [NotNull, InstantHandle] Func<T, bool> condition,
            [CanBeNull, InvokerParameterName] string message = null)
            => ObjectState(value, null, condition, message);

        /// <summary>Устаревшая версия проверки того, что значение является допустимым для данного типа перечня (enum)</summary>
        /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
        /// <param name="type">Тип значения</param>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [Obsolete("Удалите передачу типа Enum, начиная c C# 7.2 он не требуется"),
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T EnumInRange<T>(
            [NotNull] Type type,
            T value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct, Enum
            => EnumInRange(value, valueName, message);

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
        {
            if (!Enabled)
                return value;

            if (!Enum.IsDefined(typeof(T), value))
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"The value {valueName} = {Convert.ToInt64(value)} is invalid for Enum type '{typeof(T)}'."
                        : $"The value ({Convert.ToInt64(value)}) is invalid for Enum type '{typeof(T)}'."
                    : message;
                throw new InvalidEnumArgumentException(message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
        /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        protected internal static void EnumInRange<T>(
            [NotNull] object value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct, Enum
        {
            Debug.ArgumentNotNull(value, valueName);

            if (!Enabled)
                return;

            if (!Enum.IsDefined(typeof(T), value))
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"The value {valueName} = {Convert.ToInt64(value)} is invalid for Enum type '{typeof(T)}'."
                        : $"The value ({Convert.ToInt64(value)}) is invalid for Enum type '{typeof(T)}'."
                    : message;
                throw new InvalidEnumArgumentException(message);
            }
        }

        [NotNull] public delegate Exception EnumInRangeCustomExceptionFactory<in T>(T value)
            where T : struct, Enum;

        /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
        /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
        /// <param name="value">Значение</param>
        /// <param name="getExceptionFunc">Метод-конструктор исключительной ситуации</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static T EnumInRangeCustom<T>(
            T value,
            [NotNull] EnumInRangeCustomExceptionFactory<T> getExceptionFunc)
            where T : struct, Enum
        {
            if (!Enabled)
                return value;

            Debug.ArgumentNotNull(getExceptionFunc, nameof(getExceptionFunc));

            if (!Enum.IsDefined(typeof(T), value))
                // ReSharper disable once PossibleNullReferenceException
                throw getExceptionFunc(value);

            return value;
        }

        /*
        /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
        /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
        /// <param name="type">Тип значения</param>
        /// <param name="values">Список значений</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [Obsolete("Удалите передачу типа Enum, начиная c C# 7.2 он не требуется"),
         ContractAnnotation("values:null => halt"), NotNull,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> AllEnumInRange<T>(
            [NotNull] Type type,
            [CanBeNull] IEnumerable<T> values,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct, Enum
            => AllEnumInRange(values, valueName, message);
            */

        /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
        /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
        /// <param name="values">Список значений</param>
        /// <param name="valueName">Наименование коллекции</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("values:null => halt"), NotNull,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static IEnumerable<T> AllEnumInRange<T>(
            [CanBeNull] IEnumerable<T> values,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
            where T : struct, Enum
        {
            if (!Enabled)
                return values;

            Debug.NotNull(values, valueName ?? string.Empty);

            foreach (T item in values)
                if (!Enum.IsDefined(typeof(T), item))
                {
                    message = string.IsNullOrWhiteSpace(message)
                        ? !string.IsNullOrWhiteSpace(valueName)
                            ? $"Collection {valueName} contains value {Convert.ToInt64(item)} with is invalid for Enum type '{typeof(T)}'."
                            : $"Collection contains value {Convert.ToInt64(item)} with is invalid for Enum type '{typeof(T)}'."
                        : message;
                    throw new InvalidEnumArgumentException(message);
                }

            return values;
        }

        /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
        /// <exception cref="NullReferenceException">Если объект null</exception>
        /// <exception cref="InvalidCastException">Если тип переданного объекта не <see cref="T" /></exception>
        /// <param name="value">Проверяемый объект</param>
        /// <param name="valueName">Наименование переданного объекта</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static T Is<T>(
            [CanBeNull] object value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            NotNull(value, valueName, message);

            if (!(value is T))
                throw CreateInvalidCastException(typeof(T), valueName, message);

            return (T) value;
        }

        /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
        /// <exception cref="NullReferenceException">Если объект null</exception>
        /// <exception cref="InvalidCastException">Если тип переданного объекта не <see cref="T" /></exception>
        /// <param name="value">Проверяемый объект</param>
        /// <param name="valueName">Наименование переданного объекта</param>
        /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static T Is<T>(
            [CanBeNull] object value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName,
            [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
        {
            NotNull(value, valueName);
            Debug.ArgumentNotNull(messageFactory, nameof(messageFactory));

            if (!(value is T))
                throw new InvalidCastException(messageFactory(value));

            return (T) value;
        }

        /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
        /// <exception cref="NullReferenceException">Если объект null</exception>
        /// <exception cref="Exception">Если тип переданного объекта не <see cref="T" /></exception>
        /// <param name="value">Проверяемый объект</param>
        /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
        [ContractAnnotation("value:null => halt"), NotNull, MethodImpl(MethodImplOptions.AggressiveInlining),
         DebuggerHidden]
        public static T Is<T>(
            [CanBeNull] object value,
            [NotNull, InstantHandle] ObjectMessageFactory messageFactory)
            => Is<T>(value, null, messageFactory);

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
        {
            if (!Enabled)
                return value;

            ArgumentNotNullOrWhitespace(value, valueName);

            if (!File.Exists(value))
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? string.IsNullOrWhiteSpace(valueName)
                        ? $"File {valueName}=\"{value}\" not found"
                        : $"File \"{value}\" not found"
                    : message;
                throw new FileNotFoundException(message, value);
            }

            return value;
        }

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
        {
            if (!Enabled)
                return value;

            NotNullOrWhitespace(value, valueName);

            if (!Directory.Exists(value))
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? string.IsNullOrWhiteSpace(valueName)
                        ? $"Folder {valueName}=\"{value}\" not found"
                        : $"Folder \"{value}\" not found"
                    : message;
                throw new DirectoryNotFoundException(message);
            }

            return value;
        }

        /// <summary>Проверка что стрим не равен null, что имеет ненулевую длину и текущая позиция не находится в конце стрима</summary>
        /// <exception cref="InvalidOperationException">Если длина стрима равна 0</exception>
        /// <exception cref="ArgumentNullException">Если переданный стрим == null</exception>
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
        {
            if (!Enabled)
                return value;

            NotNull(value, valueName);

            if (!value.CanRead)
                throw new InvalidOperationException(string.IsNullOrWhiteSpace(message)
                                                        ? $"Stream {valueName} of type {value.GetType()} can not be read."
                                                        : message);

            if (value.Length == 0)
                throw new InvalidOperationException(string.IsNullOrWhiteSpace(message)
                                                        ? $"Stream {valueName} has zero length."
                                                        : message);

            if (value.Position >= value.Length - 1)
                throw !string.IsNullOrWhiteSpace(message)
                    ? new EndOfStreamException(message)
                    : new EndOfStreamException();

            return value;
        }

        /// <summary>Проверка что строка содержит корректный Uri</summary>
        /// <exception cref="InvalidUriException">Если Uri некорректен</exception>
        /// <exception cref="ArgumentNullException">Если строка описывающая Uri == null</exception>
        /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка описывающая Uri == string.Empty</exception>
        /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка описывающая Uri состоит только из пробелов</exception>
        /// <param name="value">Строка, содержащая Uri</param>
        /// <param name="valueName">(Optional) Наименование строки</param>
        /// <param name="scheme">(Optional) Схема Uri которой должен соответствовать адрес. Например UriScheme.Http для Http
        ///                      адреса. Если null - схема не проверяется</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        /// <returns>Строка, содержащая Uri</returns>
        [ContractAnnotation("value:null => halt"), NotNull, NotWhitespace,
         MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static string UriCorrect(
            [CanBeNull] string value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            UriScheme scheme = UriScheme.Any,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            ArgumentNotNullOrWhitespace(value, valueName);

            if (Uri.TryCreate(value, UriKind.Absolute, out Uri uri) &&
                (scheme == UriScheme.Any || scheme == UriScheme.None ||
                 // ReSharper disable once PossibleNullReferenceException
                 UriSchemes.Name2Value[uri.Scheme] == scheme))
                return value;
            throw new InvalidUriException(value, valueName, message);
        }

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
        {
            if (!Enabled)
                return dictionary;

            NotNull(dictionary, nameof(dictionary));

            if (!dictionary.ContainsKey(key))
                throw new ItemNotFoundException<TKey>(key, message);

            return dictionary;
        }

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
        {
            if (!Enabled)
                return span;

            Debug.ArgumentNotNull(predicate, nameof(predicate));

            foreach (T item in span)
                if (!predicate(item))
                    throw ItemValidationExceptionException.FromSpan(span, spanName, message);

            return span;
        }

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
            => SpanAll(span, null, predicate, message);

        /// <summary>Проверка что диапазон не пуст</summary>
        /// <exception cref="CollectionIsEmptyException">Если диапазон пуст</exception>
        /// <param name="span">Диапазон, который не должен быть пуст</param>
        /// <param name="spanName">Наименование диапазона</param>
        /// <param name="message">Сообщение об ошибке</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static Span<T> SpanNotEmpty<T>(
            Span<T> span,
            [CanBeNull, NotWhitespace, InvokerParameterName] string spanName = null,
            [CanBeNull] string message = null)
            where T : struct
        {
            if (!Enabled)
                return span;

            if (span.Length == 0)
                throw new CollectionIsEmptyException(spanName, message);
            return span;
        }
        */

        /// <summary>Проверка того, что значение больше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [PositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static int IsPositive(
            int value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value <= 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a positive number."
                        : "Value must be a positive number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение больше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [PositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static long IsPositive(
            long value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value <= 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a positive number."
                        : "Value must be a positive number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение больше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [PositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static float IsPositive(
            float value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value <= 0.0f)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a positive number."
                        : "Value must be a positive number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение больше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [PositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static double IsPositive(
            double value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value <= 0.0d)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a positive number."
                        : "Value must be a positive number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение равно или больше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static int IsZeroOrPositive(
            int value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value < 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a zero or positive number."
                        : "Value must be a zero or positive number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение равно или больше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static long IsZeroOrPositive(
            long value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value < 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a zero or positive number."
                        : "Value must be a zero or positive number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение равно или больше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static float IsZeroOrPositive(
            float value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value < 0.0f)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a zero or positive number."
                        : "Value must be a zero or positive number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение равно или больше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static double IsZeroOrPositive(
            double value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value < 0.0d)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a zero or positive number."
                        : "Value must be a zero or positive number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение меньше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NegativeNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static int IsNegative(
            int value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value >= 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a negative number."
                        : "Value must be a negative number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение меньше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NegativeNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static long IsNegative(
            long value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value >= 0)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a negative number."
                        : "Value must be a negative number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение меньше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NegativeNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static float IsNegative(
            float value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value >= 0.0f)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a negative number."
                        : "Value must be a negative number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

        /// <summary>Проверка того, что значение меньше нуля</summary>
        /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
        /// <param name="value">Значение</param>
        /// <param name="valueName">Наименование проверяемого параметра</param>
        /// <param name="message">Сообщение об ошибке</param>
        [NegativeNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
        public static double IsNegative(
            double value,
            [CanBeNull, NotWhitespace, InvokerParameterName] string valueName = null,
            [CanBeNull] string message = null)
        {
            if (!Enabled)
                return value;

            if (value >= 0.0d)
            {
                message = string.IsNullOrWhiteSpace(message)
                    ? !string.IsNullOrWhiteSpace(valueName)
                        ? $"{valueName} must be a negative number."
                        : "Value must be a negative number."
                    : message;
                throw new ValueOutOfRangeException(valueName, message);
            }

            return value;
        }

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
            Debug.ArgumentNotNullOrWhitespace(objectName, nameof(objectName));

            if (Enabled && notNullRef is null)
                    throw !string.IsNullOrWhiteSpace(message)
                        ? new ObjectDisposedException(objectName, message)
                        : new ObjectDisposedException(objectName);
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
            where T: IDisposable
        {
            if (Enabled && notNullRef is null)
                    throw !string.IsNullOrWhiteSpace(message)
                        ? new ObjectDisposedException(typeof(T).Name, message)
                        : new ObjectDisposedException(typeof(T).Name);
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
            Debug.ArgumentNotNullOrWhitespace(objectName, nameof(objectName));

            if (Enabled && disposedFlag)
                throw !string.IsNullOrWhiteSpace(message)
                    ? new ObjectDisposedException(objectName, message)
                    : new ObjectDisposedException(objectName);
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
            where T: IDisposable
        {
            if (Enabled && disposedFlag)
                throw !string.IsNullOrWhiteSpace(message)
                    ? new ObjectDisposedException(typeof(T).Name, message)
                    : new ObjectDisposedException(typeof(T).Name);
        }

        /// <summary>Проверка что объект заблокирован конструкцией lock. Служит для проверки того, что контекст вызова
        ///     потокобезопасен</summary>
        /// <exception cref="InvalidOperationException">Если объект не заблокирован конструкцией lock</exception>
        /// <param name="syncObject">Объект, который должен быть заблокирован конструкцией lock</param>
        /// <param name="syncObjectName">(Optional) Имя объекта, который используется для блокирования доступа к контексту вызова</param>
        /// <param name="message">(Optional) Сообщение об ошибке</param>
        [NotNull, DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SyncLocked<T>(
            [NotNull] T syncObject,
            [CanBeNull, NotWhitespace] string syncObjectName = null,
            [CanBeNull] string message = null)
            where T: class
        {
            NotNull(syncObject, syncObjectName);

            if (!Enabled || Monitor.IsEntered(syncObject))
                return syncObject;

            throw new InvalidOperationException(
                !string.IsNullOrWhiteSpace(message)
                    ? message
                    : !string.IsNullOrWhiteSpace(syncObjectName)
                        ? $"{syncObjectName} must locked!"
                        : "Sync object must be locked!");
        }
    }
}