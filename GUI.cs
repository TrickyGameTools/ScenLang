// Lic:
// 	Scenario Language
// 	GUI getup and management
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
using System.Reflection;
using System.Collections.Generic;
using Gtk;
using TrickyUnits;
using TrickyUnits.GTK;

namespace ScenLang
{
    static class GUI
    {
        static MainWindow win;
        static bool success;
        static VBox mainbox;
        static HBox submainbox;
        static TextView statusconsole;
        static ListBox entrylist;
        static Image mascot;
        static HBox headbox;
        static HBox editbox;
        static ListBox taglist;
        static Entry enPicDir;
        static Entry enPicSpecific;
        static Entry enAltFont;
        static CheckButton tbAllowTrim;
        static CheckButton tbNameLinking;

        static public bool NameLinking { get => tbNameLinking.Active; }
        static bool failquit;

        static List<Widget> requiretag = new List<Widget>();
        static List<Widget> requirefile = new List<Widget>();

        static public Gdk.Color RGB(byte R, byte G, byte B) => new Gdk.Color(R, G, B);

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
            WriteLn($"ScenLang v{MKL.Newest}");
            WriteLn("Coded by: Tricky");
            WriteLn($"(c) Jeroen P. Broks, 2015-20{qstr.Left(MKL.Newest,2)} released under the terms of the GPL");
            statusconsole.SetSizeRequest(win.WidthRequest, 250);
            statusconsole.Editable = false;
            statusconsole.ModifyText(StateType.Normal, RGB(250, 180, 0));
            statusconsole.ModifyBase(StateType.Normal,RGB(25, 18, 0));
            mainbox.Add(submainbox);
            mainbox.Add(statusconsole);
        }

        static void CreateEntryList(){
            entrylist = new ListBox("Entries");
            entrylist.SetSizeRequest(230,submainbox.HeightRequest-314);
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
            sw.SetSizeRequest(150, 400);
            taglist = new ListBox("Tags");
            sw.Add(taglist.Gadget);
            headbox.Add(sw);
            var sdata = new VBox();
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
            sdata.Add(bxAllowTrim);
            sdata.Add(bxNameLinking);
            sdata.Add(bxAltFont);
            headbox.Add(sdata);
        }

        public static void Fail(string message){
            QuickGTK.Error($"FAILURE!\n\n{message}");
            success = false;
            if (failquit) Application.Quit();
        }

        public static void Assert(bool condition,string error){
            if (!condition) Fail(error);
        }

        public static void CreateEditBoxes(){

        }

        public static void init(string[] args)
        {
            MKL.Version("Scenario Language - GUI.cs","18.10.15");
            MKL.Lic    ("Scenario Language - GUI.cs","GNU General Public License 3");
            Application.Init();
            Data.LoadFromArgs(args); if (!Data.Loaded) { QuickGTK.Error("Project file not properly loaded!\nExiting!"); return; }
            success = true;
            CreateWindow();
            CreateMainBox();
            CreateSubMainBox();
            CreateEntryList();
            CreateEditMain();
            CreateHeadbox();
            CreateEditBoxes();
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
