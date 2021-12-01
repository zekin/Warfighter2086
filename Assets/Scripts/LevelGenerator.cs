using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
//add something for this later

/*
public class RoomExits
{

    public List<string> exitDirection;
    void initializeLists()
    {
        string[] lists = new string[] { "up", "left", "right", "down" };
        foreach (var i in lists)
        {
            if (roomsByDirection[i] == null)
            {
                roomsByDirection[i] = new List<RoomExits>();
            }
        }

    }
    public RoomExits(bool hasUp, bool hasLeft, bool hasRight, bool hasDown)
    {
        initializeLists();
        exitDirection = new List<string>();

        if (hasUp)
            exitDirection.Add("up");
        if (hasDown)
            exitDirection.Add("down");
        if (hasLeft)
            exitDirection.Add("left");
        if (hasRight)
            exitDirection.Add("right");

        if (hasUp)
            roomsByDirection["up"].Add(this);
        if (hasDown)
            roomsByDirection["down"].Add(this);
        if (hasLeft)
            roomsByDirection["left"].Add(this);
        if (hasRight)
            roomsByDirection["right"].Add(this);
    }
}
*/
public class Room
{

    //    static 
    public string type;
//    public Room exitLeft;
//    public Room exitRight;
//    public Room exitUp;
//    public Room exitDown;

    public Dictionary<string, Room> dir = new Dictionary<string, Room>();    
    public Room(string roomType)
    {
        type = roomType;
    }

}

//[System.Serializable] public class PrefabDictionary : Dictionary<string, GameObject> { };
/*[System.Serializable]
public struct NamedPrefab
{
    public string name;
    public GameObject prefab;
    
}*/



public class LevelGenerator : MonoBehaviour
{
    [SerializeField] int generationDepth = 5;
    [SerializeField] Vector2 RoomSize = new Vector2(24f,24f);
    Room graph;
    Room graphStart;
    //    [SerializeField] PrefabDictionary prefabDict = new Dictionary<string,GameObject>();
    //    [SerializeField] public List<GameObject> prefabList = new List<GameObject>();
    //    public GameObject prefab;
    //    public List<GameObject> tile = new List<GameObject>();
    public Dictionary<string, (int, int)> translation = new Dictionary<string, (int, int)>()
    {
        { "up", (0,1) },
        {"down",(0,-1) },
        {"left", (-1,0) },
        {"right",(1,0) }
    };

    public Dictionary<string, GameObject> prefabList = new Dictionary<string, GameObject>();
    //    public Dictionary<string, Dictionary<string, bool>> isTileAt = new Dictionary<string, Dictionary<string, bool>>();
    //https://softwareengineering.stackexchange.com/questions/319264/dictionary-of-dictionaries-design-in-c
    //    public NAryDictionary<int, int, bool> isTileAt = new NAryDictionary<int, int, bool>();
    public Dictionary<(int, int), bool> isTileAt = new Dictionary<(int, int), bool>();
    public Dictionary<(int, int), List<string>> doorsList = new Dictionary<(int, int), List<string>>();
    public Dictionary<(int, int), int> processedDoors = new Dictionary<(int, int), int>();

    private void Awake()
    {
        prefabList.Add("AllHallway", GameObject.Find("AllHallway"));
        prefabList.Add("HallwayHorizontal", GameObject.Find("HorizontalHallway"));
        prefabList.Add("HallwayVertical", GameObject.Find("VerticalHallway"));
        prefabList.Add("RoomFull", GameObject.Find("RoomFull"));
        prefabList.Add("Room", GameObject.Find("Room"));
        prefabList.Add("RoomGoal", GameObject.Find("RoomGoal"));
        prefabList.Add("RoomStart", GameObject.Find("RoomStart"));
        prefabList.Add("DoorHalf", GameObject.Find("DoorHalf"));
        prefabList.Add("DoorHalfSide", GameObject.Find("DoorHalfSide"));


        //        tile = Resources.LoadAll<GameObject>("").ToList();
    }
    private void OnEnable()
    {
        EventManager.EventDie.AddListener(handleDeath);
        EventManager.EventDebug.AddListener(handleDebug);
        EventManager.EventWin.AddListener(handleWin);
    }
    private void OnDisable()
    {
        EventManager.EventDie.RemoveListener(handleDeath);
        EventManager.EventDebug.RemoveListener(handleDebug);
        EventManager.EventWin.AddListener(handleWin);
    }
    private void handleDebug()
    {
        GameObject.Find("Debug").transform.Find("DebugSun").gameObject.SetActive(true);
    }
    public void handleWin()
    {
        SceneManager.LoadScene("Credits");

    }

