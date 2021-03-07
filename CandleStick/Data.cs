using System.Collections.Generic;
namespace CandleStick
{
    public enum LineType
    {
        none,
        vertical,
        horizontal,
        trend
    }

    public enum SymbolType
    {
        SmileFace,
        UpTriangle,
        DownTriangle,
        LeftTriangle,
        RightTriangle,
        UpArrow,
        DownArrow,
        LeftArrow,
        RightArrow
    }
    public class HorizontalLineData: Line
    {
        public float Value;
        public bool IsDash;
        public HorizontalLineData(float _value, bool _isDash = false)
        {
            Value = _value;
            Type = LineType.horizontal;
            IsDash = _isDash;
        }
    }
    public class VerticalLineData: Line
    {
        public int Index;
        public bool IsDash;
        public VerticalLineData(int _index, bool _isDash = false)
        {
            Index = _index;
            Type = LineType.vertical;
            IsDash = _isDash;
        }
    }
    public class TrendLineData: Line
    {
        public PointForFreeLine FirstPoint;
        public PointForFreeLine EndPoint;
        public bool IsDash;
        public void Move(float deltaIndex, float deltaY)
        {
            FirstPoint.Index += deltaIndex;
            FirstPoint.Value += deltaY;
            EndPoint.Index += deltaIndex;
            EndPoint.Value += deltaY;
        }
        public TrendLineData(PointForFreeLine _firstPoint, PointForFreeLine _endPoint, bool _isDash = false)
        {
            FirstPoint = _firstPoint;
            EndPoint = _endPoint;
            Type = LineType.trend;
            IsDash = _isDash;
        }
    }
    public class PointForFreeLine
    {
        public float Index;
        public float Value;
        public PointForFreeLine(int index, float value)
        {
            Index = index;
            Value = value;
        }
    }
    public class Line
    {
        public LineType Type;
        public bool IsSelected;
    }
}
