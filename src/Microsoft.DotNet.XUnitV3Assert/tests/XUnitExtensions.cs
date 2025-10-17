#if XUNIT_SPAN

using System;

internal static class SpanExtensions
{
	public static Span<T> Spanify<T>(this T[]? values)
	{
		return new Span<T>(values);
	}

	public static Span<char> Spanify(this string? value)
	{
		return new Span<char>((value ?? string.Empty).ToCharArray());
	}
}

internal static class MemoryExtensions
{
	public static Memory<T> Memoryify<T>(this T[]? values) =>
		new(values);

	public static Memory<char> Memoryify(this string? value) =>
		new((value ?? string.Empty).ToCharArray());
}

#endif