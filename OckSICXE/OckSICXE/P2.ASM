PROGRAM3     START   0
             BASE    NUMB1
FIRST       +LDB     NUMB1
	     STL     RETADR
             CLEAR   X
             LDA     =C'123'
             LDA     =C'ABC'
             LDA     =C'123'
LOOP         ADD     NUMB2
             STA     ARRAY,X
             TIX     LIMIT
            +ADD     =X'01'
            +STA     NUMB3
             J       @RETAD
RETADR       RESW    1
LIMIT        WORD    10
ARRAY        RESB    2048
NUMB1       WORD    5
NUMB2       WORD    10
NUMB3       RESW    1
             END     FIRST
