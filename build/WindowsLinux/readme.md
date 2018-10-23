# Windows Linux

This folder will be were the building script will place the ScenLang.exe file, which should in Windows just work, although if you start it through Mono in Windows you can be pretty sure the GTKSharp dependencies are properly present.
The Linux script will just run Mono and call the ScenLang.exe file with it.

NOTE: Do *NOT* use Wine.... That will very likely NOT work!
NOTE: Also note that ScenLang is able to accept project files as parameters, but that this batch file and shellscript are both not scripted to do so.

