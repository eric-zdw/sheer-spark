using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StageEntry", menuName = "StageEntry", order = 51)]
public class StageEntry : ScriptableObject {
	
	[SerializeField]
	private string stageName;

	[SerializeField]
	private int sceneIndex;

	[SerializeField]
	private string[] prerequisites;

	[SerializeField] 
	private bool completed = false;

	[SerializeField]
	private GameObject previewModel;

	public string StageName 
	{
		get { return stageName; }
	}

	public int SceneIndex 
	{
		get { return sceneIndex; }
	}

	public string[] Prerequisites 
	{
		get { return prerequisites; }
	}

	public bool IsCompleted
	{
		get { return completed; }
		set { completed = value; }
	}

}
