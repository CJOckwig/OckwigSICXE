PROGRAM4     START   0
             BASE    NUMB1
SIZE	     EQU     2048
FIRST        +LDB    #NUMB1
             STL     RETAD
             CLEAR   X
             LDA     =X'000000'
LOOP         ADD     NUMB2
             STA     ARRAY,X
             TIX     LIMIT
             ADD     #1
             STA     NUMB3
             J       @RETAD
RETAD   RESW    1
LIMIT        WORD    10
ARRAY        RESB    SIZE
NUMB1       WORD    5
NUMB2       WORD    10
NUMB3       RESW    1
INPUT        BYTE    X'F1'
             END     FIRST
