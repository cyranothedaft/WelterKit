using System;

namespace WelterKit.FunctionalContainers_tests.ContainerTests.State_Tests;

[TestClass]
public class SampleUsage {
   [TestMethod]
   public void xxx() {

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

   //   gcd' :: GCDState -> (Int, GCDState)
   // gcd' s = 
   //   let (x, y) = s in   -- unpack the state into x, y
   //     case compare x y of
   //       EQ -> (x, s)       -- if x == y, return x and the state tuple
   //       LT -> gcd' (y, x)  -- if x < y, flip the arguments
   //       GT -> gcd' (y, x - y)
   //
   private static (int, GcdState) gcd_(GcdState s) {
      (int x,int y) = s;
      return x.CompareTo(y) switch {
            0 =>(x,initialState: s),
            <0=>gcd_(new GcdState( y,x)),
            _ => gcd_((y,x-y))
         };
   }
}
