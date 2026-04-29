using System;

namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_Tests;

[TestClass]
public class SampleUsage {
   [TestMethod]
   public void xxx() {
      Assert.AreEqual(8, gcd(1024, 40));
      Assert.AreEqual(8, run_gcd_(1024, 40));
   }


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
   private static (GcdState state, int value) gcd_(GcdState s)
      => s.X.CompareTo(s.Y) switch
         {
            0   => (s, s.X),
            < 0 => gcd_(new GcdState(s.Y, s.X)),
            _   => gcd_(new GcdState(s.Y, s.X - s.Y))
         };

   // helper for running it
   // run_gcd' :: Int -> Int -> Int
   // run_gcd' x y = fst (gcd' (x, y))
   private static int run_gcd_(int x, int y)
      => gcd_(new GcdState(x, y)).value;



}
