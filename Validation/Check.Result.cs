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
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Contract.Validation
{
    /// <summary>Runtime валидация условий</summary>
    public abstract partial class Check
    {
        /// <summary>Валидация условий для возвращаемых методами значений. В DEBUG билде проверки осуществляются, иначе - нет</summary>
        public abstract class Result
        {
            /// <summary>Производить ли проверку значений, возвращаемых методами</summary>
            public static bool Enabled
#if DEBUG || FULL_CHECK
                = true;
#else
                = false;
#endif

            /// <summary>Проверка что строка, являющаяся результатом выполнения метода, содержит guid</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="FormatException">Если строка не содержит GUID</exception>
            /// <param name="guid">Строка, которая должна содержать Guid</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [NotNull, GuidStr, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string IsGuid(
                [CanBeNull] string guid,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsGuid(guid, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : guid;

            /// <summary>Проверка что строка, являющаяся результатом выполнения метода, не содержит непустой guid</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <exception cref="FormatException">Если строка не содержит GUID</exception>
            /// <exception cref="ValueEmptyException">Если GUID пуст</exception>
            /// <param name="guid">Строка, которая должна содержать непустой Guid</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [NotNull, NotEmptyGuid, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string GuidNotEmpty(
                [CanBeNull] string guid,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.ArgumentGuidNotEmpty(guid, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : guid;

            /// <summary>Проверка что guid являющийся результатом выполнения метода не пуст</summary>
            /// <exception cref="ValueEmptyException">Если guid == Guid.Empty</exception>
            /// <param name="guid">Guid, который не должен быть равен Guid.Empty</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Guid GuidNotEmpty(
                Guid guid,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.GuidNotEmpty(
                        guid, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : guid;

            /// <summary>Проверка что результат выполнения метода не null</summary>
            /// <exception cref="NullReferenceException">Если значение, присваиваемое свойству null</exception>
            /// <param name="value">Значение, которое пытаются присвоить свойству</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T NotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class
                => Enabled
                    ? Check.NotNull(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что результат выполнения метода не null</summary>
            /// <exception cref="NullReferenceException">Если значение, присваиваемое свойству null</exception>
            /// <param name="value">Значение, которое пытаются присвоить свойству</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода,
            ///     результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static T NotNull<T>(
                [CanBeNull, NoEnumeration] T? value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => Enabled
                    ? Check.NotNull(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : value.Value;

            /// <summary>Проверка что результат выполнения метода не null</summary>
            /// <exception cref="NullReferenceException">Если переданное значение == null</exception>
            /// <param name="value">Объект, который не должен быть равен null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода,
            ///     результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt; => NotNull"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static T GenericNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.GenericNotNull(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : value;


            /// <summary>Проверка что результат выполнения метода не равен значению по-умолчанию для типа T</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ValueNotEmpty<T>(
                T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => Enabled
                    ? Check.ValueNotEmpty(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что значение не пусто (пустое значение отличается от default)</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue">Пустое значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ValueNotEmpty<T>(
                T value,
                T emptyValue,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => Enabled
                    ? Check.ValueNotEmpty(value, emptyValue,
                                          callerMemberName != null
                                              ? $"result of {callerMemberName}"
                                              : "result", message)
                    : value;

            /// <summary>Проверка что значение не пусто (пустое значение отличается от default)</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == default(T)</exception>
            /// <param name="value">Значение, которое не должно быть равно значению по-умолчанию для своего типа</param>
            /// <param name="emptyValue1">Пустое значение</param>
            /// <param name="emptyValue2">Пустое значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ValueNotEmpty<T>(
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
                => Enabled
                    ? Check.ValueNotEmpty(value, emptyValue1, emptyValue2,
                                          callerMemberName != null
                                              ? $"result of {callerMemberName}"
                                              : "result", message)
                    : value;

            /// <summary>Проверка что значение IntPtr не пусто (пустое значение отличается от IntPtr.Zero)</summary>
            /// <exception cref="ValueEmptyException">Если аргумент == IntPtr.Zero</exception>
            /// <param name="value">Значение, которое не должно быть равно IntPtr.Zero</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IntPtr ValueNotEmpty(
                IntPtr value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.ValueNotEmpty(value,
                                          callerMemberName != null
                                              ? $"result of {callerMemberName}"
                                              : "result", message)
                    : value;

            /// <summary>Проверка попадания значения в допустимый диапазон</summary>
            /// <exception cref="ValueOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
            /// <param name="value">Значение, которое должно попадать в допустимый диапазон</param>
            /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("condition: false => halt; value:null => null"),
             NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T InRange<T>(
                [CanBeNull] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.InRange(value, condition,
                                    callerMemberName != null ? $"result of {callerMemberName}" : "result",
                                    message)
                    : value;

            /// <summary>Проверка попадания значения в допустимый диапазон</summary>
            /// <exception cref="ValueOutOfRangeException">Если значение выходит за рамки допустимого диапазона значений</exception>
            /// <param name="value">Значение, которое должно попадать в допустимый диапазон</param>
            /// <param name="condition">Внешний метод проверки попадания значения в допустимый диапазон</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("condition: false => halt; value:null => null"),
             NotEmpty, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T InRange<T>(
                [CanBeNull] T value,
                [NotNull, InstantHandle] Func<bool> condition,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.InRange(value, condition,
                                    callerMemberName != null ? $"result of {callerMemberName}" : "result",
                                    message)
                    : value;

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ValueOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int IndexInRange(
                int index,
                int count,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IndexInRange(index, count,
                                         callerMemberName != null ? $"result of {callerMemberName}" : "result",
                                         message)
                    : index;

            /// <summary>Проверка того, что индекс не выходит за пределы коллекции</summary>
            /// <exception cref="ValueOutOfRangeException">Если индекс выходит за пределы коллекции</exception>
            /// <param name="index">Значение индекса</param>
            /// <param name="count">Число элементов коллекции</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ZeroOrPositiveNumber, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long IndexInRange(
                long index,
                long count,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IndexInRange(index, count,
                                         callerMemberName != null ? $"result of {callerMemberName}" : "result",
                                         message)
                    : index;

            /// <summary>Проверка перечисление на отсутствие значений по-умолчанию</summary>
            /// <exception cref="ValueEmptyException">Если в перечислении присутствует значение == default(T)</exception>
            /// <param name="value">Перечисление значений, которые не должны быть равны значению по-умолчанию для своего типа</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ValuesNotEmpty<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => Enabled
                    ? Check.ValuesNotEmpty(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка строки на null и на равенство string.Empty</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string NotNullOrEmpty(
                [CanBeNull] string value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.NotNullOrEmpty(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка строки на null и на равенство string.Empty или состоять только из пробелов</summary>
            /// <exception cref="NullReferenceException">Если строка == null</exception>
            /// <exception cref="EmptyStringNotAllowedException">Если строка == string.Empty</exception>
            /// <exception cref="WhitespaceNotAllowedException">Если строка состоит только из пробелов</exception>
            /// <param name="value">Строковый аргумент, который не должен быть равен null или string.Empty, или состоять только из пробелов</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotWhitespace,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string NotNullOrWhitespace(
                [CanBeNull] string value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.NotNullOrWhitespace(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что значение, присваиваемое свойству не null и не DBNull</summary>
            /// <exception cref="NullReferenceException">Если значение == null или DBNull</exception>
            /// <param name="value">Значение, которое пытаются присвоить свойству</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T NotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class
                => Enabled
                    ? Check.NotNullNotDbNull(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что значение, присваиваемое свойству не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Значение, которое должно быть не null</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T NotNull<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class
                where TException : NullReferenceException
                => Enabled
                    ? Check.NotNull<T, TException>(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что значение, присваиваемое свойству не null</summary>
            /// <exception><cref>TException</cref>Если условие не выполняется</exception>
            /// <param name="value">Значение, которое должно быть не null</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), MethodImpl(MethodImplOptions.AggressiveInlining),
             DebuggerHidden]
            public static T NotNull<T, TException>(
                [CanBeNull, NoEnumeration] T? value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                where TException : NullReferenceException
                => Enabled
                    ? Check.NotNull<T, TException>(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    // ReSharper disable once PossibleInvalidOperationException
                    : value.Value;

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ItemsNotNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class, IEnumerable
                => Enabled
                    ? Check.ItemsNotNull(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что элементы последовательности не null</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют элементы равные null</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ItemsNotNull<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<T?> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => Enabled
                    ? Check.ItemsNotNull(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : ConvertAll<T>(value);

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T CollectionNotEmpty<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class, ICollection
                => Enabled
                    ? Check.CollectionNotEmpty(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IReadOnlyCollection<T> ReadOnlyCollectionNotEmpty<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IReadOnlyCollection<T> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.ReadOnlyCollectionNotEmpty(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что коллекция не пуста</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если коллекция пуста</exception>
            /// <param name="value">Коллекция, которая не должна быть пуста</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static TCollection ReadOnlyCollectionNotEmpty<TCollection, T>(
                [CanBeNull, NoEnumeration] TCollection value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where TCollection : class, IReadOnlyCollection<T>
                => Enabled
                    ? Check.ReadOnlyCollectionNotEmpty<TCollection, T>(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что последовательность не пусто</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="CollectionIsEmptyException">Если последовательность пусто</exception>
            /// <param name="value">Последовательность, которая не должна быть пуста</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T EnumerationNotEmpty<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class, IEnumerable
                => Enabled
                    ? Check.EnumerationNotEmpty(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что все строки последовательности не null и не пусты</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null или string.Empty</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<string> StringsNotEmpty(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<string> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.StringsNotEmpty(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что все строки последовательности не null, не пустые строки и не строки состоящие из одних пробелов</summary>
            /// <exception cref="NullReferenceException">Если <see cref="value"/> == null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если в последовательности присутствуют строки равные null</exception>
            /// <exception cref="ItemEmptyStringNotAllowedException">Если в последовательности присутствуют пустые строки</exception>
            /// <exception cref="ItemWhitespaceNotAllowedException">Если в последовательности присутствуют строки не содержащие
            ///                                                     ничего кроме пробелов</exception>
            /// <param name="value">Последовательность строк, которые быть не должны быть равны null, string.Empty
            ///                     или заполнены одними только пробелами</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotWhitespace,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<string> StringsNotWhitespace(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable<string> value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.StringsNotWhitespace(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что элементы коллекции не null и не DBNull</summary>
            /// <exception cref="ItemNullsNotAllowedException">Если коллекция содержит null</exception>
            /// <exception cref="ItemNullsNotAllowedException">Если коллекция содержит DBNull</exception>
            /// <param name="value">Коллекция, элементы которой должен быть не null</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull, ItemNotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ItemsNotNullNotDbNull<T>(
                [CanBeNull, NoEnumeration] T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : class, IEnumerable
                => Enabled
                    ? Check.ItemsNotNullNotDbNull(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка условия</summary>
            /// <exception cref="Exception">Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение, если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("condition:false => halt; value:null => null"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Assert<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.Assert(value, condition,
                                   string.IsNullOrEmpty(message)
                                       ? $"result of {callerMemberName} assertion"
                                       : message)
                    : value;

            /// <summary>Проверка условия</summary>
            /// <exception><cref>TException</cref>: Если условие не выполняется</exception>
            /// <param name="value">Возвращаемое значение, если проверка пройдена</param>
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
            /// <param name="value">Возвращаемое значение, если проверка пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("condition:false => halt; value:null => null"),
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Assert<T, TException>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where TException : Exception
                => Enabled
                    ? Check.Assert<T, TException>(value, condition,
                                                  string.IsNullOrEmpty(message)
                                                      ? $"result of {callerMemberName} assertion"
                                                      : message)
                    : value;

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ItemsIs<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.ItemsIs<T>(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : ConvertAll<T>(value);

            /// <summary>Проверка того, что все элементы последовательности являются объектами нужного типа</summary>
            /// <exception cref="ArgumentNullException">Если перечисление равно null</exception>
            /// <exception cref="InvalidCastException">Если обнаружен элемент не являющийся объектом нужного типа</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="messageFactory">Метод-фабрика сообщений об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, ItemNotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> ItemsIs<T>(
                [CanBeNull, ItemCanBeNull, NoEnumeration] IEnumerable value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.ItemsIs<T>(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : null,
                        messageFactory)
                    : ConvertAll<T>(value);

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                    переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
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
                => Enabled
                    ? Check.All(value, callerMemberName != null ? $"result of {callerMemberName}" : null,
                                predicate, message)
                    : value;

            /// <summary>Условие, которое должно выполняться для всех элементов перечисления</summary>
            /// <exception cref="ArgumentNullException">Если перечисление или условие проверки элемента равно null</exception>
            /// <exception cref="ItemValidationExceptionException">Если для какого-нибудь элемента перечисления не выполнится
            ///                                                    переданное условие</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="predicate">Условие</param>
            /// <param name="messageFactory">Метод-конструктор сообщения об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
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
                => Enabled
                    ? Check.All(value, callerMemberName != null ? $"result of {callerMemberName}" : null,
                                predicate, messageFactory)
                    : value;

            /// <summary>Проверка, что перечисление не пусто</summary>
            /// <exception cref="NullReferenceException">Если перечисление равно null</exception>
            /// <exception cref="CollectionIsEmptyException">Если перечисление пусто</exception>
            /// <param name="value">Коллекция</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static TEnumerable NotNullNotEmpty<TEnumerable>(
                [CanBeNull, NoEnumeration] TEnumerable value,
                [CanBeNull, CanBeEmpty] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where TEnumerable : class, IEnumerable
                => Enabled
                    ? Check.NotNullNotEmpty(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка состояния объекта (значений полей/свойств)</summary>
            /// <exception cref="NullReferenceException">Если value == null</exception>
            /// <exception cref="InvalidOperationException">Если condition == false</exception>
            /// <param name="value">Значение, которое будет возвращено, если проверка будет пройдена</param>
            /// <param name="condition">Условие, которое должно быть true</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("condition:false => halt; value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T ObjectState<T>(
                [CanBeNull, NoEnumeration] T value,
                [NotNull, InstantHandle] Func<T, bool> condition,
                [CanBeNull, InvokerParameterName] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.ObjectState(
                        value,
                        callerMemberName != null ? $"result of {callerMemberName}" : null,
                        condition,
                        string.IsNullOrWhiteSpace(message)
                            ? $"result of {callerMemberName} invalid operation"
                            : message)
                    : value;

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="type">Тип значения</param>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [Obsolete("Удалите передачу типа Enum, начиная c C# 7.2 он не требуется"), Pure,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T EnumInRange<T>(
                [NotNull] Type type,
                T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct, Enum
                => Enabled
                    ? Check.EnumInRange(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T EnumInRange<T>(
                T value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct, Enum
                => Enabled
                    ? Check.EnumInRange(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
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

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="type">Тип значения</param>
            /// <param name="values">Список значений</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [Obsolete("Удалите передачу типа Enum, начиная c C# 7.2 он не требуется"), Pure,
             ContractAnnotation("values:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> AllEnumInRange<T>(
                [NotNull] Type type,
                [CanBeNull, NoEnumeration] IEnumerable<T> values,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct, Enum
                => Enabled
                    ? Check.AllEnumInRange(
                        values, callerMemberName != null ? $"result of {callerMemberName}" : "result",
                        message)
                    : values;

            /// <summary>Проверка того, что значение является допустимым для данного типа перечня (enum)</summary>
            /// <exception cref="InvalidEnumArgumentException">Если значение является недопустимым</exception>
            /// <param name="values">Список значений</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("values:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IEnumerable<T> AllEnumInRange<T>(
                [CanBeNull, NoEnumeration] IEnumerable<T> values,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct, Enum
                => Enabled
                    ? Check.AllEnumInRange(
                        values, callerMemberName != null ? $"result of {callerMemberName}" : "result",
                        message)
                    : values;

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="InvalidCastException">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Is<T>(
                [CanBeNull, NoEnumeration] object value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.Is<T>(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : (T) value;

            /// <summary>Проверка типа объекта, выбрасывает исключительную ситуацию если проверка не пройдена</summary>
            /// <exception cref="NullReferenceException">Если объект null</exception>
            /// <exception cref="InvalidCastException">Если тип переданного объекта не <see cref="T" /></exception>
            /// <param name="value">Проверяемый объект</param>
            /// <param name="messageFactory">Внешняя ф-ия получения сообщения об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            [ContractAnnotation("value:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static T Is<T>(
                [CanBeNull, NoEnumeration] object value,
                [NotNull, InstantHandle] ObjectMessageFactory messageFactory,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.Is<T>(value, callerMemberName != null ? $"result of {callerMemberName}" : null,
                                  messageFactory)
                    : (T) value;

            /// <summary>Проверка того, что файл по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="FileNotFoundException">Если файл отсутствует на диске</exception>
            /// <param name="value">Путь к файлу</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            /// <returns>Путь к файлу</returns>
            [ContractAnnotation("value:null => halt"), NotNull, FileExists, NotWhitespace,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string FileExists(
                [CanBeNull] string value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.FileExists(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что папка по указанному пути существует на диске</summary>
            /// <exception cref="ArgumentNullException">Если указанный путь == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если указанный путь == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если указанный путь состоит только из пробелов</exception>
            /// <exception cref="DirectoryNotFoundException">Если папка отсутствует на диске</exception>
            /// <param name="value">Путь к папке</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            /// <returns>Путь к папке</returns>
            [ContractAnnotation("value:null => halt"), NotNull, DirectoryExists, NotWhitespace,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string DirectoryExists(
                [CanBeNull] string value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.DirectoryExists(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что стрим не равен null, что имеет ненулевую длину и текущая позиция не находится в конце стрима</summary>
            /// <exception cref="ArgumentNullException">Если переданный стрим == null</exception>
            /// <exception cref="StreamNotEmpty">Если длина стрима равна 0</exception>
            /// <exception cref="EndOfStreamException">Если позиция в преданном стриме находится в его конце</exception>
            /// <param name="value">Стрим</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование метода, результат работы которого контролируется</param>
            /// <returns>Стрим</returns>
            [ContractAnnotation("value:null => halt"), NotNull, NotEmpty,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Stream StreamNotEmpty(
                [CanBeNull] Stream value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.StreamNotEmpty(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка что строка содержит корректный Uri</summary>
            /// <exception cref="ArgumentNullException">Если строка описывающая Uri == null</exception>
            /// <exception cref="ArgumentEmptyStringNotAllowedException">Если строка описывающая Uri == string.Empty</exception>
            /// <exception cref="ArgumentWhitespaceNotAllowedException">Если строка описывающая Uri состоит только из пробелов</exception>
            /// <exception cref="InvalidUriException">Если Uri некорректен</exception>
            /// <param name="value">Строка, содержащая Uri</param>
            /// <param name="scheme">(Optional) Схема Uri которой должен соответствовать адрес. Например UriScheme.Http для Http
            ///                      адреса. Если null - схема не проверяется</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) Наименование строки</param>
            /// <returns>Строка, содержащая Uri</returns>
            [ContractAnnotation("value:null => halt"), NotNull, NotWhitespace,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static string UriCorrect(
                [CanBeNull] string value,
                UriScheme scheme = UriScheme.Any,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.UriCorrect(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : null, scheme,
                        message)
                    : value;

            /// <summary>Проверка что в словаре присутствует запись с переданным ключом</summary>
            /// <exception cref="ArgumentItemNotFoundException">Если ключ не найден</exception>
            /// <param name="dictionary">Словарь</param>
            /// <param name="key">Ключ, который должен присутствовать в словаре</param>
            /// <param name="message">(Optional) Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование метода, результат работы которого
            ///                                контролируется</param>
            [ContractAnnotation("dictionary:null => halt"), NotNull,
             MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static IReadOnlyDictionary<TKey, TValue> ContainsKey<TKey, TValue>(
                [CanBeNull, NoEnumeration] IReadOnlyDictionary<TKey, TValue> dictionary,
                [NotNull, NoEnumeration] TKey key,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.ContainsKey(dictionary, key,
                                        message ?? (callerMemberName != null
                                            ? $"result of {callerMemberName} not contains key {key}"
                                            : null))
                    : dictionary;

            /*
            /// <summary>Проверка что элементы диапазона удовлетворяют переданному условию</summary>
            /// <exception cref="ItemValidationExceptionException">Если в диапазоне присутствуют элементы не удовлетворяющие
            ///                                                    переданному условию</exception>
            /// <param name="span">Диапазон</param>
            /// <param name="predicate">Условие, которому должен соответствовать каждый элемент диапазона</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Optional) (Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Span<T> SpanAll<T>(
                Span<T> span,
                [NotNull, InstantHandle] Func<T, bool> predicate,
                [CanBeNull] string message,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => Enabled
                    ? Check.SpanAll(span, callerMemberName != null ? $"result of {callerMemberName}" : null,
                                    predicate, message)
                    : span;

            /// <summary>Проверка что диапазон не пуст</summary>
            /// <exception cref="CollectionIsEmptyException">Если диапазон пуст</exception>
            /// <param name="span">Диапазон, который не должен быть пуст</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static Span<T> SpanNotEmpty<T>(
                [NoEnumeration] Span<T> span,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                where T : struct
                => Enabled
                    ? Check.SpanNotEmpty(
                        span, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : span;
            */

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int IsPositive(
                int value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsPositive(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long IsPositive(
                long value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsPositive(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static float IsPositive(
                float value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsPositive(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static double IsPositive(
                double value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsPositive(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int IsZeroOrPositive(
                int value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsZeroOrPositive(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long IsZeroOrPositive(
                long value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsZeroOrPositive(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static float IsZeroOrPositive(
                float value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsZeroOrPositive(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение равно или больше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение меньше нуля</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static double IsZeroOrPositive(
                double value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsZeroOrPositive(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static int IsNegative(
                int value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsNegative(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static long IsNegative(
                long value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsNegative(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static float IsNegative(
                float value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsNegative(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;

            /// <summary>Проверка того, что значение меньше нуля</summary>
            /// <exception cref="ValueOutOfRangeException">Если переданное значение больше или равно нулю</exception>
            /// <param name="value">Значение</param>
            /// <param name="message">Сообщение об ошибке</param>
            /// <param name="callerMemberName">(Заполняется компилятором) Наименование свойства, чьё значение изменяется</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerHidden]
            public static double IsNegative(
                double value,
                [CanBeNull] string message = null,
#if DEBUG || FULL_CHECK
                [CallerMemberName]
#endif
                [CanBeNull]
                string callerMemberName = null)
                => Enabled
                    ? Check.IsNegative(
                        value, callerMemberName != null ? $"result of {callerMemberName}" : "result", message)
                    : value;
        }
    }
}