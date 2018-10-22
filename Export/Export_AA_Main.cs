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
using UseJCR6;

namespace ScenLang.Export
{
    abstract class XCLASS{
        abstract public string Whole();
        abstract public string Entry(string tag);
        abstract public string extension();

        public string TabIt(string str){
            var s = str.Split('\n');
            var ret = "";
            foreach(string line in s){
                ret += $"\t{line}\n";
            }
            return ret;
        }
    }

    static class Export
    {
        public static SortedDictionary<string,XCLASS> Drivers = new SortedDictionary<string,XCLASS>();
        static Window win;
        static ListBox ExportTo;
        static RadioButton ToJCR;
        static RadioButton ToDir;
        static CheckButton lzma;
        static string Storage { get { if (lzma.Active) return "lzma"; return "Store"; }}

        static void DoExport(object sender,EventArgs e){
            if (ExportTo.ItemText == "") { QuickGTK.Error("I do need a target type to export to!"); return; }
            var xd = Drivers[ExportTo.ItemText];
            if (ToJCR.Active) {
                var of = QuickGTK.RequestFileSave("Output JCR File"); if (of == "") return;
                if (!qstr.Suffixed(of.ToLower(), ".jcr")) of += ".jcr";
                var oj = new TJCRCreate(of, Storage);

                foreach (string E in Data.Entries){
                    GUI.WriteLn($"Exporting: {E}");
                    oj.AddString(xd.Entry(E), $"{E}.{xd.extension()}", Storage);
                    if (JCR6.JERROR != "") QuickGTK.Error("Something went wrong during exporting:\n" + JCR6.JERROR);
                }
                oj.Close();
                GUI.WriteLn($"Exported: {of}");
            } else if (ToDir.Active) {
                var of = QuickGTK.RequestDir();
                if (of == "") return;
                if (!QuickGTK.Confirm("Please note, any existing files in the chosen directory will be mercilessly overwritten if the name matches with any file that will be exported!\n\nAre you sure you wish to continue?")) return;
                foreach (string E in Data.Entries){
                    GUI.WriteLn($"Exporting: {of}/{E}.{xd.extension()}");
                    var p = System.IO.Path.GetDirectoryName($"{of}/{E}");
                    System.IO.Directory.CreateDirectory(p);
                    QOpen.SaveString($"{of}/{E}.{xd.extension()}", xd.Entry(E));
                }
            } else {
                var of = QuickGTK.RequestFileSave($"Save this to {xd.extension()} file:");
                if (of == "") return;
                if (!qstr.Suffixed(of.ToLower(), $".{xd.extension()}")) of += $".{xd.extension()}";
                GUI.WriteLn($"Exporting to file: {of}");
                QOpen.SaveString(of, xd.Whole());
            }
            win.Destroy();
            GUI.bExport.Sensitive = true;
        }


        static void CancelExport(object sender, EventArgs e){
            win.Destroy();
            GUI.bExport.Sensitive = true;
        }

        // Alternate GUI
        static public void Go()
        {
            GUI.bExport.Sensitive = false;
            win = new Window("Export");
            ExportTo = new ListBox();
            ToDir = new RadioButton("Directory"); ToDir.Active = false;
            ScrolledWindow ToDirScroll = new ScrolledWindow(); ToDirScroll.Add(ExportTo.Gadget);
            foreach (string drv in Drivers.Keys) ExportTo.AddItem(drv);
            ToJCR = new RadioButton(ToDir,"JCR package"); ToJCR.Active = true;
            RadioButton ToFile = new RadioButton(ToDir,"Complete script file"); ToJCR.Active = false;
            lzma = new CheckButton("lzma compression (JCR package only)"); lzma.Active = true;
            Button Cancel = new Button("Cancel");
            Button ok = new Button("Ok");
            VBox Main = new VBox(); win.Add(Main);
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
            win.DeleteEvent += CancelExport;
            Cancel.Clicked += CancelExport;
            Main.Add(bexportto);
            Main.Add(XType);
            Main.Add(Bottom);
            win.ShowAll();
        }

        public static void Init(){
            MKL.Version("Scenario Language - Export_AA_Main.cs","18.10.22");
            MKL.Lic    ("Scenario Language - Export_AA_Main.cs","GNU General Public License 3");
            Drivers["JSON"] = new XJSON();
        }
    }
}
