using System;
using System.Collections.Generic;
using System.Reflection;
using WelterKit.Std.Functional;
using WelterKit.Std.StaticUtilities;



namespace WelterKit.Std {
   public static class AppInfoHelper {
      public static Maybe<string> GetAppTitle()   => getAttribValue<string>(getEntryAssembly().GetCustomAttributesData(), typeof( AssemblyTitleAttribute ), 0);
      public static Maybe<string> GetAppVersion() => getAttribValue<string>(getEntryAssembly().GetCustomAttributesData(), typeof( AssemblyInformationalVersionAttribute ), 0);


      private static Assembly getEntryAssembly() {
         var asm = Assembly.GetEntryAssembly();
         if ( asm == null ) throw new Exception("Unable to get entry assembly.");
         return asm;
      }


      private static Maybe<T> getAttribValue<T>(IList<CustomAttributeData> customAttribData, Type attributeType, int constructorArgumentIndex)
         => customAttribData.FirstOrNone(a => a.AttributeType == attributeType)
                            .Map(a => ( T )a.ConstructorArguments[constructorArgumentIndex].Value);
   }
}
