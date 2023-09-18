// Lic:
// Scenario Language
// Call back manager
// 
// 
// 
// (c) Jeroen P. Broks, 2015, 2017, 2018, 2021
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Please note that some references to data like pictures or audio, do not automatically
// fall under this licenses. Mostly this is noted in the respective files.
// 
// Version: 21.02.16
// EndLic

using System;
using Gtk;
using TrickyUnits;
using TrickyUnits.GTK;


namespace ScenLang
{
    static class Callback
    {

        const string allow_RegEx = @"[A-Za-z0-9_\.\/\- ]*";

        public static void IMKL()
        {
            MKL.Version("Scenario Language - Callback.cs","21.02.16");
            MKL.Lic    ("Scenario Language - Callback.cs","GNU General Public License 3");
        }

        static string renameoriginal = "";
        public static bool dontedit;
        public static bool dontlink;
        static bool _modified = false;

        static public bool modified {
            get => _modified;
            set {
                if (value && (!_modified)) GUI.WriteLn("Modifications made! Auto-save enabled");
                _modified = value;
            }
        }

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
            modified = true;
        }

        public static void EditPicSpecific(object sender, EventArgs arg) {
            if (dontedit) return;
            var p = (Entry)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.PictureSpecific = p.Text;
            if (dp.AllowTrim) dp.PictureSpecific = dp.PictureSpecific.Trim();
            modified = true;
        }

        public static void EditAudio(object sender,EventArgs e) {
            if (dontedit) return;
            var p = (Entry)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.Audio = p.Text;
            if (dp.AllowTrim) dp.Audio = dp.Audio.Trim();
            modified = true;
        }
        public static void EditAltFont(object sender, EventArgs arg) {
            if (dontedit) return;
            var p = (Entry)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.AltFont = p.Text;
            if (dp.AllowTrim) dp.AltFont = dp.AltFont.Trim();
            modified = true;
        }
        public static void EditAllowTrim(object sender, EventArgs arg){
            if (dontedit) return;
            var p = (CheckButton)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.AllowTrim = p.Active;
            modified = true;
        }
        public static void EditNameLinking(object sender, EventArgs arg) {
            if (dontedit) return;
            var p = (CheckButton)sender;
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.NameLinking = p.Active;
            modified = true;
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
            modified = true;
        }
        public static void EditLText(object sender,EventArgs arg){
            if (dontedit) return;
            var b = (TextBuffer)sender;
            var w = GUI.WidgetFromBuffer(b);
            //var p = (TextView)sender;
            var i = GUI.GetIndex(w);
            var dp = (DataTextbox)Data.ChosenTextBox();
            dp.Content[i] = b.Text;
            modified = true;
        }


        public static void PickEntry(object sender, EventArgs arg){
            dontedit = true;
            GUI.UpdateTagList();
            GUI.AutoEnable();
            dontedit = false;
            modified = true;
        }

        public static void PickTag(object sender, EventArgs arg){
            Data.PickBox(0);
        }

        public static void AddEntry(object sender,EventArgs arg){
            QuickInputBox.Create("Please enter a name for the new entry", delegate (string s, bool ok)
            {
                if (ok) { Data.NewEntry(s); }
            },"",null,allow_RegEx);
            modified = true;
        }
        public static void RenEntry(object sender,EventArgs arg){
            renameoriginal = GUI.ChosenEntry;
            QuickInputBox.Create($"Please enter a new name for entry {renameoriginal}",delegate (string s, bool ok){
                if (!ok) return;
                var us = s.ToUpper();
                Data.RenameEntry(renameoriginal, us);
            },renameoriginal,GUI.win, allow_RegEx);
            modified = true;
        }
        public static void RemEntry(object sender, EventArgs arg) => Data.RemoveEntry(GUI.ChosenEntry);

        public static void AddTag(object sender,EventArgs arg){
            QuickInputBox.Create("Please enter a name for the new tag", delegate (string s, bool ok){
                if (!ok) return;
                Data.NewTag(s);
            },"",null,@"[A-Za-z0-9_\.]*");
            modified = true;
        }
        public static void RenTag(object sender,EventArgs arg){
            renameoriginal = GUI.ChosenTag;
            QuickInputBox.Create($"Please enter a new name for tag {renameoriginal}",delegate (string s, bool ok){
                if (!ok) return;
                var e = Data.GetEntry(GUI.ChosenEntry);
                e.RenameTag(renameoriginal, s);
            }, renameoriginal, GUI.win, @"[A-Za-z0-9_\.]*");
            modified = true;
        }
        public static void RemTag(object sender,EventArgs arg){
            if (!QuickGTK.Confirm($"Do you really want to delete {GUI.ChosenTag} from {GUI.ChosenEntry}?")) return;
            Data.GetEntry(GUI.ChosenEntry).RemoveTag(GUI.ChosenTag);
            Data.GetEntry(GUI.ChosenEntry).ReDoTagList();
            GUI.UpdateTagList();
            modified = true;
        }

        public static void MovTag(object sender,EventArgs arg) {
            QuickInputBox.Create("To which entry does this scenario tag have to move?", delegate (string s, bool ok) {
                if (!ok) return;
                Data.GetEntry(GUI.ChosenEntry).MoveTag(GUI.ChosenTag,s);
                GUI.UpdateTagList();
            }, "", null, allow_RegEx);
            modified = true;
        }


        public static void InsertPage(object sender,EventArgs a){
            if (!QuickGTK.Confirm("Do you really want to insert a page here?")) return;
            Data.GetEntry(GUI.ChosenEntry).GetTag(GUI.ChosenTag).InsertTextBox(Data.Page);
            modified = true;
        }

        public static void NextPage(object sender,EventArgs a){
            if (Data.Page < Data.GetEntry(GUI.ChosenEntry).GetTag(GUI.ChosenTag).CountTextBoxes) Data.PickBox(Data.Page + 1);
            else if (QuickGTK.Confirm("No more pages.\nDo you want to add one?")) {
                Data.GetEntry(GUI.ChosenEntry).GetTag(GUI.ChosenTag).InsertTextBox();
                Data.PickBox(Data.GetEntry(GUI.ChosenEntry).GetTag(GUI.ChosenTag).CountTextBoxes);
            }
        }

        public static void KillPage(object sender,EventArgs a){
            if (Data.GetEntry(GUI.ChosenEntry).GetTag(GUI.ChosenTag).CountTextBoxes<1){
                QuickGTK.Error("A tag must have at least 1 page!");
                return;
            }
            if (!QuickGTK.Confirm("Do you really want to kill this scenario page?")) return;
            Data.GetEntry(GUI.ChosenEntry).GetTag(GUI.ChosenTag).KillTextBox(Data.Page);
            Data.PickBox(0);
            modified = true;
        }

        public static void OpenExport(object sender, EventArgs a) => Export.Export.Go();

        public static void Save(object sender, EventArgs a) => Data.Save();
    }
}