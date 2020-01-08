# -*- coding: utf-8 -*-
"""
Created on Tue May 22 15:49:15 2018

@author: Andrea Sequenzia
"""

class Utility:
    
    
    def print_board(self, board, turn):
        print '\n'
        print '----Turno ' + str(turn) + '----'
        print 'A|B|C|D|E|F|G|H'
        print '---------------'
        print board
        print '---------------'
        print 'A|B|C|D|E|F|G|H'
        
    def get_legal_moves(self, board):
        result = ""
        for move in board.legal_moves:
            result = str(move) + "," + result
        return result
