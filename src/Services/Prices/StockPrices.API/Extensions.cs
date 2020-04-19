using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TradingBot.Services.StockPrices.API
{
	public static class Extensions
	{
        public static void Forget(this Task task)
        {
            // Only care about tasks that may fault or are faulted,
            // so fast-path for SuccessfullyCompleted and Canceled tasks
            if (!task.IsCompleted || task.IsFaulted)
            {
                _ = ForgetAwaited(task);
            }

        }

        private static async Task ForgetAwaited(Task task)
        {
            try
            {
                // No need to resume on the original SynchronizationContext
                await task.ConfigureAwait(false);
            }
            catch(Exception e)
            {
                // Nothing to do here
            }
        }

        public static string Sha256Hash(this string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
