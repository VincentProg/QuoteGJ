using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomStats
{
    public Room AssociateRoom { get { return associateRoom; } set { associateRoom = value; } }
    private Room associateRoom;
    public string roomName = null;
    public int enemiesInRoom = 0;
    public int interactionInRoom = 0;
}

public class RoomManager : MonoBehaviour
{
    public List<Room> rooms = new List<Room>();
    public List<RoomStats> roomsStats = new List<RoomStats>();

    // Start is called before the first frame update
    void Start()
    {
        InitialiseStats();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRoomStats();
    }

    void InitialiseStats()
    {
        foreach (Room room in rooms)
        {
            RoomStats stats = new RoomStats();
            stats.roomName = room.name;
            stats.enemiesInRoom = room.enemiesCollider.Count;
            stats.interactionInRoom = room.interactionCollider.Count;
            stats.AssociateRoom = room;

            roomsStats.Add(stats);
        }
    }

    void UpdateRoomStats()
    {
        foreach (RoomStats stat in roomsStats)
        {
            stat.enemiesInRoom = stat.AssociateRoom.enemiesCollider.Count;
            stat.interactionInRoom = stat.AssociateRoom.interactionCollider.Count;
        }
    }
}
