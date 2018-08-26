@echo off
rem Merges DLL for Description and Tags
"c:\program files (x86)\microsoft\ilmerge\ilmerge.exe"  /out:DescriptionAndTags.dll  "bin\Debug\Plugins\DescriptionAndTags.dll" "bin\Debug\Csvhelper.dll"  /lib:"C:\General\MSCRM\XrmToolbox\New Plugins\DescriptionAndTags\bin\Debug" /targetplatform:v4
pause