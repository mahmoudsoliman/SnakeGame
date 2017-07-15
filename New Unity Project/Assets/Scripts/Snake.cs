using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

	public List<GameObject> snake;
	public GameObject snakeUnit;
	public AudioClip bite;
	public GameObject unitsParent;

	private Vector3 currentDirection;
	private Vector3 newDirection;
	private FieldManager fieldManager;
	private GameManager gameManager;
	private FruitManager fruitManager;
	private AudioSource audioSource;
	private bool growed;
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void Initialize(FieldManager _fieldManager, GameManager _gameManager, FruitManager _fruitManager)
	{
		fieldManager = _fieldManager;
		gameManager = _gameManager;
		fruitManager = _fruitManager;
		audioSource = GetComponent<AudioSource> ();
		growed = false;
		Vector3[] directions = new Vector3[4];
		directions [0] = Vector3.right;
		directions [1] = Vector3.left;
		directions [2] = Vector3.forward;
		directions [3] = Vector3.back;

		newDirection = directions [(int)Random.Range (0, 3)];

		List<Vector3> snakePos = fieldManager.GetInstance ().SetSnakePosition (newDirection);
		for (int i = 0; i < snake.Count; i++) 
		{
			snake [i].transform.position = snakePos [i];
			if(newDirection == Vector3.forward || newDirection == Vector3.back)
				snake [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
			else
				snake [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			fieldManager.GetInstance ().field [(int)snakePos [i].z, (int)snakePos [i].x] = 1;
		}
		InvokeRepeating("Move", 0.3f, 0.3f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameManager.GetInstance().isGamePaused && Input.GetKey (KeyCode.RightArrow) && currentDirection != Vector3.left)
			newDirection = Vector3.right;
		else if (!gameManager.GetInstance().isGamePaused && Input.GetKey (KeyCode.DownArrow) && currentDirection != Vector3.forward)
			newDirection = Vector3.back;
		else if (!gameManager.GetInstance().isGamePaused && Input.GetKey (KeyCode.LeftArrow) && currentDirection != Vector3.right)
			newDirection = Vector3.left;
		else if (!gameManager.GetInstance().isGamePaused && Input.GetKey (KeyCode.UpArrow) && currentDirection != Vector3.back)
			newDirection = Vector3.forward;
	}

	public void Move()
	{
		if (gameManager.GetInstance ().isGamePaused)
			return;
		currentDirection = newDirection;
		Vector3 parentPosition = snake [0].transform.position;
		snake [0].transform.position = snake [0].transform.position + currentDirection;
		//adjust rotation
		if (currentDirection == Vector3.forward || currentDirection == Vector3.back) 
			snake [0].transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
		else 
			snake [0].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		CommonVariables.CollisionTypes col = fieldManager.GetInstance().CheckCollision (snake[0].transform.position);
		fieldManager.GetInstance ().field [(int)snake [0].transform.position.z, (int)snake [0].transform.position.x] = 1;
		for (int i = 1; i < snake.Count; i++) 
		{
			//place the new snake unit
			if (growed && i == snake.Count - 1) 
			{
				snake [snake.Count - 2].SetActive (true);
				snake [snake.Count - 2].transform.rotation = snake[snake.Count - 3].transform.rotation;
				continue;
			}
			Vector3 tmpPos = snake [i].transform.position;
			Vector3 unitDir = parentPosition - tmpPos;
			snake [i].transform.position = parentPosition;
			// adjust rotation
			if (unitDir == Vector3.forward || unitDir == Vector3.back)
				snake [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 90, 0));
			else
				snake [i].transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			parentPosition = tmpPos;
		}
		fieldManager.GetInstance ().field [(int)parentPosition.z, (int)parentPosition.x] = 0;
		growed = false;
		if (col == CommonVariables.CollisionTypes.Obstacle || col == CommonVariables.CollisionTypes.Boundry || col == CommonVariables.CollisionTypes.Self) 
		{
			gameManager.GetInstance ().isGamePaused = true;
			anim.SetTrigger ("Die");
		}
		else if(col == CommonVariables.CollisionTypes.Collectable)
		{
			Collect ();
		}
	}

	public void Lose()
	{
		gameManager.GetInstance().Lose();
	}

	public void Collect()
	{
		audioSource.clip = bite;
		audioSource.Play ();
		Grow();
		fruitManager.GetInstance ().PlaceFruit ();
	}

	public void Grow()
	{
		GameObject unitGO = Instantiate (snakeUnit) as GameObject;
		unitGO.transform.parent = unitsParent.transform;
		unitGO.SetActive (false);
		snake.Insert (snake.Count - 1, unitGO);
		growed = true;
	}
}
