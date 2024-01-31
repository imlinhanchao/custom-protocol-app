@echo off

reg.exe add HKCU\Software\Classes\protocol /ve /d "URL:protocol" /f
reg.exe add HKCU\Software\Classes\protocol /v "URL Protocol" /d "" /f
reg.exe add HKCU\Software\Classes\protocol\shell /f
reg.exe add HKCU\Software\Classes\protocol\shell\open /f
reg.exe add HKCU\Software\Classes\protocol\shell\open\command /ve /d "\"%~dp0ProtocolApp\bin\Debug\ProtocolApp.exe\" \"%%1\"" /f
