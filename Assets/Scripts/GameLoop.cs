using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameLoop : MonoBehaviour
{

    private Queue<IQueueItem> events;
    private Queue<ITurnTaker> turns;
    public Movement farmer;
    // Start is called before the first frame update


    void Start()
    {
        events = new Queue<IQueueItem>();
        // Just for testing purposes.
        farmer.SnapTo(0, 1);
        events.Enqueue(farmer.MoveTo(1, 1));
        events.Enqueue(farmer.MoveTo(2, 1));
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
