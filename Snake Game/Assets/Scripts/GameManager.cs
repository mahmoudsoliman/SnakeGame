using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static GameManager instance = null;
	public int level;
	public int difficulty;
	public GameObject snakePrefab;
	private GameObject snakeGO;
	public GameObject fieldManager;
	public GameObject fruitManager;
	public bool isGamePaused;
	public AudioClip soundtrack;
	private AudioSource audioSource;
	public GameObject loseMenuPrefab;
	public GameObject countDownMenuPrefab;
	private GameObject loseMenu;
	private GameObject countDownMenu;

	public GameManager GetInstance()
	{
		if (instance == null)
			instance = this;
		
		return instance;
	}
		
	void Start () {
		isGamePaused = true;
		audioSource = GetComponent<AudioSource> ();
		Initialize ();
	}

	public void Initialize()
	{
		Clear ();
		fieldManager.GetComponent<FieldManager> ().GetInstance ().Initialize ();
		fieldManager.GetComponent<FieldManager> ().GetInstance ().SetObstacles (difficulty);
		snakeGO = Instantiate (snakePrefab) as GameObject;
		snakeGO.GetComponent<Snake> ().Initialize (fieldManager.GetComponent<FieldManager> (), this, fruitManager.GetComponent<FruitManager>());
		fruitManager.GetComponent<FruitManager> ().GetInstance ().Initialize (fieldManager.GetComponent<FieldManager> ());
		countDownMenu = Instantiate (countDownMenuPrefab) as GameObject;
		countDownMenu.GetComponent<CountDownMenu> ().Initialize (this);
		countDownMenu.GetComponent<CountDownMenu> ().StartCountDown ();
	}

	public void StartGame()
	{
		if (countDownMenu != null)
			Destroy (countDownMenu);
		audioSource.clip = soundtrack;
		audioSource.Play ();
		isGamePaused = false;
	}

	public void Clear()
	{
		if (loseMenu != null)
			Destroy (loseMenu);
		if(snakeGO != null)
			Destroy (snakeGO);
		fruitManager.GetComponent<FruitManager> ().GetInstance ().Clear ();
		fieldManager.GetComponent<FieldManager> ().GetInstance ().Clear ();
	}

	public void Lose()
	{
		audioSource.Stop ();
		loseMenu = Instantiate (loseMenuPrefab) as GameObject;
		loseMenu.GetComponent<LoseMenu> ().Initialize (this);
	}
}
