using System;
using System.Runtime.Serialization;



namespace WelterKit.Std {
   [Serializable]
   // TODO: make WelterKitException abstract, forcing more specific usages
   public class WelterKitException : Exception {
      public WelterKitException() { }
      public WelterKitException(string message) : base(message) { }
      public WelterKitException(string message, Exception inner) : base(message, inner) { }

      protected WelterKitException(SerializationInfo info, StreamingContext context) : base(info, context) { }
   }


   [Serializable]
   public class WelterKitNotEqualException : WelterKitException {
      public WelterKitNotEqualException() { }
      public WelterKitNotEqualException(string message) : base(message) { }
      public WelterKitNotEqualException(string message, Exception inner) : base(message, inner) { }
   }
}
