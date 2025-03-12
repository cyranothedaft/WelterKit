using System;


namespace WelterKit.Std.AbstractDataTypes.Graphs {
   public class MutableGraphEdge<TV, TE> {
      public TE Value { get; set; }
      public MutableGraphVertex<TV, TE> FromVertex { get; set; }
      public MutableGraphVertex<TV, TE> ToVertex { get; set; }


      internal MutableGraphEdge(TE value, MutableGraphVertex<TV, TE> fromVertex, MutableGraphVertex<TV, TE> toVertex) {
         Value      = value;
         FromVertex = fromVertex;
         ToVertex   = toVertex;
      }
   }
}
