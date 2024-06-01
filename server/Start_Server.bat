@echo off
set SERVER_EXE="dedicated\Dedicated_Server.console.exe"
set SERVER_ARGS=--server --headless

cmd /k "%SERVER_EXE% %SERVER_ARGS%"
