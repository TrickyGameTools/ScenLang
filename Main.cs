using System;
using TrickyUnits;
namespace ScenLang
{
    public class SLMain
    {
        public static void Main(string[] args)
        {
            MKL.Version("", "");
            MKL.Lic("", "");
            GUI.init(args);
            GUI.run();
        }
    }
}
