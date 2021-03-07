using System.Drawing;
namespace CandleStick
{
    public static class ChartDefaults
    {
        public static Color DefaultForeGroundColor = Color.White;
        public static Color DefaultBackGroundColor = Color.Black;
        public static Color DefaultUpGoingInsideColor = Color.Green;
        public static Color DefaultUpGoingBorderColor = Color.Red;
        public static Color DefaultDownGoingInsideColor = Color.Red;
        public static Color DefaultDownGoingBorderColor = Color.Green;
        public static Color DefaultRecentHorizontalColor = Color.Gray;
        internal static string[] DefaultSymbolPattern =
        {
            "☻",
            "▲",
            "▼",
            "◄",
            "►",
            "↑",
            "↓",
            "←",
            "→"
        };
        public static void DefaultColorSetting(Color _forground, Color _background, Color _upgoingInside, Color _upgoingBorder, Color _downgoingInside, Color _downgoingBorder)
        {
            DefaultForeGroundColor = _forground;
            DefaultBackGroundColor = _background;
            DefaultUpGoingInsideColor = _upgoingInside;
            DefaultUpGoingBorderColor = _upgoingBorder;
            DefaultDownGoingInsideColor = _downgoingInside;
            DefaultDownGoingBorderColor = _downgoingBorder;
        }
        public static void DefaultSymbolPatternSetting(string[] symbols)
        {
            DefaultSymbolPattern = symbols;
        }
    }
}
