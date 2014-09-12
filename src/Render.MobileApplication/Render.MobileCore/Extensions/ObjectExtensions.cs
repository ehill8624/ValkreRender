using System;
using AutoMapper;

namespace Render.MobileCore.Extensions
{
	public static class ObjectExtensions
	{
		public static void Remap<T>(this T current, T updated){
			//Nothing to update
			if (updated == null)
				return;

			Mapper.Map<T, T>(updated, current);
		}
	}
}

