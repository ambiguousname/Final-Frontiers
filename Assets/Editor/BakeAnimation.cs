using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BakeAnimation : EditorWindow
{
    public AnimationClip animToBake;
    public BakedAnimation saveTo;

    [MenuItem("Window/BakeAnimations")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(BakeAnimation));
    }

    private void UpdateVectorArray(ref Vector3[] arr, Vector3 toAdd, AnimationCurve c, int numFrames, float frameRate) {
        arr[0] = Vector3.zero;
        for (int i = 1; i < numFrames; i++) {
            float time = (i - 1) / frameRate;
            arr[i] += (toAdd * (c.Evaluate(time + (1 / frameRate)) - c.Evaluate(time)));
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Bake Animations", EditorStyles.boldLabel);
        animToBake = (AnimationClip)EditorGUILayout.ObjectField(animToBake, typeof(AnimationClip), false);
        saveTo = (BakedAnimation)EditorGUILayout.ObjectField(saveTo, typeof(BakedAnimation), false);
        if (GUILayout.Button("Bake Anim")) {
            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(animToBake);
            int size = (int) (animToBake.length * animToBake.frameRate);
            Vector3[] position = new Vector3[size];
            Vector3[] rotation = new Vector3[size];
            foreach (EditorCurveBinding binding in bindings) {
                char axis = binding.propertyName[binding.propertyName.Length - 1];
                Vector3 toAddVector = Vector3.zero;
                switch (axis)
                {
                    case 'x':
                        toAddVector = new Vector3(1, 0, 0);
                        break;
                    case 'y':
                        toAddVector = new Vector3(0, 1, 0);
                        break;
                    case 'z':
                        toAddVector = new Vector3(0, 0, 1);
                        break;
                    default:
                        break;
                }
                AnimationCurve c = AnimationUtility.GetEditorCurve(animToBake, binding);

                if (binding.propertyName.Contains("m_LocalPosition"))
                {
                    UpdateVectorArray(ref position, toAddVector, c, size, animToBake.frameRate);
                }
                else if (binding.propertyName.Contains("localEulerAnglesRaw"))
                {
                    UpdateVectorArray(ref rotation, toAddVector, c, size, animToBake.frameRate);
                }
            }
            saveTo.position = position;
            saveTo.rotation = rotation;
            saveTo.secondsPerFrame = 1 / animToBake.frameRate;
            saveTo.animationName = animToBake.name;
            saveTo.frames = size;
        }
    }
}
