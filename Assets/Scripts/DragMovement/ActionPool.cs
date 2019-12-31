using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPool : MonoBehaviour
{
    // List penampung gerakan (bergerak, mengambil, looping)
    private List<ActionResources> allActionMoves;
    // List penampung gerakan temporary (digunakan untuk logic loopinh)
    private List<ActionResources> tmpActionMoves;
    // Index untuk jumlah aksi yang dapat ditampung di pool
    private int _count = -1;
    // Gameobject penampung object yg ditanam dalam pool
    GameObject _moves;
    // Object gambar & warna yang diubah saat resource aksi memasuki wilayah pool
    private Image colors;  
    private Color imageColorToBeUsed;
    // 
    // private Point movePoint;

    // Button referensi move dan reset
    public Button movesBtn;
    public Button resetBtn;

    // Boolean yang menentukan kapan harus bergerak
    private bool _moveNow;

    // Waktu tunggu
    private float waitTime;
    // Nilai awal waktu tunggu
    private float startWaitTime = 1f;
    // index looping / gerakan yang nantinya harus dilakukan (index object + index looping)
    private int _index = 0;

    // index logic looping
    private int _loopIndex = 0;

    // Referensi object character
    private CharacterControllers character;

    // Start is called before the first frame update
    void Start()
    {
        // inisialisasi
        allActionMoves = new List<ActionResources>();   
        colors = GetComponent<Image>();
        // movePoint = new Point(0, 0);
        _moveNow = false;
        // buat event listener untuk button move dan reset
        movesBtn.onClick.AddListener(CheckMoves);
        resetBtn.onClick.AddListener(ResetMoves);
        // set waktu tunggu
        waitTime = startWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        // cari gameobject character
        if(character == null){
            character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllers>();
        }

        // Jika ada waktu tunggu, kurangi
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }

        if(waitTime < 0)
        {
            waitTime = 0;
        }

        // Jika harus bergerak dan tidak ada waktu tunggu, panggil aksi didalam pool
        if(_moveNow && waitTime == 0){
            GetPooledAction();
            // Debug.Log("i do");
        }
    }

    // Fungsi saat button moves dipanggil, check apakah ada gerakan didalam pool
    private void CheckMoves(){
        if(allActionMoves.Count > 0){
            _moveNow = true;        
        }else{
            Debug.Log("Action Pool Empty");
        }
    }

    // Logic memasukan resource gerakan kedalam pool, terutama bagian looping
    public void SetToPool(ActionResources moves){
        int _loopCount = 0;
        // maksimal yg dapat ditampung pool 7+1 (8 object)
        if(_count < 7){
            _count++;
            if(moves.type != "loop"){
                allActionMoves.Add(moves);
            }else{
                allActionMoves.Add(moves);
                if(allActionMoves.Count > 0){
                    _loopCount = int.Parse(moves.action);
                    tmpActionMoves = new List<ActionResources>(allActionMoves);
                    // tmpActionMoves = allActionMoves;
                    for(int i = 1; i < _loopCount; i++){
                        // allActionMoves = allActionMoves.Concat(tmpActionMoves).ToList();
                        allActionMoves.AddRange(tmpActionMoves);
                        // Debug.Log(allActionMoves.Count + "-" + tmpActionMoves.Count);
                        _loopIndex += tmpActionMoves.Count;
                    }

                    tmpActionMoves = null;
                }
            }
            // Debug.Log("loop: "+ _loopIndex);

            // Buat object di dalam pool sesuai dengan resource aksi yang dipanggil
            _moves = Instantiate(allActionMoves[_count+_loopIndex].actionObject, transform.position, Quaternion.identity);
            _moves.transform.SetParent (GameObject.FindGameObjectWithTag("Pool").transform, false);
            _moves.transform.position = gameObject.transform.GetChild(_count).transform.position;
        }
        
    }

    // Fungsi saat button reset dipanggil, hapus semua yg ada dalam pool
    private void ResetMoves(){
        foreach (Transform child in GameObject.FindGameObjectWithTag("Pool").transform) {
            Destroy(child.gameObject);
            ResetPoolCounter();
        }
    }

    // reset list dan object di dalam pool, ubah kondisi ke tidak bisa bergerak
    public void ResetPoolCounter(){
        _count = -1;
        _loopIndex = 0;
        allActionMoves.Clear();
        _moveNow = false; 
    }

    // jika bisa bergerak dan tidak ada waktu tunggu, jalankan gerakan, perintahkan karakter untuk bergerak
    public void GetPooledAction(){            
        if(_index < allActionMoves.Count)
        {
            character.DoMoves(allActionMoves[_index].action);
            waitTime = startWaitTime;
            _index++;
        }else{
            // jika semua gerakan sudah dilakukan, reset
            ResetMoves();
            _index = 0;
        }
    }

    // Set warna gambar
    public void SetImageColor(){
        imageColorToBeUsed.a = 0.8f;
        imageColorToBeUsed.r = 1f;
        imageColorToBeUsed.g = 0.86f;
        imageColorToBeUsed.b = 0.62f;
        colors.color = imageColorToBeUsed;
    }

    // Reset warna gambar (default)
    public void ResetImageColor(){
        imageColorToBeUsed.a = 0.6f;
        imageColorToBeUsed.r = 1f;
        imageColorToBeUsed.g = 1f;
        imageColorToBeUsed.b = 1f;
        colors.color = imageColorToBeUsed;
    }
}
