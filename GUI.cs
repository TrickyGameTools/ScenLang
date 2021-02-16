// Lic:
// Scenario Language
// Graphic User Interface
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
???using System;
using System.Reflection;
using System.Collections.Generic;
using Gtk;
using TrickyUnits;
using TrickyUnits.GTK;

namespace ScenLang
{
    static class GUI
    {
        static public MainWindow win;
        static bool success;
        static VBox mainbox;
        static HBox submainbox;
        static TextView statusconsole;
        static ListBox entrylist;
        static Image mascot;
        static HBox headbox;
        static HBox editbox;
        static ListBox taglist;
        static public Button bExport;
        public static Entry enPicDir;
        public static Entry enPicSpecific;
        public static Entry enAudio;
        public static Entry enAltFont;
        public static CheckButton tbAllowTrim;
        public static CheckButton tbNameLinking;
        public static Label PaginationLabel;

        public static Entry[] EntryHead;
        public static TextView[] EntryText;

        static public bool NameLinking { get => tbNameLinking.Active; }
        static bool failquit;

        static List<Widget> requiretag = new List<Widget>();
        static List<Widget> requirefile = new List<Widget>();

        static public Gdk.Color RGB(byte R, byte G, byte B) => new Gdk.Color(R, G, B);

        static readonly Dictionary<Widget, int> WidgetIndexes = new Dictionary<Widget, int>();
        static int NumLangs; // used in order not having to scan this all the time.

        static public int GetIndex(Widget w) {
            Assert(WidgetIndexes.ContainsKey(w), "Widget index failure. Unable to grasp the widget's index!");
            return WidgetIndexes[w];
        }


        static public void ADDENTRY(string e) => entrylist.AddItem(e);
        static public Widget WidgetFromBuffer(TextBuffer t){
            Widget ret = null;
            foreach(Widget w in WidgetIndexes.Keys){
                if (w.GetType() == typeof(TextView) && ((TextView)w).Buffer == t) ret = w;
            }
            Assert(ret != null, "Tried to find and index of an unindex widget");
            AutoEnable();
            return ret;
        }

        static public void UPDATEENTRIES(){
            entrylist.Clear();
            foreach (string k in Data.Entries) {
                entrylist.AddItem(k);
                Console.WriteLine($"UE: {k}");
            }
            AutoEnable();
        }

        static void CreateWindow(){
            win = new MainWindow();
            win.Title = $"ScenLang {MKL.Newest}";
            win.ModifyBg(StateType.Normal, RGB(18, 0, 25));
            win.SetSizeRequest(1200, 1000);
        }

        static void CreateMainBox(){
            mainbox = new VBox();
            win.Add(mainbox);
        }

        static public void Write(string txt) {
            statusconsole.Buffer.Text += txt;
        }

        static public void WriteLn(string txt) => Write($"{txt}\n");

        static void CreateSubMainBox(){
            submainbox = new HBox();
            statusconsole = new TextView();
            var statusconsolescroll = new ScrolledWindow();
            WriteLn($"ScenLang v{MKL.Newest}");
            WriteLn("Coded by: Tricky");
            WriteLn($"(c) Jeroen P. Broks, 2015-20{qstr.Left(MKL.Newest,2)} released under the terms of the GPL\n\nProject: {Data.Project}");
            statusconsole.SetSizeRequest(win.WidthRequest, 250);
            statusconsole.Editable = false;
            statusconsole.ModifyText(StateType.Normal, RGB(250, 180, 0));
            statusconsole.ModifyBase(StateType.Normal,RGB(25, 18, 0));
            mainbox.Add(submainbox);
            statusconsolescroll.Add(statusconsole);
            mainbox.Add(statusconsolescroll);
        }

