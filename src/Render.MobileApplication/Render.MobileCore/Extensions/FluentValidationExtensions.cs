using System;
using FluentValidation;

namespace Render.MobileCore.Extensions
{
	public static class FluentValidationExtensions
	{
		public static IRuleBuilderOptions<T, string> Numeric<T>(this IRuleBuilder<T, string> ruleBuilder){
			return ruleBuilder.Must (BeNumeric).WithMessage ("'{PropertyName}' must be a number.");
		}

		private static bool BeNumeric(string stringIn){
			return stringIn.IsNumeric ();
		}
	}
}

