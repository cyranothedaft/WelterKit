using System;
using System.Collections.Generic;
using System.Linq;


namespace WelterKit.AbstractDataTypes.Graphs {
   // TODO:
   //public class MutableDirectedGraph<TV> : MutableDirectedGraph<TV, _> {
   //}


   public class MutableDirectedGraph<TV, TE> {
      public List<MutableGraphVertex<TV, TE>> Vertices { get; } = new List<MutableGraphVertex<TV, TE>>();
      public List<MutableGraphEdge<TV, TE>>   Edges    { get; } = new List<MutableGraphEdge<TV, TE>>();


      public MutableGraphVertex<TV, TE> AddVertex(TV value, List<MutableGraphEdge<TV, TE>>? inEdges = null, List<MutableGraphEdge<TV, TE>>? outEdges = null) {
         var newVertex = new MutableGraphVertex<TV, TE>(value, inEdges, outEdges);
         Vertices.Add(newVertex);
         foreach ( var edge in newVertex.InEdges )
            edge.ToVertex = newVertex;
         foreach ( var edge in newVertex.OutEdges )
            edge.FromVertex = newVertex;
         return newVertex;
      }


      public MutableGraphEdge<TV, TE> AddEdge(TE value, MutableGraphVertex<TV, TE> fromVertex, MutableGraphVertex<TV, TE> toVertex) {
         var newEdge = new MutableGraphEdge<TV, TE>(value, fromVertex, toVertex);
         Edges.Add(newEdge);
         newEdge.FromVertex.OutEdges.Add(newEdge);
         newEdge.ToVertex.InEdges.Add(newEdge);
         return newEdge;
      }


      public IEnumerable<MutableGraphVertex<TV, TE>> AddVertices(IEnumerable<(TV value, List<MutableGraphEdge<TV, TE>>? inEdges, List<MutableGraphEdge<TV, TE>>? outEdges)> vertices)
         => vertices.Select(v => AddVertex(v.value, v.inEdges, v.outEdges));

      public IEnumerable<MutableGraphVertex<TV, TE>> AddVertices(params (TV value, List<MutableGraphEdge<TV, TE>>? inEdges, List<MutableGraphEdge<TV, TE>>? outEdges)[] vertices)
         => AddVertices(( IEnumerable<(TV, List<MutableGraphEdge<TV, TE>>? inEdges, List<MutableGraphEdge<TV, TE>>? outEdges)> )vertices);


      public IEnumerable<MutableGraphVertex<TV, TE>> AddVertices(IEnumerable<TV> vertexValues)
         => vertexValues.Select(v => AddVertex(v));

      public IEnumerable<MutableGraphVertex<TV, TE>> AddVertices(params TV[] vertexValues)
         => AddVertices(( IEnumerable<TV> )vertexValues);


      public IEnumerable<MutableGraphEdge<TV, TE>> AddEdges(IEnumerable<(TE value, MutableGraphVertex<TV, TE> @from, MutableGraphVertex<TV, TE> to)> edges)
         => edges.Select(e => AddEdge(e.value, e.from, e.to));

      public IEnumerable<MutableGraphEdge<TV, TE>> AddEdges(params (TE value, MutableGraphVertex<TV, TE> @from, MutableGraphVertex<TV, TE> to)[] edges)
         => AddEdges(( IEnumerable<(TE value, MutableGraphVertex<TV, TE> @from, MutableGraphVertex<TV, TE> to)> )edges);
   }
}
