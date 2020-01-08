using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour {
    public static Utility Instance { get; set; }

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public string Translate(int x, int y)
    {
        int uci_x = (int)Mathf.Floor(x + 1);
        int uci_y = (int)Mathf.Floor(y + 1);

        string literal_x = "";

        switch (uci_x)
        {
            case 1:
                literal_x = "a";
                break;
            case 2:
                literal_x = "b";
                break;
            case 3:
                literal_x = "c";
                break;
            case 4:
                literal_x = "d";
                break;
            case 5:
                literal_x = "e";
                break;
            case 6:
                literal_x = "f";
                break;
            case 7:
                literal_x = "g";
                break;
            case 8:
                literal_x = "h";
                break;
        }


        string result = literal_x + uci_y;
        return result;
    }

    public int TranslateFromUci(char uci)
    {
        int x = 0;
        switch (uci)
        {
            case 'a':
                x = 0;
                break;
            case 'b':
                x = 1;
                break;
            case 'c':
                x = 2;
                break;
            case 'd':
                x = 3;
                break;
            case 'e':
                x = 4;
                break;
            case 'f':
                x = 5;
                break;
            case 'g':
                x = 6;
                break;
            case 'h':
                x = 7;
                break;
        }

        return x;
    }

    public Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

}
