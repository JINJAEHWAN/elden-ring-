using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;

public class AnimationController : AnimProperty
{

    public UnityEvent<bool> comboCheck;
    public UnityEvent attackAction;
    public UnityEvent RollCheck;
    int hashAttack;

    private void Start()
    {
        hashAttack = Animator.StringToHash("IsAttacking");
    }


    public void onRolling()
    {
        myAnim.SetBool("isRolling", true);
    }

    public void OnAttack()
    {
        attackAction?.Invoke();
    }

    public void offRolling()
    {
        myAnim.SetBool("isRolling", false);
    }

    public void AttackStart()
    {
        myAnim.SetBool(hashAttack, true);
    }

    public void AttackEnd()
    {
        myAnim.SetBool(hashAttack, false);
    }

    public void CombocheckStart()
    {
        comboCheck?.Invoke(true);
    }

    public void CombocheckEnd()
    {
        comboCheck?.Invoke(false);
    }
}
