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
// Version: 18.10.18
// EndLic
﻿using System;
using Gtk;
using TrickyUnits;
using TrickyUnits.GTK;
namespace ScenLang
{
    static class Callback
    {
        public static bool dontedit;
        public static bool dontlink;
        public static void EditPicDir(object sender,EventArgs arg){
            //QuickGTK.Info($"Hoi! {dontedit} / {dontlink}");
            if (dontedit) return;
            var p = (Entry)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.Picture = p.Text;
            if (dp.AllowTrim) dp.Picture = dp.Picture.Trim();
            if (dontlink || (!dp.NameLinking)) return;
            dontlink = true;
            foreach (Entry e in GUI.EntryHead) e.Text = p.Text;
            dontlink = false;
        }

        public static void EditPicSpecific(object sender, EventArgs arg) {
            if (dontedit) return;
            var p = (Entry)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.PictureSpecific = p.Text;
            if (dp.AllowTrim) dp.PictureSpecific = dp.PictureSpecific.Trim();
        }
        public static void EditAltFont(object sender, EventArgs arg) {
            if (dontedit) return;
            var p = (Entry)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.AltFont = p.Text;
            if (dp.AllowTrim) dp.AltFont = dp.AltFont.Trim();
        }
        public static void EditAllowTrim(object sender, EventArgs arg){
            if (dontedit) return;
            var p = (CheckButton)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.AllowTrim = p.Active;
        }
        public static void EditNameLinking(object sender, EventArgs arg) {
            if (dontedit) return;
            var p = (CheckButton)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.NameLinking = p.Active;
        }

        public static void EditLHead(object sender,EventArgs arg){
            if (dontedit) return;
            var w = (Widget)sender;
            var i = GUI.GetIndex(w);
            var p = (Entry)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.Head[i] = p.Text;
            if (dp.AllowTrim) dp.Head[i] = dp.Head[i];
            if (dontlink || (!dp.NameLinking)) return;
            dontlink = true;
            GUI.enPicDir.Text = p.Text;
            for (int j = 0; j < GUI.EntryHead.Length;j++){
                if (j != i) GUI.EntryHead[j].Text = p.Text;
            }
            dontlink = false;
        }
        public static void EditLText(object sender,EventArgs arg){
            if (dontedit) return;
            var b = (TextBuffer)sender;
            var w = GUI.WidgetFromBuffer(b);
            //var p = (TextView)sender;
            var i = GUI.GetIndex(w);
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.Content[i] = b.Text;
        }


        public static void PickEntry(object sender, EventArgs arg){
            dontedit = true;
            GUI.UpdateTagList();
            GUI.AutoEnable();
            dontedit = false;
        }

        public static void PickTag(object sender, EventArgs arg){
            Data.PickBox(0);
        }

        public static void AddEntry(object sender,EventArgs arg){
            QuickInputBox.Create("Please enter a name for the new entry", delegate (string s, bool ok)
            {
                if (ok) { Data.NewEntry(s); }
            },"",null,"[A-Za-z0-9_/]*");
        }
        public static void RenEntry(object sender,EventArgs arg){}
        public static void RemEntry(object sender,EventArgs arg){}

        public static void AddTag(object sender,EventArgs arg){}
        public static void RenTag(object sender,EventArgs arg){}
        public static void RemTag(object sender,EventArgs arg){}

        public static void IMKL()
        {
            MKL.Version("Scenario Language - Callback.cs","18.10.18");
            MKL.Lic    ("Scenario Language - Callback.cs","GNU General Public License 3");
        }


    }
}
