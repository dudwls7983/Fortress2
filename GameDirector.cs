using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector_TankData
{
    public TankController tank;
    public bool turn;
}

public class GameDirector : MonoBehaviour {

    public int tankCount = 2;
    public readonly int spawnCount = 12;

    public GameDirector_TankData[] tankList;
    public GameObject[] spawnList;

    public static GameDirector instance;

	// Use this for initialization
	void Start () {
        StartCoroutine(GameRules());
        tankList = new GameDirector_TankData[tankCount];
        for (int i = 0; i < tankCount; i++) tankList[i] = new GameDirector_TankData();
        spawnList = new GameObject[spawnCount];
        for (int i = 0; i < spawnCount; i++) spawnList[i] = GameObject.Find("SpawnPoint" + i);

        for (int i = 0; i < tankCount; i++)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/tank"), spawnList[Random.Range(0, spawnCount)].transform.position, new Quaternion());
            if (go == null)
            {
                Debug.LogError("Can't find tank. check tank prefabs");
                continue;
            }
            go.name = "tank" + i;
        }

        instance = this;
    }

    IEnumerator GameRules()
    {
        yield return new WaitForSeconds(1);

        TankController leastDelayedTank = null;
        int leastDelayCount = int.MaxValue;
        for(int i = 0; i < tankCount; i++)
        {
            GameObject go = GameObject.Find("tank" + i);
            if(go == null)
            {
                Debug.LogError("Can't find tank. check tank is valid");
                continue;
            }

            TankController tank = go.GetComponent<TankController>();
            tankList[i].tank = tank;
            tank.ShowMe();

            int delay = tank.tankData.m_iDelay;
            if(delay < leastDelayCount)
            {
                leastDelayCount = delay;
                leastDelayedTank = tank;
            }

            yield return new WaitForSeconds(1);
        }

        TurnTank(leastDelayedTank);
    }

    public void TurnTank(TankController turn)
    {
        for (int i = 0; i < tankCount; i++)
        {
            GameObject go = GameObject.Find("tank" + i);
            if (go == null)
                continue;

            TankController tank = go.GetComponent<TankController>();
            if(turn == tank)
            {
                tank.HasTurn();
                tankList[i].turn = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static TankController GetNextTank(bool forceTurn = true)
    {
        TankController leastDelayedTank = null;
        int leastDelayCount = int.MaxValue, lastIndex = 0, remain_turn = 0, alive_count = 0;
        for (int i = 0; i < instance.tankCount; i++)
        {
            GameObject go = GameObject.Find("tank" + i);
            if (go == null)
            {
                Debug.LogError("Can't find tank. check tank name");
                continue;
            }

            TankController tank = go.GetComponent<TankController>();

            if (tank.tankData.m_isDeadTank)
                continue;

            alive_count++;

            if (instance.tankList[i].turn)
                continue;

            int delay = tank.tankData.m_iDelay;
            if (delay < leastDelayCount)
            {
                leastDelayCount = delay;
                leastDelayedTank = tank;
                lastIndex = i;
            }
            remain_turn++;
        }

        if(alive_count <= 1)
        {
            // Game End
            return null;
        }
        
        // all player had turn.
        if (remain_turn <= 1)
        {
            for (int i = 0; i < instance.tankCount; i++)
            {
                GameObject go = GameObject.Find("tank" + i);
                if (go == null)
                {
                    continue;
                }

                TankController tank = go.GetComponent<TankController>();

                // 죽은 탱크는 배제한다.
                if (tank.tankData.m_isDeadTank)
                    continue;

                instance.tankList[i].turn = false;
            }

        }

        if (remain_turn == 0 && alive_count > 0)
        {
            return GetNextTank(forceTurn);
        }
        

        instance.tankList[lastIndex].turn = forceTurn;
        return leastDelayedTank;
    }
}
