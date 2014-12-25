using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class RandomExtension
{
	#region Random.Range with excluded range
	private static int[] CollectAllowedElements (int[] allElements, int[] excludedElements)
	{
		var allowedElements = new List<int>();
		foreach (int element in allElements)
			if (!excludedElements.Contains(element))
				allowedElements.Add(element);
		return allowedElements.ToArray();
	}

	private static int SelectRandomElement (int[] allowedElements)
	{
		int randomIndex = UnityEngine.Random.Range(0, allowedElements.Length);
		return allowedElements[randomIndex];
	}

	public static int RangeExcluded (int min, int max, int[] excludedElements)
	{
		int[] allElements = Enumerable.Range(min, max - min + 1).ToArray();
		int[] allowedElements = CollectAllowedElements(allElements, excludedElements);
		return SelectRandomElement(allowedElements);
	}
	#endregion

	#region random element from List<T>
	private static IList<T> CollectAllowedElements<T> (IList<T> allElements, IList<T> excludedElements)
	{
		var allowedElements = new List<T>();
		foreach (T element in allElements)
			if (!excludedElements.Contains(element))
				allowedElements.Add(element);
		return allowedElements;
	}

	private static T SelectRandomElement<T> (IList<T> allowedElements)
	{
		int randomIndex = UnityEngine.Random.Range(0, allowedElements.Count);
		return allowedElements[randomIndex];
	}

	public static T Random<T> (this IList<T> allElements, IList<T> excludedElements = null)
	{
		IList<T> allowedElements = excludedElements == null ? allElements : CollectAllowedElements(allElements, excludedElements);
		return SelectRandomElement(allowedElements);
	}
	#endregion
}