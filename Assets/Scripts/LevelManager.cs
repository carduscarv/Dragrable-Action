﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefab;

    [SerializeField]
    private CameraMovement cameraMovement;

    private Point gateClose, gateOpen, charPos;
    
    [SerializeField]
    private GameObject gateClosePrefab;

    [SerializeField]
    private GameObject gateOpenPrefab;

    [SerializeField]
    private GameObject charPrefab;

    [SerializeField]
    private Transform parent;

    private CharacterControllers _char;

    public Dictionary<Point, TileScript> Tiles {get; set;}

    public float TileSize {
       get { return tilePrefab[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    // Start is called before the first frame update
    void Start()
    {
        Point p = new Point(0,0);
        Createlevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Createlevel(){
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadTextLevel();

        int mapSizeX = mapData[0].ToCharArray().Length;
        int mapSizeY = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapSizeY; y++)
        {
            char[] changeToMapTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapSizeX; x++)
            {
                PlaceTile(changeToMapTiles[x].ToString(),x, y, worldStartPos);
            }
        }

        maxTile = Tiles[new Point(mapSizeX-1, mapSizeY-1)].transform.position;

        cameraMovement.SetLimit(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        SpawnGate();
        SpawnCharacter();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStartPos){
        int tileIndex = int.Parse(tileType);
        TileScript newTile = Instantiate(tilePrefab[tileIndex]).GetComponent<TileScript>();
        
        newTile.Setup(new Point(x, y), new Vector3(worldStartPos.x + (TileSize*x), worldStartPos.y - (TileSize*y), 0), parent);
        
        // Tiles.Add(new Point(x,y), newTile);
    }

    private string[] ReadTextLevel(){
        TextAsset bindData = Resources.Load("TilesMap") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    private void SpawnGate(){
        gateClose = new Point(0, 0);
        Instantiate(gateClosePrefab, Tiles[gateClose].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

        gateOpen = new Point(12, 6);
        Instantiate(gateOpenPrefab, Tiles[gateOpen].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }

    private void SpawnCharacter(){
        charPos = new Point(5, 3);
        _char = Instantiate(charPrefab, Tiles[charPos].GetComponent<TileScript>().WorldPosition, Quaternion.identity).GetComponent<CharacterControllers>();
        Debug.Log(_char.transform.position);
    }
}
