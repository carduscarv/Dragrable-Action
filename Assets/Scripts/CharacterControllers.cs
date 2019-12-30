using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControllers : MonoBehaviour
{
    private ActionPool pools;
    private List<ActionResources> whereToMoves;
    private float _tileSize;
    private Vector2 _coor;
    private Transform _destination;
    private float _speed = 2f;
    private Vector2 tilePos;

    private float waitTime;
    private float startWaitTime = 1f;

    private int _count = 0;
    private IEnumerator coroutine;
    private float _step;

    private bool _moveNow;

    protected Rigidbody2D characterRB;
    
    // Start is called before the first frame update
    void Start()
    {
        tilePos = GameObject.FindGameObjectWithTag("Tiles").GetComponent<TileScript>().WorldPosition;
        if(pools == null){
            pools = GameObject.FindGameObjectWithTag("PoolParent").GetComponent<ActionPool>();
            // Debug.Log("Gocha!");
        }
        whereToMoves = new List<ActionResources>();
        // whereToMoves = pools.GetPooledAction();

        _tileSize = LevelManager.Instance.TileSize;
        _destination = GameObject.FindGameObjectWithTag("Destination").GetComponent<Transform>();
        _destination.transform.position = transform.position;
        // _destination = transform;
        _coor = new Vector2(5, 3);

        waitTime = startWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        // ready to jump again
        if(waitTime < 0)
        {
            waitTime = 0;
        }

        if(transform.position != _destination.position){
            waitTime = startWaitTime;
            transform.position = Vector3.MoveTowards(transform.position, _destination.position, _speed * Time.deltaTime);

            if(Vector2.Distance(transform.position, _destination.position) < 0.00002f){
                _destination.transform.position = transform.position;
                //  waitTime -= Time.deltaTime;
            }
        } 
    }

    // private void CheckMoves(){
    //     _moveNow = true;
    //     whereToMoves = pools.GetPooledAction();
        
    // }

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

        // characterRB.velocity = new Vector2(_speed, characterRB.velocity.y);
        _destination.transform.position = new Vector3((tilePos.x + (_tileSize*_coor.x)), tilePos.y - (_tileSize*_coor.y), 0);   
    }

    // private void ResetMoves(){
    //     foreach (Transform child in GameObject.FindGameObjectWithTag("Pool").transform) {
    //         Destroy(child.gameObject);
    //         pools.ResetPoolCounter();
    //     }
    // }

    // private void MoveOneByOne(){
    //         _destination.transform.position = new Vector3((tilePos.x + (_tileSize*_coor.x)), tilePos.y - (_tileSize*_coor.y), 0);

    //         // Move our position a step closer to the _destination.
            
    //         transform.position = Vector3.MoveTowards(transform.position, _destination.position, _step);
    //         Debug.Log(transform.position);

    //         // if(Vector2.Distance(transform.position, _destination.position) < 0.001f){
    //         //     waitTime = startWaitTime;
    //         // }
    // }

    // IEnumerator MoveWait(float wait, int i)
    // {
    //     int _count = 0;
    //     while (_count < i)
    //     {
    //         yield return new WaitForSeconds (wait);
    //         MoveOneByOne();
    //         _count++;
    //     }  
    // }
}
