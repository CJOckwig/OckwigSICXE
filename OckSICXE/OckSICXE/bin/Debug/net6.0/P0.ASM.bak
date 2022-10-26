PGRAM1       START   0
FIRST        LDB     NUMB1
             STL     RETAD
             LDA     NUMB1
LOOP         ADD     NUMB2
             STA     ARRAY,X
             TIX     LIMIT
             ADD     #1
             STA     NUMB3
             J       @RETAD
RETAD        RESW    1
LIMIT        WORD    10
ARRAY        RESB    1024
NUMB1        WORD    5
NUMB2        WORD    10
NUMB3        RESW    1
             END     FIRST
