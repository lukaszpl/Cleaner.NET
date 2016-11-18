/* This file is part of Cleaner .NET

    Cleaner .NET is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 3 of the License.

    Cleaner .NET is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA */
using System;

namespace Cleaner.NET
{
    public class Informations
    {
        public static string ProgramName
        {
            get
            {
                string bit = null;
                if (Environment.Is64BitProcess)
                    bit = " 64 bit";
                else
                    bit = " 32 bit";
                return "Cleaner .NET v" + Version + bit;
            }
        }
        public static string TextBuild
        {
            get { return "Build " + Build._Build; }
        }
        public static string Version
        {
            get { return "1.0"; }
        }
        public static string Username
        {
            get { return  Languages.Lang.User + ": " + SystemInformation.GetUsername(); }
        }
        public static string SystemName
        {
            get { return SystemInformation.GetWindowsVersion() + "build: " + SystemInformation.GetWindowsCompilation(); }
        }
        public static string LicenseAuthor
        {
            get { return Languages.Lang.License + ": GNU GPL\n" + Languages.Lang.Author + ": Łukasz Soroczyński\n"; }
        }
    }
}
