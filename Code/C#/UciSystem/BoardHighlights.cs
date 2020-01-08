using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour {

    public static BoardHighlights Instance {set;get;}

    public GameObject highlightPrefab;
    private List<GameObject> highlights;

    public GameObject lastMovePrefab;
    private List<GameObject> lastMove;

    public void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
        lastMove = new List<GameObject>();
    }

    private GameObject GetHighlightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);

        if(go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    private GameObject GetlastMoveHighlightObject()
    {
        GameObject go = lastMove.Find(g => !g.activeSelf);

        if (go == null)
        {
            go = Instantiate(lastMovePrefab);
            lastMove.Add(go);
        }

        return go;
    }


    public void HighlightAllowedMoves(bool[,] moves)
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j=0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetHighlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i+0.5f, 0.02f, j+0.5f);
                }
            }
        }
    }

    public void HighlightLastMove(bool[,] moves)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetlastMoveHighlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + 0.5f, 0.02f, j + 0.5f);
                }
            }
        }
    }

    public void Hidehighlights()
    {
        foreach (GameObject go in highlights)
        {
            go.SetActive(false);
        }
    }

    public void Hidelastmove()
    {
        foreach (GameObject go in lastMove)
        {
            go.SetActive(false);
        }
    }
}