    private void handleDeath(GameObject obj)
    {
        if (obj.tag == "Player")
        {
            SceneManager.LoadScene("Credits");
        }
        Destroy(obj);
        if (obj.name.Contains("Boss"))
        {
            EventManager.EventWin.Invoke();

        }

        //find nearest spawner, spawn new monster;
    }


    public static Dictionary<string, List<string>> exitLookup = new Dictionary<string, List<string>>()
        {

            { "AllHallway", new List<string>() { "left", "right", "up", "down" } },
            { "HallwayHorizontal", new List<string>() { "left", "right" } },
            { "HallwayVertical", new List<string>() { "down", "up" } },
            { "RoomFull", new List<string>() { "left", "right", "up", "down" } },
            { "Room", new List<string>() {"up", "left", "right", "down" } },
            { "RoomGoal", new List<string>() { } },
            { "RoomStart", new List<string>() { "right" } }
        };
    public static Dictionary<string, List<string>> roomTypesByDirection = new Dictionary<string, List<string>>()
    {
        { "up", new List<string>() },
        { "down", new List<string>() },
        { "left", new List<string>() },
        { "right", new List<string>() }
    };

    public static Dictionary<string, string> convertEntranceToExit = new Dictionary<string, string>
        {
            { "up", "down" },
            { "left", "right" },
            { "right", "left" },
            {  "down", "up" }
        };
    string removeChoiceFromList(ref List<string> list)
    {
        int listIndex = UnityEngine.Random.Range(0, list.Count);
        string choice = list[listIndex];
        list.RemoveAt(listIndex);

        return choice;
    }
    Room pickRoomByDirection(string direction) {

        List<string> rooms = roomTypesByDirection[convertEntranceToExit[direction]];

        int randomIndex = UnityEngine.Random.Range(0, rooms.Count);
        string choice = rooms[randomIndex];


        return new Room(choice);
    }
    (int,int) moveTo((int,int) from, string direction)
    {
        return (from.Item1 + translation[direction].Item1, from.Item2 + translation[direction].Item2);
//        return (from.Item1 + direction.Item1, pos.Item2 + translation[goal].Item2);

    }
    //deprecated
    void createDoors(Vector3 position, List<string> exits)
    {

        foreach (var exit in exits)
        {
            string doorType = "DoorHalf";
            Vector3 exitPosition = new Vector3(translation[exit].Item1 * 12, 0f, translation[exit].Item2 * 12f);
            if (Math.Abs(exitPosition.x) >= 1)
            {
                doorType = "DoorHalfSide";
            }
            Instantiate(prefabList[doorType], position + exitPosition, Quaternion.identity);
        }
    }
    Room recurse(Room room, int iteration, (int,int) pos)
    {
        isTileAt[pos] = true;
        List<string> exits = new List<string>(exitLookup[room.type]);

        if (iteration == 0)
        {
            exits = new List<string>(exitLookup["RoomGoal"]);
            Vector3 roomPosition = new Vector3(RoomSize.x * pos.Item1, 0f, RoomSize.y * pos.Item2);
            doorsList[pos] = exits;

            createDoors(roomPosition, exits);

            Instantiate(prefabList["RoomGoal"], roomPosition, Quaternion.identity);
            return null;
        }
        else
        {
            Vector3 roomPosition = new Vector3(RoomSize.x * pos.Item1, 0f, RoomSize.y * pos.Item2);
            doorsList[pos] = exits;
            createDoors(roomPosition, exits);
            
            Instantiate(prefabList[room.type], new Vector3(RoomSize.x * pos.Item1, 0f, RoomSize.y * pos.Item2), Quaternion.identity);
        }



        //        List<string> exits = new List<string>(exitLookup[room.type]);

        if (iteration == -1)
        {
            foreach (var exit in exits)
            {
                if (isTileAt.ContainsKey(moveTo(pos, exit)) == false && UnityEngine.Random.Range(0f,1f) > .6f )
                {
                    room.dir[exit] = recurse(pickRoomByDirection(exit), -1, moveTo(pos, exit));
                }
            }
            return null;
        }



        string goal;
        do
        {
            goal = removeChoiceFromList(ref exits);


        } while (isTileAt.ContainsKey(moveTo(pos, goal)) == true);
        room.dir[goal] = recurse(pickRoomByDirection(goal), iteration - 1, moveTo(pos, goal));// pos + directionToTranslation[goal] );


        foreach (var exit in exits)
        {
            if (isTileAt.ContainsKey(moveTo(pos, exit)) == false)
            {
                room.dir[exit] = recurse(pickRoomByDirection(exit), -1, moveTo(pos, exit));
            }
        }

        return room;

    }


