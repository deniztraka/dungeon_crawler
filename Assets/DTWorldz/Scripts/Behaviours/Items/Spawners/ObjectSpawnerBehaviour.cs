using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours;
using DTWorldz.Behaviours.ProceduralMapGenerators;
using UnityEngine;
using Random = System.Random;

public class ObjectSpawnerBehaviour : MonoBehaviour
{
    public List<GameObject> SpawnPrefabs;
    public LevelBehaviour CurrentLevel;
    public float SpawnFrequency;
    public int MaxAliveCount;
    public float RangeX;
    public float RangeY;
    public Color Color;

    [SerializeField]
    private List<GameObject> aliveObjects;
    [SerializeField]
    private float spawnTime;
    private Random random;
    // Start is called before the first frame update
    void Start()
    {
        aliveObjects = new List<GameObject>();
        random = new Random(DateTime.Now.Millisecond);
        spawnTime = SpawnFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnPrefabs != null && SpawnPrefabs.Count > 0 && spawnTime <= 0)
        {
            CleanAliveList();

            if (MaxAliveCount > aliveObjects.Count)
            {
                var spawnedObject = Spawn(SpawnPrefabs[random.Next(0, SpawnPrefabs.Count)]);
                if (spawnedObject != null)
                {
                    aliveObjects.Add(spawnedObject);
                }
                else
                {
                    spawnTime = 0;
                }

            }
            spawnTime = SpawnFrequency;
        }
        else
        {
            spawnTime -= Time.deltaTime;
        }
    }

    public Vector3 GetRandomPointInside()
    {
        var randomPosition = Vector3.zero;
        var isNotFoundYet = true;
        var maxTryCount = 10;
        var tryCount = 0;
        while (isNotFoundYet && tryCount <= maxTryCount)
        {
            randomPosition = gameObject.transform.position + new Vector3(
                ((float)random.NextDouble() - 0.5f) * RangeX,
                ((float)random.NextDouble() - 0.5f) * RangeY,
                0
             );
            var cellPos = CurrentLevel.WallMap.WorldToCell(randomPosition);
            var tile = CurrentLevel.WallMap.GetTile(cellPos);
            if (tile == null)
            {
                isNotFoundYet = false;
            }
            tryCount++;
        }
        return tryCount <= maxTryCount ? randomPosition : Vector3.zero;
    }

    public GameObject Spawn(GameObject gameObject)
    {
        var randomPosition = GetRandomPointInside();
        if(randomPosition == Vector3.zero){
            return null;
        }

        var spawnedObject = Instantiate(gameObject, randomPosition, Quaternion.identity);
        var movementBehaviour = spawnedObject.GetComponent<MovementBehaviour>();
        if (movementBehaviour != null)
        {
            movementBehaviour.SetMovementGrid(CurrentLevel.WallMap);
            movementBehaviour.SetSpawner(this);
        }

        var healthBehaviour = spawnedObject.GetComponent<HealthBehaviour>();
        if (healthBehaviour != null)
        {
            healthBehaviour.OnDeath += new HealthBehaviour.HealthChanged(OnSpawnedObjectDead);
        }
        return spawnedObject;
    }

    private void OnSpawnedObjectDead(float currentHealth, float maxHealth)
    {
        CleanAliveList();
    }

    private void CleanAliveList()
    {
        //Clear nulls from list
        for (int i = aliveObjects.Count - 1; i >= 0; i--)
        {
            if (aliveObjects[i] == null)
            {
                aliveObjects.RemoveAt(i);
            }
        }
    }
}
