using System;

namespace WelterKit.Std {
   public interface IError {
      public string DisplayText { get; }
   }


   public interface IExceptionError : IError {
      public Exception Exception { get; }
   }
}



// TODO: make (something like) this work:
// public interface IErrorOr<T>;
// 
// public abstract record ErrorOr<T> : Either<IError, T>, IErrorOr<T> {
//    public static implicit operator ErrorOr<T>(IError leftValue ) => new ErrorOr_Left <T>(leftValue);
//    public static implicit operator ErrorOr<T>(T      rightValue) => new ErrorOr_Right<T>(rightValue);
// }
// public          record ErrorOr_Left <T>(IError Error) :Left<IError,T>(Error) ,IErrorOr<T> ;
// public          record ErrorOr_Right<T>(T Value     ) :Right<IError,T>(Value),IErrorOr<T> ;
