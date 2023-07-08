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

    public Farmer farmer;
    public List<Mole> moles;

    // Start is called before the first frame update


    void Start()
    {
        events = new Queue<IQueueItem>();
        turns = new Queue<ITurnTaker>();


        int i = 0;
        Level level = levels[currentLevel];
        foreach (var m in molePool)
        {
            
            if (i < level.MolesStart.Count)
            {
                m.gameObject.SetActive(true);
                moles.Add(molePool[i]);
                m.GetComponent<Movement>().SnapTo(level.MolesStart[i].x, level.MolesStart[i].y);
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
        turns.Enqueue(farmer);

    }

    IQueueItem[] GetNextTurn()
    {
        turns.Enqueue(turns.Dequeue());
        return turns.Peek().TakeTurn();
    }

    // Update is called once per frame
    void Update()
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
}
