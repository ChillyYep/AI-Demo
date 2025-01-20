using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTest : Singleton_CSharp<SingletonTest>
{
    public string name = "default";
    public SingletonTest() : base() { }
}
public class SingletonTets : MonoBehaviour
{
    [ContextMenu("TestSingleton")]
    void TestSingleton()
    {
        //new SingletonTest();
        //new SingletonTest();
        Debug.Log(SingletonTest.Instance.name);
        Debug.Log(SingletonTest.Instance.name);
        Debug.Log(SingletonTest.Instance.name);
    }
}
