using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMVP.Domain.Core;

public static class Utils
{
	/// <summary>
	/// Creates an array of random non-repeating numbers from a specified range.
	/// </summary>
	/// <param name="min">Start of the range (included).</param>
	/// <param name="max">End of the range (included).</param>
	/// <param name="count">Amount of random numbers to return.</param>
	/// <param name="exclusions">Numbers from the range to be excluded from beaing able to be randomly chosen.</param>
	/// <returns>Array of the random numbers.</returns>
	/// <exception cref="ArgumentException"></exception>
	/// <remarks>
	/// The sum of <paramref name="count"/> and <paramref name="exclusions"/> cannot exceed the total amount of numbers from the range.
	/// </remarks>
	public static int[] RandomUniqueNumbers(int min, int max, int count, int[]? exclusions = null)
	{
		exclusions ??= [];

		// CONSIDER: Aren't these checks too much?
		int candidateCount = max - min + 1;
		if (count < 0) throw new ArgumentException("Count has to be positive.", nameof(count));
		if (count > candidateCount) throw new ArgumentException("Count cannot be greater than the range size.", nameof(count));
		foreach (var exclusion in exclusions)
		{
			if (exclusion < min || max < exclusion)
			{
				throw new ArgumentException($"Exclusion '{exclusion}' is outside of range '<{min}, {max}>'", nameof(exclusions));
			}
		}
		if (count > candidateCount - exclusions.Length) throw new ArgumentException("Too many exclusions.", nameof(exclusions));

		int[] numbers;
		if (exclusions != null)
		{
			numbers = new int[candidateCount - exclusions.Length];
			int i = 0;
			foreach (int number in LinearSequence(min, 1, max))
			{
				if (exclusions.Contains(number)) continue; // Do not increment i
				numbers[i++] = number;
			}
		}
		else numbers = LinearSequence(min, 1, max).ToArray();

		Shuffle(numbers);
		return numbers[0..count];
	}

	/// <summary>
	/// Shuffles an array.
	/// </summary>
	/// <param name="array">Array of numbers to be shuffled.</param>
	/// <remarks>
	/// The <paramref name="array"/> is modified.<br/>
	/// This method is using Fisher–Yates shuffle algorithm.
	/// </remarks>
	public static void Shuffle(int[] array)
	{
		Random rng = new();
		for (int i = array.Length - 1; i > 0; i--)
		{
			int j = rng.Next(0, i + 1);
			(array[i], array[j]) = (array[j], array[i]);
		}
	}

	/// <summary>
	/// Creates a linear sequence.
	/// </summary>
	/// <param name="start">Start of the sequence.</param>
	/// <param name="step">Sequence step.</param>
	/// <param name="end">End of the sequence.</param>
	/// <returns>Lazy linear sequence.</returns>
	/// <remarks>
	/// The <paramref name="end"/> is not reached if <paramref name="step"/> would overshoot it.<br/>
	/// If <paramref name="end"/> is not given, <c>int.MaxValue</c> or <c>int.MinValue</c> is chosen based the monotonicity of the sequence.<br/>
	/// The monotonicity is based on <paramref name="step"/> being positive/negative.
	/// </remarks>
	/// <example>
	/// <code>
	/// Utils.NumberSequence();			//Output: [0,1,2, ... ,int.MaxValue]
	/// Utils.NumberSequence(-3, 3, 7); //Output: [-3,0,3,6] (end is not reached)
	/// Utils.NumberSequence(0, 1, -1); //Output: [] (when <paramref name="end"/> is smaller than <paramref name="start"/> while sequence being increasingly monotonic, based on <paramref name="step"/>)
	/// Utils.NumberSequence(5, 0);		//Output: [5,5,5, ...]	(when <c>step == 0</c>)
	/// </code>
	/// </example>
	public static IEnumerable<int> LinearSequence(int start = 0, int step = 1, int? end = null)
	{
		// By having user define 'end' instead of number of sequence items (like in LinearSequenceByItemCount),
		// the method doesn't have to check for the sequence number overflowing int32
		end ??= step > 0 ? int.MaxValue : int.MinValue;

		int num = start;
		while (step > 0 ? num <= end : num >= end) // Special case when step is 0 does not need to be accomodated
		{
			yield return num;
			num += step;
		}
	}

	[Obsolete("LinearSequenceByItemCount is deprecated, please use LinearSequence instead.")]
	public static IEnumerable<int> LinearSequenceByItemCount(int start = 0, int step = 1, int itemCount = int.MaxValue)
	{
		if (itemCount < 0) throw new ArgumentException("Item count has to be positive.", nameof(itemCount));

		int num = start;
		for (int i = 0; i < itemCount; i++)
		{
			yield return num;

			if (step > 0 && int.MaxValue - step <= num || step < 0 && int.MinValue + step >= num)
				throw new OverflowException("Sequence overflowed the bounds of Int32.");

			num += step;
		}
	}

	/// <summary>
	/// Creates all permutations with repetition from a number set.
	/// </summary>
	/// <param name="numberSet">Array of numbers representing the set.</param>
	/// <param name="permutationLength">Length of the permutations.</param>
	/// <returns>Lazy sequence of permutations.</returns>
	/// <remarks>
	/// The uniqueness of the numbers in <paramref name="numberSet"/> array is not tested.
	/// </remarks>
	/// <example>
	/// <code>
	/// Utils.PermutationsRep([0, 1], 4); // Output: digits of all binary numbers with 4 digits
	/// </code>
	/// </example>
	public static IEnumerable<int[]> PermutationsRep(int[] numberSet, int permutationLength)
	{
		int[] permutation = new int[permutationLength];
		int[] setIndexes = new int[permutationLength];
		do
		{
			for (int i = 0; i < permutationLength; i++)
			{
				permutation[i] = numberSet[setIndexes[i]];
			}
			yield return (int[])permutation.Clone(); // Cloning to avoid reference issues
		}
		while (IncDigits(setIndexes, numberSet.Length - 1));
	}

	/// <summary>
	/// Increments a number represented by an array of digits.
	/// </summary>
	/// <param name="digits">Array of digits.</param>
	/// <param name="maxDigitValue">Tha maximum value of a digit.</param>
	/// <returns>False, if overflow happened, else True.</returns>
	/// <remarks>
	/// The <paramref name="digits"/> array is modified.
	/// </remarks>
	/// <example>
	/// <code>
	/// Utils.IncDigits({0,0,0}, 1) // Output: True (sets digits to [0,0,1] ... binary, base 2) 
	/// Utils.IncDigits({0,0,9}, 9) // Output: True (sets digits to [0,1,0] ... decimal, base 10) 
	/// Utils.IncDigits({11,11,11}, 11) // Output: False (sets digits to [0,0,0] ... duodecimal, base 12)
	/// </code>
	///	</example>
	public static bool IncDigits(int[] digits, int maxDigitValue)
	{
		for (int i = digits.Length - 1; i >= 0; i--)
		{
			digits[i] = (digits[i] + 1) % (maxDigitValue + 1);
			if (digits[i] == 0) continue;
			return true;
		}
		return false; // Overflow
	}
}
