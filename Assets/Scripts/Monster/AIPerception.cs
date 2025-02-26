using UnityEngine;
using UnityEngine.Events;

public class AIPerception : MonoBehaviour
{
    public LayerMask enemyMask;
    public UnityEvent<Transform> findEnemy;
    public UnityEvent lostEnemy;
    public Transform myTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (myTarget != null) return;
        if ((1 << other.gameObject.layer & enemyMask) != 0)
        {
            findEnemy?.Invoke(myTarget = other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & enemyMask) != 0)
        {
            if (myTarget == other.transform)
            {
                lostEnemy?.Invoke();
                myTarget = null;
            }
        }
    }
}
