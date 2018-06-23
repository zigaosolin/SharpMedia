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
using SharpMedia.Testing;
using System.Xml;
using SharpMedia.Math.Shapes.Compounds;
using SharpMedia.Math.Shapes;
using SharpMedia.Math;
using SharpMedia.Math.Matrix;
using System.Globalization;

namespace SharpMedia.Graphics.Vector.Fonts.SVG
{
    /// <summary>
    /// A SVG importer.
    /// </summary>
    internal class SVGImport
    {

        // Internal data that is filled while importing.
        int horizOriginX = 0;
        int horizOriginY = 0;
        int horizAdvX = 0;
        SortedList<string, SVGGlyph> glyphs = new SortedList<string, SVGGlyph>(StringComparer.Ordinal);
        SortedList<string, char> glyphNameToUnicode = new SortedList<string, char>();
        SortedList<ComparablePair<char, char>, float> kernelling = new SortedList<ComparablePair<char, char>, float>();
        Vector2f maxDimensions = Vector2f.Zero;

        public SVGFontProvider ImportSVG(XmlDocument document)
        {
            XmlNode font = document["svg"]["defs"]["font"];
            if (font == null)
            {
                throw new ArgumentException("The document does not include any fonts.");
            }

            // We write basic properties (for normalization).
            string name = font["font-face"]["font-family"] != null ? font["font-face"]["font-family"].InnerText : "unknown";

            // We extract data.
            if (font.Attributes["horiz-origin-x"] != null)
            {
                horizOriginX = int.Parse(font.Attributes["horiz-origin-x"].InnerText);
            }
            if (font.Attributes["horiz-origin-y"] != null)
            {
                horizOriginY = int.Parse(font.Attributes["horiz-origin-y"].InnerText);
            }

            if (horizOriginX != 0 || horizOriginY != 0)
            {
                throw new NotSupportedException();
            }

            if (font.Attributes["horiz-adv-x"] != null)
            {
                horizAdvX = int.Parse(font.Attributes["horiz-adv-x"].InnerText);
            }

            // We now extract glyphs.

            // TODO: extract missing gylph.

            SVGGlyph def = ProcessDefGlyph(font["missing-glyph"]);

            // Extract each glyph.
            foreach (XmlNode node in font)
            {
                if (node.Name == "glyph")
                {
                    ProcessGlyph(node);
                }
            }


            
            // We now rescale each glyph to range in y to [0,1].
            float scale = 1.0f / maxDimensions.Y;
            Matrix3x3f matrix = Matrix3x3f.CreateScale(new Vector3f(scale, scale, scale));

            foreach (KeyValuePair<string, SVGGlyph> pair in glyphs)
            {
                // We scale outline.
                pair.Value.Outline.Transform(matrix);
                pair.Value.Advance *= scale;
            }

            // Transform default.
            def.Advance *= scale;
            def.Outline.Transform(matrix);

            ImportKernellings(font);

            SortedList<ComparablePair<char, char>, float> r = new SortedList<ComparablePair<char, char>, float>();

            // We normalize kernellig (also negatie, since the + meaning is shrink in SVG, in our format,
            // it is enlarge).
            for(int z = 0; z < kernelling.Count; z++)
            {
                r.Add(kernelling.Keys[z], kernelling.Values[z] * -scale);
            }

            return new SVGFontProvider(name, glyphs, r, def);

        }

        void ImportKernellings(XmlNode font)
        {
            // Extract each glyph.
            foreach (XmlNode node in font)
            {
                if (node.Name == "hkern")
                {
                    ImportKern(node);
                }
            }
        }

        void ImportKern(XmlNode node)
        {
            float value = float.Parse(node.Attributes["k"].InnerText);
            string src = node.Attributes["g1"].InnerText;
            string dst = node.Attributes["g2"].InnerText;

            if(src.Contains(",") || dst.Contains(","))
            {
                throw new Exception("Enumerated kernelligns not supported.");
            }

            // For now, we may not support "multi-char" kernellings.
            if (!this.glyphNameToUnicode.ContainsKey(src) |
               !this.glyphNameToUnicode.ContainsKey(dst)) return;

            kernelling.Add(new ComparablePair<char,char>(this.glyphNameToUnicode[src], this.glyphNameToUnicode[dst]), value);
        }

