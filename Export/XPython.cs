// Lic:
// 	ScenLang
// 	Python Export
// 	
// 	
// 	
// 	(c) Jeroen P. Broks, 2018, All rights reserved
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
namespace ScenLang.Export
{
    class XPython : XCLASS
    {
        XJSON J = new XJSON();
        public override string Entry(string tag) => $"scenario = {J.Entry(tag)}\n\n# You can access this with the import command";
        public override string Whole() => $"scenario = {J.Whole()}\n\n# You can access this with the import command";
        public override string extension() => "py";
    }
}
