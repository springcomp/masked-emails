using Microsoft.Extensions.Logging;

namespace CosmosDb.Utils.Logging {
	internal static class LoggingExtensions {
		public static void TraceRequestCharge(this ILogger logger, string message, double requestCharge)
		{
			logger.LogTrace(
				Events.Trace,
				Templates.RequestChargeOperation,
				new object[]{
					message,
					requestCharge,
				}
			);
		}
	}
}