using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utility;
using System.Xml;

public class DataManager : MonoBehaviour {
    public static DataManager instance;
    public Sprite IslandSprite;
    public Sprite ConqueredSprite;
    public Sprite GoalSprite;
    public PlayerStats Player
    { get; set; }
    public List<WorldNodeStats> Nodes
    { get; private set; }
    public float cameraZoom = 5.0f;

    public WorldNodeStats ActiveNode
    { get; set; }

    public Dictionary<string, Piece> BoardPieces { get; set; }

    //XML loading variables
    private string xmlfilepath = "assets/scripts/XML/BoardPieces.xml";
    private string piecesString = "Pieces/piece";
    private string identifier = "enumName";
    private string pieceName = "flavorName";
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
        Player = new PlayerStats();
        BoardPieces = new Dictionary<string, Piece>();
        Nodes = new List<WorldNodeStats>();
        loadBoardPieces();
        //Load world
        InitializeNodes();

    }

    private void loadBoardPieces()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlfilepath);
        XmlNodeList pieces = xmlDoc.SelectNodes(piecesString);
        foreach(XmlNode node in pieces)
        {
            BoardPieces.Add(node.SelectSingleNode(identifier).InnerText,new Piece( node.SelectSingleNode(pieceName).InnerText));
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

}
