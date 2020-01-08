# -*- coding: utf-8 -*-
"""
Created on Tue May 29 00:14:59 2018

@author: Andrea Sequenzia
"""

import chess
from move_manager import MoveManager
from utility import Utility
from AIEngine import AIAgent
import time
from server import Communicator
# Inizializzazioni
board = chess.Board()
mover = MoveManager()
utility = Utility()
agent = AIAgent()

print '---Inizio partita--- \n'
utility.print_board(board, 0)

# Turno
whiteTurn = True

# Contatore mosse
n_move = 1

# Creazione connessione
com = Communicator()
com.communicate()

file = open("Mosse.txt", "w")

# Game loop
while True:
    start_time = time.time() # Contatore tempo
    
        # Condizioni partita
    if board.is_check():
        print 'SCACCO!'
    if board.is_checkmate():
        print 'SCACCO MATTO!'
        break
    if board.can_claim_draw():
        print 'STALLO!'
        break
    
    if whiteTurn:
        # print utility.get_legal_moves(board)
        com.send_move(utility.get_legal_moves(board))
        input_move = com.receive_move()
    else:
        input_move = agent.make_best_move(board)
        com.send_move(input_move)
        
    # Interrupt
    if input_move == 'quit':
        break
    
    # Condizioni partita
    if board.is_checkmate():
        file.write("SCACCO MATTO! \n")
        print 'SCACCO MATTO!'
        break
    
    if board.is_check():
        file.write("Scacco! \n")
        print 'SCACCO!'

    if board.can_claim_draw():
        print 'STALLO!'
        file.write("STALLO! \n")
        break
    
    # Mossa legale?

    if not(mover.make_move(input_move, board)):
        continue
    
    # Stampa mossa
    if whiteTurn:
        print "Il bianco muove: " + str(input_move)
        file.write("Il bianco muove: " + str(input_move) + "\n")
    else:
        print "Il nero muove: " + str(input_move)  
        file.write("Il nero muove: " + str(input_move) + "\n")
        print "Tempo di esecuzione:", time.time() - start_time
        file.write("Tempo di esecuzione:"+str(time.time() - start_time) + " \n")
    
    
    # Cambio turno
    whiteTurn = not(whiteTurn)
    
    # Stampa della scacchiera
    utility.print_board(board, n_move)
    n_move += 1
    