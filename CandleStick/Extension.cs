using System;
using System.Drawing;

namespace CandleStick
{
    internal static class ObjectExtension
    {
        public static string OpenValueName = "Open";
        public static string ChartTimeValueName = "Time";
        public static string CloseValueName = "Close";
        public static string HighValueName = "High";
        public static string LowValueName = "Low";

        public static string LinePointTimeValueName = "Time";
        public static string LinePointValueName = "Point";
        public static string LineColorValueName = "Color";
        public static string LineSizeValueName = "Size";

        public static string SymbolName = "Symbol";
        public static string SymbolPointTimeValueName = "Time";
        public static string SymbolPointValueName = "Point";
        public static string SymbolColorValueName = "Color";
        public static string SymbolSizeValueName = "Size";

        public static string TextName = "Text";
        public static string TextPointTimeValueName = "Time";
        public static string TextPointValueName = "Point";
        public static string TextColorValueName = "Color";
        public static string TextSizeValueName = "Size";

        static internal object GetParams(this Object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj);
        }
        internal static DateTime ChartTime(this object obj)
        {
            return (DateTime)obj.GetParams(ChartTimeValueName);
        }
        internal static float Open(this Object obj)
        {
            return (float)(double)obj.GetParams(OpenValueName);
        }
        internal static float Close(this Object obj)
        {
            return (float)(double)obj.GetParams(CloseValueName);
        }
        internal static float Low(this Object obj)
        {
            return (float)(double)obj.GetParams(LowValueName);
        }
        internal static float High(this Object obj)
        {
            return (float)(double)obj.GetParams(HighValueName);
        }

        internal static DateTime LinePointTime(this object obj)
        {
            return (DateTime)obj.GetParams(LinePointTimeValueName);
        }
        internal static float LinePointValue(this Object obj)
        {
            return (float)(double)obj.GetParams(LinePointValueName);
        }
        internal static Color LineColor(this Object obj)
        {
            return (Color)obj.GetParams(LineColorValueName);
        }
        internal static int LineSize(this Object obj)
        {
            return (int)obj.GetParams(LineSizeValueName);
        }

        internal static int Symbol(this Object obj)
        {
            return (int)obj.GetParams(SymbolName);
        }
        internal static DateTime SymbolPointTime(this object obj)
        {
            return (DateTime)obj.GetParams(SymbolPointTimeValueName);
        }
        internal static float SymbolPointValue(this Object obj)
        {
            return (float)(double)obj.GetParams(SymbolPointValueName);
        }
        internal static Color SymbolColor(this Object obj)
        {
            return (Color)obj.GetParams(SymbolColorValueName);
        }
        internal static int SymbolSize(this Object obj)
        {
            return (int)obj.GetParams(SymbolSizeValueName);
        }

        internal static string Text(this object obj)
        {
            return (string)obj.GetParams(TextName);
        }
        internal static DateTime TextPointTime(this object obj)
        {
            return (DateTime)obj.GetParams(TextPointTimeValueName);
        }
        internal static float TextPointValue(this Object obj)
        {
            return (float)(double)obj.GetParams(TextPointValueName);
        }
        internal static Color TextColor(this Object obj)
        {
            return (Color)obj.GetParams(TextColorValueName);
        }
        internal static int TextSize(this Object obj)
        {
            return (int)obj.GetParams(TextSizeValueName);
        }
    }
    public static class PointExtension
    {
        public static Point Add(this Point orgPt, Point addPt)
        {
            return new Point(orgPt.X + addPt.X, orgPt.Y + addPt.Y);
        }
        public static Point Substract(this Point orgPt, Point subPt)
        {
            return new Point(orgPt.X - subPt.X, orgPt.Y - subPt.Y);
        }
    }
}
