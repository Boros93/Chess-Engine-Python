# -*- coding: utf-8 -*-
"""
Created on Tue May 22 16:14:23 2018

@author: Andrea Sequenzia
"""

import random
import chess 
from minmax_node import Node

class AIAgent:
    
    def __init__(self):
        self.__depth = 3
        self.__node_max = None
        
    def make_best_move(self, board):
        moves = board.legal_moves
        for move in moves:
            print move
        board_copy = board.copy()
        node = Node(board_copy, "root", None)
        
        # --- Commentare per scegliere quale dei due algoritmi usare
        bestMove = self.alphabeta(node, self.__depth, -float("inf"), float("inf"), True)  
        #bestMove = self.minimax(node, self.__depth, True)  

        move_to_do = str(self.explore(bestMove.move, ""))

        if move_to_do == "":
           move_to_do = self.random_move(board)

        #print "Mossa da fare:", move_to_do
        # Mossa random
        # return self.random_move(board)

        return move_to_do

    
    def random_move(self, board):
        moves = board.legal_moves
        moves_list = []
        for move in moves:
            moves_list.append(str(move))
            '''if len(str(move)) == 5:
                print ("PROMOTE")
                return str(move)'''
        return random.choice(moves_list)
    
    
    def explore(self, node, move):
        if node is None:
            return move
        if node.pred is None:
            return move
        move = node.get_move()
        return self.explore(node.pred, move)
             
    def minimax(self, configuration, depth, maximizingPlayer):
        if depth == 0 or configuration.isCheckMate:
            evalMove = BestMove()
            evalMove.move = configuration
            evalMove.value = configuration.evalutation_function()
            return evalMove
        if maximizingPlayer:
            bestMove = BestMove()
            bestMove.value = -float("inf")
            bestMove.move = None
            for child in configuration.generate_children():
                move = self.minimax(child, depth - 1, False)
                if move.value > bestMove.value:
                    bestMove = move
            return bestMove
        else:
            minMove = BestMove()
            minMove.value = float("inf")
            minMove.move = None
            for child in configuration.generate_children():
                move = self.minimax(child, depth - 1, True)
                if move.value < minMove.value:
                    minMove = move
            return minMove
           
    def alphabeta(self, configuration, depth, alpha, beta, maximizingPlayer):
        if depth == 0 or configuration.isCheckMate:
            evalMove = BestMove()
            evalMove.move = configuration
            evalMove.value = configuration.evalutation_function()
            return evalMove
        if maximizingPlayer:
            bestMove = BestMove()
            bestMove.value = -float("inf")
            bestMove.Move = None
            for child in configuration.generate_children():
                move = self.alphabeta(child, depth-1, alpha, beta, False)
                if move.value > bestMove.value:
                    bestMove = move
                    alpha = move.value
                if beta <= alpha:
                    break
            return bestMove
        else:
            minMove = BestMove()
            minMove.value = float("inf")
            minMove.Move = None
            for child in configuration.generate_children():
                move = self.alphabeta(child, depth-1, alpha, beta, True)
                if move.value < minMove.value:
                    minMove = move
                    beta = move.value
                if beta <= alpha:
                    break
            return minMove
    
class BestMove:
    
    
    def __init__(self):
        self.value = 0
        self.move = None
            