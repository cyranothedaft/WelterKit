using System;
using System.Collections.Generic;


namespace WelterKit.Std.AbstractDataTypes.Graphs {
   public class MutableGraphVertex<TV, TE> {
      public TV Value { get; set; }
      public List<MutableGraphEdge<TV, TE>> InEdges { get; }
      public List<MutableGraphEdge<TV, TE>> OutEdges { get; }


      internal MutableGraphVertex(TV value, List<MutableGraphEdge<TV, TE>>? inEdges = null, List<MutableGraphEdge<TV, TE>>? outEdges = null) {
         Value    = value;
         InEdges  = inEdges ?? new List<MutableGraphEdge<TV, TE>>();
         OutEdges = outEdges ?? new List<MutableGraphEdge<TV, TE>>();
      }
   }
}
