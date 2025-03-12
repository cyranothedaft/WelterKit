using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace WelterKit.StaticUtilities {
   public class Reiterable<T> : IEnumerable<T> {
      public IEnumerable<T> Sequence { get; }


      private Reiterable(IEnumerable<T> sequence) {
         Sequence = sequence;
      }


      public IEnumerator<T> GetEnumerator() => Sequence.GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();


      public static Reiterable<T> From(IEnumerable<T> sequence)
         => sequence switch
            {
               List<T> list             => list,
               T[] array                => array,
               Reiterable<T> reiterable => reiterable,
               _                        => new Reiterable<T>(sequence.ToList()) // materialize the sequence
            };


      public static implicit operator Reiterable<T>(List<T> sequence) => new Reiterable<T>(sequence);
      public static implicit operator Reiterable<T>(T[]     sequence) => new Reiterable<T>(sequence);
   }



   public static class ReiterableExtensions {
      public static Reiterable<T> ToReiterable<T>(this IEnumerable<T> sequence)
         => Reiterable<T>.From(sequence);
   }
}
