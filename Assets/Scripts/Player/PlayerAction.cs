using System.Collections;
using UnityEngine;

public class PlayerAction : AnimProperty
{
    public Transform myModel;
    bool IsComboCheck = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 moveDir = myModel.localRotation * Vector3.forward;

        float angle = Vector3.Angle(moveDir, inputDir);

        float rotDir = Vector3.Dot(inputDir, myModel.localRotation * Vector3.right) < 0.0f ? -1.0f : 1.0f; // ���û��� ������ ���ϱ�

        float delta = Time.deltaTime * 720.0f;
        if (delta > angle) delta = angle;

        myModel.Rotate(Vector3.up * delta * rotDir);
        myAnim.SetFloat("Speed", inputDir.magnitude);

        if (Input.GetKeyDown(KeyCode.Space) && !myAnim.GetBool("isRolling") && !myAnim.GetBool("isCombo"))
        {
            myAnim.SetBool("isRolling", true);
            myAnim.SetTrigger("isRoll");
        }

        if(Input.GetMouseButtonDown(0) && !myAnim.GetBool("isCombo") && !myAnim.GetBool("isRolling"))
        {
            myAnim.SetBool("isCombo", true);
            myAnim.SetTrigger("isAttack");
        }

        if (Input.GetKey(KeyCode.LeftShift)  && !myAnim.GetBool("isRolling") && !myAnim.GetBool("isCombo"))
        {
            myAnim.SetBool("isRunning", true);
        }
        else {
            myAnim.SetBool("isRunning", false);
        }
    }

    public void CombCheck(bool isStart)
    {
        if (isStart)
        {
            StartCoroutine(ComboChecking());
        }
        else
        {
            IsComboCheck = false;
        }
    }

    IEnumerator ComboChecking()
    {
        IsComboCheck = true;
        int inputCount = 0;
        while (IsComboCheck)
        {
            if (Input.GetMouseButtonDown(0))
            {
                inputCount++;
            }
            yield return null;
        }
        if (inputCount == 0)
        {
            myAnim.SetBool("isCombo", false);
        }
    }
}
