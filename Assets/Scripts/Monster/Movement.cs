using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public delegate bool CheckAction();

public class Movement : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float rotSpeed = 360.0f;
    protected UnityAction ArrivedAction;
    protected UnityAction followAction;

    Coroutine moveCo;
    Coroutine rotCo;

    protected NavMeshPath myPath = null;
    //????
    protected IEnumerator MovingToPos(Vector3 point)
    {
        Vector3 moveDir = point - transform.position;
        float moveDist = moveDir.magnitude;
        moveDir.Normalize();

        //yield return StartCoroutine(Rotating(moveDir));
        //yield return new WaitForSeconds(1.0f);
        rotCo = StartCoroutine(Rotating(moveDir));

        while (!Mathf.Approximately(moveDist, 0.0f))
        {
            float delta = Time.deltaTime * moveSpeed;
            if (delta > moveDist) delta = moveDist;
            transform.Translate(moveDir * delta, Space.World);
            moveDist -= delta;
            yield return null;
        }
    }

    IEnumerator Rotating(Vector3 moveDir)
    {
        float angle = Vector3.Angle(transform.forward, moveDir);
        float rotDir = Vector3.Dot(transform.right, moveDir) < 0.0f ? -1.0f : 1.0f;

        while (!Mathf.Approximately(angle, 0.0f))
        {
            float delta = Time.deltaTime * rotSpeed;
            if (delta > angle)
            {
                delta = angle;
            }
            transform.Rotate(Vector3.up * delta * rotDir);
            angle -= delta;
            yield return null;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public virtual Coroutine MoveToPos(Vector3 point)
    {
        /*
        float d = Vector3.Dot(transform.forward, moveDir);
        float r = Mathf.Acos(d);
        // r : pi = y : 180;
        // r / pi = y / 180;
        // (r / pi) * 180 = y;
        angle = r * Mathf.Rad2Deg;//(r / Mathf.PI) * 180.0f;
        */

        //StopAllCoroutines();
        if (moveCo != null)
        {
            StopCoroutine(moveCo);
            moveCo = null;
        }
        if (rotCo != null)
        {
            StopCoroutine(rotCo);
            rotCo = null;
        }
        return moveCo = StartCoroutine(MovingToPos(point));
    }

    public void FollowTarget(Transform target, float dest = 0.0f, CheckAction movalbe = null)
    {
        if (moveCo != null)
        {
            StopCoroutine(moveCo);
            moveCo = null;
        }
        if (rotCo != null)
        {
            StopCoroutine(rotCo);
            rotCo = null;
        }
        moveCo = StartCoroutine(FollowingTarget(target, dest, movalbe));
    }

    IEnumerator FollowingTarget(Transform target, float dest, CheckAction movable)
    {
        while (target != null)
        {
            followAction?.Invoke();
            Vector3 dir = Vector3.zero;
            float dist = 0.0f;

            if (myPath == null) myPath = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, target.position, -1, myPath))
            {
                switch (myPath.status)
                {
                    case NavMeshPathStatus.PathComplete:
                    case NavMeshPathStatus.PathPartial:
                        dir = myPath.corners[1] - myPath.corners[0];
                        dist = dir.magnitude;
                        dir.Normalize();
                        break;
                }
            }


            float delta = Time.deltaTime * moveSpeed;
            if (delta > dist)
            {
                delta = dist;
            }

            if (myPath.corners.Length > 2 || dist > dest)
            {
                if (movable != null && !movable())
                {
                }
                else
                    transform.Translate(dir * delta, Space.World);
            }
            else
            {
                //Arrived
                ArrivedAction?.Invoke();
            }

            float angle = Vector3.Angle(transform.forward, dir);
            float rotDir = Vector3.Dot(transform.right, dir) < 0.0f ? -1.0f : 1.0f;
            delta = Time.deltaTime * rotSpeed;
            if (delta > angle) delta = angle;
            transform.Rotate(Vector3.up * delta * rotDir);

            yield return null;
        }
    }
}