    void initializeLists()
    {
        foreach(KeyValuePair<string, List<string>> exitsList in exitLookup)
        {
            foreach(var exit in exitsList.Value)
            {

                //restrictions on what connects to goal room path list
                if (exitsList.Key == "RoomGoal" || exitsList.Key == "RoomStart")
                    continue;

                //can build other selection lists for biomes in here as well
                roomTypesByDirection[exit].Add(exitsList.Key);
            }
        }
    }  

    (int, int) doorCoords((int,int) pos, string exit)
    {

        return (pos.Item1 * 24 + translation[exit].Item1 * 12, pos.Item2 * 24 + translation[exit].Item2 * 12);
    }
    void initializeDoors()
    {
        foreach(var lis in doorsList)
        {
            //foreach exit, check exit at coord exists, if exists - make open, else - create closed
//            if (!openDoorsList.ContainsKey(item.Key)) {
//                continue;
//            }
            foreach(var exit in lis.Value)
            {
                var coords = doorCoords(lis.Key, exit);

                if (processedDoors.ContainsKey(coords))
                {
                    //then make it open
                    if (exit == "up" || exit == "down")
                    {
                        processedDoors[coords] = 2;
                    }
                    else if (exit == "left" || exit == "right")
                    {
                        processedDoors[coords] = 3;
                    }
                }
                else
                {
                    //then make it closed
                    if (exit == "up" || exit == "down")
                    {
                        processedDoors[coords] = 0;
                    }
                    else if (exit == "left" || exit == "right")
                    {
                        processedDoors[coords] = 1;
                    }
                }
            }
        }
    }
    void instantiateDoors()
    {
        foreach(var door in processedDoors)
        {
            var exitType = (door.Value == 1 || door.Value == 3) ? "DoorHalfSide" : "DoorHalf";
            Vector3 exitPosition = new Vector3(door.Key.Item1, 0f, door.Key.Item2);

            GameObject obj = Instantiate(prefabList[exitType], exitPosition, Quaternion.identity);
            if (door.Value == 0 || door.Value == 1)
            {
                obj.GetComponent<DoorOpener>().setClosed();
            }
        }
    }
    void Start()
    {
        initializeLists();
        graphStart = new Room("RoomStart");
        graph = recurse(graphStart, generationDepth, (0,0));
        //this needs to be debugged before I can use it, ran out of time so taking it out
//        initializeDoors();
//        instantiateDoors();
        Debug.Log("Hello world");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
