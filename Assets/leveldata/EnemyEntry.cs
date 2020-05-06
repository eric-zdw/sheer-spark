using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New EnemyEntry", menuName = "EnemyEntry", order = 53)]
public class EnemyEntry : ScriptableObject {

	[SerializeField]
	private GameObject obj;

	[SerializeField]
	private int score;

	[SerializeField]
	private bool isGroundEnemy;

	[SerializeField]
	private bool isBossEnemy;

	public GameObject Object
	{
		get { return obj; }
	}

	public int Score
	{
		get { return score; }
	}

	public bool IsGroundEnemy
	{
		get { return isGroundEnemy; }
	}

	public bool IsBossEnemy
	{
		get { return isBossEnemy; }
	}

}
