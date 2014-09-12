using System;

namespace Render.MobileCore.Extensions
{
	public static class StringExtensions
	{
		public delegate bool TryDelegate<T>(string s, out T result);

		public static bool IsNumeric(this string stringToValidate){
			double parsed;

			return Double.TryParse (
				stringToValidate, 
				System.Globalization.NumberStyles.Any,
				System.Globalization.NumberFormatInfo.InvariantInfo, 
				out parsed);
		}
			
		public static bool TryParseNullable<T>(this string s, out T? result, TryDelegate<T> tryDelegate) where T : struct
		{
			if (s == null)
			{
				result = null;
				return true;
			}

			T temp;
			bool success = tryDelegate(s, out temp);
			result = temp;
			return success;
		}

		public static T? ParseNullable<T>(this string s, TryDelegate<T> tryDelegate) where T : struct
		{
			if (s == null)
			{
				return null;
			}

			T temp;
			return tryDelegate(s, out temp)
				? (T?)temp
					: null;
		} 

		public static bool Contains(this string s, string innerString, StringComparison comparisonType){
			return s.IndexOf (innerString, comparisonType) >= 0;
		}
	}
}

