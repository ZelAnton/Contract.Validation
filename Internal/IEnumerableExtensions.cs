using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Contract.Validation;

using JetBrains.Annotations;

namespace Contract.Internal
{
    // ReSharper disable once InconsistentNaming
    internal static class IEnumerableExtensions
    {
        /// <summary>Безопасная конвертация, поддерживающая например конвертацию IEnumerable из decimal в IEnumerable из long (обычный Cast выбрасывает exception).
        ///          Критично для работы с СУБД, например Oracle числа возвращает в виде decimal</summary>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        public static IEnumerable<T> ConvertAll<T>(
            [NotNull] this IEnumerable enumeration,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));

            if (enumeration is IEnumerable<T> typedResult)
                return typedResult;

            if (enumeration is ICollection collection &&
                collection.Count == 0)
                return Array.Empty<T>();

            return _ConvertAll<T>(enumeration, formatProvider);
        }

        [NotNull, ItemCanBeNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        private static IEnumerable<T> _ConvertAll<T>(
            [NotNull] IEnumerable enumeration,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));

            Type type = typeof(T);
            foreach (object item in enumeration)
                yield return (T)Convert.ChangeType(item, type, formatProvider);
        }

        /// <summary>Попытаться получиться кол-во элементов последовательности преобразовывая её к интерфейсам ICollection,
        ///          ICollection_T и IReadOnlyCollection_T</summary>
        /// <typeparam name="T">Generic type parameter</typeparam>
        /// <param name="enumeration">Последовательность</param>
        /// <param name="result">[out] Число элементов если число удалось найти, иначе -1</param>
        /// <returns>True, если число элементов удалось найти, иначе false</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining),
         CollectionAccess(CollectionAccessType.None),
         ContractAnnotation("enumeration: Null => false")]
        public static bool TryGetCount<T>(
            [CanBeNull, NoEnumeration] this IEnumerable<T> enumeration,
            out int result)
        {
            switch (enumeration)
            {
            case ICollection collection:
                result = collection.Count;
                return true;

            case ICollection<T> typedCollection:
                result = typedCollection.Count;
                return true;

            case IReadOnlyCollection<T> roCollection:
                result = roCollection.Count;
                return true;
            }

            result = -1;
            return false;
        }

        /// <summary>Безопасная конвертация, поддерживающая например конвертацию IEnumerable из decimal в IEnumerable из long (обычный Cast выбрасывает exception).
        ///          Критично для работы с СУБД, например Oracle числа возвращает в виде decimal</summary>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        public static IEnumerable<TOutput> ConvertAll<T, TOutput>(
            [NotNull] this IEnumerable<T> enumeration,
            [NotNull] Converter<T, TOutput> converter)
        {
            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));

            if (enumeration is IEnumerable<TOutput> result)
                return result;

            if (enumeration is ICollection collection &&
                collection.Count == 0)
                return Array.Empty<TOutput>();

            return _ConvertAll(enumeration, converter);
        }

        [NotNull, ItemCanBeNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        private static IEnumerable<TOutput> _ConvertAll<T, TOutput>(
            [NotNull] IEnumerable<T> enumeration,
            [NotNull] Converter<T, TOutput> converter)
        {
            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));
            Check.Debug.ArgumentNotNull(converter, nameof(converter));

            foreach (T item in enumeration)
                yield return converter(item);
        }

        /// <summary>Безопасная конвертация, поддерживающая например конвертацию IEnumerable из decimal в IEnumerable из long (обычный Cast выбрасывает exception).
        ///          Критично для работы с СУБД, например Oracle числа возвращает в виде decimal</summary>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        public static IEnumerable<T> ConvertAll<T>(
            [NotNull] this IEnumerable enumeration,
            [NotNull] Converter<object, T> converter)
        {
            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));

            if (enumeration is IEnumerable<T> result)
                return result;

            if (enumeration is ICollection collection &&
                collection.Count == 0)
                return Array.Empty<T>();

            return _ConvertAll(enumeration, converter);
        }

        [NotNull, ItemCanBeNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        private static IEnumerable<T> _ConvertAll<T>(
            [NotNull] IEnumerable enumeration,
            [NotNull] Converter<object, T> converter)
        {
            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));
            Check.Debug.ArgumentNotNull(converter, nameof(converter));

            IEnumerator enumerator = enumeration.GetEnumerator();
            while (enumerator.MoveNext())
                yield return converter(enumerator.Current);
        }
    }
}