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
using SharpMedia.AspectOriented;

namespace SharpMedia.Graphics.Shaders
{
    /// <summary>
    /// It is capable of constucting ConstantLayouts.
    /// </summary>
    /// <remarks>It is not thread safe. Once layout is contructed and CreateLayout is called, it cannot be used anymore.</remarks>
    public class ConstantBufferLayoutBuilder
    {
        #region Private Members

        uint currentOffset = 0;
        SortedList<string, ConstantBufferLayout.ParamOffset> data = new SortedList<string, ConstantBufferLayout.ParamOffset>();

        #endregion

        #region Public Members

        /// <summary>
        /// Adds element at specified offset.
        /// </summary>
        /// <param name="name">The name of parameter.</param>
        /// <param name="format">The format of parameter.</param>
        /// <param name="offset">The offset of element.</param>
        public void AddElement([NotEmpty] string name, PinFormat format, uint arraySize, uint offset)
        {
            // Validate format.
            PinFormat scalar = PinFormatHelper.ToScalar(format);
            if (scalar != PinFormat.Integer && scalar != PinFormat.Float)
            {
                throw new ArgumentException("Only Float and Integer format acceptable.");
            }

            if (data.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("Name {0} is already defined.", name));
            }

            // We add element.
            ConstantBufferLayout.ParamOffset d = new ConstantBufferLayout.ParamOffset();
            d.Description = new ParameterDescription(name, new Pin(format, arraySize, null));
            d.Offset = offset;
            data.Add(name, d);
        }


        /// <summary>
        /// Creates layout based on current members
        /// </summary>
        /// <returns></returns>
        public ConstantBufferLayout CreateLayout()
        {
            currentOffset = uint.MaxValue;
            return new ConstantBufferLayout(data);
        }

        /// <summary>
        /// Appends element to format.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="format"></param>
        public void AppendElement([NotEmpty] string name, PinFormat format)
        {
            currentOffset = PinFormatHelper.Align(format, currentOffset); 
            AddElement(name, format, Pin.NotArray, currentOffset);
            currentOffset += PinFormatHelper.Advance(format, Pin.NotArray);
        }

        /// <summary>
        /// Appends element to format.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="format"></param>
        public void AppendElement([NotEmpty] string name, PinFormat format, uint arraySize)
        {
            currentOffset = PinFormatHelper.Align(format, currentOffset); 
            AddElement(name, format, Pin.NotArray, currentOffset);
            currentOffset += PinFormatHelper.Advance(format, arraySize);
        }

        #endregion


    }
}
