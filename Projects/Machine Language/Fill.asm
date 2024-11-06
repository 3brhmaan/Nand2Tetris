// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/4/Fill.asm

// Runs an infinite loop that listens to the keyboard input. 
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel. When no key is pressed, 
// the screen should be cleared.

/*
if(RAM[KBD] != 0)
{
	for(i=0; screen+i < KBD ; i++)
	{
		RAM[screen+i] = RAM[0]
	}
}
else{
	for(i=0; screen+i < KBD ; i++)
	{
		RAM[screen+i] = RAM[0]
	}
}

*/


(START)
// if(RAM[KBD] == 0) goto else
	@KBD
	D=M
	@ElSE
	D;JEQ


// set RAM[0] = 1
	@0
	M=-1

(FORLOOP)
// initialize i = 0
	@i		
	M=0

(LOOP)
// if(SCREEN+i == KBD) goto end
	@SCREEN
	D=A
	@i
	D=D+M
	@KBD
	D=A-D
	@END
	D;JEQ

// Set RAM[screen+i] = RAM[0]
	// get the regired register address
	@SCREEN
	D=A
	@i
	D=D+M
	// store address in a temp variable
	@tmpAddress
	M=D
	// get RAM[0] value
	@0
	D=M
	// set RAM[tmpAddress] = RAM[0]
	@tmpAddress
	A=M
	M=D

// i++
	@i
	M=M+1

	@LOOP
	0;JMP

(ElSE)
// set RAM[0] = 0
	@0
	M=0

// go to the for loop
	@FORLOOP
	0;JMP


(END)
	@START
	0;JMP
