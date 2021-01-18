namespace TypeReferences.Editor.TypeDropdown
{
    using System;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    // Reduced-down and slightly refactored version of https://gist.github.com/LordJZ/92b7decebe52178a445a0b82f63e585a
    // It exposes only (T separator) overload of the Split method which was enough for my needs.
    public static class SpanExtensions
    {
        public readonly ref struct Enumerable<T>
            where T : IEquatable<T>
        {
            private readonly ReadOnlySpan<T> _span;
            private readonly T _separator;

            public Enumerable(ReadOnlySpan<T> span, T separator)
            {
                _span = span;
                _separator = separator;
            }

            [PublicAPI]
            public Enumerator<T> GetEnumerator() => new Enumerator<T>(_span, _separator);
        }

        public ref struct Enumerator<T>
            where T : IEquatable<T>
        {
            private const int SeparatorLength = 1;
            private readonly ReadOnlySpan<T> _trailingEmptyItemSentinel;

            private readonly T _separator;
            private ReadOnlySpan<T> _span;
            private ReadOnlySpan<T> _current;

            public Enumerator(ReadOnlySpan<T> span, T separator)
            {
                _span = span;
                _separator = separator;
                _current = default;
                _trailingEmptyItemSentinel = Unsafe.As<T[]>(nameof(_trailingEmptyItemSentinel)).AsSpan();

                if (_span.IsEmpty)
                    TrailingEmptyItem = true;
            }

            [PublicAPI]
            public ReadOnlySpan<T> Current => _current;

            private bool TrailingEmptyItem
            {
                get => _span == _trailingEmptyItemSentinel;
                set => _span = value ? _trailingEmptyItemSentinel : default;
            }

            [PublicAPI]
            public bool MoveNext()
            {
                if (TrailingEmptyItem)
                {
                    TrailingEmptyItem = false;
                    _current = default;
                    return true;
                }

                if (_span.IsEmpty)
                {
                    _span = _current = default;
                    return false;
                }

                int idx = _span.IndexOf(_separator);
                if (idx < 0)
                {
                    _current = _span;
                    _span = default;
                }
                else
                {
                    _current = _span.Slice(0, idx);
                    _span = _span.Slice(idx + SeparatorLength);
                    if (_span.IsEmpty)
                        TrailingEmptyItem = true;
                }

                return true;
            }
        }

        [Pure, PublicAPI]
        public static Enumerable<T> Split<T>(this ReadOnlySpan<T> span, T separator)
            where T : IEquatable<T> => new Enumerable<T>(span, separator);
    }
}