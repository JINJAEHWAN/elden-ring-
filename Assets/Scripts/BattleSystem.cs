using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct BattleData
{
    public float AttackRange;
    public float AttackPoint;
    public float AttackDelay;
    public float MaxHealPoint;
    public event UnityAction changeHp;
    [SerializeField] float _curHp;
    public float CurHp
    {
        get => _curHp;
        set
        {
            _curHp = value;
            changeHp?.Invoke();
        }
    }
    public float attackTime;
}

public interface IDamage
{
    void OnDamage(float dmg);
}

public class BattleSystem : Movement, IDamage
{
    public event UnityAction deathAlarm;
    [SerializeField] protected BattleData battleData;
    public Animator myAnim;
    protected Transform myAttackTarget;
    public LayerMask attackMask;
    public void OnDamage(float dmg)
    {
        battleData.CurHp -= dmg;
        if (dmg > 0.0f)
        {
            if (battleData.CurHp > 0.0f)
            {
                myAnim.SetTrigger("OnDamage");
            }
            else
            {
                deathAlarm?.Invoke();
                myAnim.SetTrigger("OnDead");
            }
        }
    }


    //public void onAttack()
    //{
    //    Collider[] list = Physics.OverlapSphere(transform.position + transform.forward * battleData.AttackRange, battleData.AttackRange, attackMask);

    //    foreach (Collider c in list)
    //    {
    //        c.GetComponent<BattleSystem>()?.OnDamage(battleData.AttackPoint);
    //    }
    //}

    public void OnAttack()
    {
        float dist = Vector3.Distance(transform.position, myAttackTarget.position);
        if (dist - GetComponent<CapsuleCollider>().radius <= battleData.AttackRange ||
            Mathf.Approximately(dist, battleData.AttackRange))
        {
            myAttackTarget?.GetComponent<BattleSystem>().OnDamage(battleData.AttackPoint);
        }
    }
}