        static void CreateEntryList(){
            entrylist = new ListBox("Entries");
            entrylist.SetSizeRequest(230,submainbox.HeightRequest-289);
            entrylist.Gadget.CursorChanged += Callback.PickEntry;
            var m = new HBox();
            var add = new Button("Add");
            var ren = new Button("Rename");
            var rem = new Button("Remove");
            add.Clicked += Callback.AddEntry;
            ren.Clicked += Callback.RenEntry;
            rem.Clicked += Callback.RemEntry;
            add.ModifyBg(StateType.Normal, RGB(18, 25, 0));
            add.Child.ModifyFg(StateType.Normal, RGB(180, 255, 0));
            ren.ModifyBg(StateType.Normal, RGB(25, 18, 0));
            ren.Child.ModifyFg(StateType.Normal, RGB(255, 180, 0));
            rem.ModifyBg(StateType.Normal, RGB(25, 0, 0));
            rem.Child.ModifyFg(StateType.Normal, RGB(255,0, 0));
            add.ModifyBg(StateType.Prelight, RGB(180, 255, 0));
            ren.ModifyBg(StateType.Prelight, RGB(255, 180, 0));
            rem.ModifyBg(StateType.Prelight, RGB(255, 0, 0));
            ren.ModifyBg(StateType.Insensitive, RGB(0, 0, 0));
            rem.ModifyBg(StateType.Insensitive, RGB(0, 0, 0));
            m.Add(add);
            m.Add(ren);
            m.Add(rem);
            m.SetSizeRequest(230, 25);
            requirefile.Add(ren);
            requirefile.Add(rem);
            bExport = new Button("Export");
            bExport.Clicked += Callback.OpenExport;
            bExport.ModifyBg(StateType.Normal,RGB(0, 0, 25));
            bExport.ModifyBg(StateType.Prelight,RGB(0, 0, 255));
            bExport.Child.ModifyFg(StateType.Normal, RGB(0, 0, 255));
            bExport.Child.ModifyFg(StateType.Prelight, RGB(0, 0, 0));
            Button Save = new Button("Save");
            Save.Clicked += Callback.Save;
            Save.ModifyBg(StateType.Normal, RGB(0, 0, 25));
            Save.ModifyBg(StateType.Prelight, RGB(0, 0, 255));
            Save.Child.ModifyFg(StateType.Normal, RGB(0, 0, 255));
            Save.Child.ModifyFg(StateType.Prelight, RGB(0, 0, 0));
            Assembly asm = Assembly.GetExecutingAssembly();
            System.IO.Stream stream;
            //= asm.GetManifestResourceStream("MyData.Properties.Icon.png");
            //Gdk.Pixbuf PIX = new Gdk.Pixbuf(stream);
            //win.Icon = PIX;
            //stream.Dispose();
            stream = asm.GetManifestResourceStream("ScenLang.Assets.Mascot.png");
            mascot = new Image(stream);
            mascot.SetAlignment(0, 2);
            stream.Dispose();
            var sw = new ScrolledWindow();
            sw.SetSizeRequest(230, submainbox.HeightRequest - 314);
            sw.Add(entrylist.Gadget);
            var lb = new VBox();
            lb.Add(sw);
            lb.Add(m);
            lb.Add(bExport);
            lb.Add(Save);
            lb.Add(mascot);
            submainbox.Add(lb);
        }

        static void CreateEditMain(){
            headbox = new HBox();
            editbox = new HBox();
            var mb = new VBox();
            mb.Add(headbox);
            mb.Add(editbox);
            submainbox.Add(mb);
        }

