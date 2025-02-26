using UnityEngine;

public class RootMotion : AnimProperty
{
    public LayerMask crashMask;
    Vector3 deltaPosition = Vector3.zero;
    Quaternion deltaRotation = Quaternion.identity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnAnimatorMove()
    {
        deltaRotation *= myAnim.deltaRotation;
        deltaPosition += myAnim.deltaPosition;
    }

    private void FixedUpdate()
    {
        transform.parent.position += deltaPosition;
        transform.parent.rotation *= deltaRotation;
        deltaPosition = Vector3.zero;
        deltaRotation = Quaternion.identity;
    }
}
