using System;

namespace WelterKit {
   public interface IError {
      public string DisplayText { get; }
   }


   public interface IExceptionError : IError {
      public Exception Exception { get; }
   }
}
