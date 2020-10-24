using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Contract.Exceptions;

using JetBrains.Annotations;

// ReSharper disable ArrangeAttributes
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Contract.Validation
{
    /// <summary>Runtime валидация условий</summary>
    public abstract partial class Check
    {
        /// <summary>Валидация условий для значения передаваемого в сеттер свойств. В DEBUG билде проверки осуществляются, иначе - нет</summary>
        public abstract class SetValue
        {
            /// <summary>Проверка что строка содержит guid</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="ArgumentException">Если строка не содержит GUID</exception>
            /// <param name="guid">Строка, которая должна содержать Guid</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), GuidStr,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsGuid(
                [CanBeNull] string guid,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.ArgumentIsGuid(guid, callerMemberName, message);

            /// <summary>Проверка что строка содержит непустой guid</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="ArgumentException">Если строка не содержит GUID</exception>
            /// <exception cref="ArgumentValueEmptyException">Если GUID пуст</exception>
            /// <param name="guid">Строка, которая должна содержать непустой Guid</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            /// <param name="message">Сообщение об ошибке</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void GuidNotEmpty(
                [CanBeNull] string guid,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.ArgumentGuidNotEmpty(guid, callerMemberName, message);

            /// <summary>Проверка что guid не пуст</summary>
            /// <exception cref="ArgumentValueEmptyException">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void GuidNotEmpty(
                Guid guid,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentGuidNotEmpty(guid, callerMemberName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class
                => ArgumentNotNull(value, callerMemberName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNull<T>(
                [CanBeNull, NoEnumeration] T? value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => ArgumentNotNull(value, callerMemberName, message);

            /// <summary>Проверка что значение, присваиваемое свойству, не null</summary>
            /// <exception cref="ArgumentNullException">Если аргумент == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [ContractAnnotation("value:null => halt"), Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ArgumentGenericNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.ArgumentGenericNotNull(value, callerMemberName, message);

            /// <summary>Проверка значения на значение по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ValueNotEmpty<T>(
                T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => ArgumentValueNotEmpty(value, callerMemberName, message);

            /// <summary>Проверка что значение не пусто (пустое значение отличается от default)</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue">Пустое значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ValueNotEmpty<T>(
                T value,
                T emptyValue,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => ArgumentValueNotEmpty(value, emptyValue, callerMemberName, message);

            /// <summary>Проверка что значение не пусто (пустое значение отличается от default)</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue1">Пустое значение</param>
            /// <param name="emptyValue2">Пустое значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ValueNotEmpty<T>(
                T value,
                T emptyValue1,
                T emptyValue2,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => ArgumentValueNotEmpty(value, emptyValue1, emptyValue2, callerMemberName, message);

            /// <summary>Проверка что значение IntPtr не пусто (пустое значение отличается от IntPtr.Zero)</summary>
            /// <exception cref="ArgumentValueEmptyException">Если аргумент == IntPtr.Zero</exception>
            /// <param name="value">Значение, которое не должно быть равно IntPtr.Zero</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ValueNotEmpty(
                IntPtr value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentValueNotEmpty(value, callerMemberName, message);

            /// <summary>Проверка попадания значения в допустимый диапазон</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если условие проверки не выполняется</exception>
            /// <param name="value">Значение, которое должно попадать в допустимый диапазон</param>
            /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void InRange<T>(
                [CanBeNull] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentInRange(value, condition, callerMemberName, message);

            /// <summary>Проверка попадания значения в допустимый диапазон</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
            /// <param name="value">Значение, которое должно попадать в допустимый диапазон</param>
            /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void InRange<T>(
                [CanBeNull] T value,
                [NotNull, InstantHandle] Func<bool> condition,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentInRange(value, condition, callerMemberName, message);

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), DebuggerHidden,
             ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void IndexInRange(
                int index,
                int count,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.ArgumentIndexInRange(index, count, callerMemberName, message);

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ArgumentOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), DebuggerHidden,
             ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void IndexInRange(
                long index,
                long count,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.ArgumentIndexInRange(index, count, callerMemberName, message);

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ArgumentValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="value">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ValuesNotEmpty<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => ArgumentValuesNotEmpty(value, callerMemberName, message);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                            переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> All<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentAll(value, callerMemberName, predicate, message);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ArgumentItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                            переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> All<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T> value,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [NotNull] TemplateMessageFactory<T> messageFactory,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentAll(value, callerMemberName, predicate, messageFactory);

            /// <summary>Проверка строки на null и на равенство string.Empty</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNullOrEmpty(
                [CanBeNull] string value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentNotNullOrEmpty(value, callerMemberName, message);

            /// <summary>Проверка строки на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="ArgumentNullException">Если строка == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из
            ///                     пробелов</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNullOrWhitespace(
                [CanBeNull] string value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentNotNullOrWhitespace(value, callerMemberName, message);

            /// <summary>Проверка что объект не null и не DBNull</summary>
            /// <exception cref="ArgumentNullException">Если объект == null или DBNull</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class
                => ArgumentNotNullNotDbNull(value, callerMemberName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNull<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class
                where TException : ArgumentNullException
                => ArgumentNotNull<T, TException>(value, callerMemberName, message);

            /// <summary>Проверка что объект не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Объект, который должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNull<T, TException>(
                [CanBeNull, NoEnumeration] T? value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                where TException : ArgumentNullException
                => ArgumentNotNull<T, TException>(value, callerMemberName, message);

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="ArgumentNullException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class, IEnumerable
                => ArgumentItemsNotNull(value, callerMemberName, message);

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="ArgumentNullException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsNotNull<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T?> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => ArgumentItemsNotNull(value, callerMemberName, message);

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="ArgumentNullException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void CollectionNotEmpty<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class, ICollection
                => ArgumentCollectionNotEmpty(value, callerMemberName, message);

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="ArgumentNullException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void ReadOnlyCollectionNotEmpty<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IReadOnlyCollection<T> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentReadOnlyCollectionNotEmpty(value, callerMemberName, message);

            /// <summary>Проверка что последовательность не пусто</summary>
            /// <exception cref="ArgumentNullException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если последовательность пусто</exception>
            /// <param name="value">Последовательность, которая не должна быть пуста</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void EnumerationNotEmpty<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class, IEnumerable
                => ArgumentEnumerationNotEmpty(value, callerMemberName, message);

            /// <summary>Проверка что все строки последовательности не null и не пусты</summary>
            /// <exception cref="ArgumentNullException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null или string.Empty</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void StringsNotEmpty(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<string> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentStringsNotEmpty(value, callerMemberName, message);

            /// <summary>Проверка что все строки последовательности не null, не пустые строки и не строки состоящие из одних пробелов</summary>
            /// <exception cref="ArgumentNullException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ArgumentItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ArgumentItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не
            ///                                                             содержащие ничего кроме пробелов</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null, string.Empty или заполнены
            ///                     одними только пробелами</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void StringsNotWhitespace(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<string> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentStringsNotWhitespace(value, callerMemberName, message);

            /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если коллекция содержит null</exception>
            /// <exception cref="ArgumentItemNullsNotAllowedException">Если коллекция содержит DBNull</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsNotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class, IEnumerable
                => ArgumentItemsNotNullNotDbNull(value, callerMemberName, message);

            /// <summary>Проверка условия</summary>
            /// <exception cref="ArgumentException">Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение, если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void Assert<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Argument(value, condition,
                            string.IsNullOrEmpty(message)
                                ? $"Attempt to assign {value} to {callerMemberName}"
                                : message);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение, если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="exceptionParams">Параметры, которые будут переданы в конструктор исключительной ситуации</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [NotNull, ItemCanBeNull] object[] exceptionParams)
                where TException : ArgumentException
                => Argument<T, TException>(value, condition, exceptionParams);

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение, если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             ContractAnnotation("condition:false => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where TException : ArgumentException
                => Argument<T, TException>(
                    value,
                    condition,
                    string.IsNullOrEmpty(message)
                        ? $"Attempt to assign {value} to {callerMemberName}"
                        : message);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsIs<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.ItemsIs<T>(value, callerMemberName, message);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ItemsIs<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.ItemsIs<T>(value, callerMemberName, messageFactory);

            /// <summary>Проверка, что перечисление не пусто</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если перечисление пусто</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void NotNullNotEmpty<TEnumerable>(
                [CanBeNull, NoEnumeration] TEnumerable value,
                [CanBeNull, CanBeEmpty] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where TEnumerable : class, IEnumerable
                => ArgumentNotNullNotEmpty(value, callerMemberName, message);

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="ArgumentNullException">Если value == null</exception>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             ContractAnnotation("condition:false => halt; value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void ObjectState<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.ObjectState(
                    value,
                    callerMemberName,
                    condition,
                    string.IsNullOrWhiteSpace(message)
                        ? $"Attempt to assign {value} to {callerMemberName}"
                        : message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="type">Тип значения</param>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Obsolete("Удалите передачу типа Enum, начиная c C# 7.2 он не требуется"), Pure,
             Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void EnumInRange<T>(
                [NotNull] Type type,
                T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct, Enum
                => Check.EnumInRange(value, callerMemberName, message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void EnumInRange<T>(
                T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct, Enum
                => Check.EnumInRange(value, callerMemberName, message);

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
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Obsolete("Удалите передачу типа Enum, начиная c C# 7.2 он не требуется"), Pure,
             Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("values:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void AllEnumInRange<T>(
                [NotNull] Type type,
                [CanBeNull, NoEnumeration] IEnumerable<T> values,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct, Enum
                => Check.AllEnumInRange(values, callerMemberName, message);

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="values">Список значений</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("values:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void AllEnumInRange<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> values,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct, Enum
                => Check.AllEnumInRange(values, callerMemberName, message);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="ArgumentNullException">Если объект null</exception>
            /// <exception cref="InvalidCastException">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Is<T>(
                [CanBeNull, NoEnumeration] object value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.Is<T>(value, callerMemberName, message);

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="ArgumentNullException">Если объект null</exception>
            /// <exception cref="InvalidCastException">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void Is<T>(
                [CanBeNull, NoEnumeration] object value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.Is<T>(value, callerMemberName, messageFactory);

            /// <summary>Проверка того, что файл по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="FileNotFoundException">Если файл отсутствует на диске</exception>
            /// <param name="value">Путь к файлу</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void FileExists(
                [CanBeNull] string value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.FileExists(value, callerMemberName, message);

            /// <summary>Проверка того, что папка по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="DirectoryNotFoundException">Если папка отсутствует на диске</exception>
            /// <param name="value">Путь к папке</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void DirectoryExists(
                [CanBeNull] string value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.DirectoryExists(value, callerMemberName, message);

            /// <summary>Проверка что стрим не равен null, что имеет ненулевую длину и текущая позиция не находится в конце стрима</summary>
            /// <exception cref="ArgumentNullException">Если переданный стрим == null</exception>
            /// <exception cref="StreamNotEmpty">Если длина стрима равна 0</exception>
            /// <exception cref="EndOfStreamException">Если позиция в преданном стриме находится в его конце</exception>
            /// <param name="value">Стрим</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, значение которого меняется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void StreamNotEmpty(
                [CanBeNull] Stream value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.StreamNotEmpty(value, callerMemberName, message);

            /// <summary>Проверка что строка содержит корректный Uri</summary>
            /// <exception cref="ArgumentNullException">Если строка описывающая Uri == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка описывающая Uri == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка описывающая Uri состоит только из пробелов</exception>
            /// <exception cref="InvalidUriException">Если Uri некорректен</exception>
            /// <param name="value">Строка, содержащая Uri</param>
            /// <param name="scheme">(Optional) Схема Uri которой должен соответствовать адрес. Например UriScheme.Http для Http адреса. Если
            ///                      null - схема не проверяется</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) Наименование строки</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"), ContractAnnotation("value:null => halt"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void UriCorrect(
                [CanBeNull] string value,
                UriScheme scheme = UriScheme.Any,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Check.UriCorrect(value, callerMemberName, scheme, message);

            /// <summary>Проверка что в словаре присутствует запись с переданным ключом</summary>
            /// <exception cref="ArgumentItemNotFoundException">Если ключ не найден</exception>
            /// <param name="dictionary">Словарь</param>
            /// <param name="key">Ключ, который должен присутствовать в словаре</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             ContractAnnotation("dictionary:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static void ContainsKey<TKey, TValue>(
                [CanBeNull, NoEnumeration] IReadOnlyDictionary<TKey, TValue> dictionary,
                [NotNull, NoEnumeration] TKey key,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentContainsKey(dictionary, key, callerMemberName, message);

            /*
            /// <summary>Проверка что элементы диапазона удовлетворяют переданному условию</summary>
            /// <exception cref="ArgumentItemValidationExceptionException">Если в диапазоне присутствуют элементы не удовлетворяющие
            ///                                                            переданному условию</exception>
            /// <param name="span">Диапазон</param>
            /// <param name="predicate">Условие, которому должен соответствовать каждый элемент диапазона</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void SpanAll<T>(
                Span<T> span,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => ArgumentSpanAll(span, callerMemberName, predicate, message);

            /// <summary>Проверка что диапазон не пуст</summary>
            /// <exception cref="ArgumentCollectionIsEmptyException">Если диапазон пуст</exception>
            /// <param name="span">Диапазон, который не должен быть пуст</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void SpanNotEmpty<T>(
                [NoEnumeration] Span<T> span,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => ArgumentSpanNotEmpty(span, callerMemberName, message);
            */

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsPositive(
                int value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsPositive(value, callerMemberName, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsPositive(
                long value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsPositive(value, callerMemberName, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsPositive(
                float value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsPositive(value, callerMemberName, message);

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsPositive(
                double value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsPositive(value, callerMemberName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsZeroOrPositive(
                int value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsZeroOrPositive(value, callerMemberName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsZeroOrPositive(
                long value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsZeroOrPositive(value, callerMemberName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsZeroOrPositive(
                float value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsZeroOrPositive(value, callerMemberName, message);

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsZeroOrPositive(
                double value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsZeroOrPositive(value, callerMemberName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsNegative(
                int value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsNegative(value, callerMemberName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsNegative(
                long value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsNegative(value, callerMemberName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsNegative(
                float value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsNegative(value, callerMemberName, message);

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [Conditional("DEBUG"), Conditional("FULL_CHECK"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static void IsNegative(
                double value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => ArgumentIsNegative(value, callerMemberName, message);
        }
    }
}