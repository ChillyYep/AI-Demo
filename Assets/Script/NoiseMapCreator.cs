using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseMapCreator : MonoBehaviour
{
    public enum NoiseType
    {
        ValueNoise,
        PerlinNoise,
        WorleyNoise
    }
    public NoiseType m_noiseType;
    public RawImage m_rawImage;

    public int m_width, m_height;

    public int m_row, m_col;

    private Texture2D m_noiseMap;
    private Color[] m_colors;
    // 缓和曲线，坐标与像素值的关系
    public float EaseCurve(float t)
    {
        // s(t)=6t^5-15t^4+10t^3
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    public float InterPolation(float x, float y, float[] vertexRandom)
    {
        return Mathf.Lerp(
            Mathf.Lerp(vertexRandom[0], vertexRandom[1], EaseCurve(x)),
            Mathf.Lerp(vertexRandom[2], vertexRandom[3], EaseCurve(x)),
            EaseCurve(y));
    }
    private float fract(float p)
    {
        return p - Mathf.Sign(p) * Mathf.Floor(Mathf.Abs(p));
    }
    //一个随机性的哈希函数
    private Vector3 fract(Vector3 p)
    {
        return new Vector3(fract(p.x), fract(p.y), fract(p.z));
    }
    private Vector2 fract(Vector2 p)
    {
        return new Vector2(fract(p.x), fract(p.y));
    }

    float random(Vector2 p)
    {
        return fract(Mathf.Sin(Vector2.Dot(p, new Vector2(13.0f, 78.23f))) * 51043.0123f);
    }

    Vector2 grad(float x, float y)
    {
        Vector2 vec;
        vec.x = x * 127.1f + y * 311.7f;
        vec.y = x * 269.5f + y * 183.3f;

        float sin0 = Mathf.Sin(vec[0]) * 43758.5453123f;
        float sin1 = Mathf.Sin(vec[1]) * 43758.5453123f;
        vec.x = (sin0 - Mathf.Floor(sin0)) * 2.0f - 1.0f;
        vec.y = (sin1 - Mathf.Floor(sin1)) * 2.0f - 1.0f;


        return vec.normalized;
    }
    Vector2 grad(Vector2 p)
    {
        return grad(p.x, p.y);
    }

    float[] vertexRandom = new float[4];
    Vector2[] bordersVecs = new Vector2[4];
    public float ComputeValueNoise(float x, float y, float cellWidth, float cellHeight)
    {
        int cellColNum = Mathf.FloorToInt(x / cellWidth);
        int cellRowNum = Mathf.FloorToInt(y / cellHeight);

        Vector2 pi = new Vector2(cellWidth * cellColNum, cellHeight * cellRowNum);
        vertexRandom[0] = random(pi);
        vertexRandom[1] = random(new Vector2(cellWidth * cellColNum + cellWidth, cellHeight * cellRowNum));
        vertexRandom[2] = random(new Vector2(cellWidth * cellColNum, cellHeight * cellRowNum + cellHeight));
        vertexRandom[3] = random(new Vector2(cellWidth * cellColNum + cellWidth, cellHeight * cellRowNum + cellHeight));

        return InterPolation((x - pi.x) / cellWidth, (y - pi.y) / cellHeight, vertexRandom);
    }
    public float ComputePerlinNoise(float x, float y, float cellWidth, float cellHeight)
    {
        int cellColNum = Mathf.FloorToInt(x / cellWidth);
        int cellRowNum = Mathf.FloorToInt(y / cellHeight);

        Vector2 pi = new Vector2(cellWidth * cellColNum, cellHeight * cellRowNum);
        Vector2 p = new Vector2((x - pi.x) / cellWidth, (y - pi.y) / cellHeight);

        bordersVecs[0] = new Vector2(cellWidth * cellColNum, cellHeight * cellRowNum);
        bordersVecs[1] = new Vector2(cellWidth * cellColNum + cellWidth, cellHeight * cellRowNum);
        bordersVecs[2] = new Vector2(cellWidth * cellColNum, cellHeight * cellRowNum + cellHeight);
        bordersVecs[3] = new Vector2(cellWidth * cellColNum + cellWidth, cellHeight * cellRowNum + cellHeight);

        vertexRandom[0] = Vector2.Dot(grad(bordersVecs[0]), (p - bordersVecs[0]).normalized);
        vertexRandom[1] = Vector2.Dot(grad(bordersVecs[1]), (p - bordersVecs[1]).normalized);
        vertexRandom[2] = Vector2.Dot(grad(bordersVecs[2]), (p - bordersVecs[2]).normalized);
        vertexRandom[3] = Vector2.Dot(grad(bordersVecs[3]), (p - bordersVecs[3]).normalized);

        return InterPolation(p.x, p.y, vertexRandom);
    }
    Vector2Int[] neighboors = new Vector2Int[]
    {
        new Vector2Int(0,0),
        new Vector2Int(-1,-1),
        new Vector2Int(0,-1),
        new Vector2Int(1,-1),
        new Vector2Int(1,0),
        new Vector2Int(1,1),
        new Vector2Int(0,1),
        new Vector2Int(-1,1),
        new Vector2Int(-1,0),
    };
    public Vector2[] CreateFeaturePoints(int rowCount, int colCount, float cellWidth, float cellHeight)
    {
        Vector2[] featruePoints = new Vector2[rowCount * colCount];
        for (int i = 0; i < colCount; ++i)
        {
            for (int j = 0; j < rowCount; ++j)
            {
                featruePoints[j * colCount + i] = new Vector2(UnityEngine.Random.Range(i * cellWidth, (i + 1) * cellWidth), UnityEngine.Random.Range(j * cellHeight, (j + 1) * cellHeight));
            }
        }
        return featruePoints;
    }
    public float ComputeWorleyNoise(float x, float y, float cellWidth, float cellHeight, Vector2[] featruePoints)
    {
        if (featruePoints == null)
        {
            return 0.0f;
        }
        Vector2Int curPos = new Vector2Int(Mathf.FloorToInt(x / cellWidth), Mathf.FloorToInt(y / cellHeight));

        float mindist = float.MaxValue;
        for (int i = 0; i < neighboors.Length; ++i)
        {
            Vector2Int newPos = curPos + neighboors[i];
            if (newPos.x >= 0 && newPos.x < m_col &&
                newPos.y >= 0 && newPos.y < m_row)
            {
                Vector2 featurePoint = featruePoints[newPos.y * m_col + newPos.x];
                var distance = ((featurePoint - new Vector2(x, y)) / new Vector2(cellWidth, cellHeight)).sqrMagnitude;
                //var distance = Vector2.Distance(featurePoint, new Vector2(x, y));
                if (distance < mindist)
                {
                    mindist = distance;
                }
            }
        }
        return Mathf.Clamp01(mindist);
        //return Mathf.Clamp01(1f - 1.5f * Mathf.Clamp01(mindist));
    }
    [ContextMenu("CreateNoiseMap")]
    public void CreateNoiseMap()
    {
        if (m_width > 0 && m_height > 0)
        {
            if (m_noiseMap == null || m_width != m_noiseMap.width || m_height != m_noiseMap.height)
            {
                m_noiseMap = new Texture2D(m_width, m_height, TextureFormat.RGB24, false);
                m_colors = new Color[m_width * m_height];
            }
            float cellWidth = m_width / (float)m_row;
            float cellHeight = m_height / (float)m_col;
            Vector2[] featurePoints = null;
            if (m_noiseType == NoiseType.WorleyNoise)
            {
                featurePoints = CreateFeaturePoints(m_row, m_col, cellWidth, cellHeight);
            }
            for (int i = 0; i < m_height; ++i)
            {
                for (int j = 0; j < m_width; ++j)
                {
                    float value;
                    if (m_noiseType == NoiseType.WorleyNoise)
                    {
                        value = ComputeWorleyNoise(j, i, cellWidth, cellHeight, featurePoints);
                    }
                    else if(m_noiseType == NoiseType.PerlinNoise)
                    {
                        value = ComputePerlinNoise(j, i, cellWidth, cellHeight);
                    }
                    else
                    {
                        value = ComputeValueNoise(j, i, cellWidth, cellHeight);
                    }
                    Color color = new Color();
                    color.r = value;
                    color.g = value;
                    color.b = value;
                    m_colors[i * m_width + j] = color;
                }
            }
            m_noiseMap.SetPixels(m_colors);
            m_noiseMap.Apply();

            if (m_rawImage != null)
            {
                m_rawImage.texture = m_noiseMap;
            }
        }
    }
}
