using System.Collections;
using System.Collections.Generic;
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

public class Farmer : MonoBehaviour, ITurnTaker
{
    public GameLoop gameloop;
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

        if (!CheckWalkable(space)) return new IQueueItem[] { new EmptyItem() };


        if (!space.HasItem()) return new IQueueItem[] { farmerMovement.MoveTo(currentPos.x + x, currentPos.y + y) };

        var thing = space.GetItem();

        if (thing.TryGetComponent<Mole>(out Mole mole))
        {
            return new IQueueItem[] { Attack(mole) };
        }

        return new IQueueItem[] { new EmptyItem() };
    }

    bool CheckWalkable(GridInfo space)
    {
        if (space.GetTerrain() != null) return false;
        if (space.GetMole().aboveground) return false;
        return true;
    }

    IQueueItem Attack(Mole mole)
    {
        print("Wack da mole");
        return new EmptyItem();
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
