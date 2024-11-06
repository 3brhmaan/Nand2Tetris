// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/4/Mult.asm

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)
// The algorithm is based on repetitive addition.

/*
	this program used to multiply 2 numbers and store the result in memroy
	- assuming both numbers stored in RAM[0] & RAM[1]
	- and the result is stored in RAM[2]

	-> psudo code for 'num1*num2' :
	int mult = 0
	for(i = 0 ; i<num1 ; i++){
		mult += num2;
	}
*/
	

	@R2
	M=0		// reset the result RAM[2] = 0

	@R0
	D=M
	@num1	
	M=D		// num1(n) = RAM[0]

	@R1
	D=M
	@num2
	M=D		// num2 = RAM[1]

	@mult
	M=0		// mult = 0

	@i
	M=0		// i=0 -> index


(LOOP)
	@i
	D=M		//D = i
	@num1
	D=M-D	// D = num1(n) - i
	/*
		if i<num1 continue
		but if i>=num1 goto result
		same as num1-i(D) <= 0 goto result
	*/
	@RESULT
	D;JLE	

	@mult
	D=M
	@num2
	D=D+M	
	@mult
	M=D		// mult += num2

	@i
	M=M+1	// i++

	@LOOP
	0;JMP

(RESULT)
	@mult
	D=M
	@R2
	M=D		// RAM[2]=mult

(END)
	@RESULT
	0;JMP


