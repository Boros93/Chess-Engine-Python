# -*- coding: utf-8 -*-
"""
Created on Wed May 23 21:14:13 2018

@author: Andrea Sequenzia
"""
import chess
from pieces_tables import Piece_tables

class Node:
    
    def __init__(self, board, move, pred):
        # Valori in centipawns
        self.P = 100
        self.N = 320
        self.B = 330
        self.R = 500
        self.Q = 900
        self.K = 20000
        # Board
        self.__board= board
        self.__move= move
        # Score
        self.__score= 0
        
        self.pred = pred
        
        self.__children= []
        
        self.isCheckMate = board.is_checkmate()
        
        if move != "root":
            self.isCastling = board.is_castling(move)
    
        # Tabelle score posizionali
        self.tables = Piece_tables()
        
        if move != "root":
            # Se non è la radice allora fai una mossa
            self.__board.push(move)
        else:
            # La root non ha pred
            self.pred = None

    # FUNZIONE DI VALUTAZIONE
    def evalutation_function(self):
        fen = self.__board.fen()
        
        

        dQueen = self.count_of_queens(chess.BLACK, fen) - self.count_of_queens(chess.WHITE, fen)
        dRooks = self.count_of_rooks(chess.BLACK, fen) - self.count_of_rooks(chess.WHITE, fen)
        dBishops = self.count_of_bishops(chess.BLACK, fen) - self.count_of_bishops(chess.WHITE, fen)
        dKnights = self.count_of_knights(chess.BLACK, fen) - self.count_of_knights(chess.WHITE, fen)
        dPawns = self.count_of_pawns(chess.BLACK, fen) - self.count_of_pawns(chess.WHITE, fen)
        
        #number_of_captures = 0
        #for captures in self.__board.generate_legal_captures():
        #    number_of_captures += 1
        
        #                     --- Calcolo score posizionale  ---
        # ---Pawn---
        black_pawn_pos = self.calculate_positional_score("p", self.tables.pawn_table_black, fen)
        white_pawn_pos = self.calculate_positional_score("P", self.tables.pawn_table_white, fen)
        pawn_positional_score = black_pawn_pos - white_pawn_pos
        # ---Knight---
        black_knigth_pos = self.calculate_positional_score("n", self.tables.knight_table_black, fen)
        white_knight_pos = self.calculate_positional_score("N", self.tables.knight_table_white, fen)
        knight_positional_score = black_knigth_pos - white_knight_pos
        # ---Bishop---
        black_bishop_pos = self.calculate_positional_score("b", self.tables.bishop_table_black, fen)
        white_bishop_pos = self.calculate_positional_score("B", self.tables.bishop_table_white, fen)
        bishop_positional_score = black_bishop_pos - white_bishop_pos
        # ---Rook--
        black_rook_pos = self.calculate_positional_score("r", self.tables.rook_table_black, fen)
        white_rook_pos = self.calculate_positional_score("R", self.tables.rook_table_white, fen)
        rook_positional_score = black_rook_pos - white_rook_pos
        # ---Queen---
        black_queen_pos = self.calculate_positional_score("q", self.tables.queen_table_black, fen)
        white_queen_pos = self.calculate_positional_score("Q", self.tables.queen_table_white, fen)
        queen_positional_score = black_queen_pos - white_queen_pos
        # Score finale
        self.__score = self.Q * dQueen + self.R * dRooks + self.B * dBishops + self.N * dKnights \
        + self.P * dPawns
        #print "Score without positional:", self.__score, 'of', self.__move
        self.__score += pawn_positional_score + knight_positional_score + bishop_positional_score + rook_positional_score + queen_positional_score
        #print "Score:", self.__score, 'of', self.__move
        
        if self.isCastling:
           self.__score += 50
            
        if self.__board.is_checkmate():
            print "--------Checkmate!"
            print self.__board
            self.__score += 250
    
        if self.__board.is_check():
            # print 'EVALUTATION SCACCO!'
            self.__score += 200
            
        return self.__score
    
    
    def generate_children(self):
        legal_move = self.__board.legal_moves
        # Aggiungo figli per ogni mossa
        for move in legal_move:
            child_board = self.__board.copy()
            child = Node(child_board, move, self)
            self.__children.append(child)
            
        return self.__children
            
            
    
    # Contatore pezzi
    def count_of_rooks(self, color, fen):
        result = 0
        for piece in fen:
            if color == chess.BLACK and piece == 'r':
                result += 1
            elif color == chess.WHITE and piece == 'R':
                result += 1
        return result
    
    
    def count_of_bishops(self, color, fen):
        result = 0
        for piece in fen:
            if color == chess.BLACK and piece == 'b':
                result += 1
            elif color == chess.WHITE and piece == 'B':
                result += 1
        return result
    
    
    def count_of_knights(self, color, fen):
        result = 0
        for piece in fen:
            if color == chess.BLACK and piece == 'n':
                result += 1
            elif color == chess.WHITE and piece == 'N':
                result += 1
        return result
    
    
    def count_of_pawns(self, color, fen):
        result = 0
        for piece in fen:
            if color == chess.BLACK and piece == 'p':
                result += 1
            elif color == chess.WHITE and piece == 'P':
                result += 1
        return result
    
    
    def count_of_queens(self, color, fen):
        result = 0
        for piece in fen:
            if piece == ' ':
                break
            if color == chess.BLACK and piece == 'q':
                result += 1
            elif color == chess.WHITE and piece == 'Q':
                result += 1
        return result
    
    def choose_child(self, value):
        # print "VALORI FIGLI"
        for child in self.__children__:
            # print child.__score
            if child.__score == value:
                return child.__move
            
    def calculate_positional_score(self, target, table, fen):
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
    
    def get_move(self):
        return self.__move
    
    
    def get_score(self):
        return self.__score