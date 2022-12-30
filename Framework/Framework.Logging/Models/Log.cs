﻿using System;
using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;
using Framework.Logging.Enums;
using Framework.Logging.Interfaces;

namespace Framework.Logging.Models
{
	public class Log : object, ILog
	{
		public Log() : base()
		{
			TimeStamp =
				DateTimeOffset.UtcNow
					.ToUnixTimeMilliseconds();

			Exceptions = new();
		}

		public LogLevel Level { get; set; }

		public long TimeStamp { get; set; }

		public string Namespace { get; set; }

		public string ClassName { get; set; }

		public string MethodName { get; set; }



		public string RemoteIP { get; set; }

		public string Username { get; set; }

		public string RequestPath { get; set; }

		public string HttpReferrer { get; set; }



		public string Message { get; set; }

		public Hashtable Parameters { get; set; }

		public ExceptionModel Exceptions { get; set; }
		public string TraceId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override string ToString()
		{
			var options = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder
					.UnsafeRelaxedJsonEscaping
			};
			return JsonSerializer.
			Serialize(this, options);
		}

	}
}