using System;
using System.Globalization;



namespace WelterKit.StaticUtilities {
   // TODO: unit test
   public static class DateTimeUtil {
      public static string FormatForSync(this DateTime dateTime) {
         return dateTime.ToString("O");
      }


      public static DateTime ParseDateTimeForSync_utc(this string dateTimeStr_utc) {
         return DateTime.ParseExact(dateTimeStr_utc, "O", DateTimeFormatInfo.InvariantInfo).ToUniversalTime();
      }
   }
}
