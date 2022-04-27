using System;

namespace CarStore.Authentication.Commons
{
    public class CarStoreDataHelper
    {
        /// <summary>
        /// Convert a datetime value to a datetime with format yyyy-MM-dd HH':'mm':'ss
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>string</returns>
        public static string GetDateTimeFormat(DateTime? dt)
        {
            if (dt == null)
            {
                return string.Empty;
            }

            try
            {
                DateTime formatDt = Convert.ToDateTime(dt);
                return formatDt.ToString("yyyy-MM-dd HH':'mm':'ss");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static readonly Random random = new();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 32).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
