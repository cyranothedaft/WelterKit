using System;
using System.Collections.Generic;
using System.Linq;



namespace WelterKit.Std {
   internal readonly struct DSpan<T> {
      public static DSpan<T> Empty = new DSpan<T>(Array.Empty<T>(), 0, 0);


      private readonly IList<T> _list;
      private readonly int _start;
      private readonly int _length;


      public int Length => _length;


      public DSpan(IList<T> list)
         : this(list, 0, list.Count) {
      }


      public DSpan(IList<T> list, int start, int length) {
         _list = list;
         _start = start;
         _length = length;
      }


      public DSpan<T> Slice(int start, int length)
         => new DSpan<T>(_list, _start + start, length);


      public DSpan<T> Slice(int start)
         => new DSpan<T>(_list, _start + start, Length - start);


      public IList<T> ToList()
         => ToEnumerable().ToList();


      public IEnumerable<T> ToEnumerable()
         => _list.Skip(_start)
                 .Take(_length);
   }



   internal static class Extensions {
      internal static DSpan<T> ToSpan<T>(this IList<T> list)
         => new DSpan<T>(list);
   }
}