        static void CreateHeadbox(){
            var sw = new ScrolledWindow();
            var selbox = new VBox();
            sw.SetSizeRequest(150, 370);
            taglist = new ListBox("Tags");
            taglist.Gadget.CursorChanged += Callback.PickTag;
            sw.Add(taglist.Gadget);
            selbox.Add(sw);
            var m = new HBox();
            var add = new Button("Add");
            var ren = new Button("Rename");
            var rem = new Button("Remove");
            var mov = new Button("Move");
            add.Clicked += Callback.AddTag;
            ren.Clicked += Callback.RenTag;
            rem.Clicked += Callback.RemTag;
            mov.Clicked += Callback.MovTag;
            add.ModifyBg(StateType.Normal, RGB(18, 25, 0));
            add.Child.ModifyFg(StateType.Normal, RGB(180, 255, 0));
            ren.ModifyBg(StateType.Normal, RGB(25, 18, 0));
            ren.Child.ModifyFg(StateType.Normal, RGB(255, 180, 0));
            rem.ModifyBg(StateType.Normal, RGB(25, 0, 0));
            rem.Child.ModifyFg(StateType.Normal, RGB(255, 0, 0));
            mov.Child.ModifyFg(StateType.Normal, RGB(0, 0, 255));
            add.ModifyBg(StateType.Prelight, RGB(180, 255, 0));
            ren.ModifyBg(StateType.Prelight, RGB(255, 180, 0));
            rem.ModifyBg(StateType.Prelight, RGB(255, 0, 0));
            add.ModifyBg(StateType.Insensitive, RGB(0, 0, 0));
            ren.ModifyBg(StateType.Insensitive, RGB(0, 0, 0));
            rem.ModifyBg(StateType.Insensitive, RGB(0, 0, 0));
            mov.ModifyBg(StateType.Insensitive, RGB(0, 0, 0));
            m.Add(add);
            m.Add(ren);
            m.Add(rem);
            m.Add(mov);
            m.SetSizeRequest(230, 30);
            requiretag.Add(ren);
            requiretag.Add(rem);
            requiretag.Add(mov);
            requirefile.Add(add);
            selbox.Add(m);
            headbox.Add(selbox);
            var sdata = new VBox();
            // Pagination
            var pagebox = new HBox();
            var newpage = new Button("Insert"); requiretag.Add(newpage);
            var rempage = new Button("Destroy"); requiretag.Add(rempage);
            var prvpage = new Button("<"); requiretag.Add(prvpage);
            var labpage = new Label("--/--"); PaginationLabel = labpage;
            var nxtpage = new Button(">"); requiretag.Add(nxtpage);
            newpage.ModifyBg(StateType.Normal, RGB(18, 25, 0));
            rempage.ModifyBg(StateType.Normal, RGB(25, 0, 0));
            prvpage.ModifyBg(StateType.Normal, RGB(25, 18, 0));
            nxtpage.ModifyBg(StateType.Normal, RGB(25, 18, 0));
            newpage.ModifyBg(StateType.Prelight, RGB(180, 255, 0));
            rempage.ModifyBg(StateType.Prelight, RGB(250, 0, 0));
            prvpage.ModifyBg(StateType.Prelight, RGB(0, 180, 255));
            nxtpage.ModifyBg(StateType.Prelight, RGB(0, 180, 255));
            labpage.ModifyFg(StateType.Normal, RGB(180, 0, 255));
            newpage.Child.ModifyFg(StateType.Normal, RGB(180, 255, 0));
            rempage.Child.ModifyFg(StateType.Normal, RGB(255, 0, 0));
            prvpage.Child.ModifyFg(StateType.Normal, RGB(255, 180, 0));
            nxtpage.Child.ModifyFg(StateType.Normal, RGB(255, 180, 0));
            var pglst = new Button[] { newpage, rempage, prvpage, nxtpage};
            foreach(Button b in pglst){
                b.ModifyBg(StateType.Insensitive, RGB(18, 0, 25));
                b.Child.ModifyFg(StateType.Insensitive, RGB(18, 0, 25));
            }
            newpage.Clicked += Callback.InsertPage;
            prvpage.Clicked += delegate (object s, EventArgs a) { if (Data.Page > 0) Data.PickBox(Data.Page - 1); };
            nxtpage.Clicked += Callback.NextPage;
            rempage.Clicked += Callback.KillPage;
            pagebox.Add(newpage);
            pagebox.Add(rempage);
            pagebox.Add(prvpage);
            pagebox.Add(labpage);
            pagebox.Add(nxtpage);
            sdata.Add(pagebox);

            // Picture dir
            var bxPicDir = new HBox();
            var lbPicDir = new Label("Picture Dir:");
            enPicDir = new Entry();
            enPicDir.Changed += Callback.EditPicDir;
            enPicDir.ModifyBase(StateType.Normal, RGB(0, 25, 0));
            enPicDir.ModifyText(StateType.Normal, RGB(180, 255, 0));
            enPicDir.ModifyBase(StateType.Insensitive, RGB(0, 18, 0));
            enPicDir.ModifyText(StateType.Insensitive, RGB(100, 120, 0));
            lbPicDir.SetAlignment((float).95, (float).5);
            lbPicDir.ModifyFg(StateType.Normal, RGB(180, 0, 255));
            lbPicDir.SetSizeRequest(200, 20);
            enPicDir.SetSizeRequest(200, 20);
            bxPicDir.Add(lbPicDir);
            bxPicDir.Add(enPicDir);
            requiretag.Add(enPicDir);

            // Specific Picture
            var bxPicSpecific = new HBox();
            var lbPicSpecific = new Label("Specific Picture:");
            enPicSpecific = new Entry();
            enPicSpecific.Changed += Callback.EditPicSpecific;
            enPicSpecific.ModifyBase(StateType.Normal, RGB(0, 25, 0));
            enPicSpecific.ModifyText(StateType.Normal, RGB(180, 255, 0));
            enPicSpecific.ModifyBase(StateType.Insensitive, RGB(0, 18, 0));
            enPicSpecific.ModifyText(StateType.Insensitive, RGB(100, 120, 0));
            lbPicSpecific.SetAlignment((float).95, (float).5);
            lbPicSpecific.ModifyFg(StateType.Normal, RGB(180, 0, 255));
            lbPicSpecific.SetSizeRequest(200, 20);
            enPicSpecific.SetSizeRequest(200, 20);
            bxPicSpecific.Add(lbPicSpecific);
            bxPicSpecific.Add(enPicSpecific);
            requiretag.Add(enPicSpecific);

            // Audio
            var bxAudio = new HBox();
            var lbAudio = new Label("Audio:");
            enAudio = new Entry();
            enAudio.Changed += Callback.EditAudio;
            enAudio.ModifyBase(StateType.Normal, RGB(0, 25, 0));
            enAudio.ModifyText(StateType.Normal, RGB(180, 255, 0));
            enAudio.ModifyBase(StateType.Insensitive, RGB(0, 18, 0));
            enAudio.ModifyText(StateType.Insensitive, RGB(100, 120, 0));
            lbAudio.SetAlignment((float).95, (float).5);
            lbAudio.ModifyFg(StateType.Normal, RGB(180, 0, 255));
            lbAudio.SetSizeRequest(200, 20);
            enAudio.SetSizeRequest(200, 20);
            bxAudio.Add(lbAudio);
            bxAudio.Add(enAudio);
            requiretag.Add(enAudio);


            // Allow Trimming
            var bxAllowTrim = new HBox();
            var lbAllowTrim = new Label("Allow Trimming:");
            tbAllowTrim = new CheckButton("Yes");
            tbAllowTrim.Clicked += Callback.EditAllowTrim;
            lbAllowTrim.SetAlignment((float).95, (float).5);
            lbAllowTrim.ModifyFg(StateType.Normal, RGB(180, 0, 255));
            tbAllowTrim.Child.ModifyFg(StateType.Normal, RGB(180, 0, 255));
            tbAllowTrim.Child.ModifyFg(StateType.Selected, RGB(190, 0, 255));
            tbAllowTrim.Child.ModifyFg(StateType.Active, RGB(190, 0, 255));
            tbAllowTrim.Child.ModifyFg(StateType.Insensitive, RGB(100, 0, 155));
            tbAllowTrim.Child.ModifyFg(StateType.Prelight, RGB(255, 0, 255));
            lbAllowTrim.SetSizeRequest(200, 20);
            tbAllowTrim.SetSizeRequest(200, 20);
            bxAllowTrim.Add(lbAllowTrim);
            bxAllowTrim.Add(tbAllowTrim);
            requiretag.Add(tbAllowTrim);

            // Name Linking
            var bxNameLinking = new HBox();
            var lbNameLinking = new Label("Name Linking:");
            tbNameLinking = new CheckButton("Yes");
            tbNameLinking.Clicked += Callback.EditNameLinking;
            lbNameLinking.SetAlignment((float).95, (float).5);
            lbNameLinking.ModifyFg(StateType.Normal, RGB(180, 0, 255));
            tbNameLinking.Child.ModifyFg(StateType.Normal, RGB(180, 0, 255));
            tbNameLinking.Child.ModifyFg(StateType.Selected, RGB(190, 0, 255));
            tbNameLinking.Child.ModifyFg(StateType.Active, RGB(190, 0, 255));
            tbNameLinking.Child.ModifyFg(StateType.Insensitive, RGB(100, 0, 155));
            tbNameLinking.Child.ModifyFg(StateType.Prelight, RGB(255, 0, 255));
            lbNameLinking.SetSizeRequest(200, 20);
            tbNameLinking.SetSizeRequest(200, 20);
            bxNameLinking.Add(lbNameLinking);
            bxNameLinking.Add(tbNameLinking);
            requiretag.Add(tbNameLinking);

            // Alternate Font
            var bxAltFont = new HBox();
            var lbAltFont = new Label("Alternate Font:");
            enAltFont = new Entry();
            enAltFont.Changed += Callback.EditAltFont;
            enAltFont.ModifyBase(StateType.Normal, RGB(0, 25, 0));
            enAltFont.ModifyText(StateType.Normal, RGB(180, 255, 0));
            enAltFont.ModifyBase(StateType.Insensitive, RGB(0, 18, 0));
            enAltFont.ModifyText(StateType.Insensitive, RGB(100, 120, 0));
            lbAltFont.SetAlignment((float).95, (float).5);
            lbAltFont.ModifyFg(StateType.Normal, RGB(180, 0, 255));
            lbAltFont.SetSizeRequest(200, 20);
            enAltFont.SetSizeRequest(200, 20);
            bxAltFont.Add(lbAltFont);
            bxAltFont.Add(enAltFont);
            requiretag.Add(enAltFont);


            // Merge it all together
            sdata.Add(bxPicDir);
            sdata.Add(bxPicSpecific);
            sdata.Add(bxAudio);
            sdata.Add(bxAllowTrim);
            sdata.Add(bxNameLinking);
            sdata.Add(bxAltFont);
            headbox.Add(sdata);
        }

