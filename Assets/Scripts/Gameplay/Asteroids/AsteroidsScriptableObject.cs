﻿using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/AsteroidsSO", fileName = "AsteroidsSO.asset")]
[System.Serializable]
public class AsteroidsScriptableObject: ScriptableObject
{
    public static AsteroidsScriptableObject S;

    public AsteroidsScriptableObject()
    {
        S = this;
    }

    public float minVel = 5;
    public float maxVel = 10;
    public float maxAngularVel = 10;
    public int initialSize = 3;
    public float asteroidScale = 0.75f;
    public int numSmallerAsteroidsToSpawn = 2;
    public int[] pointsForAsteroidSize = { 0, 400, 200, 100 };

    public GameObject[] asteroidPrefabs;
    public GameObject[] asteroidExplosionPrefabs;


    public GameObject GetAsteroidPrefab()
    {
        int ndx = Random.Range(0, asteroidPrefabs.Length);
        return asteroidPrefabs[ndx];
    }

}
