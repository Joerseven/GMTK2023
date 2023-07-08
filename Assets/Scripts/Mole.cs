using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class PopupItem : IQueueItem
{
    Animator info;

    public PopupItem(Animator animator)
    {
        info = animator;
        info.speed = 1;
        info.Play("Mole_Up_a");
    }

    public bool IsFinished()
    {
        var stateInfo = info.GetCurrentAnimatorStateInfo(0);
        bool isFinshed = stateInfo.IsName("Mole_Idle_a");
        return isFinshed;
    }

    public void Update(float dt)
    {
        
    }
}

public class RetreatItem : IQueueItem
{
    Animator anim;
    public RetreatItem(Animator animator)
    {
        anim = animator;
        anim.Play("Mole_Down_a");
    }
    public bool IsFinished()
    {
        var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        var tim = stateInfo.normalizedTime;
        if (tim > 1.0f && stateInfo.IsName("Mole_Down_a"))
        {
            return true;
        }

        return false;
    }

    public void Update(float dt)
    {

    }
}

public class Mole : MonoBehaviour, ITurnTaker
{
    public bool aboveground;
    [SerializeField]
    private int _counter;
    public TextMesh textLabel;
    public int popupCounter {
        get
        {
            return _counter;
        }

        set
        {
            _counter = value;
            textLabel.text = value.ToString();
        }
    }
    public int popupTracker;
    private Animator animator;
    public IQueueItem[] TakeTurn()
    {

        if (!aboveground)
        {
            popupCounter -= 1;
        }

        if (popupCounter == 0)
        {
            if (!aboveground)
            {
                return Popup();
            }

            return Retract();
        }

        return new IQueueItem[] { new EmptyItem() };
    }

    IQueueItem[] Popup()
    {
        aboveground = true;
        GetComponent<SpriteRenderer>().enabled = true;
        popupCounter = popupTracker;

        var farmer = CheckFarmer();
        if (farmer != null)
        {
            farmer.Kill();
            return new IQueueItem[] { new EmptyItem() };
        }  else
        {
            return new IQueueItem[] { new PopupItem(animator) };
        }
    }

    IQueueItem[] Retract()
    {
        aboveground = false;
        popupCounter = popupTracker;
        textLabel.gameObject.SetActive(true);
        return new IQueueItem[] { new RetreatItem(animator) };
    }



    private Farmer CheckFarmer()
    {
        var movementInfo = GetComponent<Movement>();
        var gridPos = movementInfo.GetPosition();
        var onSpace = movementInfo.spaces.CheckGrid(gridPos.x, gridPos.y);

        return onSpace.GetFarmer();
    }

    public bool ShouldChase()
    {
        return aboveground;
    }

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void BeginLevel()
    {
        animator.speed = 1;
        animator.Play("Mole_Idle_a");
        aboveground = false;
        popupCounter = popupTracker;

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }


    public void StartTurns()
    {
        animator.Play("Mole_Up_a");
        animator.speed = 0;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