        /// <summary>
        /// Parses and converts position.
        /// </summary>
        Vector2f[] ParsePosition(string data, Vector2f prev, bool rel)
        {
            string[] num = data.Split(' ');

            Vector2f[] res = new Vector2f[num.Length / 2];

            for (int i = 1; i < num.Length; i += 2)
            {
                Vector2f ret = new Vector2f(float.Parse(num[i-1]), float.Parse(num[i]));
                if (rel)
                {
                    ret += prev;
                }
                res[i / 2] = ret;

                maxDimensions.X = ret.X > maxDimensions.X ? ret.X : maxDimensions.X;
                maxDimensions.Y = ret.Y > maxDimensions.Y ? ret.Y : maxDimensions.Y;
            }

            

            return res;
        }

        /// <summary>
        /// Parses and converts to position.
        /// </summary>
        Vector2f[] ParsePosition1D(string data, Vector2f prev, bool rel, bool horizontal)
        {
            string[] num = data.Split(' ');

            Vector2f[] res = new Vector2f[num.Length];

            for (int i = 0; i < num.Length; i ++)
            {
                float x = float.Parse(num[i]);
                Vector2f ret;
                if (rel)
                {

                    ret = prev + new Vector2f(horizontal ? 0 : x, horizontal ? x : 0);
                }
                else
                {
                    ret = new Vector2f(horizontal ? prev.X : x, horizontal ? x : prev.Y);
                }
                res[i] = ret;

                maxDimensions.X = ret.X > maxDimensions.X ? ret.X : maxDimensions.X;
                maxDimensions.Y = ret.Y > maxDimensions.Y ? ret.Y : maxDimensions.Y;
            }

            return res;
        }

        LineSegment2f[] GenerateLineList(Vector2f[] list)
        {
            LineSegment2f[] seg = new LineSegment2f[list.Length - 1];
            for (int i = 1; i < list.Length; i++)
            {
                seg[i - 1] = new LineSegment2f(list[i - 1], list[i]);
            }

            return seg;
        }

        Bezier2f[] GenerateBeziers(Vector2f[] list)
        {
            Bezier2f[] res = new Bezier2f[(list.Length - 1) / 2];

            for (int i = 2; i < list.Length; i += 2)
            {
                res[(i - 2) / 2] = new Bezier2f(list[i - 2], list[i - 1], list[i]);
            }

            return res;
        }

        Bezier2f[] GenerateBeziersRefl(Vector2f[] list)
        {
            Bezier2f[] res = new Bezier2f[list.Length - 2];

            for (int i = 2; i < list.Length; i ++)
            {
                Vector2f reflected = list[i - 2] + 2.0f * (list[i - 1] - list[i - 2]);
                res[i-2] = new Bezier2f(list[i - 1], reflected, list[i]);
            }

            return res;

        }

