using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownMenu : MonoBehaviour {

	public Text text;
	private GameManager gameManager;
	private int count = 3;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize(GameManager _gameManager)
	{
		gameManager = _gameManager;
	}

	public void StartCountDown()
	{
		
	}

	public void CountDown()
	{
		count--;
		text.text = count.ToString ();
		if (count == 0)
			gameManager.GetInstance ().StartGame ();
		
	}
}
