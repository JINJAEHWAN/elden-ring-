using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class CharacterMovement : BattleSystem
{
    Coroutine moveCo = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override Coroutine MoveToPos(Vector3 point)
    {
        return MoveToPos(point, null);
    }

    public Coroutine MoveToPos(Vector3 point, UnityAction done)
    {
        if (moveCo != null)
        {
            StopCoroutine(moveCo);
            moveCo = null;
        }
        return moveCo = StartCoroutine(Moving(point, done));
    }

    IEnumerator Moving(Vector3 point, UnityAction done)
    {
        myAnim.SetBool("walk", true);
        yield return StartCoroutine(MovingToPos(point));
        myAnim.SetBool("walk", false);
        done?.Invoke();
    }

    protected void Follow(Transform target)
    {
        myAttackTarget = target;
        BattleSystem bs = myAttackTarget.GetComponent<BattleSystem>();
        if (bs != null)
        {
            bs.deathAlarm += () => StopAllCoroutines();
        }

        myAnim.SetBool("walk", true);
        followAction = () =>
        {
            myAnim.SetBool("walk", true);
            if (!myAnim.GetBool("attack")) battleData.attackTime -= Time.deltaTime;
        };
        ArrivedAction = AttackCheck;
        FollowTarget(target, battleData.AttackRange, () => !myAnim.GetBool("attack"));
    }

    void AttackCheck()
    {
        if (battleData.attackTime <= 0.0f)
        {
            myAnim.SetTrigger("OnAttack");
            battleData.attackTime = battleData.AttackDelay;
        }
        else
        {
            myAnim.SetBool("walk", false);
        }
    }

    protected IEnumerator DisApearing()
    {
        yield return new WaitForSeconds(3.0f);
        float dist = 2.0f;
        while (dist > 0.0f)
        {
            float delta = Time.deltaTime * 0.5f;
            transform.Translate(Vector3.down * delta, Space.World);
            dist -= delta;
            yield return null;
        }
        Destroy(gameObject);
    }
    public void MoveTo(Vector3 pos)
    {
        MoveTo(pos, null);
    }
    public void MoveTo(Vector3 pos, UnityAction done)
    {
        if (myPath == null) myPath = new NavMeshPath();
        StopAllCoroutines();
        if (NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, myPath))
        {
            switch (myPath.status)
            {
                case NavMeshPathStatus.PathComplete:
                case NavMeshPathStatus.PathPartial:
                    MoveByPath(myPath.corners, done);
                    break;
                case NavMeshPathStatus.PathInvalid:
                    done?.Invoke();
                    break;
            }
        }
        else
            done?.Invoke();
    }

    protected void MoveByPath(Vector3[] path, UnityAction done = null)
    {
        StartCoroutine(MovingByPath(path, done));
    }

    IEnumerator MovingByPath(Vector3[] path, UnityAction done)
    {
        int i = 1;
        while (i < path.Length)
        {
            yield return MoveToPos(path[i++]);
        }
        done?.Invoke();
    }
    protected void DrawPath()
    {
        if (myPath == null) return;
        for (int i = 0; i < myPath.corners.Length - 1; ++i)
        {
            Debug.DrawLine(myPath.corners[i], myPath.corners[i + 1], Color.red);
        }
    }
}
