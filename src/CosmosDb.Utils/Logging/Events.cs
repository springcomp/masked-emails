using Microsoft.Extensions.Logging;

namespace CosmosDb.Utils.Logging {
	internal sealed class Templates {
		public const string RequestChargeOperation = "Message={Message}, RequestCharge={RequestCharge}";
	}

	internal sealed class Events {
		private static readonly EventId trace_ = new EventId(0, "trace");

		public static EventId Trace => trace_;
	}
}