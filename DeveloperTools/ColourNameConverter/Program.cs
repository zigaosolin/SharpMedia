// This file constitutes a part of the SharpMedia project, (c) 2007 by the SharpMedia team
// and is licensed for your use under the conditions of the NDA or other legally binding contract
// that you or a legal entity you represent has signed with the SharpMedia team.
// In an event that you have received or obtained this file without such legally binding contract
// in place, you MUST destroy all files and other content to which this lincese applies and
// contact the SharpMedia team for further instructions at the internet mail address:
//
//    legal@sharpmedia.com
//
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ColourNameConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader reader = new StreamReader("colourmap.in");
            StreamWriter writer = new StreamWriter("genned.cs");

            while (!reader.EndOfStream)
            {
                string str = reader.ReadLine();

                string[] strings = str.Split(' ');
                if(strings.Length < 2) continue;
                string name = string.Empty, bytes;

                bytes = strings[0].Substring(1);

                name = strings[1];
                for (int i = 2; i < strings.Length; i++)
                {
                    name = name + char.ToUpper(strings[i][0]) + strings[i].Substring(1);
                }

                writer.WriteLine("\t\t///<summary>");
                writer.WriteLine("\t\t///Returns " + name + ".");
                writer.WriteLine("\t\t///</summary>");
                writer.WriteLine("\t\tpublic static Colour " + name);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\t\treturn new Colour(0x" + bytes + "FF);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

            }
            writer.Dispose();
        }
    }
}
