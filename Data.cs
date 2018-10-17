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
// Version: 18.10.17
// EndLic
﻿using System;
using TrickyUnits;
using TrickyUnits.GTK;
using UseJCR6;
using System.Collections.Generic;

namespace ScenLang{

    class DataTextbox{
        // The int in the dictionaries is the language ID, not the page number!
        public Dictionary<int, string> Head = new Dictionary<int, string>();
        public Dictionary<int, string> Content = new Dictionary<int, string>();
        public string Picture = "";
        public string PictureSpecific = "";
        public string AltFont = "";
        public bool AllowTrim = true;
        public bool NameLinking = true;

    }

    class DataTag{
        // And in this dictionary the 'int' is the page number... Seems odd, but is not as odd as it seems.
        public Dictionary<int, DataTextbox> TextBox = new Dictionary<int, DataTextbox>();
    }

    class DataEntry{
        public DataEntry() => Console.WriteLine($"Created new entry"); // debug
        public Dictionary<string, DataTag> Tags = new Dictionary<string, DataTag>();
        public List<string> TagList = new List<string>();
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
        static TJCRDIR PriJCR { get => JCR[0]; }
        static Dictionary<string, DataEntry> Entry = new Dictionary<string, DataEntry>();
        //static Dictionary<string, List<string>> TagList = new Dictionary<string, List<string>>();

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
            MKL.Version("Scenario Language - Data.cs","18.10.17");
        }

        static public void LoadFromArgs(string[] args){
            if (args.Length > 0)
                LoadProject(args[0]);
            else {
                var prj = QuickGTK.RequestFile("Please choose a project file:");
                if (prj != "") LoadProject(prj);
            }

        }

        static void Parse(int id,string EntryName){
            Console.WriteLine($"{id}: Parsing: {EntryName}");
            List<string> tm;
            DataEntry en ;
            DataTag tg;
            DataTextbox tb=null;
            var indexes = new Dictionary<string, int>();
            if (id == 0)
            {
                en = new DataEntry();
                tm = new List<string>();
                Entry[EntryName] = en;
                en.TagList = tm;
            }
            else
            {
                en = Entry[EntryName];
                tm = en.TagList;
            }
            GUI.Assert(tm != null, $"Internal Error!\nNull for tagmap received when parsing {EntryName} for language #{id}.");
            var ctag = "CRASH";
            var J = JCR[id];
            GUI.Assert(J.Exists(EntryName), $"Entry {EntryName} not found in language #{id}");
            foreach(string tl in J.ReadLines(EntryName,true)){
                var l = tl.Trim();
                if (qstr.Prefixed(l,"[") && qstr.Suffixed(l,"]")){
                    ctag = l.ToUpper();
                    if (ctag=="[SCENARIO]"){
                        foreach(string t in tm){
                            if (!en.Tags.ContainsKey(t)) en.Tags[t] = new DataTag();
                        }
                    }
                } else if (l!="" && !qstr.Prefixed(l,"--")){
                    switch(ctag){
                        case "CRASH":
                            Console.WriteLine($"{qstr.Left(l,1)} ... {qstr.Right(l, 1)} => {l} len: {l.Length}");
                            GUI.Assert(false, $"Tagless command in language #{id} entry {EntryName}\n\n{l}");
                            break;
                        case "[REM]":
                            break;
                        case "[TAGS]":
                            if (id==0){
                                if (tm.Contains(l.ToUpper()))
                                    QuickGTK.Warn("Duplicate tag found!");
                                else
                                    tm.Add(l.ToUpper());
                            } else {
                                if (!tm.Contains(l.ToUpper())) {
                                    QuickGTK.Warn("Language #{id} contains the non-existent tag {l}.\n\nI will continue, but do expect crashes!");
                                }
                            }
                            break;
                        case "[SCENARIO]":
                            var c = l[0];
                            var s = qstr.Right(l, l.Length - 1);
                            var tt = "";
                            switch(c){
                                case '@':
                                    GUI.Assert(tm.Contains(s.ToUpper()), $"Reference to non-existent tag {s}");
                                    tt = s.ToUpper();
                                    tg = en.Tags[s.ToUpper()];
                                    if (!indexes.ContainsKey(tt)) { indexes[tt] = -1; }
                                    indexes[tt]++;
                                    if (id == 0)
                                    {
                                        tb = new DataTextbox();
                                        tg.TextBox[indexes[tt]] = tb;
                                    }
                                    else {
                                        tb = tg.TextBox[indexes[tt]];
                                    }
                                    tb.Content[id] = "";
                                    break;
                                case '!':
                                    tb.Head[id] = s;
                                    break;
                                case '*':
                                    GUI.Assert(id == 0 || tb.Picture == s, $"Picture mismatch!\n{id}:{tt}\n{tb.Picture}!={s}");
                                    tb.Picture = s;
                                    Console.WriteLine($"Defined Picture\n{id}:{tt}\n{tb.Picture}");
                                    break;
                                case ':':
                                    GUI.Assert(id == 0 || tb.PictureSpecific == s, "Picture specific mismatch");
                                    tb.PictureSpecific = s;
                                    break;
                                case '#':
                                    if (tb.Content[id] != "") tb.Content[id] += "\n";
                                    tb.Content[id] += s;
                                    break;
                                default:
                                    GUI.Fail($"Unknown scenario tag {c}");
                                    break;
                            }
                            break;
                        default:
                            GUI.Assert(false, $"I do not know how to handle tag {ctag}");
                            break;
                    }
                }
            }
        }

        static void JCR_Reload(bool updategui=true){
            for (int i = 0; i < NumLanguages;i++){
                JCR[i] = JCR6.Dir(Config($"Lang{i+1}.File",true));
                if (!GUI.Assert(JCR[i] !=null, $"JCR6 ERROR\n{JCR6.JERROR}")) return;
            }
            if (updategui) GUI.ListEntries(Entries);
            foreach(string ekey in Entries){
                Entry[ekey] = new DataEntry();
                for (int i = 0; i < NumLanguages;i++){
                    Parse(i, ekey);
                }
            }
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
