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
using UseJCR6;
using System.Collections.Generic;

namespace ScenLang{

    class DataTextbox{
        // The int in the dictionaries is the language ID, not the page number!
        public Dictionary<int, string> Head;
        public Dictionary<int, string> Content;
    }

    class DataTag{
        public string Picture;
        public string PictureSpecific;
        public string AltFont;
        public bool AllowTrim = true;
        public bool NameLinking = true;
        // And in this dictionary the 'int' is the page number... Seems odd, but is not as odd as it seems.
        public Dictionary<int, DataTextbox> TextBox = new Dictionary<int, DataTextbox>();
    }

    class DataEntry{
        public Dictionary<string, DataTag> Tags = new Dictionary<string, DataTag>();
    }


    /// <summary>
    /// This class contains all the data and functions to manipulate them.
    /// </summary>
    static public class Data
    {
        static TGINI MainConfig;
        static string _project;
        static public bool Loaded => MainConfig != null;
        static public string Project { get => _project; }
        static string[] languages;
        static string[] _entries;
        static TJCRDIR[] JCR;
        static Dictionary<string, DataEntry> Entry = new Dictionary<string, DataEntry>();

        /// <summary>
        /// The only reason this was added the way it is, is because the guys at Microsoft are completely demented and will not allow me to do this the CLEAN and QUICK way as that will cause the C# compiler to throw errors for reasons that do not exist, but C# only makes them exist for intentions that are beyond any brain of INTELLIGENT human beings!
        /// </summary>
        /// <value>The entries.</value>
        static public string[] Entries { get {
                if (_entries!=null) return _entries;
                var J = JCR[0];
                var n = J.CountEntries;
                var ret = new string[n];
                var i = -1;
                foreach(string k in J.Entries.Keys) {
                    i++;
                    ret[i] = k;
                }
                _entries = ret;
                return ret;
            }
        }

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

        static void JCR_Reload(bool updategui=true){
            for (int i = 0; i < NumLanguages;i++){
                JCR[i] = JCR6.Dir(Config($"Lang{i+1}.File",true));
                if (!GUI.Assert(JCR[i] !=null, $"JCR6 ERROR\n{JCR6.JERROR}")) return;
            }
            if (updategui) GUI.ListEntries(Entries);
        }

        static public void LoadProject(string GINIFile)
        {
            MainConfig = GINI.ReadFromFile(GINIFile);
            _project = GINIFile;
            JCR = new TJCRDIR[NumLanguages];
            JCR_Reload(false);
        }

        static public int NumLanguages{ get {
                int ret=0;
                if (languages != null) return languages.Length;
                while (MainConfig.C($"Lang{ret + 1}.Name") != "") {
                    ret++;
                    GUI.Assert(MainConfig.C($"Lang{ret}.File") != "", $"No file for language #{ret}");
                }
                languages = new string[ret];
                for (int i = 0; i < ret; i++) languages[i] = MainConfig.C($"Lang{i + 1}.Name");
                return ret;
            }
        }

        static public string LanguagePure(int i) {
            if (languages == null) { var oh = NumLanguages; }
            GUI.Assert(i >= 0 && i < languages.Length, $"Language index out of range! ({i}/{languages.Length})");
            return languages[i];
        }

        static public string Language(int i) => LanguagePure(i - 1);

        static string Config(string key, bool crash=false){
            var ret = MainConfig.C(key);
            GUI.Assert((!crash) || ret != "", $"Project config needed key {key}, but it's either empty or not defined at all.");
            return ret;
        }
    }
}
