using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
	public Transform[] startingPositions;
	public GameObject[] rooms;
	public List<GameObject> mainPath = new List<GameObject>();
	public List<GameObject> altPath = new List<GameObject>();

	public float moveAmount;
	public float startTimeBrwRoom = 0.25f;
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;
	public float tileRange;

	private int direction;
	private int previousDirection;
	private float timeBtwRoom;
	private bool stopGeneration;
	private bool startFilling;
	private bool dungeonFilled;

	private const int ROOM_LR = 0;
	private const int ROOM_LRB = 1;
	private const int ROOM_LRT = 2;
	private const int ROOM_LRBT = 3;


	// Start is called before the first frame update
	void Start()
	{
		timeBtwRoom = startTimeBrwRoom;
		int randStartingPos = Random.Range(0, startingPositions.Length);
		transform.position = startingPositions[randStartingPos].position;

		direction = NextRoomDirection();
		previousDirection = 0;

		int roomType = ROOM_LR;
		if (direction == 5)
		{
			roomType = ROOM_LRB;
		}

		GameObject instance = Instantiate(rooms[roomType], transform.position, Quaternion.identity);
		instance.GetComponent<RoomTemplates>().SetRoomInformation(true, transform.position);
		mainPath.Add(instance);
	}

	private void Update()
	{
		if (timeBtwRoom <= 0 && !stopGeneration)
		{
			Move();
			timeBtwRoom = startTimeBrwRoom;
		}
		else
		{
			timeBtwRoom -= Time.deltaTime;
		}
		if (stopGeneration && !startFilling)
		{
			startFilling = true;
			SpawnRandomRoom();
		}
	}

	private void Move()
	{
		Vector2 nextRoomPos = Vector2.zero;

		switch (direction)
		{
			case 1:
			case 2:
				//Move Right
				nextRoomPos = Vector2.right;
				break;
			case 3:
			case 4:
				//Move Left
				nextRoomPos = Vector2.left;
				break;
			case 5:
				//Move Down
				nextRoomPos = Vector2.down;
				break;
		}

		Vector2 newPos;
		newPos = new Vector2(transform.position.x + nextRoomPos.x * moveAmount, transform.position.y + nextRoomPos.y * moveAmount);
		transform.position = newPos;

		previousDirection = direction;
		direction = NextRoomDirection();

		int randomRoom = ChooseRoomType();
		GameObject instance = Instantiate(rooms[randomRoom], transform.position, Quaternion.identity);
		instance.GetComponent<RoomTemplates>().SetRoomInformation(true, transform.position);
		mainPath.Add(instance);
	}

	private int NextRoomDirection()
	{
		int dir = Random.Range(1,6);

		switch (dir)
		{
			case 1:
			case 2:
				//Move right
				if (previousDirection == 3)
				{
					if (transform.position.x <= minX)
					{
						if (transform.position.y > minY)
						{
							dir = 5;
						}
						else
							stopGeneration = true;
					}
					else
					{
						dir = 3;
					}
				}
				else if (previousDirection == 4)
				{
					if (transform.position.y > minY)
						dir = 5;
					else
						stopGeneration = true;
				}
				if (transform.position.x >= maxX)
				{
					if (transform.position.y > minY)
					{
						dir = 5;
					}
					else
						stopGeneration = true;
				}
				break;
			case 3:
			case 4:
				//Move left
				if (previousDirection == 1)
				{
					if (transform.position.x >= maxX)
					{
						if (transform.position.y > minY)
						{
							dir = 5;
						}
						else
							stopGeneration = true;
					}
					else
					{
						dir = 1;
					}
				}
				else if (previousDirection == 2)
				{
					if (transform.position.y > minY)
						dir = 5;
					else
						stopGeneration = true;
				}
				if (transform.position.x <= minX)
				{
					if (transform.position.y > minY)
					{
						dir = 5;
					}
					else
						stopGeneration = true;
				}
				break;
			case 5:
				if (transform.position.y <= minY)
				{
					stopGeneration = true;
				}
				break;
		}

		return dir;
	}

	private int ChooseRoomType()
	{
		int roomType = 0;

		switch (previousDirection)
		{
			case 0:
				if (direction == 5)
				{
					roomType = ROOM_LRBT;
				}
				else
				{
					roomType = Random.Range(ROOM_LR, rooms.Length);
				}
				break;
			case 5:
				if (direction == 5)
				{
					roomType = ROOM_LRBT;
				}
				else
				{
					roomType = Random.Range(ROOM_LRT, rooms.Length);
				}
				break;
			default:
				if (direction == 5)
				{
					roomType = ROOM_LRB;
				}
				else
				{
					roomType = Random.Range(ROOM_LR, rooms.Length);
				}
				break;
		}

		return roomType;
	}

	private void SpawnRandomRoom()
	{
		for (float i = maxY; i >= minY; i -= tileRange)
		{
			for (float j = minX; j <= maxX; j += tileRange)
			{
				bool isFilled = false;
				for (int p=0; p<mainPath.Count; p++)
				{
					Vector3 path = mainPath[p].transform.position;
					if (path.x == j && path.y == i)
					{
						isFilled = true;
					}
				}
				if (!isFilled)
				{
					int randType = Random.Range(0, rooms.Length);
					GameObject instance = Instantiate(rooms[randType], new Vector3(j,i,0), Quaternion.identity);
					instance.GetComponent<RoomTemplates>().SetRoomInformation(false, transform.position);
					altPath.Add(instance);
				}
			}
		}

		dungeonFilled = true;
	}
}
