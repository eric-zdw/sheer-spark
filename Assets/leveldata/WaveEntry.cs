using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New WaveEntry", menuName = "WaveEntry", order = 52)]
public class WaveEntry : ScriptableObject {
	
	[SerializeField]
	private EnemyData[] enemies;

	[SerializeField]
	private int initialSpawns;

	[SerializeField]
	private float spawnRate;

	[SerializeField]
	private int maxActiveEnemies;

	//if true, spawn enemies in order of enemy list ordering.
	//if false, spawn enemies randomly, in proportion to enemy counts.
	[SerializeField]
	private bool spawnInOrder;

	[SerializeField]
	private bool isBossWave;

	public EnemyData[] Enemies 
	{
		get { return enemies; }
	}

	public int InitialSpawns 
	{
		get { return initialSpawns; }
	}

	public float SpawnRate 
	{
		get { return spawnRate; }
	}

	public int MaxActiveEnemies
	{
		get { return maxActiveEnemies; }
	}

	public bool SpawnInOrder
	{
		get { return spawnInOrder; }
	}

	public bool IsBossWave
	{
		get { return isBossWave; }
	}

}

//data for each specific EnemyEntry.
[Serializable]
public class EnemyData {

	[SerializeField]
	private EnemyEntry enemyEntry;

	//negative values denote infinite spawns (e.g. for boss waves)
	[SerializeField]
	private int enemyCount;

	public EnemyEntry Entry
	{
		get { return enemyEntry; }
	}

	public int EnemyCount 
	{
		get { return enemyCount; }
	}

	public GameObject Object
	{
		get { return enemyEntry.Object; }
	}

	public int Score
	{
		get { return enemyEntry.Score; }
	}

	public bool IsGroundEnemy
	{
		get { return enemyEntry.IsGroundEnemy; }
	}

	public bool IsBossEnemy
	{
		get { return enemyEntry.IsBossEnemy; }
	}
}