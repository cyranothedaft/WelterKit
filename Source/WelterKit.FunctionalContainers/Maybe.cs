// using System;
// using WelterKit.FunctionalContainers.Framework;
//
//
//
// namespace WelterKit.FunctionalContainers;
//
// public abstract partial record Maybe <T>;
// public partial record Some<T>(T Value): Maybe <T>;
// public partial record None<T>: Maybe <T>;
//
//
//
// public interface IFunctor<F> where F:IFunctor<F> {
//    K<F, B> FMap<A, B>(K<F, A> a, Func<A, B> func);
// }
//
//
//
// public class Maybe : IFunctor<Maybe> {
//    public K<Maybe, B> FMap<A, B>(K<Maybe, A> a, Func<A, B> func) {
//    }
// }
//
//
// partial record Maybe <T> : IFunctor<Maybe<T>>;
