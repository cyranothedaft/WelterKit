using WelterKit.FunctionalContainers.Framework;



namespace WelterKit.FunctionalContainers.Containers;

public static class EitherExtensions {
   public static Either<L, R> As<L, R>(this K<Either<L>, R> ma) => (Either<L, R>)ma;


   // TODO: this has a slight code smell.. is there a better (maybe more fp-idiomatic) way to accomplish the same thing?
   // TODO: somehow make these type parameters inferrable...
   public static Left< L, R> AsLeft <L, R>(this L leftValue ) => new(leftValue );
   public static Right<L, R> AsRight<L, R>(this R rightValue) => new(rightValue);
}
