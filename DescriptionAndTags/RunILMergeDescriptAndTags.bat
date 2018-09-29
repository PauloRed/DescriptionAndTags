@echo off
rem Merges DLL for Description and Tags
"c:\program files (x86)\microsoft\ilmerge\ilmerge.exe"  /out:DescriptionAndTags.dll  "bin\release\DescriptionAndTags.dll" "bin\release\Csvhelper.dll"  /lib:"C:\General\MSCRM\XrmToolbox\New Plugins\DescriptionAndTags\bin\release" /targetplatform:v4
pause