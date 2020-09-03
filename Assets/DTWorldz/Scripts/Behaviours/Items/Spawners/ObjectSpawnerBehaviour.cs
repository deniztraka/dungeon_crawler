using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.ProceduralMapGenerators;
using UnityEngine;
using Random = System.Random;

public class ObjectSpawnerBehaviour : MonoBehaviour
{
    public List<GameObject> SpawnPrefabs;
    public List<GameObject> BossPrefabs;
    public LevelBehaviour CurrentLevel;
    public int SpawnFrequency;
    public int MaxAliveCount;
    public float RangeX;
    public float RangeY;
    public Color Color;

    [SerializeField]
    private List<GameObject> aliveObjects;
    [SerializeField]
    private float spawnTime;
    private Random random;

    public bool SpawnBoss;

    // Start is called before the first frame update
    void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        aliveObjects = new List<GameObject>();
        random = new Random(DateTime.Now.Millisecond);
        spawnTime = 0;

        //Spawn at start
        for (int i = 0; i < MaxAliveCount; i++)
        {
            var spawnedObject = Spawn(SpawnPrefabs[random.Next(0, SpawnPrefabs.Count)]);
            if (spawnedObject != null)
            {
                aliveObjects.Add(spawnedObject);
            }
        }

        //spawn boss
        if (SpawnBoss && BossPrefabs != null && BossPrefabs.Count > 0)
        {
            var spawnedObject = Spawn(BossPrefabs[random.Next(0, BossPrefabs.Count)]);
            if (spawnedObject != null)
            {
                aliveObjects.Add(spawnedObject);
            }
        }
    }

    public List<GameObject> GetAliveObjects()
    {
        return aliveObjects;
    }
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
        if (CurrentLevel == null)
        {
            return Vector3.zero;
        }

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
        if (randomPosition == Vector3.zero)
        {
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
