# BrainfuckCompiler
 Turns brainfuck programs into windows executables.
 
# Features
The Compiler supports:  
Dead Code Removal  (experimental)
Consecutive Instruction Compression  
Out of Bounds Analysis (experimental)
File and Console IO  

# How it works

This compiler works by transpiling all your bf code into C.  After that it uses a windows C compiler to turn that C code into an windows executable.

# Requirments

This program requires the [mingw-w64](https://mingw-w64.org/doku.php/download) C compiler.

This program also requires the PATH environment variable to reference the bin folder of your mingw-w64 installation.

# How to use the compiler

The executable is in the [/bin/Debug/netcoreapp3.0](/bin/Debug/netcoreapp3.0) folder.  The program is designed to be used from the command line.

To compile a program use 'BrainfuckCompiler <Your_bf_program>'.  This will create an executable with the same name that will also be runnable from the command line.

If using the build flag "-fio" the IO files will be named <program_name>\_input.txt and <program_name>\_output.txt the output file will be created at runtime if it is not precreated but the input file will not, so it needs to be created before runtime.

# Build Flags
-cio: Uses console IO rather than file IO (this flag is redundant as it is set by default)  
-fio: Uses file IO rather than console IO  
-s: Keeps the C source file instead of deleting it

# Optimizations
This compiler is capable of combining several identical instructions into one instruction.

Previously a sequence of instructions like:
`>>>>>` 
Would be transpiled into:
```
i++;
i++;
i++;
i++;
i++;
```
However the compiler will optimize this to:
`i+=5;`
# Issues
First of all, going out of bounds on the tape is completly undefined.  The compiler will tell you if it thinks your program will go out of bounds.  However, if Compiler thinks the program will go out of bounds it will not be able to complete optimization.  The program will still compile, but unpredictable and unexpected behaivour is fully expected.

1: 'gcc' is not recognized as an internal or external command, operable program or batch file.

Fix: Please create a reference to the bin folder of your mingw-w64 installation in the PATH environment variable.
