# -*- coding: utf-8 -*-
"""
Created on Tue May 22 15:07:28 2018

@author: Andrea
"""

import chess

class MoveManager:
    def make_move(self, input_str, board):
        try:
            move = chess.Move.from_uci(input_str)
        except ValueError:
            print 'Mossa in formato non uci'
            return False
        # Mossa legale?
        if move in board.legal_moves:
            board.push(move) 
            return True
        else:
            print 'Mossa illegale'
            return False