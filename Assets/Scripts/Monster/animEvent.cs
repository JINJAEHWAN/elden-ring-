using UnityEngine;
using UnityEngine.Events;

public class animEvent : AnimProperty
{

    public UnityEvent attackAction;
    public UnityEvent skillAction;
    public UnityEvent<bool> comboCheck;

    int hashAttack;
    private void Start()
    {
        hashAttack = Animator.StringToHash("attack");
    }

    public void OnAttack()
    {
        attackAction?.Invoke();
    }

    public void OnSkill()
    {
        skillAction?.Invoke();
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
