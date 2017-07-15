using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseMenu : MonoBehaviour {

	private GameManager gameManager;

	public void Initialize(GameManager _gameManager)
	{
		gameManager = _gameManager;
	}

	public void TryAgain()
	{
		gameManager.GetInstance ().Initialize ();
	}
}
