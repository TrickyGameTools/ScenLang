// Lic:
// 	ScenLang
// 	XML exporter
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
namespace ScenLang.Export
{
    class XXML : XCLASS
    {
        public override string extension() => "xml";

        public override string Entry(string tag)
        {
            var ret = "";
            var E = Data.GetEntry(tag);
            var TList = E.TagList;
            foreach (string Tag in TList) {
                if (ret != "") ret += "\n";
                ret += $"<tag name=\"{Tag}\">\n";
                var T = E.GetTag(Tag);
                var num = T.CountTextBoxes;
                for (int i = 0; i <= num; i++) {
                    var tb = (DataTextbox)T.GetTextBox(i);
                    ret += $"\t<textbox idx={i} Picture=\"{tb.Picture}\" PictureSpecific=\"{tb.PictureSpecific}\" AltFont=\"{tb.AltFont}\">\n";
                    foreach (int taal in tb.Head.Keys)
                    {
                        var tn = Data.LanguagePure(taal);
                        ret += $"\t\t<{tn}_head>{tb.Head[taal]}</{tn}_head>\n";
                        ret += $"\t\t<{tn}_content>{tb.Content[taal]}</{tn}_content>\n";
                    }
                    ret += $"\t</textbox>\n";
                }
                ret += "</tag>";
            }
            return ret;
        }

        public override string Whole(){
            var ret = "";
            foreach (string E in Data.Entries){
                ret += $"<entry name='{E}'>{Entry(E)}</entry>\n";
            }
            return ret;
        }

    }
}
