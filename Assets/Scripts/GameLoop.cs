using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameLoop : MonoBehaviour
{

    private Queue<IQueueItem> events;
    private Queue<ITurnTaker> turns;

    [SerializeField]
    private List<Level> levels;
    [SerializeField]
    private Mole[] molePool;
    private int currentLevel = 0;

    private int levelState = 0;

    public Farmer farmer;
    public List<Mole> moles;

    // Start is called before the first frame update


    void Start()
    {
        BeginLevel();
    }

    public void BeginLevel()
    {
        events = new Queue<IQueueItem>();
        turns = new Queue<ITurnTaker>();
        moles = new List<Mole>();

        levelState = 0;


        int i = 0;
        Level level = levels[currentLevel];
        foreach (var m in molePool)
        {

            if (i < level.MolesStart.Count)
            {
                m.gameObject.SetActive(true);
                m.popupTracker = level.MolesStart[i];
                m.GetComponent<Collider2D>().enabled = true;
                m.BeginLevel();

                moles.Add(molePool[i]);
                GetComponent<MovementSpaces>().ShelfMole(m);
            }
            else
            {
                m.gameObject.SetActive(false);
            }

            i++;
        }

        //Farmer
        var farmerPos = farmer.GetComponent<Movement>();
        farmerPos.SnapTo(level.FarmerStart.x, level.FarmerStart.y);
    }

    public void StartTurns()
    {

        if (!GetComponent<MovementSpaces>().CheckShelfEmpty()) return;
        print("Starting Game");
        levelState = 1;

        foreach (var m in moles)
        {
            m.StartTurns();
        }


        foreach (var m in moles)
        {
            turns.Enqueue(m);
            turns.Enqueue(farmer);
        }
            
    }

    IQueueItem[] GetNextTurn()
    {
        turns.Enqueue(turns.Dequeue());
        print(turns.Peek());
        return turns.Peek().TakeTurn();
    }

    void UpdateQueueItems()
    {
        if (events.Count == 0)
        {
            foreach (var i in GetNextTurn())
                events.Enqueue(i);
        };

        var current = events.Peek();

        if (current.IsFinished())
        {
            events.Dequeue();
            this.Update();
        }

        current.Update(Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelState == 1)
        {
            UpdateQueueItems();
        }
    }
}
