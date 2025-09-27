using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Godot.Common;

public static class Utils
{
	// If includedTimeSpanElements = n, the first n TimeSpanElements will be included (if they are non zero)
	// But it will at mininum display the nth included TimeSpanElement, even if it is zero
	// For example if n == 5, and all TimeSpanElements are zero, it will at least display "0ms"
	public static string BuildTimeSpanString(TimeSpan timeSpan, int includedTimeSpanElements = 5)
	{
		bool includeRemaningElements = false;
		List<(int Value, string Symbol)> TimeSpanElements =
		[
			(timeSpan.Days, "d"),
			(timeSpan.Hours, "h"),
			(timeSpan.Minutes, "m"),
			(timeSpan.Seconds, "s"),
			(timeSpan.Milliseconds, "ms"),
			(timeSpan.Microseconds, "μs"),
			(timeSpan.Nanoseconds, "ns")
		];

		List<string> strings = [];
		foreach (var (Value, Symbol) in TimeSpanElements)
		{
			if (includedTimeSpanElements == 1 || (includedTimeSpanElements > 0 && (includeRemaningElements || Value > 0)))
			{
				includeRemaningElements = true;
				strings.Add($"{Value}{Symbol}");
			}
			includedTimeSpanElements--;
		}

		return string.Join(" ", strings);
	}
}
