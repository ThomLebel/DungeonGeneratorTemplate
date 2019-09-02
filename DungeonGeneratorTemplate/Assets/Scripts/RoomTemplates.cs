using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public void SetRoomInformation(bool isMainPath, Vector2 position)
	{
		Room roomScript = GetComponentInChildren<Room>();
		roomScript.isMainPath = isMainPath;
		roomScript.position = position;
	}
}
