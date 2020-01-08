using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class KingUci : Chessman
{

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        string[] available_moves = Middleware.Instance.legal_moves;

        foreach (string moves in available_moves)
        {
            // L'ultima stringa è vuota
            if (moves.Length == 0)
            {
                break;
            }

            int x = Utility.Instance.TranslateFromUci(moves[0]);
            int y = (int)Char.GetNumericValue(moves[1]) - 1;

            if (CurrentX == x && CurrentY == y)
            {
                int new_x = Utility.Instance.TranslateFromUci(moves[2]);
                int new_y = (int)Char.GetNumericValue(moves[3]) - 1;
                r[new_x, new_y] = true;
            }
        }

        return r;
    }
}

