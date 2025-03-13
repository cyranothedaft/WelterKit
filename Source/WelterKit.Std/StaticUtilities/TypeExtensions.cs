using System;
using System.Linq;


namespace WelterKit.Std.StaticUtilities {
   public static class TypeExtensions {
      public static string GetFriendlyName(this Type type, int namespaceDepth = 1)
         => $"{namespacePart(type, namespaceDepth)}{namePart(type, namespaceDepth)}";


      private static string namespacePart(Type type, int count = 1)
         => string.Concat(type.Namespace.Split('.')
                                        .TakeLast(count)
                                        .Select(s => s + "."));


      private static string namePart(Type type, int namespaceDepth = 1)
         => type.IsGenericType
                  ? @$"{type.Name.Until("`")}<{type.GenericTypeArguments
                                                   .Select(t => GetFriendlyName(t, namespaceDepth))
                                                   .JoinString(", ")}>"
                  : type.Name;
   }
}
