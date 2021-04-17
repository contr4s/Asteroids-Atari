using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using TMPro;

[RequireComponent(typeof(AsteroidsPool))]
[RequireComponent(typeof(PoliceShipPool))]
[RequireComponent(typeof(ExplosionsPool))]
public class GameManager : Singleton<GameManager>
{  
    private int _score = 0;
    public int Score 
    {       
        get => _score; 
        set
        {
            if (_score < 0)
                Debug.LogError("score can't be less than 0");
            _score = value;
            mangerUI.UpdateScoreBoard(string.Format("Score: {0}", _score));
        } 
    }

    private int _curLevel = 0;
    public int CurLevel { 
        get => _curLevel; 
        private set 
        {
            _curLevel = value;
            mangerUI.UpdateLevelBoard(string.Format("Level: {0}", _curLevel));
        } 
    }

    [Header("Set in Inspector")]
    public AsteroidsScriptableObject asteroidsSO;

    public UIManager mangerUI;

    [SerializeField]
    private float _timeBetweenSpawningPoliceShips= 1f;

    [SerializeField]
    private int _startAsteroidsAmount;
    [SerializeField]
    private int _startPoliceShipAmount;

    private AsteroidsPool _asteroidsPool;
    private PoliceShipPool _policeShipsPool;
    public ExplosionsPool explosionsPool;
    private const float _minDistToPlayerShip = 5;

    protected override void Awake()
    {
        base.Awake();

        _asteroidsPool = GetComponent<AsteroidsPool>();
        _policeShipsPool = GetComponent<PoliceShipPool>();
        explosionsPool = GetComponent<ExplosionsPool>();
    }

    private void Start()
    {
        StartCoroutine(StartGame(mangerUI.TimeOfShowingPreGamePanel));
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

    public void GameOver()
    {
        mangerUI.GameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator StartGame(float timeOfShowingPreGamePanel)
    {
        yield return new WaitForSeconds(timeOfShowingPreGamePanel);
        mangerUI.PregameUI.SetActive(false);
        StartLevel();
    }

    private void StartLevel()
    { 
        for (int i = 0; i < _curLevel + _startAsteroidsAmount; i++)
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
        while (policeShipAmount < _startPoliceShipAmount + _curLevel)
        {
            var policeShip = _policeShipsPool.GetAvailableObject();

            policeShip.transform.position = FindGoodLocation();
            policeShip.gameObject.SetActive(true);
            policeShipAmount++;
            yield return new WaitForSeconds(_timeBetweenSpawningPoliceShips);
        }
    }
}
