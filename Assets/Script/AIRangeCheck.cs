using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIRangeCheck
{
    /// <summary>
    /// ���μ��
    /// </summary>
    public static bool SectorRangeCheck(Vector2 origin, float radius, Vector2 lookDir, float angleRadian, Vector2 targetPosition)
    {
        Vector2 offset = targetPosition - origin;
        float distance = offset.magnitude;
        // �������ΰ뾶�ض���������
        if (distance > radius)
        {
            return false;
        }
        float halfRadian = angleRadian / 2;
        // �н���С�ڵ������ΰ�ǣ���0-180�ȼ�����ֵ�ݼ��Ƕȵ���
        if (Vector2.Dot(offset.normalized, lookDir.normalized) >= Mathf.Cos(halfRadian))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ��Բ�����⣬ԭ���ڳ�����ĳ��
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

        // ��Բ����ĳ��p����distance(p-c1)+distance(p-c2)=2*a��������������Բ�ڣ�������distance(p-c1)+distance(p-c2)<=2*a
        if (Vector2.Distance(targetPosition, c1) + Vector2.Distance(targetPosition, c2) <= 2 * a)
        {
            return true;
        }
        return false;
    }
}
