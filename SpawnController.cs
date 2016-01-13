using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour
{
    //objects/classes
    private GameObject bugPrefab;
    private PlayerController playerController;
    private UIController uiController;

    //members/variables
    private int _nextSpawn;
    private int _prevSpawn;
    private float _spawnWait;
    private const float _startWait = 1.0f;
    private const float _minSpawnWait = 0.5f;
    private const float _maxSpawnWait = 1.0f;
    private int[] _spawnPositionX = new int[] { -16, -16, 0, 16, 16 };
    private int[] _spawnPositionY = new int[] { 1, 11, 11, 11, 1 };
    private int[] _spawnRotationZ = new int[] { 90, 45, 0, -45, -90 };

    //called before start: get references
    private void Awake ()
    {
        bugPrefab = (GameObject)Resources.Load("Bug");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        uiController = GameObject.Find("Player").GetComponent<UIController>();
    }

    //initalise members
    private void Start ()
    {
        _prevSpawn = -1;
    }

    //coroutine: can suspend its execution until the given yield instruction finishes
    private IEnumerator SpawnBugs()
    {
        yield return new WaitForSeconds(_startWait);

        for (int i = 0; i < playerController.TotalBugs; i++)
        {
            uiController.UpdateBugUI(playerController.Bugs, playerController.TotalBugs, true);
            //dont spawn bug in the same place as last time
            _nextSpawn = Random.Range(0, 5);
            if(_prevSpawn != -1)
            {
                if(_nextSpawn == _prevSpawn)
                {
                    do
                    {
                        _nextSpawn = Random.Range(0, 5);
                    } 
                    while(_nextSpawn == _prevSpawn);
                }
            }

            Instantiate(bugPrefab, new Vector3(_spawnPositionX[_nextSpawn], _spawnPositionY[_nextSpawn], 0.0f), Quaternion.Euler(0.0f, 0.0f, _spawnRotationZ[_nextSpawn]));
            _prevSpawn = _nextSpawn;

            //more bugs = less time between spawns.
            _spawnWait = Mathf.Clamp(10.0f / playerController.Bugs, _minSpawnWait, _maxSpawnWait);

            yield return new WaitForSeconds(_spawnWait);
        }

        yield return new WaitForSeconds(1.0f);
        playerController.Testing = false;
        playerController.ApplicationRenderer.color = Color.white;
        uiController.UpdateBugUI(false);
    }
}
