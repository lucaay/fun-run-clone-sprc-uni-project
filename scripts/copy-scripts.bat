@echo off
setlocal enabledelayedexpansion

set "outputFile=output.txt"
set "fileList=GameManager.cs MultiplayerController.cs Player.cs PlayerInfo.cs PowerUp.cs SceneManager.cs" 

REM Create an empty output file
type nul > "%outputFile%"

REM Loop through each file in the list and append its content to the output file
for %%f in (%fileList%) do (
    type "%%f" >> "%outputFile%"
)

echo "Content of all files copied to %outputFile%"

pause
