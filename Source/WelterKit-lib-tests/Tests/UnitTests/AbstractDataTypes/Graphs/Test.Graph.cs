using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WelterKit.AbstractDataTypes.Graphs;


namespace WelterKit_Tests.UnitTests.AbstractDataTypes.Graphs {
   [TestClass]
   public class Test_Graph {
      [TestMethod]
      public void Add_Single_Sample() {
         var graph = new MutableDirectedGraph<string, string>();
         var v1 = graph.AddVertex("v1");
         var v2 = graph.AddVertex("v2");
         var v3 = graph.AddVertex("v3");
         var e12 = graph.AddEdge("e12", v1, v2);
         var e23 = graph.AddEdge("e23", v2, v3);

         CollectionAssert.AreEqual(new[] { "v1", "v2", "v3" }, graph.Vertices.Select(v => v.Value).ToArray());
         CollectionAssert.AreEqual(new[] { "e12", "e23" },     graph.Edges   .Select(e => e.Value).ToArray());

         testVertex(( value: "v1", inEdges: Array.Empty<MutableGraphEdge<string, string>>(), outEdges: new[] { e12 } ),                            v1);
         testVertex(( value: "v2", inEdges: new[] { e12 },                            outEdges: new[] { e23 } ),                            v2);
         testVertex(( value: "v3", inEdges: new[] { e23 },                            outEdges: Array.Empty<MutableGraphEdge<string, string>>() ), v3);

         testEdge(( value: "e12", from: v1, to: v2 ), e12);
         testEdge(( value: "e23", from: v2, to: v3 ), e23);
      }


      private void testVertex((string value, MutableGraphEdge<string, string>[] inEdges, MutableGraphEdge<string, string>[] outEdges) expected, MutableGraphVertex<string, string> v) {
         Assert.AreEqual(expected.value, v.Value);
         CollectionAssert.AreEquivalent(expected.inEdges,  v.InEdges);
         CollectionAssert.AreEquivalent(expected.outEdges, v.OutEdges);
      }


      private void testEdge((string value, MutableGraphVertex<string, string> @from, MutableGraphVertex<string, string> to) expected, MutableGraphEdge<string, string> e) {
         Assert.AreEqual(expected.value, e.Value);
         Assert.AreSame(expected.from,   e.FromVertex);
         Assert.AreSame(expected.to,     e.ToVertex);
      }


      [TestMethod]
      public void Add_Multi_Sample() {
         var graph = new MutableDirectedGraph<string, string>();
         var vs = graph.AddVertices(( "v1" ),
                                    ( "v2" ),
                                    ( "v3" ))
                       .ToArray();
         var es = graph.AddEdges(( value: "e12", from: vs[0], to: vs[1] ),
                                 ( value: "e23", from: vs[1], to: vs[2] ))
                       .ToArray();

         CollectionAssert.AreEqual(new[] { "v1", "v2", "v3" }, graph.Vertices.Select(v => v.Value).ToArray());
         CollectionAssert.AreEqual(new[] { "e12", "e23" },     graph.Edges.Select(e => e.Value).ToArray());

         testVertex(( value: "v1", inEdges: Array.Empty<MutableGraphEdge<string, string>>(), outEdges: new[] { es[0] } ),                          vs[0]);
         testVertex(( value: "v2", inEdges: new[] { es[0] },                          outEdges: new[] { es[1] } ),                          vs[1]);
         testVertex(( value: "v3", inEdges: new[] { es[1] },                          outEdges: Array.Empty<MutableGraphEdge<string, string>>() ), vs[2]);

         testEdge(( value: "e12", from: vs[0], to: vs[1] ), es[0]);
         testEdge(( value: "e23", from: vs[1], to: vs[2] ), es[1]);
      }
   }
}
