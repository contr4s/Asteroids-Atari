using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using TMPro;

[RequireComponent(typeof(AsteroidsPool))]
[RequireComponent(typeof(PoliceShipPool))]
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
            _scoreBoard.text = string.Format("Score: {0}", _score);
        } 
    }

    static public int curLevel = 0;

    [Header("Set in Inspector")]
    public AsteroidsScriptableObject asteroidsSO;

    public TextMeshProUGUI livesBoard;
    [SerializeField] private GameObject[] UIExtraLives;
    [SerializeField] private TextMeshProUGUI _scoreBoard;

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

    private void Start()
    {
        StartCoroutine(StartGame());
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

    public void RefreshUILives(int lives)
    {
        for (int i = 0; i < UIExtraLives.Length; i++)
        {
            if (i < lives)
            { 
                UIExtraLives[i].SetActive(true);
            }
            else
            {
                UIExtraLives[i].SetActive(false);
            }
        }
    }

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
