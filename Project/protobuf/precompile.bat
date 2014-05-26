@echo off
cd /d %~dp0
set basepath=%cd%
set exepath=%basepath%\tools\Precompile\
cd ../Assets/Plugins
set dllpath=%cd%
del ProtobufSerializer.dll
del ProtobufSerializer.dll.meta
%exepath%\precompile.exe ProtoGenData.dll -o:ProtobufSerializer.dll -t:ProtobufSerializer
pause