using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class RoomStats
//{
//    public Room AssociateRoom { get { return associateRoom; } set { associateRoom = value; } }
//    private Room associateRoom;
//    public string roomName = null;
//    public int enemiesInRoom = 0;
//    public int interactionInRoom = 0;
//}

public class GameManager : MonoBehaviour
{
    static public GameManager Instance; 
    [HideInInspector]
    public List<Room> rooms = new List<Room>();
    public List<Hunter> hunters = new List<Hunter>();
    public List<Candle> allCandles = new List<Candle>();
    public GameObject candle;
    
    //public List<RoomStats> roomsStats = new List<RoomStats>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            Room roomChild =  child.GetComponent<Room>();
            rooms.Add(roomChild);
        }
        InstantiateCandles();


        //InitialiseStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.timeScale != 1) Time.timeScale = 1;
            else Time.timeScale = 20;
        }
    }

    //void InitialiseStats()
    //{
    //    foreach (Room room in rooms)
    //    {
    //        RoomStats stats = new RoomStats();
    //        stats.roomName = room.name;
    //        stats.enemiesInRoom = room.enemiesCollider.Count;
    //        stats.interactionInRoom = room.interactionCollider.Count;
    //        stats.AssociateRoom = room;

    //        roomsStats.Add(stats);
    //    }
    //}

    //void UpdateRoomStats()
    //{
    //    foreach (RoomStats stat in roomsStats)
    //    {
    //        stat.enemiesInRoom = stat.AssociateRoom.enemiesCollider.Count;
    //        stat.interactionInRoom = stat.AssociateRoom.interactionCollider.Count;
    //    }
    //}

    private void InstantiateCandles()
    {
        foreach (Room room in rooms)
        {
            for (int i = 0; i < hunters.Count; i++)
            {
                GameObject newCandle = Instantiate(candle, room.transform);
                newCandle.transform.localScale = new Vector3(
                    newCandle.transform.localScale.x / newCandle.transform.lossyScale.x,
                    newCandle.transform.localScale.y / newCandle.transform.lossyScale.y,
                    newCandle.transform.localScale.z / newCandle.transform.lossyScale.z) / 3f;
                SpriteRenderer candleZone = room.transform.GetChild(0).GetComponent<SpriteRenderer>();
                float minX = candleZone.bounds.min.x;
                float maxX = candleZone.bounds.max.x;
                float minY = candleZone.bounds.min.y;
                float maxY = candleZone.bounds.max.y;
                newCandle.transform.position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), candleZone.transform.position.z);

                Candle candleScript = newCandle.GetComponent<Candle>();
                room.myCandles.Add(candleScript);
                candleScript.room = room;
                allCandles.Add(candleScript);
            }
        }
    }
}