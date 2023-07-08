using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

class EmptyItem : IQueueItem
{
    bool finished = false;
    public bool IsFinished()
    {
        return finished;
    }

    public void Update(float dt)
    {
        finished = true;
    }
}

class AttackItem : IQueueItem
{
    Animator animator;

    public AttackItem(Animator anim)
    {
        animator = anim;
        animator.Play("Farmer_Attack");
    }
    public bool IsFinished()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var tim = stateInfo.normalizedTime;
        if (tim > 1.0f && stateInfo.IsName("Farmer_Attack"))
        {
            return true;
        }

        return false;
    }

    public void Update(float dt)
    {
        
    }
}

public class Farmer : MonoBehaviour, ITurnTaker
{
    public GameLoop gameloop;
    public bool isDead;
    public bool hasKilled;
    private Animator animator;
    public IQueueItem[] TakeTurn()
    {
        Transform nearest = null;
        var farmerMovement = GetComponent<Movement>();
        
        foreach (var m in gameloop.moles)
        {
            if (m.ShouldChase())
            {
                if (nearest == null)
                    nearest = m.transform;

                else if ((m.transform.position - transform.position).sqrMagnitude < (nearest.transform.position - transform.position).sqrMagnitude)
                {
                    nearest = m.transform;
                }
            }
        }

        if (nearest == null)
        {
            return new IQueueItem[]{ new EmptyItem() };
        }

        var v = nearest.position - transform.position;

        if (v == Vector3.zero)
        {
            return new IQueueItem[] { new EmptyItem() };
        }

        int x,y;

        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            x = (int)(v.x / Mathf.Abs(v.x));
            y = 0;
        } else
        {
            y = (int)(v.y / Mathf.Abs(v.y));
            x = 0;
        }

        var currentPos = farmerMovement.GetPosition();
        var space = farmerMovement.spaces.CheckGrid(currentPos.x + x, currentPos.y + y);

        if (CheckWalkable(space))
        {
            return new IQueueItem[] { farmerMovement.MoveTo(currentPos.x + x, currentPos.y + y, animator) };
        }

        if (CheckAttackable(space)) return new IQueueItem[] { Attack(space.GetMole()) };

        return new IQueueItem[] { new EmptyItem() };
    }

    bool CheckWalkable(GridInfo space)
    {
        if (space.GetTerrain() != null) return false;
        if (space.GetMole() != null && space.GetMole().aboveground) return false;
        return true;
    }

    bool CheckAttackable(GridInfo space)
    {
        if (space.GetMole() != null && space.GetMole().aboveground) return true;
        return false;
    }

    public void KillMole()
    {
        hasKilled = true;
    }

    public void Kill()
    {
        isDead = true;
        print("Farmer has been killed!");
    }

    IQueueItem Attack(Mole mole)
    {
        return new AttackItem(animator);
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void BeginLevel()
    {
        animator.Play("Farmer_Idle");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
