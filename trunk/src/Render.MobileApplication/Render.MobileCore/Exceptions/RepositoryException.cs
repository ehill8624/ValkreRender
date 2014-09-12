using System;

namespace Render.MobileCore.Exceptions
{

	public enum RepositoryExceptionType{
		General = 0,

		InvalidIdentifier = 1000,


		Unknown = 9999
	}

	public class RepositoryException : Exception
	{

		public RepositoryExceptionType RepositoryExceptionType {
			get;
			set;
		}

		public RepositoryException (string message, RepositoryExceptionType exceptionType) : base(message)
		{
			RepositoryExceptionType = exceptionType;
		}
	}
}

