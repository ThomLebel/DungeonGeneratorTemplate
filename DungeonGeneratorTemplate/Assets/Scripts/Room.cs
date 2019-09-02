using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public int type;
	public bool isMainPath = false;
	public Vector2 position;

	public void RoomDestruction()
	{
		Destroy(gameObject);
	}
}
