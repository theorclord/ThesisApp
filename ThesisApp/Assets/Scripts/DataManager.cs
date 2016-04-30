using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using System.Xml;
using Assets.Scripts.Events;

public class DataManager : MonoBehaviour {
    public bool specialActive = false;

    public static DataManager instance;
    public Sprite IslandSprite;
    public Sprite ConqueredSprite;
    public Sprite GoalSprite;
    public PlayerStats Player { get; set; }
    public List<WorldNodeStats> Nodes { get; private set; }
    public float cameraZoom = 5.0f;

    public WorldNodeStats ActiveNode { get; set; }

    public Dictionary<string, Piece> BoardPieces { get; set; }

    public Dictionary<string, Faction> Factions { get; set; }

    public List<SavedResult> SavedEvents { get; set; }
    public SavedResult ActiveDiplomaticEvent { get; set; }

    public int TurnCounter { get; set; }

    //XML loading variables
    private string boardPiecesXml = "assets/scripts/XML/BoardPieces.xml";
    private string piecesString = "Pieces/piece";
    private string identifier = "enumName";
    private string pieceName = "flavorName";

    private string factionXml = "assets/scripts/XML/Factions.xml";
    private string factionString = "factions/faction";
    private string factionId = "enumName";
    private string factionName = "name";
    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        SavedEvents = new List<SavedResult>();
        Player = new PlayerStats();
        BoardPieces = new Dictionary<string, Piece>();
        Nodes = new List<WorldNodeStats>();
        Factions = new Dictionary<string, Faction>();
        loadBoardPieces();
        loadFactions();
        //Load world
        InitializeNodes();

    }

    private void loadFactions()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(factionXml);
        XmlNodeList pieces = xmlDoc.SelectNodes(factionString);
        foreach (XmlNode node in pieces)
        {
            //Debug.Log(node.SelectSingleNode(factionName).InnerText);
            Faction newFaction = new Faction(node.SelectSingleNode(factionName).InnerText);
            
            Factions.Add(node.SelectSingleNode(factionId).InnerText, newFaction);
        }
        //set faction relations
        foreach(KeyValuePair<string,Faction> factionPair in Factions)
        {
            Player.FactionRelations.Add(factionPair.Value, 0);
        }
    }

    private void loadBoardPieces()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(boardPiecesXml);
        XmlNodeList pieces = xmlDoc.SelectNodes(piecesString);
        foreach(XmlNode node in pieces)
        {
            Piece newPiece = new Piece(node.SelectSingleNode(pieceName).InnerText);
            if(node.SelectSingleNode("type").InnerText == "room")
            {
                newPiece.Type = BoardType.ROOM;
            } else
            {
                newPiece.Type = BoardType.RESOURCE;
            }
            BoardPieces.Add(node.SelectSingleNode(identifier).InnerText, newPiece);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void clearNodes()
    {
        Nodes.Clear();
    }
    
    public void InitializeNodes()
    {
        LSystem ls = new LSystem();
        RuleCollection rc = new RuleCollection();
        rc.GenerateRules();
        Debug.Log("Size = " + rc.GetCollection().Count);
        int selectedIndex = Random.Range(0, rc.GetCollection().Count);
        LRule r = (LRule)rc.GetCollection()[selectedIndex];
        ls.axiom = r.axiom;
        ls.delta = r.delta;
        // JUST GO THROUGH THE RANDOMLY SELECTED RULE, USE ITS COLLECTION IN THE EXPAND AND INTERPRET
        Debug.Log(r.name);
        ls.expand(r.expandingIterations, r.rules);
        ls.interpret();
    }

    /// <summary>
    /// Randomizes the array 
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static int[] randomArray(int size)
    {
        int[] randomArr = new int[size];
        for (int i = 0; i < randomArr.Length; i++)
        {
            randomArr[i] = i;
        }
        for (int i = 0; i < randomArr.Length; i++)
        {
            int random = Random.Range(0, randomArr.Length);
            int temp = randomArr[random];
            randomArr[random] = randomArr[i];
            randomArr[i] = temp;
        }
        return randomArr;
    }

}
