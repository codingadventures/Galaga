@ECHO OFF

mkdir .\GalagaNetworkingMirror

mklink /D .\GalagaNetworkingMirror\Assets ..\Assets
ECHO Link to Assets Created

mklink /D .\GalagaNetworkingMirror\ProjectSettings ..\ProjectSettings
ECHO Link to Project Settings Created
