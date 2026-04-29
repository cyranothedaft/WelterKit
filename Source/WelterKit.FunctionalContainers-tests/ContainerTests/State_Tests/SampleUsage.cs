using System;
using WelterKit.FunctionalContainers.Containers;
using WelterKit.FunctionalContainers.Framework;
using WelterKit.FunctionalContainers.Framework.Kinds;



namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_Tests;

[TestClass]
public class SampleUsage {
   [TestMethod]
   public void xxx() {
      Assert.AreEqual(8, gcd(1024, 40));
      Assert.AreEqual(8, run_gcd_(1024, 40));
      Assert.AreEqual(8, run_gcd_s1_(1024, 40));
   }


//   // getState :: State s s
//   // getState = State (\st -> (st, st))
//   private static State<S, S> getState<S>() => new State<S, S>(st => (st, st));
//
//   // putState :: GCDState -> State GCDState ()
//   // putState s = State (\_ -> ((), s))
//   private static State<S, Unit> putState<S>(S s) => new State<S, Unit>(_ => (s, Unit.Value));


   // gcd :: Int -> Int -> Int
   // gcd a b | a == b = a
   //         | a < b  = gcd b a
   //         | otherwise = gcd b (a - b)
   //
   private static int gcd(int a, int b)
      =>  a == b ? a
        : a < b  ? gcd(b, a)
                 : gcd(b, a - b);



   private record GcdState(int X, int Y);

   // "stateful" version of gcd
   // gcd' :: GCDState -> (Int, GCDState)
   // gcd' s = 
   //   let (x, y) = s in   -- unpack the state into x, y
   //     case compare x y of
   //       EQ -> (x, s)       -- if x == y, return x and the state tuple
   //       LT -> gcd' (y, x)  -- if x < y, flip the arguments
   //       GT -> gcd' (y, x - y)
   //
   private static (GcdState state, int value) gcd_(GcdState s)
      => s.X.CompareTo(s.Y) switch
         {
            0   => (s, s.X),
            < 0 => gcd_(new GcdState(s.Y, s.X)),
            _   => gcd_(new GcdState(s.Y, s.X - s.Y))
         };

   // gcd state transformer function:
   // gcd_s1' :: State GCDState Int
   // gcd_s1' = State (\s -> 
   //                    let (x, y) = s in
   //                      case compare x y of
   //                        EQ -> (x, (x, y))
   //                        LT -> f (y, x)
   //                        GT -> f (y, x - y))
   //   where
   //     f :: GCDState -> (Int, GCDState)
   //     f = runState gcd_s1'
   //
   private static State<GcdState, int> gcd_s1_ { get; }
      = new(s => {
               (int x, int y) = s;
               return x.CompareTo(y) switch
                  {
                     0   => (new(x, y), x), // why new(x,y) instead of s?
                     < 0 => f(new(y, x)),
                     _   => f(new(y, x - y))
                  };

               static (GcdState, int) f(GcdState gs)
                  => gcd_s1_.runState(gs);
            });


   // gcd_s2 :: State GCDState Int
   // gcd_s2 = 
   //   getState >>= (\(x, y) ->
   //     case compare x y of
   //       EQ -> return x
   //       LT -> (putState (y, x) >> gcd_s2)
   //       GT -> (putState (y, x - y) >> gcd_s2))
   //
   private static State<GcdState, int> gcd_s2() {
      State<GcdState, GcdState> a=State<GcdState>.get;
      State<GcdState, int> b=a
                            .Bind(xy => {
                                     (int x, int y) = xy;
                                     return x.CompareTo(y) switch
                                        {
                                           0   => x.Return<State<GcdState>, int>(),
                                           < 0 => State<GcdState>.put(new GcdState(y, x)).Bind(_ => gcd_s2()), // TODO: write and use "bind2" or something similar for the ">>" operation
                                           _   => State<GcdState>.put(new GcdState(y, x - y)).Bind(_ => gcd_s2())
                                        };
                                  })
                            .As();
      return b=====;
   }

   // helpers for running these:

   // run_gcd' :: Int -> Int -> Int
   // run_gcd' x y = fst (gcd' (x, y))
   private static int run_gcd_(int x, int y)
      => gcd_(new GcdState(x, y)).value;
   
   // gcd_s1 :: State GCDState Int
   // gcd_s1 = State gcd'
   private static State<GcdState, int> gcd_s1 { get; }
      = new(gcd_);

   // run_gcd_s1' :: Int -> Int -> Int
   // run_gcd_s1' x y = fst (runState gcd_s1' (x, y))
   private static int run_gcd_s1_(int x, int y)
      => gcd_s1_.runState(new(x, y)).Item2;

}
