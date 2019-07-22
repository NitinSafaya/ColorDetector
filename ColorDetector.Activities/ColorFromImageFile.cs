using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace ColorDetector.Activities
{
    [Description("Provides unique colors from an image")]
    public class ColorFromImageFile : CodeActivity
    {
        public enum ColorType
        {
            PrimaryColor,
            KnownColor,
        }
        [Category("Input")]
        [RequiredArgument]
        [Description("Input the full path of the image file")]
        [DisplayName("Image File")]
        public InArgument<string> ImageFile { get; set; }
        [Category("Input")]
        [RequiredArgument]
        [Description("Select the color scheme as PrimaryColor or KnownColor. PrimaryColor contains 12 basic colors and KnownColor consists of all the colors present in KnowColor Enum under System.Drawing")]
        [DisplayName("Color Scheme")]
        public ColorType ColorScheme { get; set; }
        [Category("Output")]
        public OutArgument<Dictionary<int, String>> ColorMap { get; set; }

        public static Dictionary<int, String> Global_ColorMap = new Dictionary<int, String>();


        protected override void Execute(CodeActivityContext context)
        {
            Dictionary<int, String> Color_Map = new Dictionary<int, String>();
            string filepath = ImageFile.Get(context);
            ColorType Scheme = ColorScheme;
            ColorLibrary co = new ColorLibrary();
            Image in_Image = co.ReadImage(filepath);
            Bitmap img = new Bitmap(in_Image);
            Console.WriteLine("Color scheme is: " + Scheme);
            
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    //DataRow inRow = color_DT.NewRow();
                    Color pixel = img.GetPixel(i, j);
                    string colorVal = String.Empty;
                    if (Global_ColorMap.ContainsKey(pixel.GetHashCode()))
                    {
                        // This just looks up from the Dictionary
                        colorVal = Global_ColorMap[pixel.GetHashCode()];
                    }
                    else
                    {
                        // This is the call where the colour is looked up
                        if (Scheme.Equals(ColorType.PrimaryColor) == true)
                        {
                            colorVal = co.GetPrimaryColor(pixel);
                        }
                        else
                        {
                            colorVal = co.GetKnownColor(pixel);
                        }
                        // Then added to the Dictionary
                        Global_ColorMap.Add(pixel.GetHashCode(), colorVal);
                    }

                    if (Color_Map.ContainsValue(colorVal) == true)
                    {
                        //do nothing
                    }
                    else
                    {
                        Color_Map.Add(pixel.GetHashCode(), colorVal);
                    }

                }
            }
            ColorMap.Set(context, Color_Map);
        }
    }
}