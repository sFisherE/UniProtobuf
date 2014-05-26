@echo off
cd /d %~dp0
set basepath=%cd%
set exepath=%basepath%\tools\ProtoGen\
del /q /s cs\*.cs
cd proto
for %%i in (*.proto) do (
echo %%i
%exepath%\protogen.exe -i:%%i -o:%basepath%\cs\%%i.cs -ns:ProtoGen
)
pause
