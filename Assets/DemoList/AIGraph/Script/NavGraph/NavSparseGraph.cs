using UnityEngine;

namespace AIDemo.Graph
{
    public class NavSparseGraph : SparseGraph<NavGraphNode, NavGraphEdge>
    {
        private float unitScale = 1f;
        private Vector3 originPos;
        public int srcIndex { get; protected set; } = ConstDefine.InvalidNodeIndex;
        public int dstIndex { get; protected set; } = ConstDefine.InvalidNodeIndex;
        public NavSparseGraph(bool digraph) : base(digraph)
        {
            originPos = Vector3.zero;
        }

        public override void Load()
        {
            //8x8,0障碍物，1通路，2起点，3终点
            int[][] map = new int[8][]{
                new int[8]{ 0,0,0,0,0,0,0,0},
                new int[8]{ 0,1,1,1,1,1,3,0},
                new int[8]{ 0,1,0,0,0,0,1,0},
                new int[8]{ 0,1,1,1,1,0,1,0},
                new int[8]{ 0,1,1,1,1,0,1,0},
                new int[8]{ 0,1,0,1,1,1,0,0},
                new int[8]{ 0,1,0,1,1,0,0,0},
                new int[8]{ 0,1,1,1,1,1,2,0}
            };
            int count = 0;
            int[][] indices = new int[8][]
            {
                new int[8]{ -1,-1,-1,-1,-1,-1,-1,-1},
                new int[8]{ -1,-1,-1,-1,-1,-1,-1,-1},
                new int[8]{ -1,-1,-1,-1,-1,-1,-1,-1},
                new int[8]{ -1,-1,-1,-1,-1,-1,-1,-1},
                new int[8]{ -1,-1,-1,-1,-1,-1,-1,-1},
                new int[8]{ -1,-1,-1,-1,-1,-1,-1,-1},
                new int[8]{ -1,-1,-1,-1,-1,-1,-1,-1},
                new int[8]{ -1,-1,-1,-1,-1,-1,-1,-1},
            };
            for (int i = 0; i < map.Length; ++i)
            {
                for (int j = 0; j < map[i].Length; ++j)
                {
                    if (map[i][j] > 0)
                    {
                        if (map[i][j] == 2)
                        {
                            srcIndex = count;
                        }
                        else if (map[i][j] == 3)
                        {
                            dstIndex = count;
                        }
                        indices[i][j] = count;
                        AddNode(new NavGraphNode(count++, originPos + new Vector3(i * unitScale, 0f, j * unitScale)));
                    }
                }
            }
            for (int i = 0; i < map.Length; ++i)
            {
                for (int j = 0; j < map[i].Length; ++j)
                {
                    if (indices[i][j] > -1)
                    {
                        //下
                        if (i < indices.Length - 1 && indices[i + 1][j] > -1)
                        {
                            AddEdge(new NavGraphEdge
                            {
                                fromIndex = indices[i][j],
                                toIndex = indices[i + 1][j]
                            });
                        }
                        //上
                        if (i > 0 && indices[i - 1][j] > -1)
                        {
                            AddEdge(new NavGraphEdge
                            {
                                fromIndex = indices[i][j],
                                toIndex = indices[i - 1][j]
                            });
                        }
                        //右
                        if (j < indices[i].Length - 1 && indices[i][j + 1] > -1)
                        {
                            AddEdge(new NavGraphEdge
                            {
                                fromIndex = indices[i][j],
                                toIndex = indices[i][j + 1]

                            });
                        }
                        //左
                        if (j > 0 && indices[i][j - 1] > -1)
                        {
                            AddEdge(new NavGraphEdge
                            {
                                fromIndex = indices[i][j],
                                toIndex = indices[i][j - 1]
                            });
                        }
                    }
                }
            }
        }
    }
}
