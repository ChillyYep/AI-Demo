using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Singleton_CSharp<T> where T : class, new()
{
    protected Singleton_CSharp()
    {
        //System.Diagnostics.Debug.Assert(switchOn, "不能显式调用Singleton_CSharp的构造器");
        //只有switchOn开启才会构造该单例，而switchOn只会在Instance中开启
        //Assert.IsTrue(switchOn, "不能显式调用Singleton_CSharp的构造器");
        Assert.IsNull(_instance, "不能显式调用Singleton_CSharp的构造器");
        _instance = this as T;
    }
    //protected static bool switchOn = false;
    protected static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                new T();
            }
            return _instance;
        }
    }
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
