using System.Reflection;

namespace Assembler;

class Parser
{
    private string filePath;
    private List<string> instructions = new List<string>();
    private List<string> output = new List<string>();
    private Dictionary<string , int> symbolTable = new Dictionary<string , int>();
    private Dictionary<string , string> destTable = new Dictionary<string , string>();
    private Dictionary<string , string> compTable = new Dictionary<string , string>();
    private Dictionary<string , string> jmpTable = new Dictionary<string , string>();
    private int instructionCount = 0;
    private int memoryLocation = 16;

    public Parser(string filePath)
    {
        this.filePath = filePath;
        InitilizeSymbolTable();
        InitilizeDestTable();
        InitilizeJumpTable();
        InitilizeCompTable();
    }



    public void Start()
    {
        if (!IsFilePathExist())
            return;

        Read();
        HandleLabels();
        HandleInstructions();
    }

    public void WriteToFile()
    {
        // Get the file name without extension
        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);

        // Get the directory path
        string directory = Path.GetDirectoryName(filePath);

        // Create new file path with different extension
        string outputFilePath = Path.Combine(directory , $"{fileNameWithoutExt}.hack");

        using (var writer = new StreamWriter(outputFilePath))
        {
            for (int i = 0 ; i < output.Count ; i++)
            {
                // Write without newline for last line
                if (i == output.Count - 1)
                    writer.Write(output[i]);
                else
                    writer.WriteLine(output[i]);
            }
        }
    }

    private void HandleInstructions()
    {
        foreach (var instruction in instructions)
        {
            string binaryCode;

            if(instruction.StartsWith("(") && instruction.EndsWith(")"))
                continue;

            if (IsA_Instruction(instruction))
            {
                int decimalValue = HandleA_Instruction(instruction);
                binaryCode = GenerateA_InstructionBinaryCode(decimalValue);
            }
            else
            {
                binaryCode = HandleC_Instruction(instruction);
            }

            output.Add(binaryCode.Trim());
            //Console.WriteLine(binaryCode);
        }
    }

    private string HandleC_Instruction(string instruction)
    {
        string dest = "null", comp = "null", jmp = "null";

        if (instruction.Contains("=") && instruction.Contains(";"))
        {
            // dest=comp;jmp
            var instructionContent = instruction.Split('=');
            dest = instructionContent[0];

            instructionContent = instructionContent[1].Split(";");
            comp = instructionContent[0];
            jmp = instructionContent[1];
        }
        else if (instruction.Contains("="))
        {
            // dest=comp
            var instructionContent = instruction.Split('=');
            dest = instructionContent[0];
            comp = instructionContent[1];
        }
        else if (instruction.Contains(";"))
        {
            // comp;jmp
            var instructionContent = instruction.Split(';');
            comp = instructionContent[0];
            jmp = instructionContent[1];
        }
        else
        {
            // comp
            comp = instruction;
        }


        return "111" + GenerateC_InstructionCompBinaryCode(comp)
             + GenerateC_InstructionDestBinaryCode(dest)
             + GenerateC_InstructionJmpBinaryCode(jmp);
    }

    private string GenerateC_InstructionCompBinaryCode(string comp)
    {
        return compTable[comp];
    }

    private string GenerateC_InstructionDestBinaryCode(string dest)
    {
        return destTable[dest];
    }

    private string GenerateC_InstructionJmpBinaryCode(string jmp)
    {
        return jmpTable[jmp];
    }

    private string GenerateA_InstructionBinaryCode(int decimalValue)
    {
        return Convert.ToString(decimalValue , 2).PadLeft(16 , '0');
    }

    private int HandleA_Instruction(string instruction)
    {
        // @value
        var value = instruction.Substring(1);

        if (symbolTable.ContainsKey(value))
            return symbolTable[value];
        else
        {
            if (int.TryParse(value , out int result))
                return result;
            else
            {
                symbolTable.Add(value , memoryLocation);
                memoryLocation++;

                return symbolTable[value];
            }
        }
    }

    private bool IsA_Instruction(string instruction)
    {
        return instruction.StartsWith("@");
    }

    private void HandleLabels()
    {
        foreach (var instruction in instructions)
        {
            if (instruction.StartsWith("(") && instruction.EndsWith(")"))
            {
                string label = instruction.Substring(1 , instruction.Length - 2);
                symbolTable.Add(label , instructionCount);
            }
            else
                instructionCount++;
        }
    }

    private void Read()
    {
        var instructions = File.ReadAllLines(filePath);
        foreach (string line in instructions)
        {
            var instructionWithoutComment = RemoveComment(line);
            if (!string.IsNullOrEmpty(instructionWithoutComment))
            {
                this.instructions.Add(instructionWithoutComment);
            }
        }
    }

    private bool IsFilePathExist()
    {
        if (!File.Exists(filePath))
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"File: {filePath} Desn't Exist");
            Console.BackgroundColor = ConsoleColor.White;

            return false;
        }

        return true;
    }

    private string RemoveComment(string instruction)
    {
        var idx = instruction.IndexOf("//");
        if (idx != -1)
        {
            instruction = instruction.Substring(0 , idx).Trim();
        }

        return instruction.Trim();
    }

    private void InitilizeSymbolTable()
    {
        for (int i = 0 ; i < 16 ; i++)
        {
            symbolTable.Add("R" + i , i);
        }

        symbolTable.Add("SCREEN" , 16384);
        symbolTable.Add("KBD" , 24576);
        symbolTable.Add("SP" , 0);
        symbolTable.Add("LCL" , 1);
        symbolTable.Add("ARG" , 2);
        symbolTable.Add("THIS" , 3);
        symbolTable.Add("THAT" , 4);
    }
    private void InitilizeCompTable()
    {
        //put all comp posibilities with A into a HashMap,a=0
        compTable.Add("0" , "0101010");
        compTable.Add("1" , "0111111");
        compTable.Add("-1" , "0111010");
        compTable.Add("D" , "0001100");
        compTable.Add("A" , "0110000");
        compTable.Add("!D" , "0001101");
        compTable.Add("!A" , "0110001");
        compTable.Add("-D" , "0001111");
        compTable.Add("-A" , "0110011");
        compTable.Add("D+1" , "0011111");
        compTable.Add("A+1" , "0110111");
        compTable.Add("D-1" , "0001110");
        compTable.Add("A-1" , "0110010");
        compTable.Add("D+A" , "0000010");
        compTable.Add("D-A" , "0010011");
        compTable.Add("A-D" , "0000111");
        compTable.Add("D&A" , "0000000");
        compTable.Add("D|A" , "0010101");

        //put all comp posibilities with M into a HashMap,a=1
        compTable.Add("M" , "1110000");
        compTable.Add("!M" , "1110001");
        compTable.Add("-M" , "1110011");
        compTable.Add("M+1" , "1110111");
        compTable.Add("M-1" , "1110010");
        compTable.Add("D+M" , "1000010");
        compTable.Add("D-M" , "1010011");
        compTable.Add("M-D" , "1000111");
        compTable.Add("D&M" , "1000000");
        compTable.Add("D|M" , "1010101");
    }

    private void InitilizeDestTable()
    {
        destTable.Add("null" , "000");
        destTable.Add("M" , "001");
        destTable.Add("D" , "010");
        destTable.Add("MD" , "011");

        destTable.Add("A" , "100");
        destTable.Add("AM" , "101");
        destTable.Add("AD" , "110");
        destTable.Add("AMD" , "111");
    }

    private void InitilizeJumpTable()
    {
        jmpTable.Add("null" , "000");
        jmpTable.Add("JGT" , "001");
        jmpTable.Add("JEQ" , "010");
        jmpTable.Add("JGE" , "011");

        jmpTable.Add("JLT" , "100");
        jmpTable.Add("JNE" , "101");
        jmpTable.Add("JLE" , "110");
        jmpTable.Add("JMP" , "111");
    }
}
