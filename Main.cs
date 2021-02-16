// Lic:
// Scenario Language
// Main part
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
using TrickyUnits;
using UseJCR6;
namespace ScenLang
{
    public class SLMain
    {
        public static void Main(string[] args)
        {
            MKL.Version("Scenario Language - Main.cs","21.02.16");
            MKL.Lic    ("Scenario Language - Main.cs","GNU General Public License 3");
            TrickyUnits.GTK.QuickGTK.Hello();
            TrickyUnits.GTK.QuickInputBox.Hello();
            JCR6_zlib.Init();
            JCR6_lzma.Init();
            new JCR_QuickLink();
            Callback.IMKL();
            Export.Export.Init();
            GUI.init(args);
            GUI.run();
        }
    }
}