If you use these building scripts to build ScenLang from source, please note the following:

ScenLang uses external sources and they are only LINKED in the project file. All these sources are mine (or at least adapted by me for efficient usage... For me that is).

Before you run the script, best is to reservea folder for my projects. I'll now call this "~/Tricky" (please note, I'll limit myself to unix commands here), but you can substitute this with anything you like:

~~~shell
cd ~/Tricky
git clone https://github.com/TrickyGameTools/ScenLang
git clone https://github.com/Tricky1975/JCR6_Sharp JCR6
git clone https://github.com/Tricky1975/TrickyUnits_csharp TrickyUnits
~~~

With all this done (assuming you have the Mono/.NET SDK installed), you can either load ScenLang.sln in Visual Studio or type the following:
~~~shell
cd ~/Tricky/ScenLang/build
./buildme
~~~

This should build both the Windows/Linux build and a mac application bundle.
