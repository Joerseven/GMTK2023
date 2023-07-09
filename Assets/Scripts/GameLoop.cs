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
    [SerializeField]
    private Obstacle[] obstaclePool;
    private int currentLevel = 0;

    private int levelState = 0;

    public Farmer farmer;
    public List<Mole> moles;

    public RetryScreen retryscreen;
    public NextLevel nextscreen;
    public GameObject victoryscreen;

    // Start is called before the first frame update


    void Start()
    {
        BeginLevel();
    }

    public void BeginLevel()
    {

        gameObject.SetActive(true);

        events = new Queue<IQueueItem>();
        turns = new Queue<ITurnTaker>();
        moles = new List<Mole>();

        levelState = 0;

        var spaces = GetComponent<MovementSpaces>();

        spaces.BeginLevel();


        int i = 0;
        Level level = levels[currentLevel];


        foreach (var o in obstaclePool)
        {
            if (i < level.Obstacles.Count)
            {
                o.gameObject.SetActive(true);
                var gridSpace = spaces.CheckGrid(level.Obstacles[i].x, level.Obstacles[i].y);
                gridSpace.SetItem(o.gameObject);
                o.transform.position = GetComponent<Grid>().GetCellCenterLocal(new Vector3Int(level.Obstacles[i].x, level.Obstacles[i].y, 0));
                o.GetComponent<SpriteRenderer>().sortingOrder = 4 - (int)(o.transform.position.y);

            }
            else
            {
                o.gameObject.SetActive(false);
            }
            i++;
        }

        i = 0;

        foreach (var m in molePool)
        {

            if (i < level.MolesStart.Count)
            {
                m.gameObject.SetActive(true);
                m.popupTracker = level.MolesStart[i];
                m.SetAboveGround(level.MolesGround[i]);
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
        farmer.BeginLevel();
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

        turns.Enqueue(farmer);

        foreach (var m in moles)
        {
            turns.Enqueue(m);
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
            UpdateQueueItems();
        }

        current.Update(Time.deltaTime);
    }

    void Retry()
    {
        gameObject.SetActive(false);
        retryscreen.gameObject.SetActive(true);
    }

    void NextLevel()
    {
        currentLevel += 1;
        gameObject.SetActive(false);
        if (currentLevel >= levels.Count)
        {
            WinGame();
            return;
        }
        nextscreen.gameObject.SetActive(true);
    }

    void WinGame()
    {
        victoryscreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelState == 1)
        {
            UpdateQueueItems();
            if (farmer.hasKilled)
            {
                levelState = 0;
                Retry();
            }
            if (farmer.isDead)
            {
                levelState = 0;
                NextLevel();
            }
        }
    }
}
