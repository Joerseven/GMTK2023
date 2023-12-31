using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting.FullSerializer;
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
    SpriteRenderer s;

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

public class StuckItem : IQueueItem
{
    bool done;
    float elapsed;
    float target;

    public StuckItem()
    {
        done = false;
        target = 2.0f;
        elapsed = 0.0f;
        Debug.Log("I'm stuck!");
    }
    public bool IsFinished()
    {
        return done;
    }

    public void Update(float dt)
    {
        elapsed += dt;

        if (elapsed >= target)
        {
            elapsed = target;
            done = true;
        }
    }
}

public class NoMole : IQueueItem
{
    bool done;
    float elapsed;
    float target;

    public NoMole()
    {
        done = false;
        target = 2.0f;
        elapsed = 0.0f;
        Debug.Log("No Mole >:(");
    }
    public bool IsFinished()
    {
        return done;
    }

    public void Update(float dt)
    {
        elapsed += dt;

        if (elapsed >= target)
        {
            elapsed = target;
            done = true;
        }
    }
}

public class Farmer : MonoBehaviour, ITurnTaker
{
    public GameLoop gameloop;
    public bool isDead;
    public bool hasKilled;
    private Animator animator;
    private SpriteRenderer sRenderer;
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
            return new IQueueItem[]{ new NoMole() };
        }

        var v = nearest.position - transform.position;
        if (v.x > 0)
        {
            sRenderer.flipX = false;
        } else
        {
            sRenderer.flipX = true;
        }
        var currentPos = farmerMovement.GetPosition();

        if (nearest.position.x > transform.position.x)
        {
            v -= new Vector3(1, 0, 0);
        } else
        {
            v += new Vector3(1, 0, 0);
        }

        if (v == Vector3.zero)
        {
            var leftHit = farmerMovement.spaces.CheckGrid(currentPos.x + 1, currentPos.y);
            var rightHit = farmerMovement.spaces.CheckGrid(currentPos.x - 1, currentPos.y);

            if (CheckAttackable(rightHit))
            {
                sRenderer.flipX = true;
                return new IQueueItem[] { Attack(rightHit.GetMole()) };
            }
            if (CheckAttackable(leftHit))
            {
                sRenderer.flipX = false;
                return new IQueueItem[] { Attack(leftHit.GetMole()) };
            }

            return new IQueueItem[] { new EmptyItem() };
        }

        int x = 0,y = 0;

        if (Mathf.Abs(v.x) >= 1)
        {
            x = (int)(v.x / Mathf.Abs(v.x));
            y = 0;
        }

        if (Mathf.Abs(v.y) >= 1)
        {
            y = (int)(v.y / Mathf.Abs(v.y));
            x = 0;
        }
        
        var space = farmerMovement.spaces.CheckGrid(currentPos.x + x, currentPos.y + y);    

        if (CheckWalkable(space))
        {
            return new IQueueItem[] { farmerMovement.MoveTo(currentPos.x + x, currentPos.y + y, animator) };
        }

        return new IQueueItem[] { new EmptyItem() };
    }

    bool CheckWalkable(GridInfo space)
    {
        // Farmer can move over wheat
        //if (space.GetTerrain() != null) return false;
        if (space.GetMole() != null && space.GetMole().aboveground) return false;
        return true;
    }

    bool CheckAttackable(GridInfo v)
    {
        if (v.GetMole() != null)
        {
            print(v.GetMole());
            if (v.GetMole().aboveground)
            {
                return true;
            }
        }
        
        
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
        sRenderer = GetComponent<SpriteRenderer>();
    }

    public void BeginLevel()
    {
        animator.Play("Farmer_Idle");
        isDead = false;
        hasKilled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sRenderer.sortingOrder = 4 - (int)(transform.position.y);
    }
}
