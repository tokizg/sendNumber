using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "itemObjectList", menuName = "itemObjectList")]
public class itemObjectList : ScriptableObject
{
    public static itemObject[] list;

    public static itemObject find(string subject)
    {
        for (int i = 0; i < list.Length; i++)
            if (list[i].displayName == subject)
                return list[i];
        return null;
    }
}
[Serializable]
public class itemObject
{
    public string displayName = "アイテム";
    public string className;
}