using UnityEngine;

public class AnimationController : AnimProperty
{
    void onRolling()
    {
        myAnim.SetBool("isRolling", true);
    }

    void offRolling()
    {
        myAnim.SetBool("isRolling", false);
    }
}
