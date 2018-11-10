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

            return enumeration as IEnumerable<T> ?? _ConvertAll<T>(enumeration, formatProvider);
        }

        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        private static IEnumerable<T> _ConvertAll<T>(
            [NotNull] IEnumerable enumeration,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));

            Type type = typeof(T);
            foreach (object item in enumeration)
                yield return (T)Convert.ChangeType(item, type, formatProvider);
        }

        /// <summary>Безопасная конвертация, поддерживающая например конвертацию IEnumerable из decimal в IEnumerable из long (обычный Cast выбрасывает exception).
        ///          Критично для работы с СУБД, например Oracle числа возвращает в виде decimal</summary>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        public static IReadOnlyList<TOutput> ConvertAll<T, TOutput>(
            [NotNull] this IEnumerable<T> enumeration,
            [NotNull] Converter<T, TOutput> converter)
        {
            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));
            Check.Debug.ArgumentNotNull(converter, nameof(converter));

            return enumeration as IReadOnlyList<TOutput> ??
                (enumeration is T[] asArray
                    ? (IReadOnlyList<TOutput>)Array.ConvertAll(asArray, converter)
                    : (enumeration as List<T>)?.ConvertAll(converter) ?? _ConvertAll(enumeration, converter));
        }

        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        private static List<TOutput> _ConvertAll<T, TOutput>(
            [NotNull] IEnumerable<T> enumeration,
            [NotNull] Converter<T, TOutput> converter)
        {
            const int defaultListCapacity = 16;

            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));
            Check.Debug.ArgumentNotNull(converter, nameof(converter));

            List<TOutput> result = new List<TOutput>((enumeration as IReadOnlyCollection<object>)?.Count ?? (enumeration as ICollection)?.Count ?? defaultListCapacity);
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (T item in enumeration)
                result.Add(converter(item));
            return result;
        }

        /// <summary>Безопасная конвертация, поддерживающая например конвертацию IEnumerable из decimal в IEnumerable из long (обычный Cast выбрасывает exception).
        ///          Критично для работы с СУБД, например Oracle числа возвращает в виде decimal</summary>
        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        public static IReadOnlyList<T> ConvertAll<T>(
            [NotNull] this IEnumerable enumeration,
            [NotNull] Converter<object, T> converter)
        {
            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));
            Check.Debug.ArgumentNotNull(converter, nameof(converter));

            return enumeration as IReadOnlyList<T> ??
                (enumeration is object[] asArray
                    ? (IReadOnlyList<T>)Array.ConvertAll(asArray, converter)
                    : (enumeration as List<object>)?.ConvertAll(converter) ?? _ConvertAll(enumeration, converter));
        }

        [NotNull, MethodImpl(MethodImplOptions.AggressiveInlining), DebuggerStepThrough]
        private static List<T> _ConvertAll<T>(
            [NotNull] IEnumerable enumeration,
            [NotNull] Converter<object, T> converter)
        {
            Check.Debug.ArgumentNotNull(enumeration, nameof(enumeration));
            Check.Debug.ArgumentNotNull(converter, nameof(converter));

            List<T> result = new List<T>((enumeration as IReadOnlyCollection<object>)?.Count ?? 16);
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (T item in enumeration)
                result.Add(converter(item));
            return result;
        }
    }
}