        public static void ListEntries(string[] l){
            entrylist.Clear();
            foreach (string key in l) entrylist.AddItem(key);
        }

        public static void Fail(string message){
            Console.WriteLine("FAILURE! " + message);
            QuickGTK.Error($"FAILURE!\n\n{message}");
            success = false;
            if (failquit) Application.Quit();
            var ex =  new Exception(message);
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}\nTraceback:\n{ex.StackTrace}");
            System.Console.WriteLine($"Error: {ex.Message}\nTraceback:\n{ex.StackTrace}");
            throw ex;
        }

        public static bool Assert(bool condition,string error){
            if (!condition)            
                Fail(error);            
            return condition;
        }

        public static void CreateEditBoxes(){
            NumLangs = Data.NumLanguages;
            Assert(NumLangs > 0, "No languages to work with! Hey! What's that?");
            if (!success) return;
            EntryHead = new Entry[NumLangs];
            EntryText = new TextView[NumLangs];
            for (int i = 0; i < NumLangs;i++){
                // Creation language widgets
                var box = new VBox();
                var kop =  new Label(Data.LanguagePure(i));
                var hlab = new Label("Header:");
                var tlab = new Label("Text:");
                var hbox = new HBox();
                var tbox = new HBox();
                var hwid = new Entry(); EntryHead[i] = hwid;
                var twid = new TextView(); EntryText[i] = twid;
                var tscr = new ScrolledWindow();

                // Configure language widgets
                kop.ModifyFg(StateType.Normal, RGB(180,0, 255));
                hlab.ModifyFg(StateType.Normal, RGB(180,0 ,255));
                tlab.ModifyFg(StateType.Normal, RGB(180, 0, 255));
                hwid.ModifyBase(StateType.Normal, RGB(0,18, 25));
                hwid.ModifyText(StateType.Normal, RGB(0,180, 255));
                hwid.ModifyBase(StateType.Insensitive, RGB(0,18, 25));
                hwid.ModifyText(StateType.Insensitive, RGB(0,20, 30));
                twid.ModifyBase(StateType.Normal, RGB(25, 0, 0));
                twid.ModifyText(StateType.Normal, RGB(255, 0, 0));
                twid.ModifyBase(StateType.Insensitive, RGB(25, 0, 0));
                twid.ModifyText(StateType.Insensitive, RGB(30, 0, 0));
                box.SetSizeRequest(200, 400);
                kop.SetSizeRequest(200, 30);
                hbox.SetSizeRequest(200, 30);
                tbox.SetSizeRequest(200, 340);
                hlab.SetSizeRequest(60, 30);
                tlab.SetSizeRequest(60, 340);
                hwid.SetSizeRequest(140, 28);
                tscr.SetSizeRequest(140, 340);
                twid.WrapMode = WrapMode.Word;

                // Indexer
                WidgetIndexes[hwid] = i;
                WidgetIndexes[twid] = i;


                // Callback language widgets
                hwid.Changed += Callback.EditLHead;
                twid.Buffer.Changed += Callback.EditLText;

                // Merge it all together
                hbox.Add(hlab);
                hbox.Add(hwid);
                tbox.Add(tlab);
                tscr.Add(twid);
                tbox.Add(tscr);
                box.Add(kop);
                box.Add(hbox);
                box.Add(tbox);
                editbox.Add(box);

                requiretag.Add(hwid);
                requiretag.Add(twid);

            }
        }

