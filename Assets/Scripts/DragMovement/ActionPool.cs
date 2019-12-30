using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPool : MonoBehaviour
{
    private List<ActionResources> allActionMoves;
    private int _count = -1;
    GameObject _moves;

    private Image colors;
    private Color imageColorToBeUsed;
    private Point movePoint;

    public Button movesBtn;
    public Button resetBtn;

    private bool _moveNow;

    private float waitTime;
    private float startWaitTime = 1f;

    private int _index = 0;


    private CharacterControllers character;

    // // Event & Delegate Variable
    // public delegate void OnCharacterMove(string key);
    // public event OnCharacterMove characterMove;

    // Start is called before the first frame update
    void Start()
    {
        allActionMoves = new List<ActionResources>();
        colors = GetComponent<Image>();
        movePoint = new Point(0, 0);
        _moveNow = false;
        movesBtn.onClick.AddListener(CheckMoves);
        resetBtn.onClick.AddListener(ResetMoves);
        waitTime = startWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(character == null){
            character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllers>();
        }

        // delay for moves
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }

        // ready to jump again
        if(waitTime < 0)
        {
            waitTime = 0;
        }

        if(_moveNow && waitTime == 0){
            GetPooledAction();
            Debug.Log("i do");
        }
    }

    private void CheckMoves(){
        if(allActionMoves.Count > 0){
            _moveNow = true;        
        }else{
            Debug.Log("Action Pool Empty");
        }
    }

    public void SetToPool(ActionResources moves){
        if(_count <= 3){
            _count++;
            allActionMoves.Add(moves);
            _moves = Instantiate(allActionMoves[_count].actionObject, transform.position, Quaternion.identity);
            _moves.transform.SetParent (GameObject.FindGameObjectWithTag("Pool").transform, false);
            _moves.transform.position = gameObject.transform.GetChild(_count).transform.position;
        }
        
    }

    private void ResetMoves(){
        foreach (Transform child in GameObject.FindGameObjectWithTag("Pool").transform) {
            Destroy(child.gameObject);
            ResetPoolCounter();
        }
    }

    public void ResetPoolCounter(){
        _count = -1;
        allActionMoves.Clear();
        _moveNow = false; 
    }

    public void GetPooledAction(){            
        if(_index <= _count)
        {
            character.DoMoves(allActionMoves[_index].action);
            waitTime = startWaitTime;
            _index++;
        }else{
            ResetMoves();
            _index = 0;
        }
    }

    public void SetImageColor(){
        imageColorToBeUsed.a = 0.8f;
        imageColorToBeUsed.r = 1f;
        imageColorToBeUsed.g = 0.86f;
        imageColorToBeUsed.b = 0.62f;
        colors.color = imageColorToBeUsed;
    }

    public void ResetImageColor(){
        imageColorToBeUsed.a = 0.6f;
        imageColorToBeUsed.r = 1f;
        imageColorToBeUsed.g = 1f;
        imageColorToBeUsed.b = 1f;
        colors.color = imageColorToBeUsed;
    }


}
