using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIRangeCheck
{
    /// <summary>
    /// 扇形检测
    /// </summary>
    public static bool SectorRangeCheck(Vector2 origin, float radius, Vector2 lookDir, float angleRadian, Vector2 targetPosition)
    {
        Vector2 offset = targetPosition - origin;
        float distance = offset.magnitude;
        // 大于扇形半径必定在扇形外
        if (distance > radius)
        {
            return false;
        }
        float halfRadian = angleRadian / 2;
        // 夹角须小于等于扇形半角，在0-180度间余弦值递减角度递增
        if (Vector2.Dot(offset.normalized, lookDir.normalized) >= Mathf.Cos(halfRadian))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 椭圆区域检测，原点在长轴上某点
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="distanceFocus1"></param>
    /// <param name="distanceFocus2"></param>
    /// <param name="a"></param>
    /// <param name="lookDir"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    public static bool EllipseRangeCheck(Vector2 origin, float distanceFocus1, float distanceFocus2, float a, Vector2 lookDir, Vector2 targetPosition)
    {
        Vector2 c1 = origin + lookDir * distanceFocus1;
        Vector2 c2 = origin + lookDir * distanceFocus2;

        // 椭圆边上某点p满足distance(p-c1)+distance(p-c2)=2*a，所以若点在椭圆内，须满足distance(p-c1)+distance(p-c2)<=2*a
        if (Vector2.Distance(targetPosition, c1) + Vector2.Distance(targetPosition, c2) <= 2 * a)
        {
            return true;
        }
        return false;
    }
}
