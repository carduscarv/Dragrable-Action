using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSpawner : MonoBehaviour
{
    public ActionResources left;

    private GameObject actionClone;

    public GameObject actionPoolPos;

    private float deltaX, deltaY;

    private Vector2 mousePosition;

    private bool _hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseClick(){
        deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    public void MouseDrag(){
        
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(actionClone == null){
            actionClone = Instantiate(left.actionObject, transform.position, Quaternion.identity);
            actionClone.transform.SetParent (GameObject.FindGameObjectWithTag("Respawn").transform, false);
        }
        
        actionClone.transform.position = new Vector2(mousePosition.x - deltaX, mousePosition.y - deltaY);

        if(Mathf.Abs(actionClone.transform.position.x - actionPoolPos.transform.position.x) <= 1.2f && Mathf.Abs(actionClone.transform.position.y - actionPoolPos.transform.position.y) <= 3.7f){
            _hit = true;
            actionPoolPos.GetComponent<ActionPool>().SetImageColor();
        }else{
            _hit = false;
        }

        if(!_hit){
            actionPoolPos.GetComponent<ActionPool>().ResetImageColor();   
        }
    }

    public void MouseUp(){
        if(Mathf.Abs(actionClone.transform.position.x - actionPoolPos.transform.position.x) <= 1.2f && Mathf.Abs(actionClone.transform.position.y - actionPoolPos.transform.position.y) <= 3.7f){
            ActionPool actionPool = actionPoolPos.GetComponent<ActionPool>();
            actionPool.SetToPool(left);
        }
        _hit = false;
        actionPoolPos.GetComponent<ActionPool>().ResetImageColor();        
        Destroy(actionClone);       
        
    }
}
