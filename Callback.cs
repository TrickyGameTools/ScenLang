// Lic:
// 	Scenario Language
// 	Callback functions
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
namespace ScenLang
{
    static class Callback
    {
        public static bool dontedit;
        public static bool dontlink;
        public static void EditPicDir(object sender,EventArgs arg){}
        public static void EditPicSpecific(object sender, EventArgs arg) { }
        public static void EditAltFont(object sender, EventArgs arg) { }
        public static void EditAllowTrim(object sender, EventArgs arg){ }
        public static void EditNameLinking(object sender, EventArgs arg) { }

        public static void EditLHead(object sender,EventArgs arg){ }
        public static void EditLText(object sender,EventArgs arg){ }


        public static void PickEntry(object sender, EventArgs arg){
            GUI.UpdateTagList();
            GUI.AutoEnable();
        }

        public static void IMKL()
        {
            MKL.Version("Scenario Language - Callback.cs","18.10.17");
            MKL.Lic    ("Scenario Language - Callback.cs","GNU General Public License 3");
        }


    }
}
