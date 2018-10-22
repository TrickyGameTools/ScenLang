// Lic:
// 	ScenLang
// 	Export skeleton code
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
// Version: 18.10.22
// EndLic
﻿using System;
using System.Collections.Generic;
using Gtk;
using TrickyUnits;
using TrickyUnits.GTK;

namespace ScenLang.Export
{
    abstract class XCLASS{
        abstract public string Whole();
        abstract public string Entry(string tag);
        abstract public string extension();
    }

    static class Export
    {
        public static Dictionary<string,XCLASS> Drivers = new Dictionary<string,XCLASS>();

        static void DoExport(object sender,EventArgs e){

        }

        // Alternate GUI
        static public void Go()
        {
            Window win = new Window("Export");
            ListBox ExportTo = new ListBox();
            RadioButton ToDir = new RadioButton("Directory");
            ScrolledWindow ToDirScroll = new ScrolledWindow(); ToDirScroll.Add(ToDir);
            RadioButton ToJCR = new RadioButton("JCR package");
            RadioButton ToFile = new RadioButton("Complete script file");
            CheckButton lzma = new CheckButton("lzma compression (JCR package only)"); lzma.Active = true;
            Button Cancel = new Button("Cancel");
            Button ok = new Button("Ok");
            VBox Main = new VBox(); Main.Add(win);
            HBox bexportto = new HBox();
            bexportto.Add(new Label("Export to:"));
            bexportto.Add(ToDirScroll);
            HBox XType = new HBox();
            XType.Add(ToJCR);
            XType.Add(ToFile);
            XType.Add(ToDir);
            XType.Add(lzma);
            HBox Bottom = new HBox();
            Bottom.Add(new HBox()); // Empty space!
            HBox Buttons = new HBox(); Bottom.Add(Buttons);
            Buttons.Add(Cancel);
            Buttons.Add(ok);
            ok.Clicked += DoExport;
        }

        public static void Init(){
            MKL.Version("Scenario Language - Export_AA_Main.cs","18.10.22");
            MKL.Lic    ("Scenario Language - Export_AA_Main.cs","GNU General Public License 3");
            Drivers["JSON"] = new XJSON();
        }
    }
}
