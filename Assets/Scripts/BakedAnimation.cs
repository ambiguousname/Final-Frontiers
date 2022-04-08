using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Baked Animation", menuName = "ScriptableObjects/BakedAnimation")]
public class BakedAnimation : ScriptableObject
{
    public Vector3[] position;
    public Vector3[] rotation;
    public float secondsPerFrame;
    public string animationName;
}

