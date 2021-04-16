using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

[RequireComponent(typeof(AsteroidsPool))]
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
    private int _startAsteroidsAmount;

    private AsteroidsPool _asteroidsPool;
    private const float _minAsteroidDistToPlayerShip = 5;

    protected override void Awake()
    {
        base.Awake();

        _asteroidsPool = GetComponent<AsteroidsPool>();
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

        curLevel++;       
    }

    private void SpawnParentAsteroid()
    {
        Asteroid ast = _asteroidsPool.GetAvailableObject();

        Vector3 pos = ScreenBounds.randomOnScreenLoc;
        while ((pos - PlayerShip.position).magnitude < _minAsteroidDistToPlayerShip)
        {
            pos = ScreenBounds.randomOnScreenLoc;
        }

        ast.transform.position = pos;
        ast.size = asteroidsSO.initialSize;
        ast.generation = 0;
        ast.gameObject.SetActive(true);
    }

    public void SpawnChildAsteroid(int generation, Vector3 pos)
    {
        if (generation >= asteroidsSO.maxAsteroidGeneration)
            return;

        for (int i = 0; i < asteroidsSO.numSmallerAsteroidsToSpawn; i++)
        {
            Asteroid ast = _asteroidsPool.GetAvailableObject();

            ast.transform.position = pos;
            ast.size = asteroidsSO.initialSize * math.pow(asteroidsSO.asteroidScale, generation);
            ast.generation = generation;
            ast.gameObject.SetActive(true);
        } 
    }
}
