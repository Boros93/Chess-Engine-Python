import chess
from move_manager import MoveManager
from utility import Utility
from AIEngine import AIAgent
import time
# Inizializzazioni
board = chess.Board()
mover = MoveManager()
utility = Utility()
agent = AIAgent()



print '---Inizio partita--- \n'
utility.print_board(board, 0)

whiteTurn = True
# Contatore mosse
n_move = 1

# Game loop
while True:
    # print board.is_attacked_by(chess.WHITE, chess.B7) per vedere se è minacciato
    # print board.fen()
    start_time = time.time()
    if whiteTurn:
        input_move = str(raw_input('Inserisci mossa:'))
    else:
        input_move = agent.make_best_move(board)
        
    # Interrupt
    if input_move == 'quit':
        break
    
    # Condizioni partita
    if board.is_check():
        print 'SCACCO!'
    if board.is_checkmate():
        print 'SCACCO MATTO!'
        break
    
    # Mossa legale?

    if not(mover.make_move(input_move, board)):
        continue
    
    # Stampa mossa
    if whiteTurn:
        print "Il bianco muove: " + str(input_move)
    else:
        print "Il nero muove: " + str(input_move)  
        print "Tempo di esecuzione:", time.time() - start_time
    
    
    # Cambio turno
    whiteTurn = not(whiteTurn)
    
    # Stampa della scacchiera
    utility.print_board(board, n_move)
    n_move += 1
    
    