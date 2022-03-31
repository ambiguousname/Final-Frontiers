using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    // From https://answers.unity.com/questions/893966/how-to-find-child-with-tag.html
    public static GameObject FindChildWithTag(this GameObject parent, string tag) {
        Transform t = parent.transform;
        foreach (Transform tr in t) {
            if (tr.tag == tag) {
                return tr.gameObject;
            }
            GameObject result = tr.gameObject.FindChildWithTag(tag);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public static GameObject FindChildWithName(this GameObject parent, string name) {
        Transform t = parent.transform;
        foreach (Transform tr in t) {
            if (tr.name == name) {
                return tr.gameObject;
            }
            if (tr.childCount > 0) {
                GameObject result = tr.gameObject.FindChildWithName(name);
                if (result != null) {
                    return result;
                }
            }
        }
        return null;
    }

    public static Vector3 Mult(this Vector3 v, float constant) {
        return new Vector3(v.x * constant, v.y * constant, v.z * constant);
    }
}