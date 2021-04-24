using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singleton_CSharp<T> where T : new()
{
    protected Singleton_CSharp() { }
    private static T _instance = new T();
    public static T Instance { get; set; } = _instance;
}
namespace AIDemo.Graph
{
    public class NavManager
    {
        public NavSparseGraph curNavSparseGraph { get; set; }
        public GraphSearch<NavGraphNode, NavGraphEdge> graphSearch { get; private set; }
        public NavManager()
        {
            curNavSparseGraph = new NavSparseGraph(true);
            curNavSparseGraph.Load();
            graphSearch = new Graph_SearchDFS<NavGraphNode, NavGraphEdge>();
        }
        public void Reset()
        {
            graphSearch.SetUp(curNavSparseGraph, curNavSparseGraph.srcIndex, curNavSparseGraph.dstIndex);
        }
    }

}
