# BrainfuckCompiler
 Turns brainfuck programs into windows executables.
 
 
 Currently this compiler will compile you bf programs into an executable that will by default read input from a file named <program_name>\_input.txt and output into a file named <program_name>\_output.txt

In its current form the compiler is non-optimizing, but the in the future it will make simple optimizations.

# How it works

This compiler works by transpiling all your bf code into C.  After that it uses a windows C compiler to turn that C code into an windows executable.

# Requirments

This program requires the [mingw-w64](https://mingw-w64.org/doku.php/download) C compiler.

This program also requires the PATH environment variable to reference the bin folder of your mingw-w64 installation.

# How to use the compiler

The executable is in the [/bin/Debug/netcoreapp3.0](/bin/Debug/netcoreapp3.0) folder.  The program is designed to be used from the command line.

To compile a program use 'BrainfuckCompiler <Your_bf_program>'.  This will create an executable with the same name that will also be runnable from the command line.

# Build Flags
-cio: Uses console IO rather than file IO  
-fio: Uses file IO rather than console IO (this flag is redundant as it is set by default)  
-s   : Keeps the C source file instead of deleting it


# Issues

1: 'gcc' is not recognized as an internal or external command, operable program or batch file.

Fix: Please create a reference to the bin folder of your mingw-w64 installation in the PATH environment variable.
