using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(AsteroidsPool))]
public class AsteroidsManager: MonoBehaviour
{
    private AsteroidsPool _asteroidsPool;
    private AsteroidsScriptableObject asteroidsSO;

    private void Awake()
    {
        _asteroidsPool = GetComponent<AsteroidsPool>();
    }

    private void Start()
    {
        asteroidsSO = GameManager.S.asteroidsSO;
    }

    private void InitAsteroid(Vector3 pos, int generation)
    {
        Asteroid ast = _asteroidsPool.GetAvailableObject();

        ast.transform.position = pos;
        ast.size = asteroidsSO.initialSize * math.pow(asteroidsSO.asteroidScale, generation);
        ast.generation = generation;
        ast.gameObject.SetActive(true);
    }

    public void SpawnParentAsteroid()
    {
        Vector3 pos = GameManager.FindGoodLocation();

        InitAsteroid(pos, 0);
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
}
