using System;
using Gtk;
using TrickyUnits;

namespace ScenLang
{
    static class GUI
    {
        public static MainWindow win;

        static public Gdk.Color RGB(byte R, byte G, byte B) => new Gdk.Color(R, G, B);

        static void CreateWindow(){
            win = new MainWindow();
            win.Title = $"ScenLang {MKL.Newest}";
            win.ModifyBg(StateType.Normal, RGB(18, 0, 25));
        }

        public static void init(string[] args)
        {
            MKL.Version("","");
            MKL.Lic("", "");
            Application.Init();
            //CreateWindow();
        }


        public static void run()
        {
            win.ShowAll();
            Application.Run();
        }
    }
}
