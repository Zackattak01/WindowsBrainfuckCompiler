# BrainfuckCompiler
 Turns brainfuck programs into windows executables.
 
 
 Currently this compiler will compile you bf programs into an executable that will read input from a file named <program_name>\_input.txt and output into a file named <program_name>\_output.txt

Support for runtime IO is coming.

In its current form the compiler is non-optimizing, but the in the future it will make simple optimizations.

# How it works

This compiler works by transpiling all your bf code into c.  After that it uses a windows c compiler to turn that c code into an windows executable.

# Requirments

This program requires the [mingw-w64](https://mingw-w64.org/doku.php/download) c compiler.

This program also requires the PATH environment variable to reference the bin folder of your mingw-w64 installation.

# Issues

1: 'gcc' is not recognized as an internal or external command, operable program or batch file.

Fix: Please create a reference to the bin folder of your mingw-w64 installation in the PATH environment variable.
