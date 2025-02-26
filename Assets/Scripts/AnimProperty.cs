using UnityEngine;

public class AnimProperty : MonoBehaviour
{
    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
            }
            if (_anim == null)
            {
                _anim = GetComponentInChildren<Animator>();
            }
            return _anim;
        }
        set { }
    }
}
