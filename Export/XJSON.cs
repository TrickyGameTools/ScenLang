using System;
using TrickyUnits;
namespace ScenLang.Export
{
    class XJSON : XCLASS
    {
        public XJSON(){
            MKL.Lic("", "");
            MKL.Version("", "");
        }
        override public string Whole(){
            var ret = "";
            return ret;
        }

        override public string Entry(string tag){
            var ret = "{\n";
            var E = Data.GetEntry(tag);
            var TList = E.TagList;
            bool comma = false;
            foreach(string Tag in TList){
                if (comma) ret += ",\n"; comma = true;
                var T = E.GetTag(Tag);
                var num = T.CountTextBoxes;
                ret += $"\t\"{Tag}\" : [\n";
                for (int i = 0; i <= num; i++){
                    if (i != 0) ret += ",\n";
                    var TB = (DataTextbox)T.GetTextBox(i);
                    ret += "\t\t{\n";
                    ret += $"\t\t\t\"Picture\"         : \"{TB.Picture}\",\n";
                    ret += $"\t\t\t\"PictureSpecific\" : \"{TB.PictureSpecific}\",\n";
                    ret += $"\t\t\t\"AltFont\"         : \"{TB.AltFont}\"";
                    foreach (int taal in TB.Head.Keys ){
                        ret += ",\n";
                        ret += $"\t\t\t\"{Data.Language(taal)}.Head\" : \"{TB.Head[i]}\",\n";
                        ret += $"\t\t\t\"{Data.Language(taal)}.Content\" : `{TB.Content[i]}`";
                    }
                    ret += "\t\t}";
                }
                ret += "\n\t]";
            }
            return ret+"\n}";
        }

        override public string extension() => "json";
    }
}
