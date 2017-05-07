using System;
using System.Runtime.Serialization;

namespace Combine
{
	[Serializable]
	class PlaceFoundException : Exception
	{
		public PlaceFoundException()
		{
		}

		public PlaceFoundException(string message) : base(message)
		{
		}

		public PlaceFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected PlaceFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}