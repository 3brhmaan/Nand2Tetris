
using Assembler;

var parser = new Parser(@"D:\Repo\Nand2Tetris\Projects\Assembler\pong\Pong.asm");
parser.Start();
parser.WriteToFile();


Console.ReadLine();