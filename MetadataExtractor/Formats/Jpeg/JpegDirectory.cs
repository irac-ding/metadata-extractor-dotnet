/*
 * Copyright 2002-2015 Drew Noakes
 *
 *    Modified by Yakov Danilov <yakodani@gmail.com> for Imazen LLC (Ported from Java to C#)
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * More information about this project is available at:
 *
 *    https://drewnoakes.com/code/exif/
 *    https://github.com/drewnoakes/metadata-extractor
 */

using System.Collections.Generic;
using JetBrains.Annotations;

namespace Com.Drew.Metadata.Jpeg
{
    /// <summary>Directory of tags and values for the SOF0 JPEG segment.</summary>
    /// <remarks>Directory of tags and values for the SOF0 JPEG segment.  This segment holds basic metadata about the image.</remarks>
    /// <author>Darrell Silver http://www.darrellsilver.com and Drew Noakes https://drewnoakes.com</author>
    public sealed class JpegDirectory : Directory
    {
        public const int TagCompressionType = -3;

        /// <summary>This is in bits/sample, usually 8 (12 and 16 not supported by most software).</summary>
        public const int TagDataPrecision = 0;

        /// <summary>The image's height.</summary>
        /// <remarks>The image's height.  Necessary for decoding the image, so it should always be there.</remarks>
        public const int TagImageHeight = 1;

        /// <summary>The image's width.</summary>
        /// <remarks>The image's width.  Necessary for decoding the image, so it should always be there.</remarks>
        public const int TagImageWidth = 3;

        /// <summary>
        /// Usually 1 = grey scaled, 3 = color YcbCr or YIQ, 4 = color CMYK
        /// Each component TAG_COMPONENT_DATA_[1-4], has the following meaning:
        /// component Id(1byte)(1 = Y, 2 = Cb, 3 = Cr, 4 = I, 5 = Q),
        /// sampling factors (1byte) (bit 0-3 vertical., 4-7 horizontal.),
        /// quantization table number (1 byte).
        /// </summary>
        /// <remarks>
        /// Usually 1 = grey scaled, 3 = color YcbCr or YIQ, 4 = color CMYK
        /// Each component TAG_COMPONENT_DATA_[1-4], has the following meaning:
        /// component Id(1byte)(1 = Y, 2 = Cb, 3 = Cr, 4 = I, 5 = Q),
        /// sampling factors (1byte) (bit 0-3 vertical., 4-7 horizontal.),
        /// quantization table number (1 byte).
        /// <para />
        /// This info is from http://www.funducode.com/freec/Fileformats/format3/format3b.htm
        /// </remarks>
        public const int TagNumberOfComponents = 5;

        /// <summary>the first of a possible 4 color components.</summary>
        /// <remarks>the first of a possible 4 color components.  Number of components specified in TAG_NUMBER_OF_COMPONENTS.</remarks>
        public const int TagComponentData1 = 6;

        /// <summary>the second of a possible 4 color components.</summary>
        /// <remarks>the second of a possible 4 color components.  Number of components specified in TAG_NUMBER_OF_COMPONENTS.</remarks>
        public const int TagComponentData2 = 7;

        /// <summary>the third of a possible 4 color components.</summary>
        /// <remarks>the third of a possible 4 color components.  Number of components specified in TAG_NUMBER_OF_COMPONENTS.</remarks>
        public const int TagComponentData3 = 8;

        /// <summary>the fourth of a possible 4 color components.</summary>
        /// <remarks>the fourth of a possible 4 color components.  Number of components specified in TAG_NUMBER_OF_COMPONENTS.</remarks>
        public const int TagComponentData4 = 9;

        [NotNull] private static readonly Dictionary<int?, string> TagNameMap = new Dictionary<int?, string>();

        static JpegDirectory()
        {
            // NOTE!  Component tag type int values must increment in steps of 1
            TagNameMap[TagCompressionType] = "Compression Type";
            TagNameMap[TagDataPrecision] = "Data Precision";
            TagNameMap[TagImageWidth] = "Image Width";
            TagNameMap[TagImageHeight] = "Image Height";
            TagNameMap[TagNumberOfComponents] = "Number of Components";
            TagNameMap[TagComponentData1] = "Component 1";
            TagNameMap[TagComponentData2] = "Component 2";
            TagNameMap[TagComponentData3] = "Component 3";
            TagNameMap[TagComponentData4] = "Component 4";
        }

        public JpegDirectory()
        {
            SetDescriptor(new JpegDescriptor(this));
        }

        public override string GetName()
        {
            return "JPEG";
        }

        protected override Dictionary<int?, string> GetTagNameMap()
        {
            return TagNameMap;
        }

        /// <param name="componentNumber">
        /// The zero-based index of the component.  This number is normally between 0 and 3.
        /// Use getNumberOfComponents for bounds-checking.
        /// </param>
        /// <returns>the JpegComponent having the specified number.</returns>
        [CanBeNull]
        public JpegComponent GetComponent(int componentNumber)
        {
            var tagType = TagComponentData1 + componentNumber;
            return (JpegComponent)GetObject(tagType);
        }

        /// <exception cref="Com.Drew.Metadata.MetadataException"/>
        public int GetImageWidth()
        {
            return GetInt(TagImageWidth);
        }

        /// <exception cref="Com.Drew.Metadata.MetadataException"/>
        public int GetImageHeight()
        {
            return GetInt(TagImageHeight);
        }

        /// <exception cref="Com.Drew.Metadata.MetadataException"/>
        public int GetNumberOfComponents()
        {
            return GetInt(TagNumberOfComponents);
        }
    }
}