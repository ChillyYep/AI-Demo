using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Graph
{
    public class NavUnit : MonoBehaviour
    {
        public Transform gizmoParent;
        public GameObject prefab;
        public NavManager navManager { get; private set; }
        private Queue<int> nodePath = new Queue<int>();
        private bool overWalk = true;
        private Coroutine coroutine;
        private void Awake()
        {
            navManager = Singleton_CSharp<NavManager>.Instance;
        }
        [ContextMenu("Search")]
        public void Search()
        {
            navManager.Reset();
            if (overWalk)
            {
                navManager.graphSearch.Search();
                nodePath = new Queue<int>(navManager.graphSearch.GetPath());
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(Walk());
            }
        }
        WaitForSeconds forSeconds = new WaitForSeconds(1f);
        IEnumerator Walk()
        {
            overWalk = false;
            while (nodePath.Count > 0)
            {
                int index = nodePath.Dequeue();
                var node = navManager.curNavSparseGraph.GetNode(index);
                transform.position = node.nodePos;
                var obj = Instantiate(prefab, gizmoParent);
                obj.SetActive(true);
                obj.transform.position = node.nodePos;
                yield return forSeconds;
            }
            overWalk = true;
        }
    }
}
