using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControllers : MonoBehaviour
{
    // Ukuran tiles
    private float _tileSize;
    // Koordinat character
    private Vector2 _coor;
    // Koordinat yang dituju karakter
    private Transform _destination;
    // Kecepatan karakter
    private float _speed = 2f;
    // Posisi global tiles
    private Vector2 tilePos;
    // Waktu tunggu
    private float waitTime;
    // Awal waktu tunggu
    private float startWaitTime = 1f;

    protected Collider2D characterCol;
    
    // Start is called before the first frame update
    void Start()
    {
        characterCol = GetComponent<Collider2D>();
        characterCol.enabled = false;
        tilePos = GameObject.FindGameObjectWithTag("Tiles").GetComponent<TileScript>().WorldPosition;

        _tileSize = LevelManager.Instance.TileSize;
        _destination = GameObject.FindGameObjectWithTag("Destination").GetComponent<Transform>();
        _destination.transform.position = transform.position;
        
        // koordinat awal character, harus sama dengan koordinat karakter di Level manager
        _coor = new Vector2(5, 3);

        waitTime = startWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Logic waktu tunggu
        if(waitTime < 0)
        {
            waitTime = 0;
        }

        if(waitTime > 0){
            waitTime -= Time.deltaTime;
        }

        // jika colider character hidup tapi tidak ada waktu tunggu, matikan
        if(characterCol.enabled && waitTime == 0){
            characterCol.enabled = false;
        }

        // jika koordinat karakter dan destinasi tidak sama, pindahkan karakter
        if(transform.position != _destination.position){
            transform.position = Vector3.MoveTowards(transform.position, _destination.position, _speed * Time.deltaTime);

            // jika literally sama, sama persis kan
            if(Vector2.Distance(transform.position, _destination.position) < 0.00002f){
                _destination.transform.position = transform.position;
            }
        } 
    }

    // logic gerakan yang diterima karakter dari pool, gerakan bergerak dan mengambil
    public void DoMoves(string action){
        if(action == "right"){
            _coor.x += 1;
            _coor.y += 0;
            transform.localScale = new Vector3(-1, 1, 1); 
        }else if(action == "left"){
            _coor.x -= 1;
            _coor.y += 0;
            transform.localScale = new Vector3(1, 1, 1); 
        }else if(action == "up"){
            _coor.x += 0;
            _coor.y -= 1; 
        }else if(action == "down"){
            _coor.x += 0;
            _coor.y += 1;  
        }

        if(action == "put-object"){
            characterCol.enabled = true; 
            waitTime = startWaitTime;
        }else{
            characterCol.enabled = false;
        }
        _destination.transform.position = new Vector3((tilePos.x + (_tileSize*_coor.x)), tilePos.y - (_tileSize*_coor.y), 0);   
    }

    // Jika collider character hidup, dan kena collider trigger object 'ambil', hancurkan object tersebut
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Triggered!");
        if(other.tag == "Put"){
            Destroy(other.gameObject);
        }    
    }
}
