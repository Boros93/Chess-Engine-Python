# -*- coding: utf-8 -*-
"""
Created on Sun Jul 01 17:39:13 2018

@author: Andrea Sequenzia
"""
import chess
import numpy as np

class Piece_tables:
    def __init__(self):
        self.pawn_table_white = np.array([[0,0,0,0,0,0,0,0],
                                          [50,50,50,50,50,50,50,50],
                                          [10,10,20,30,30,20,10,10],
                                          [5,5,10,25,25,10,5,5],
                                          [0,0,0,30,30,0,0,0],
                                          [5,5,-10,0,0,-10,5,5],
                                          [5,10,10,-20,-20,10,10,5],
                                          [0,0,0,0,0,0,0,0]])
        self.pawn_table_black = np.array([[0,0,0,0,0,0,0,0],
                                          [5,10,10,-20,-20,10,10,5],
                                          [5,5,-10,0,0,-10,5,5],
                                          [0,0,0,30,30,0,0,0],
                                          [5,5,10,25,25,10,5,5],
                                          [10,10,20,30,30,20,10,10],
                                          [50,50,50,50,50,50,50,50],
                                          [0,0,0,0,0,0,0,0]])
        
        self.knight_table_white = ([[-50,-40,-30,-30,-30,-30,-40,-50],
                                    [-40,-20,  0,  0,  0,  0,-20,-40],
                                    [-30,  0, 10, 15, 15, 10,  0,-30],
                                    [-30,  5, 15, 20, 20, 15,  5,-30],
                                    [-30,  0, 15, 20, 20, 15,  0,-30],
                                    [-30,  5, 10, 15, 15, 10,  5,-30],
                                    [-40,-20,  0,  5,  5,  0,-20,-40],
                                    [-50,-40,-30,-30,-30,-30,-40,-50]])
    
        self.knight_table_black = ([[-50,-40,-30,-30,-30,-30,-40,-50],
                                    [-40,-20,  0,  5,  5,  0,-20,-40],
                                    [-30,  5, 10, 15, 15, 10,  5,-30],
                                    [-30,  0, 15, 20, 20, 15,  0,-30],
                                    [-30,  5, 15, 20, 20, 15,  5,-30],
                                    [-30,  0, 10, 15, 15, 10,  0,-30],
                                    [-40,-20,  0,  0,  0,  0,-20,-40],
                                    [-50,-40,-30,-30,-30,-30,-40,-50]])

        self.bishop_table_white = ([[-20,-10,-10,-10,-10,-10,-10,-20],
                                    [-10,  0,  0,  0,  0,  0,  0,-10],
                                    [-10,  0,  5, 10, 10,  5,  0,-10],
                                    [-10,  5,  5, 10, 10,  5,  5,-10],
                                    [-10,  0, 10, 10, 10, 10,  0,-10],
                                    [-10, 10, 10, 10, 10, 10, 10,-10],
                                    [-10, 20,  0,  0,  0,  0, 20,-10],
                                    [-20,-10,-10,-10,-10,-10,-10,-20]])

        self.bishop_table_black = ([[-20,-10,-10,-10,-10,-10,-10,-20],
                                    [-10,  5,  0,  0,  0,  0,  5,-10],
                                    [-10, 20, 10, 10, 10, 10, 20,-10],
                                    [-10,  0, 10, 10, 10, 10,  0,-10],
                                    [-10,  5,  5, 10, 10,  5,  5,-10],
                                    [-10,  0,  5, 10, 10,  5,  0,-10],
                                    [-10,  0,  0,  0,  0,  0,  0,-10],
                                    [-20,-10,-10,-10,-10,-10,-10,-20]])

        self.rook_table_white = ([[0,  0,  0,  0,  0,  0,  0,  0],
                                  [5, 10, 10, 10, 10, 10, 10,  5],
                                  [-5,  0,  0,  0,  0,  0,  0, -5],
                                  [-5,  0,  0,  0,  0,  0,  0, -5],
                                  [-5,  0,  0,  0,  0,  0,  0, -5],
                                  [-5,  0,  0,  0,  0,  0,  0, -5],
                                  [-5,  0,  0,  0,  0,  0,  0, -5],
                                  [0,  0,  0,  10,  10,  0,  0,  0]])

        self.rook_table_black = ([[0,  0,  0,  10,  10,  0,  0,  0],
                                  [-5,  0,  0,  0,  0,  0,  0, -5],
                                  [-5,  0,  0,  0,  0,  0,  0, -5],
                                  [-5,  0,  0,  0,  0,  0,  0, -5],
                                  [-5,  0,  0,  0,  0,  0,  0, -5],
                                  [-5,  0,  0,  0,  0,  0,  0, -5],
                                  [5, 10, 10, 10, 10, 10, 10,  5],
                                  [0,  0,  0,  0,  0,  0,  0,  0]])

        self.queen_table_white = ([[-20,-10,-10, -5, -5,-10,-10,-20],
                                  [-10,  0,  0,  0,  0,  0,  0,-10],
                                  [-10,  0,  5,  5,  5,  5,  0,-10],
                                  [-5,  0,  5,  5,  5,  5,  0, -5],
                                  [0,  0,  5,  5,  5,  5,  0, -5],
                                  [-10,  5,  5,  5,  5,  5,  0,-10],
                                  [-10,  0,  5,  0,  0,  0,  0,-10],
                                  [-20,-10,-10, -5, -5,-10,-10,-20]])

        self.queen_table_black = ([[-20,-10,-10, -5, -5,-10,-10,-20],
                                   [-10,  0,  5,  0,  0,  0,  0,-10],
                                   [-10,  5,  5,  5,  5,  5,  0,-10],
                                   [0,  0,  5,  5,  5,  5,  0, -5],
                                   [-5,  0,  5,  5,  5,  5,  0, -5],
                                   [-10,  0,  5,  5,  5,  5,  0,-10],
                                   [-10,  0,  0,  0,  0,  0,  0,-10],
                                   [-20,-10,-10, -5, -5,-10,-10,-20]])
 
def calculate_position(target, table, fen):
        i = 0
        j = 0
        score = 0
        for piece in fen:
            if piece == target:
                score += table[i][j]
            if piece.isdigit():
                j += int(piece)
                continue
            if piece == "/":
                i = i + 1 
                j = 0
                continue
            if piece == " ":
                break
            j = j + 1
        
        return score      

                
    # 10
if __name__ == "__main__":
    tables = Piece_tables()
    # Inizializzazioni
    board = chess.Board()
    table_black = tables.pawn_table_black
    table_white = tables.pawn_table_white
    
    
    move = chess.Move.from_uci("a2a3")
    board.push(move)
    print 'asdb',board.is_castling(move)
    move = chess.Move.from_uci("a7a6")
    board.push(move)
    
    move = chess.Move.from_uci("d2d3")
    board.push(move)
    
    move = chess.Move.from_uci("d7d6")
    board.push(move)
    
    move = chess.Move.from_uci("e7d6")
    board.push(move)
    
    print board
    print board.fen()
    
    fen =  board.fen()
    print fen
    
    black_score = calculate_position("p", table_black, fen)
    white_score = calculate_position("P", table_white, fen)
    pawn_score = black_score - white_score
    print "Black score:", black_score
    print "White score:", white_score
    print pawn_score
    