using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;

namespace ColorDetector.Activities
{
    [Description("Provides color of any pixel on the screen")]
    public class ColorFromScreen : CodeActivity
    {
        public enum ColorType
        {
            PrimaryColor,
            KnownColor,
        }

        [Category("Input")]
        [RequiredArgument]
        [Description("Select the color scheme as PrimaryColor or KnownColor. PrimaryColor contains 12 basic colors and KnownColor consists of all the colors present in KnowColor Enum under System.Drawing")]
        [DisplayName("Color Scheme")]
        public ColorType ColorScheme { get; set; }
        [Category("Input")]
        [RequiredArgument]
        [Description("Enter the X coordinate.")]
        public InArgument<int> X { get; set; }
        [Category("Input")]
        [RequiredArgument]
        [Description("Enter the Y coordinate.")]
        public InArgument<int> Y { get; set; }
        [Category("Input")]
        [RequiredArgument]
        [Description("Enter the highlight flag. If true, a yellow circular marker will appear on the XY coordinate of the screen.")]
        [DisplayName("Highlight Flag")]
        public InArgument<bool> HighlightFlag { get; set; }
        [Category("Output")]
        public OutArgument<string> Color_Value { get; set; }
       
        protected override void Execute(CodeActivityContext context)
        {
            int X_val = Convert.ToInt32(X.Get(context));
            int Y_val = Convert.ToInt32(Y.Get(context));
            ColorType Scheme = ColorScheme;
            ColorLibrary co = new ColorLibrary();
            bool highFlag = Convert.ToBoolean(HighlightFlag.Get(context));
            string colorVal = String.Empty;
            Color c = co.GetColorAt(X_val, Y_val);
            if (Scheme.Equals(ColorType.PrimaryColor) == true)
            {
                colorVal = co.GetPrimaryColor(c);
            }
            else
            {
                colorVal = co.GetKnownColor(c);
            }
            if(highFlag == true)
            {
                co.Highlight(X_val, Y_val);
            }
            Color_Value.Set(context, colorVal);
        }
    }
}
