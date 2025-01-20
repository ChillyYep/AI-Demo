using behaviac;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ReflectionTestEnum
{
    [MemberMetaInfo("AA", "Just a Enum Name")]
    AA,
    [MemberMetaInfo("BB", "Just a Enum Name,BB")]
    BB,
    [MemberMetaInfo("CC", "Just a Enum Name,CC")]
    CC,
    [MemberMetaInfo("DD", "Just a Enum Name,DD")]
    DD,
    [MemberMetaInfo("EE", "Just a Enum Name,EE")]
    EE
}
public class ReflectionTest : MonoBehaviour
{
    public ReflectionTestEnum reflectionTestEnum;
    [ContextMenu("GetEnumField")]
    void GetEnumField()
    {
        UnityEngine.Debug.LogFormat("reflectionTestEnum.ToString({0})", reflectionTestEnum.ToString());
        var fieldInfo = typeof(ReflectionTestEnum).GetField(reflectionTestEnum.ToString());
        UnityEngine.Debug.LogFormat("reflectionTestEnum.fieldInfo:{0}", fieldInfo.Name);

        Attribute[] attributes = (Attribute[])fieldInfo.GetCustomAttributes(typeof(MemberMetaInfoAttribute), false);

        for (int i = 0; i < attributes.Length; ++i)
        {
            UnityEngine.Debug.Log((attributes[i] as MemberMetaInfoAttribute).Description);
        }
    }
}
