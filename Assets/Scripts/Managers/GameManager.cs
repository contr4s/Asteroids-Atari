using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

[RequireComponent(typeof(AsteroidsPool))]
[RequireComponent(typeof(PoliceShipPool))]
public class GameManager : Singleton<GameManager>
{   
    static public int score = 0;

    static public int curLevel = 0;

    [Header("Set in Inspector")]
    public AsteroidsScriptableObject asteroidsSO;  

    public Text scoreBoard;

    [SerializeField]
    private float _timeOfShowingPreGamePanel = 1f;
    [SerializeField]
    private float _timeBetweenSpawningPoliceShips= 1f;

    [SerializeField]
    private int _startAsteroidsAmount;
    [SerializeField]
    private int _startPoliceShipAmount;

    private AsteroidsPool _asteroidsPool;
    private PoliceShipPool _policeShipPool;
    private const float _minDistToPlayerShip = 5;

    protected override void Awake()
    {
        base.Awake();

        _asteroidsPool = GetComponent<AsteroidsPool>();
        _policeShipPool = GetComponent<PoliceShipPool>();
    }

    void Start()
    {
        StartCoroutine(StartGame());
    }

    //void OnGUI()
    //{
    //    scoreBoard.text = "Score: " + score.ToString();
    //}

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(_timeOfShowingPreGamePanel);
        StartLevel();
    }

    private void StartLevel()
    { 
        for (int i = 0; i < curLevel + _startAsteroidsAmount; i++)
        {
            SpawnParentAsteroid();
        }

        StartCoroutine(SpawnPoliceShips());
    }

    private void SpawnParentAsteroid()
    {
        Vector3 pos = FindGoodLocation();

        InitAsteroid(pos, 0);
    }

    private Vector3 FindGoodLocation()
    {
        Vector3 pos = ScreenBounds.randomOnScreenLoc;
        while ((pos - PlayerShip.position).magnitude < _minDistToPlayerShip)
        {
            pos = ScreenBounds.randomOnScreenLoc;
        }
        return pos;
    }

    public void SpawnChildAsteroid(int generation, Vector3 pos)
    {
        if (generation >= asteroidsSO.maxAsteroidGeneration)
            return;

        for (int i = 0; i < asteroidsSO.numSmallerAsteroidsToSpawn; i++)
        {
            InitAsteroid(pos, generation);
        } 
    }

    private void InitAsteroid(Vector3 pos, int generation)
    {
        Asteroid ast = _asteroidsPool.GetAvailableObject();

        ast.transform.position = pos;
        ast.size = asteroidsSO.initialSize * math.pow(asteroidsSO.asteroidScale, generation);
        ast.generation = generation;
        ast.gameObject.SetActive(true);
    }

    private IEnumerator SpawnPoliceShips()
    {
        yield return new WaitForSeconds(_timeBetweenSpawningPoliceShips);

        int policeShipAmount = 0;
        while (policeShipAmount < _startPoliceShipAmount + curLevel)
        {
            var policeShip = _policeShipPool.GetAvailableObject();

            policeShip.transform.position = FindGoodLocation();
            policeShip.gameObject.SetActive(true);
            policeShipAmount++;
            yield return new WaitForSeconds(_timeBetweenSpawningPoliceShips);
        }
    }
}
