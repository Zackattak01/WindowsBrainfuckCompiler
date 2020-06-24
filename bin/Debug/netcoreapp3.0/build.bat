@echo off

gcc -o %1 temp.c


if %2==False (del temp.c)