using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : CharacterMovement
{
    public enum State
    {
        Create, Normal, Roam, Battle, Death
    }
    public State myState = State.Create;

    void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Normal:
                {
                    Vector3 dir = Vector3.forward;
                    float angle = Random.Range(0.0f, 360.0f);
                    dir = Quaternion.Euler(0, angle, 0) * dir;
                    float dist = Random.Range(0.1f, 10.0f);
                    dir = dir * dist;
                    Vector3 pos = startPos + dir;
                    MoveTo(pos, () => StartCoroutine(ResetRoaming(Random.Range(1.0f, 3.0f))));
                    ChangeState(State.Roam);
                }
                break;
            case State.Roam:
                break;
            case State.Battle:
                StopAllCoroutines();
                break;
            case State.Death:
                StopAllCoroutines();
                gameObject.GetComponent<Collider>().enabled = false;
                gameObject.GetComponentInChildren<AIPerception>().gameObject.SetActive(false);
                StartCoroutine(DisApearing());
                break;
        }
    }

    IEnumerator ResetRoaming(float t)
    {
        yield return new WaitForSeconds(t);
        ChangeState(State.Normal);
    }

    void StateProcess()
    {
        switch (myState)
        {
            case State.Normal:
                break;
            case State.Battle:
                break;
        }
    }

    Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ////MinimapIcon
        //GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MinimapIcon"),
        //    SceneData.instance.uiMinimap);
        //MinimapIcon icon = obj.GetComponent<MinimapIcon>();
        //icon.SetIcon(transform, Color.red);
        //deathAlarm += () => Destroy(icon.gameObject);
        ////HP bar
        //obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/HpBar"),
        //    SceneData.instance.uiHpBarRoot);
        //HpBarUI hpUI = obj.GetComponent<HpBarUI>();
        //if (hpUI != null)
        //{
        //    hpUI.Initialize(transform.Find("HpBarPoint"));
        //    battleData.changeHp += () => hpUI.mySlider.value =
        //    battleData.CurHp / battleData.MaxHealPoint;
        //    battleData.CurHp = battleData.MaxHealPoint;
        //    deathAlarm += () => Destroy(hpUI.gameObject);
        //}
        deathAlarm += () => ChangeState(State.Death);

        startPos = transform.position;
        ChangeState(State.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
    public void OnBattle(Transform target)
    {
        ChangeState(State.Battle);
        Follow(target);
        BattleSystem bs = target.GetComponent<BattleSystem>();
        if (bs != null)
        {
            bs.deathAlarm += () => ChangeState(State.Normal);
        }
    }

    public void LostTarget()
    {
        ChangeState(State.Normal);
    }

}