        IOutline2f[] ProcessElement(ref int i, string desc, ref Vector2f prevPoint, ref Vector2f prevBezier)
        {
            if (i >= desc.Length) return null;

            // We process element.
            int j = i;
            char id = desc[i];

            // We strip the contents until next element.
            while (++i < desc.Length && (char.IsDigit(desc[i]) || char.IsSeparator(desc[i]) || desc[i] == '-')) ;

            // We extract element.
            string element = desc.Substring(j + 1, i - j - 1);

            // Now we process element.
            bool isRelative = char.IsLower(id);
            id = char.ToUpper(id);

            switch (id)
            {
                // A moveto command, can also draw lines.
                case 'M':
                    {
                        // Moveto command, we issue recursive call.
                        Vector2f[] res = ParsePosition(element, prevPoint, isRelative);

                        if (res.Length == 1)
                        {
                            // Just change point.
                            prevPoint = res[0];

                            // We call it recursevelly.
                            return ProcessElement(ref i, desc, ref prevPoint, ref prevBezier);
                        }
                        else
                        {
                            // We create result and prev points.
                            LineSegment2f[] segs = GenerateLineList(res);
                            prevPoint = res[res.Length - 1];
                            return segs;
                        }
                    }
                // A end path command.
                case 'Z':
                    // Ends, joins.
                    return null;

                // Line-to command.
                case 'L':
                    {
                        Vector2f[] res = ParsePosition(element, prevPoint, isRelative);

                        // We must prepend the start position.
                        res = Common.ArrayMerge(new Vector2f[] { prevPoint }, res);

                        LineSegment2f[] seg = GenerateLineList(res);
                        prevPoint = res[res.Length - 1];
                        return seg;
                    }

                // Horizontal line.
                case 'V':
                    {
                        Vector2f[] res = ParsePosition1D(element, prevPoint, isRelative, true);

                        // We must prepend the start position.
                        res = Common.ArrayMerge(new Vector2f[] { prevPoint }, res);

                        LineSegment2f[] seg = GenerateLineList(res);
                        prevPoint = res[res.Length - 1];
                        return seg;

                    }
                // Vertical line.
                case 'H':
                    {
                        Vector2f[] res = ParsePosition1D(element, prevPoint, isRelative, false);

                        // We must prepend the start position.
                        res = Common.ArrayMerge(new Vector2f[] { prevPoint }, res);

                        LineSegment2f[] seg = GenerateLineList(res);
                        prevPoint = res[res.Length - 1];
                        return seg;
                    }
                // Quadratic bezier.
                case 'Q':
                    {
                        Vector2f[] res = ParsePosition(element, prevPoint, isRelative);

                        // We must prepend the start position.
                        res = Common.ArrayMerge(new Vector2f[] { prevPoint }, res);

                        // We create beziers.
                        Bezier2f[] beziers = GenerateBeziers(res);

                        // We set previous points.
                        prevBezier = res[res.Length - 2];
                        prevPoint = res[res.Length - 1];

                        return beziers;
                    }
                // Quadratic bezier (continued).
                case 'T':
                    {
                        Vector2f[] res = ParsePosition(element, prevPoint, isRelative);

                        // We must prepend the start position and previous reflected.
                        res = Common.ArrayMerge(new Vector2f[] { prevBezier, prevPoint }, res);

                        // We create beziers.
                        Bezier2f[] beziers = GenerateBeziersRefl(res);


                        // We set previous points.
                        prevBezier = res[res.Length - 2];
                        prevPoint = res[res.Length - 1];

                        prevPoint = res[res.Length - 1];

                        return beziers;
                    }

                // Cubic bezier.
                case 'C':

                // Cubic bezier (continued).
                case 'S':
                    
               
                default:
                    throw new NotSupportedException();
            }
        }

        OutlineCompound2f ConvertPath(string desc)
        {
            List<IOutline2f> outlines = new List<IOutline2f>();

            if (desc == "")
            {
                // We return empty outline.
                return new OutlineCompound2f();
            }

            // We replace characters that have "no meaning" for us.
            desc = desc.Replace(Environment.NewLine, " ");
            desc = desc.Replace('\n', ' ');

            // Back point references.
            Vector2f prevPoint = Vector2f.Zero, prevPointBez = Vector2f.Zero;

            // We now try to parse it.
            for (int i = 0; i < desc.Length;)
            {
                // We process element.
                IOutline2f[] elements = ProcessElement(ref i, desc, ref prevPoint, ref prevPointBez);

                if (elements != null)
                {
                    outlines.AddRange(elements);
                }
                else
                {
                    // No-op
                }

                
                
            }

            return new OutlineCompound2f(outlines.ToArray());
        }

        SVGGlyph ProcessDefGlyph(XmlNode node)
        {
            string unicode = string.Empty;
            OutlineCompound2f outline = ConvertPath(node.Attributes["d"] != null ? node.Attributes["d"].InnerText : "");


            float adv = horizAdvX;
            if (node.Attributes["horiz-adv-x"] != null)
            {
                adv = int.Parse(node.Attributes["horiz-adv-x"].InnerText);
            }


            return new SVGGlyph(unicode, outline, adv);
        }

        void ProcessGlyph(XmlNode node)
        {
            string unicode = node.Attributes["unicode"].InnerText;
            OutlineCompound2f outline = ConvertPath(node.Attributes["d"] != null ? node.Attributes["d"].InnerText : "");

            float adv = horizAdvX;
            if (node.Attributes["horiz-adv-x"] != null)
            {
                adv = int.Parse(node.Attributes["horiz-adv-x"].InnerText);
            }

            if (unicode.Length == 1)
            {
                glyphNameToUnicode.Add(node.Attributes["glyph-name"].InnerText, unicode[0]);
            }

            glyphs.Add(unicode, new SVGGlyph(unicode, outline, adv));
        }

        

    }

#if SHARPMEDIA_TESTSUITE
    [TestSuite]
    internal class SVGImportTest
    {
        [CorrectnessTest]
        public void Import()
        {
            XmlDocument document = new XmlDocument();
            document.Load("veramono.svg");
                
            SVGImport import = new SVGImport();
            SVGFontProvider font = import.ImportSVG(document);
            
        }
    }
#endif
}
