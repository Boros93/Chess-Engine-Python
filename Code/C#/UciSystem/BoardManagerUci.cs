using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BoardManagerUci : MonoBehaviour {

    public static BoardManagerUci Instance { set; get; }

    // tasselli verdi
    private bool[,] allowedMoves { set; get; }

    // Griglia 
    public Chessman[,] Chessmans { get; set; }
    // Pezzo selezionato
    private Chessman selectedChessman;

    // Lista Prefab
    public List<GameObject> chessmanPrefabs;
    // Pezzi non mangiati
    public List<GameObject> activeChessman = new List<GameObject>();

    private Quaternion orientation = Quaternion.Euler(0, 90, 0);

    // Selezione
    private int selectionX = -1;
    private int selectionY = -1;
    // Mossa
    private int initialX = -1;
    private int initialY = -1; 
    // Mossa AI
    public int pendingX = -1;
    public int pendingY = -1;

    public bool isPending = false;
    public bool isWhiteTurn = true;
    public bool canCastling = true;
    public bool canCastling_AI = true;
    public bool isPromoting_AI = false;
    public string promote_move_ai = "";

    // Promozione
    public string promotedChess = "";
    public string pieceToPromote = "";
    public InputField inputField;
    public bool isPromoting = false;
    public int promoteX = -1;
    public int promoteY = -1;

    private Material previousMat;
    public Material selectedMat;

    private void Start()
    {
        Instance = this;
        SpawnAllChessman();
        inputField.gameObject.SetActive(false);
    }

    private void Update()
    {
        DrawChessboard();

        if (Input.GetMouseButtonDown(0) && isWhiteTurn && !isPromoting)
        {
            UpdateSelection();

            // Se è null non ho nessun pezzo selezionato altrimenti muovo il pezzo
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedChessman == null)
                {
                    // Selezione del pezzo
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    // Muovi pezzo
                    MoveChessman(selectionX, selectionY);
                }
            }
        }

        // Mossa AI 
        if (isPending)
        {
            MoveChessmanAI();
            if (isPromoting_AI)
                Promote_AI(promote_move_ai);
        }
            
    }

    #region Inizializzazioni
    private void SpawnChessman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], Utility.Instance.GetTileCenter(x, y), orientation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }

    private void SpawnAllChessman()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        // ---Spawn del team bianco---

        // Re
        SpawnChessman(0, 4, 0);

        // Regina
        SpawnChessman(1, 3, 0);

        // Torri
        SpawnChessman(2, 0, 0);
        SpawnChessman(2, 7, 0);

        // Alfieri
        SpawnChessman(3, 2, 0);
        SpawnChessman(3, 5, 0);

        // Cavalli
        SpawnChessman(4, 1, 0);
        SpawnChessman(4, 6, 0);

        // Pedoni
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(5, i, 1);
        }

        // ---Spawn del team nero-- -

        // Re
        SpawnChessman(6, 4, 7);

        // Regina
        SpawnChessman(7, 3, 7);

        // Torri
        SpawnChessman(8, 0, 7);
        SpawnChessman(8, 7, 7);

        // Alfieri
        SpawnChessman(9, 2, 7);
        SpawnChessman(9, 5, 7);

        // Cavali
        SpawnChessman(10, 1, 7);
        SpawnChessman(10, 6, 7);

        // Pedoni
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(11, i, 6);
        }

    }

    private void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        // Draw the selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(
                    Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                    Vector3.forward * selectionY + Vector3.right * (selectionX + 1));

        }
    }
    #endregion

    #region Selezione
    private void SelectChessman(int x, int y)
    {
        // Se seleziono una casa vuota
        if (Chessmans[x, y] == null)
            return;

        // Se non è un pezzo bianco
        if (Chessmans[x, y].isWhite != isWhiteTurn)
            return;

        // Calcolo mosse possibili
        allowedMoves = Chessmans[x, y].PossibleMove();

        // Aggiornamento pezzo selezionato
        selectedChessman = Chessmans[x, y];

        // Cambio materiale 
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;

        // Illumina le case
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
        {
            return;
        }
        RaycastHit hit;
        //Selezione dei tile
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;

        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }
    #endregion

    #region Mossa
    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Chessman c = Chessmans[x, y];

            if (c != null && c.isWhite != isWhiteTurn)
            {
                // Cattura pezzo

                // Se è il re
                if (c.GetType() == typeof(KingUci))
                {
                    // End the game
                    //EndGame();
                    return;
                }
                // Tolgo il pezzo
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            // Prendo la posizione iniziale del pezzo 
            initialX = selectedChessman.CurrentX;
            initialY = selectedChessman.CurrentY;

            string initial = Utility.Instance.Translate(initialX, initialY);
            string ending = Utility.Instance.Translate(x, y);
            string finalMove = initial + ending;
            Debug.Log(finalMove);
            //---MOSSE LEGALI---
            /*
            if (!(IsLegal(finalMove)))
            {
                Debug.Log("Mossa illegale!");
                return;
            }*/

            // Metto a null la casa da cui muovo
            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            // Muovo effettivamente il pezzo
            selectedChessman.transform.position = Utility.Instance.GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;

            // Arrocco
            if(selectedChessman.GetType() == typeof(KingUci) && canCastling)
                Castling(finalMove);
            // Promozione
            if (selectedChessman.GetType() == typeof(PawnUci) && ending[1] == '8')
            {
                Promote(finalMove, x, y);
                return;
            }
            // EnPassant
            if(selectedChessman.GetType() == typeof(PawnUci) && c == null)
            {
                EnPassant(x,y);
            }
               
            // Invio la mossa al server
            Middleware.Instance.DoMove(finalMove);
            //Cambio turno
            isWhiteTurn = !isWhiteTurn;
        }
        // Togli l'evidenzia
        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
        // Eliminazione degli highlight
        BoardHighlights.Instance.Hidehighlights();

        selectedChessman = null;
    }
    #endregion

    #region AI
    public void MoveAI(string move)
    {
        // PROMOZIONE ---- IMPLEMENTARE
        
        Debug.Log("Muovo:" + move);
        selectionX = Utility.Instance.TranslateFromUci(move[0]);
        selectionY = (int)Char.GetNumericValue(move[1]) - 1;

        pendingX = Utility.Instance.TranslateFromUci(move[2]);
        pendingY = (int)Char.GetNumericValue(move[3]) - 1;

        if (move.Length == 5)
        {
            Debug.Log("PROMO");
            // AGGIUSTARE QUI

            promote_move_ai = move;
            isPromoting_AI = true;
        }

        isPending = true;

        
    }

    public void MoveChessmanAI()
    {
        // Cancello l'highlight dell'ultima mossa
        BoardHighlights.Instance.Hidelastmove();

        // Posizione finale
        Chessman c = Chessmans[pendingX, pendingY];

        // Cattura pezzo
        if (c != null)
        {
            // Se è il re
            if (c.GetType() == typeof(King))
            {
                // End the game
                //EndGame();
                return;
            }

            activeChessman.Remove(c.gameObject);
            Destroy(c.gameObject);
            selectedChessman = null;
        }
        BoardHighlights.Instance.Hidelastmove();
        // Evidenzia la mossa
        bool[,] lastMove = new bool[8, 8];
        lastMove[selectionX, selectionY] = true;
        lastMove[pendingX, pendingY] = true;
        BoardHighlights.Instance.HighlightLastMove(lastMove);
        // Partenza 
        selectedChessman = Chessmans[selectionX, selectionY];
        selectedChessman.CurrentX = selectionX;
        selectedChessman.CurrentY = selectionY;
        Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
        
        // Arrocco
        if (selectedChessman.GetType() == typeof(KingUci) && canCastling_AI)
            Castling_AI();
        // EnPassant
        if (selectedChessman.GetType() == typeof(PawnUci) && c == null)
        {
            EnPassant_AI(pendingX, pendingY);
        }
        // Arrivo
        selectedChessman.transform.position = Utility.Instance.GetTileCenter(pendingX, pendingY);
        selectedChessman.SetPosition(pendingX, pendingY);
        Chessmans[pendingX, pendingY] = selectedChessman;

        // Cambio turno
        isPending = false;
        isWhiteTurn = !isWhiteTurn;

        // Cambio Materiale
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;
        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;

        // reset
        selectedChessman = null;

    }
    #endregion

    #region mosse speciali ai

    public void Castling_AI()
    {
        Chessman rook;
        // Arrocco corto
        if(selectionX == 4 && selectionY == 7 && pendingX == 2 && pendingY == 7)
        {
            Debug.Log("Castling");
            rook = Chessmans[0, 7];
            rook.SetPosition(3, 7);
            rook.transform.position = Utility.Instance.GetTileCenter(3, 7);
            Chessmans[0, 7] = null;
            Chessmans[3, 7] = rook;
            canCastling_AI = false;
            return;
        }
        // Arrocco lungo
        if(selectionX == 4 && selectionY == 7 && pendingX == 6 && pendingY == 7)
        {
            Debug.Log("Castling");
            rook = Chessmans[7, 7];
            rook.SetPosition(5, 7);
            rook.transform.position = Utility.Instance.GetTileCenter(5, 7);
            Chessmans[7, 7] = null;
            Chessmans[5, 7] = rook;
            canCastling_AI = false;
            return;
        }
    }

    public void EnPassant_AI(int x, int y)
    {
        Debug.Log("ENTRO ENPASSANT");
        Debug.Log(selectionX + " " + selectionY);
        Debug.Log(x + " " + y);
        if (x == (selectionX + 1) || x == (selectionX - 1))
        {
            Debug.Log("EnPassant");
            // Pedone da catturare
            Chessman c = Chessmans[x, y + 1];
            activeChessman.Remove(c.gameObject);
            Destroy(c.gameObject);
            return;
        }
        return;
    }

    public void Promote_AI(string move)
    {
        // Prima rimuovo il pedone
        Chessman c = Chessmans[pendingX, pendingY];
        activeChessman.Remove(c.gameObject);
        Destroy(c.gameObject);
        // Scelta pezzo da cambiare
        int index = -1;
        switch (move[4])
        {
            case 'n':
                index = 10;
                break;
            case 'q':
                index = 7;
                break;
            case 'b':
                index = 9;
                break;
            case 'r':
                index = 8;
                break;
        }
        // Faccio spawnare il pezzo nuovo
        SpawnChessman(index, pendingX, pendingY);
    }

    #endregion
    #region mosse speciali player
    public void Castling(string move)
    {
        Chessman rook;
        switch (move)
        {
            case "e1g1":
                Debug.Log("Castling");
                rook = Chessmans[7, 0];
                rook.SetPosition(5,0);
                rook.transform.position = Utility.Instance.GetTileCenter(5, 0);
                Chessmans[7, 0] = null;
                Chessmans[5, 0] = rook;
                canCastling = false;
                break;
            case "e1c1":
                Debug.Log("Castling");
                rook = Chessmans[0, 0];
                rook.SetPosition(3, 0);
                rook.transform.position = Utility.Instance.GetTileCenter(3, 0);
                Chessmans[0, 0] = null;
                Chessmans[3, 0] = rook;
                canCastling = false;
                break;
            case "e8g8":
                Debug.Log("Castling");
                rook = Chessmans[7, 7];
                rook.SetPosition(5, 7);
                rook.transform.position = Utility.Instance.GetTileCenter(5, 7);
                Chessmans[7, 7] = null;
                Chessmans[5, 7] = rook;
                canCastling = false;
                break;
            case "e8c8":
                Debug.Log("Castling");
                rook = Chessmans[0, 7];
                rook.SetPosition(3, 7);
                rook.transform.position = Utility.Instance.GetTileCenter(3, 7);
                Chessmans[0, 7] = null;
                Chessmans[3, 7] = rook;
                canCastling = false;
                break;
        }
    }

    public void Promote(string move, int x, int y)
    {
        inputField.gameObject.SetActive(true);
        isPromoting = true;
        promoteX = x;
        promoteY = y;
        pieceToPromote = move;
    }

    public void EnPassant(int x, int y)
    {
        if (x == (initialX + 1) || x == (initialX-1))
        {
            Debug.Log("EnPassant");
            // Pedone da catturare
            Chessman c = Chessmans[x, y - 1];
            activeChessman.Remove(c.gameObject);
            Destroy(c.gameObject);
            return;
        }
    }

    public void SetPromotedChess(string piece)
    {
        promotedChess = piece;
    }

    public void ChoosePiece(string piece)
    {
        string finalmove = pieceToPromote;
        switch (promotedChess)
        {
            case "cavallo":
                finalmove += "n";
                break;
            case "regina":
                finalmove += "q";
                break;
            case "alfiere":
                finalmove += "b";
                break;
            case "torre":
                finalmove += "r";
                break;
        }
        if(finalmove.Length == 5)
            MakePromoteMove(finalmove);
    }

    public void MakePromoteMove(string promoteMove)
    {
        // Prima rimuovo il pedone
        Chessman c = Chessmans[promoteX, promoteY];
        activeChessman.Remove(c.gameObject);
        Destroy(c.gameObject);
        // Scelta pezzo da cambiare
        int index = -1;
        switch (promoteMove[4])
        {
            case 'n':
                index = 4;
                break;
            case 'q':
                index = 1;
                break;
            case 'b':
                index = 3;
                break;
            case 'r':
                index = 2;
                break;
        }
        // Faccio spawnare il pezzo nuovo
        SpawnChessman(index, promoteX, promoteY);
        // Invio la mossa al server
        Middleware.Instance.DoMove(promoteMove);
        //Cambio turno
        isWhiteTurn = !isWhiteTurn;
        // Togli l'evidenzia
        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
        // Eliminazione degli highlight
        BoardHighlights.Instance.Hidehighlights();
        BoardHighlights.Instance.Hidelastmove();
        selectedChessman = null;
        // reset variabili promozione
        isPromoting = false;
        inputField.gameObject.SetActive(false);
    }
    #endregion
}