        public static string ChosenEntry { get =>  entrylist.ItemText; }
        public static bool EntryChosen { get => ChosenEntry != ""; }
        public static string ChosenTag { get {
                if (!EntryChosen) return "";
                return taglist.ItemText;
            }
        }
        public static bool TagChosen { get => ChosenTag != ""; }

        public static void AutoEnable(){
            List<Widget> l;
            bool b;
            for (byte i = 0; i < 2;i++){
                switch(i){
                    case 0: l = requirefile; b = EntryChosen; break;
                    case 1: l = requiretag; b = TagChosen; break;
                    default: Fail("INTERNAL ERROR IN AUTO-ENABLE!"); return; // This should be impossible, but I've added this to avoid the "undefined variable" compiler error as the C# compiler is not smart enough to understand the required variables are impossible to be undefined.
                }
                foreach (Widget w in l) w.Sensitive = b;
            }
        }

        public static void UpdateTagList(){
            taglist.Clear();
            var tl = Data.GetEntry(ChosenEntry).TagList;
            tl.Sort();
            foreach (string k in tl){
                taglist.AddItem(k);
            }
            AutoEnable();
        }


        public static void init(string[] args)
        {
            MKL.Version("Scenario Language - GUI.cs","21.02.16");
            MKL.Lic    ("Scenario Language - GUI.cs","GNU General Public License 3");
            Application.Init();
            Data.LoadFromArgs(args); if (!Data.Loaded) { QuickGTK.Error("Project file not properly loaded!\nExiting!"); return; }
            success = true;
            // Creation
            CreateWindow();
            CreateMainBox();
            CreateSubMainBox();
            CreateEntryList();
            CreateEditMain();
            CreateHeadbox();
            CreateEditBoxes();

            // Base values
            ListEntries(Data.Entries);

            // Disable all shit that shouldn't work yet
            AutoEnable();

            // Timed save
            GLib.Timeout.Add(180000, delegate {
                if (Callback.modified) { 
                    WriteLn($"Autosave activated - {DateTime.Now}");
                Data.Save();
            } else {
                    WriteLn($"No modifications - Autosave skipped - {DateTime.Now}");
                }
                return true;
            });
        }


        public static void run()
        {
            Console.WriteLine($"ScenLang {MKL.Newest}\n(c) Jeroen P. Broks\nReleased under the terms of the GPL version 3\n\n{MKL.All()}");
            if (success)
            {
                Console.WriteLine($"Running project: {Data.Project}");
                failquit = true;
                win.ShowAll();
                Application.Run();
            } else {
                Application.Quit();
            }
        }
    }
}