using System;
using System.Linq;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Traits;



namespace WelterKit.FunctionalContainers_tests.ContainerTests.IO_Tests;

[TestClass]
public class SampleUsage {
   [TestMethod]
   public void Sample___() {
      infiniteLoop(0).Run();


      static IO<Unit> infiniteLoop(int value)
         => (from _ in value % 10000 == 0
                             ? writeLine($"{value}")
                             : IO.Pure(Unit.Value)
             from r in tail(infiniteLoop(value + 1))
             select Unit.Value
            ).As();

      static IO<Unit> writeLine(string line)
         => IO_lift(lift<Unit>(() => {
                                  Console.WriteLine(line);
                                  return Unit.Value;
                               }));

      static IO<A> tail<A>(IO<A> tailIO) 
         => new IOTail<A>(tailIO);
   }


   private static IO<Unit> IO_lift(Lift<Unit> ma) => IO_lift(ma.Function);
   private static IO<Unit> IO_lift(Func<Unit> ma) => lift2(ma);
   private static IO<A> lift2<A>(Func<A> f) => new IOLiftSync<A, A>(_ => f(), pure<A>);
   private static IO<A> pure<A>(A value) => new IOPure<A>(value);

   private static Lift<Unit> lift(Action action)
      => lift<Unit>(() =>
                    {
                       action();
                       return default;
                    });

    private static Lift<A> lift<A>(Func<A> function)
       => new(function);
}





record IOTail<A>(IO<A> Tail) : IO<A>;
record IOLiftSync<A, B>(Func<EnvIO, A> F, Func<A, K<IO, B>> Next) : InvokeSyncIO<B>;
record IOPure<A>(A Value) : InvokeSync<A>;
public abstract record InvokeSync<A> : IO<A>;
public abstract record InvokeSyncIO<A> : IO<A>;
