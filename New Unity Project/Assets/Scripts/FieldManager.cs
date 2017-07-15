using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldManager : MonoBehaviour {

	public int Width;
	public int Height;
	public GameObject obstaclePrefab;
	public int[,] field;

	private static FieldManager instance = null;

	public void Initialize()
	{
		field = new int[Height + 2, Width + 2];
		for (int i = 0; i <= Height; i++) 
		{
			for (int j = 0; j <= Width; j++) 
			{
				field [i,j] = 0;
			}
		}
	}

	public void SetObstacles(int numOfObstacles)
	{
		for (int i = 0; i < numOfObstacles; i++) 
		{
			int x = Random.Range (2, Width-2);
			int z = Random.Range (2, Height-2);
			while (field [z,x] != 0) 
			{
				x = Random.Range (2, Width-2);
				z = Random.Range (2, Height-2);
			}
			GameObject newObstacle = Instantiate (obstaclePrefab) as GameObject;
			newObstacle.transform.position = new Vector3 (x, 0, z);
			field [z, x] = 2;
		}
	}

	public List<Vector3> SetSnakePosition(Vector3 dir)
	{
		List<Vector3> ret = new List<Vector3> ();
		if (dir == Vector3.right) 
		{
			int x = Random.Range (3, 6);
			int z = Random.Range (1, 10);
			bool f = true;
			while (f) 
			{
				if (field [z, x] == 0 && field [z, x - 1] == 0 && field [z, x - 2] == 0) 
				{
					ret.Add (new Vector3 (x, 0, z));
					ret.Add (new Vector3 (x - 1, 0, z));
					ret.Add (new Vector3 (x - 2, 0, z));
					f = false;
				} 
				else 
				{
					x = Random.Range (3, 6);
					z = Random.Range (1, 10);
				}
			}
		} 
		else if (dir == Vector3.left) 
		{
			int x = Random.Range (5, 8);
			int z = Random.Range (1, 10);
			bool f = true;
			while (f) 
			{
				if (field [z, x] == 0 && field [z, x + 1] == 0 && field [z, x + 2] == 0) 
				{
					ret.Add (new Vector3 (x, 0, z));
					ret.Add (new Vector3 (x + 1, 0, z));
					ret.Add (new Vector3 (x + 2, 0, z));
					f = false;
				} 
				else 
				{
					x = Random.Range (5, 8);
					z = Random.Range (1, 10);
				}
			}
		} 
		else if (dir == Vector3.forward) 
		{
			int x = Random.Range (1, 10);
			int z = Random.Range (3, 6);
			bool f = true;
			while (f) 
			{
				if (field [z, x] == 0 && field [z - 1, x] == 0 && field [z - 2, x] == 0) 
				{
					ret.Add (new Vector3 (x, 0, z));
					ret.Add (new Vector3 (x, 0, z - 1));
					ret.Add (new Vector3 (x, 0, z - 2));
					f = false;
				} 
				else 
				{
					x = Random.Range (1, 10);
					z = Random.Range (3, 6);
				}
			}
		} 
		else if (dir == Vector3.back) 
		{
			int x = Random.Range (1, 10);
			int z = Random.Range (5, 8);
			bool f = true;
			while (f) 
			{
				if (field [z, x] == 0 && field [z + 1, x] == 0 && field [z + 2, x] == 0) 
				{
					ret.Add (new Vector3 (x, 0, z));
					ret.Add (new Vector3 (x, 0, z + 1));
					ret.Add (new Vector3 (x, 0, z + 2));
					f = false;
				} 
				else 
				{
					x = Random.Range (1, 10);
					z = Random.Range (5, 8);
				}
			}
		}
		return ret;
	}

	public CommonVariables.CollisionTypes CheckCollision(Vector3 snakePos)
	{
		int i = (int)snakePos.z;
		int j = (int)snakePos.x;
		if (i < 1 || i > Height || j < 1 || j > Width)
			return CommonVariables.CollisionTypes.Boundry;
		else if (field [i,j] == 0)
			return CommonVariables.CollisionTypes.None;
		else if (field [i,j] == 1)
			return CommonVariables.CollisionTypes.Self;
		else if (field [i,j] == 2)
			return CommonVariables.CollisionTypes.Obstacle;
		else
			return CommonVariables.CollisionTypes.Collectable;
	}

	public FieldManager GetInstance()
	{
		if (instance == null)
			instance = this;

		return instance;
	}
}
