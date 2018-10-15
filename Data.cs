// Lic:
// 	Scenario Language
// 	Data
// 	
// 	
// 	
// 	(c) Jeroen P. Broks, 2015, 2017, 2018, All rights reserved
// 	
// 		This program is free software: you can redistribute it and/or modify
// 		it under the terms of the GNU General Public License as published by
// 		the Free Software Foundation, either version 3 of the License, or
// 		(at your option) any later version.
// 		
// 		This program is distributed in the hope that it will be useful,
// 		but WITHOUT ANY WARRANTY; without even the implied warranty of
// 		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// 		GNU General Public License for more details.
// 		You should have received a copy of the GNU General Public License
// 		along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 		
// 	Exceptions to the standard GNU license are available with Jeroen's written permission given prior 
// 	to the project the exceptions are needed for.
// Version: 18.10.15
// EndLic
﻿using System;
using TrickyUnits;
using TrickyUnits.GTK;
namespace ScenLang
{
    /// <summary>
    /// This class contains all the data and functions to manipulate them.
    /// </summary>
    static public class Data
    {
        static TGINI MainConfig;
        static string _project;
        static public bool Loaded => MainConfig != null;
        static public string Project { get => _project; }

        static Data()
        {
            MKL.Lic    ("Scenario Language - Data.cs","GNU General Public License 3");
            MKL.Version("Scenario Language - Data.cs","18.10.15");
        }

        static public void LoadFromArgs(string[] args){
            if (args.Length > 0)
                LoadProject(args[0]);
            else {
                var prj = QuickGTK.RequestFile("Please choose a project file:");
                if (prj != "") LoadProject(prj);
            }

        }

        static public void LoadProject(string GINIFile)
        {
            MainConfig = GINI.ReadFromFile(GINIFile);
            _project = GINIFile;
        }
    }
}
