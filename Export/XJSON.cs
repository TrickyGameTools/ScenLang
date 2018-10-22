// Lic:
// 	ScenLang
// 	JSON exporter
// 	
// 	
// 	
// 	(c) Jeroen P. Broks, 2018, All rights reserved
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
// Version: 18.10.22
// EndLic
﻿using System;
using TrickyUnits;
namespace ScenLang.Export
{
    class XJSON : XCLASS
    {
        public XJSON(){
            MKL.Lic    ("Scenario Language - XJSON.cs","GNU General Public License 3");
            MKL.Version("Scenario Language - XJSON.cs","18.10.22");
        }
        override public string Whole(){
            var ret = "";
            foreach(string E in Data.Entries){
                if (ret != "") ret += ",\n";
                ret += $"\t[\"{E}\"] : {TabIt(Entry(E))}";
            }
            return "{"+ret+"}";
        }

        override public string Entry(string tag){
            var ret = "{\n";
            var E = Data.GetEntry(tag);
            var TList = E.TagList;
            bool comma = false;
            foreach(string Tag in TList){
                if (comma) ret += ",\n"; comma = true;
                var T = E.GetTag(Tag);
                var num = T.CountTextBoxes;
                ret += $"\t\"{Tag}\" : [\n";
                for (int i = 0; i <= num; i++){
                    if (i != 0) ret += ",\n";
                    var TB = (DataTextbox)T.GetTextBox(i);
                    ret += "\t\t{\n";
                    ret += $"\t\t\t\"Picture\"         : \"{TB.Picture}\",\n";
                    ret += $"\t\t\t\"PictureSpecific\" : \"{TB.PictureSpecific}\",\n";
                    ret += $"\t\t\t\"AltFont\"         : \"{TB.AltFont}\"";
                    foreach (int taal in TB.Head.Keys ){
                        ret += ",\n";
                        ret += $"\t\t\t\"{Data.LanguagePure(taal)}.Head\" : \"{TB.Head[taal]}\",\n";
                        ret += $"\t\t\t\"{Data.LanguagePure(taal)}.Content\" : `{TB.Content[taal]}`";
                    }
                    ret += "\t\t}";
                }
                ret += "\n\t]";
            }
            return ret+"\n}";
        }

        override public string extension() => "json";
    }
}
