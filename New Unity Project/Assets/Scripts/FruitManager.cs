using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour {

	public GameObject[] fruits;

	private static FruitManager instance = null;
	private FieldManager fieldManager;
	private GameObject fruitGO;

	// Use this for initialization
	void Start () {
		
	}

	public FruitManager GetInstance()
	{
		if (instance == null)
			instance = this;

		return instance;
	}

	public void Initialize(FieldManager _fieldManager)
	{
		fieldManager = _fieldManager;
		PlaceFruit ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void PlaceFruit()
	{
		if (fruitGO != null)
			Destroy (fruitGO);
		fruitGO = Instantiate (fruits [Random.Range (0, fruits.Length - 1)]) as GameObject;
		int x = Random.Range (1, 10);
		int z = Random.Range (1, 10);
		while (fieldManager.GetInstance ().field [z, x] != 0) 
		{
			x = Random.Range (1, 10);
			z = Random.Range (1, 10);
		}
		fieldManager.GetInstance ().field [z, x] = 3;
		fruitGO.transform.position = new Vector3 (x, 0, z);
	}

	public void Clear()
	{
		if(fruitGO != null)
			Destroy (fruitGO);
	}
}
