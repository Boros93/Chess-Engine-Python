using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middleware : MonoBehaviour {

    public string[] legal_moves;
    public string move;
    public static Middleware Instance { set; get; }

    // Use this for initialization
    void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoMove(string move)
    {
        Client.Instance.SendMove(move);
    }

    public void ReceiveLegalMoves(string moves)
    {
        legal_moves = moves.Split(',');
    }

    public void ReceiveMove(string _move)
    {
        Debug.Log("MOSSA RICEVUTA" + _move);
        move = _move;
        BoardManagerUci.Instance.MoveAI(move);
    }

}
