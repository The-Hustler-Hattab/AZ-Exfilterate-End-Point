


using System.Globalization;

namespace Exfilterate.utilites
{
    internal static class DataOperationsUtilites
    {
        public static string PrependDateToString(string inputString)
        {
            // Get the current date and time in the desired format
            string timestamp = DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss");

            // Concatenate the timestamp with the input string
            string result = timestamp + "-" + inputString;

            return result;
        }

        public static string GetCurrentDateAsEasternTime()
        {
            // Get the current UTC date and time
            DateTime utcNow = DateTime.UtcNow;

            // Convert UTC to Eastern Time
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, easternZone);

            // Convert the date and time to an ISO 8601 string
            string dateAsString = easternTime.ToString("o", CultureInfo.InvariantCulture);

            return dateAsString;
        }


    }
}
