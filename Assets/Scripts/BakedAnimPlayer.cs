using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakedAnimPlayer : MonoBehaviour
{
    public bool getNegative = true;

    public List<BakedAnimation> animsToPlay;
    private BakedAnimation _animation;
    private int _frame;
    private float _timeToNextFrame;
    private bool _isPlaying = false;

    private GameObject geometry;
    private Vector3 geometryOffset;

    // Start is called before the first frame update
    void Start()
    {
        geometry = GameObject.Find("Geometry");
        geometryOffset = geometry.transform.position;
    }

    public void StartAnim(string currAnim) {
        _animation = animsToPlay.Find(anim => anim.animationName == currAnim);
        if (_animation != null)
        {
            _frame = 0;
            _timeToNextFrame = 0;
            _isPlaying = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlaying)
        {
            _timeToNextFrame += Time.deltaTime;
            Debug.Log(_frame);
            if (_timeToNextFrame >= _animation.secondsPerFrame)
            {
                _timeToNextFrame = 0;
                _frame += 1;
                if (_frame >= animsToPlay.Count) {
                    _isPlaying = false;
                }
            }
            geometry.transform.position = geometryOffset + (getNegative ? -1 : 1) * _animation.position[_frame];
            geometry.transform.rotation = Quaternion.Euler((getNegative ? -1 : 1) * _animation.rotation[_frame]);
        }
    }
}
