using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpAnimator : MonoBehaviour
{
    private float targetReveal = 1;
    Material currMaterial;
    // Start is called before the first frame update
    void OnEnable()
    {
        currMaterial = GetComponent<MeshRenderer>().material;
    }

    public void TransitionReveal(float reveal) {
        targetReveal = reveal;
    }

    public void SetReveal(float reveal) {
        targetReveal = reveal;
        currMaterial.SetFloat("_Reveal", targetReveal);
    }

    // Update is called once per frame
    void Update()
    {
        currMaterial.SetFloat("_Reveal", Mathf.Lerp(currMaterial.GetFloat("_Reveal"), targetReveal, Time.deltaTime * 0.06f));
    }
}
