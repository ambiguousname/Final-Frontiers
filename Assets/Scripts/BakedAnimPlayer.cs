using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakedAnimPlayer : MonoBehaviour
{
    public bool getNegative = true;

    public GameObject objectToAnimate;

    public List<BakedAnimation> animsToPlay;
    private BakedAnimation _animation;
    private int _frame;
    private float _timeToNextFrame;
    private bool _isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
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
            if (_timeToNextFrame >= _animation.secondsPerFrame)
            {
                _timeToNextFrame = 0;
                _frame += 1;

                if (_frame >= _animation.frames)
                {
                    _isPlaying = false;
                }
                else {
                    objectToAnimate.transform.position += (getNegative ? -1 : 1) * _animation.position[_frame];

                    objectToAnimate.transform.RotateAround(this.transform.position, Vector3.right, (getNegative ? -1 : 1) * _animation.rotation[_frame][0]);
                    objectToAnimate.transform.RotateAround(this.transform.position, Vector3.up, (getNegative ? -1 : 1) * _animation.rotation[_frame][1]);
                    objectToAnimate.transform.RotateAround(this.transform.position, Vector3.forward, (getNegative ? -1 : 1) * _animation.rotation[_frame][2]);
                }
            }
        }
    }
}
