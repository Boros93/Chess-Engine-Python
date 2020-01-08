# -*- coding: utf-8 -*-
"""
Created on Mon May 28 09:48:21 2018

@author: Andrea Sequenzia
"""

import socket

class Communicator:

    def __init__(self):
        self.__move = ""
        self.__legal_moves = ""
        
    
    def communicate(self):
        host = '127.0.0.1'
        port = 50000
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.bind((host, port))
        s.listen(1)
        self.conn, self.addr = s.accept()
        print ("Connection from", self.addr)
        
    def receive_move(self):
        data = self.conn.recv(1024)
        if not data: 
            print 'No data'
        print("Mossa: "+(data))
        return str(data)
    
    def send_move(self, move):
        self.conn.sendall(move)
        
    def close_conn(self):
        self.conn.close()