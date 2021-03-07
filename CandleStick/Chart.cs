using System;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
namespace CandleStick
{
    public class Chart<BarClass, LinePointClass, SymbolPointClass, TextPointClass>
    {
        #region Interface Property
        public bool IsXSpacing
        {
            get { return _isXSpacing; }
            set
            {
                if (_isSetBar)
                {
                    _isXSpacing = value;
                    xCurrentPadding = (value) ? xPadding : 0;
                    _startIndex = 0;
                    Redraw();
                }
            }
        }
        public bool IsCrossView
        {
            get {return _isCrossView; }
            set
            {
                if (_isSetBar)
                {
                    _isCrossView = value;
                    _panXGuide.Visible = _isCrossView;
                    _panYGuide.Visible = _isCrossView;
                    Redraw();
                }
            }
        }
        public LineType DrawingType
        {
            get { return _drawingType; }
            set
            {
                _drawingType = value;
                _isSetLastPoint = false;
            }
        }
        public bool IsDash { get; set; }
        public bool IsAutoScroll
        {
            get { return _isAutoScroll; }
            set
            {
                _isAutoScroll = value;
                IsXSpacing = IsXSpacing;
            }
        }
        public int XSPace
        {
            get { return _XPaddingPercent; }
            set
            {
                if (value > 0 && value < 100)
                {
                    _XPaddingPercent = value;
                    xPadding = (_panDrwing.Width - 80) * _XPaddingPercent / 100;
                    xCurrentPadding = (_isXSpacing) ? xPadding : 0;
                }
            }
        }
        public void SetLineType(int type)
        {
            if (!IsCrossView)
            {
                switch (type)
                {
                    case 1:
                        DrawingType = LineType.vertical;
                        break;
                    case 2:
                        DrawingType = LineType.horizontal;
                        break;
                    case 3:
                        DrawingType = LineType.trend;
                        break;
                }
            }
        }
        public Color ForeGroundColor;
        public Color BackGroundColor;
        public Color UpGoingInsideColor;
        public Color UpGoingBorderColor;
        public Color DownGoingInsideColor;
        public Color DownGoingBorderColor;
        public Color RecentHorizontalColor;
        public string[] SymbolPattern;
        #endregion

        #region Internal Parameter
        private List<BarClass> chartData;
        private List<List<LinePointClass>> linesData;
        private List<HorizontalLineData> horizontalLines = new List<HorizontalLineData>();
        private List<HorizontalLineData> update_horizontalLines = new List<HorizontalLineData>();
        private List<VerticalLineData> verticalLines = new List<VerticalLineData>();
        private List<TrendLineData> trendLines = new List<TrendLineData>();
        private List<TextPointClass> textData;
        private List<SymbolPointClass> symbolData;

        private int YAxisWidth = 80;
        private int yCount = 15;
        private float yScale = 1.05f;
        private int yPadding = 5;
        private int XAxisHeight = 40;
        private int xCount = 8;
        private float xBarSpace = 15;
        private float xPadding = 50;
        private float xCurrentPadding = 50;
        private string timeFormart = "h:mm:ss t";
        private int saveOldSecond = -1;
        private Point selectedValuePoint, selectedTimePoint;
        private String selectedValueText, selectedTimeText;
        private string crossDetailText = "";
        private Point crossDetailPoint = new Point();
        #endregion

        #region Internal State Parameter
        // Parmeter For Interface Property
        private bool _isXSpacing;
        private bool _isCrossView;
        private LineType _drawingType;
        private int _XPaddingPercent;

        // Temperary State Parameter
        private bool _isAutoScroll;
        private bool _isSetBar;
        private bool _isSetBarParams;
        private bool _isSetBarData;
        private bool _isSetLine;
        private bool _isSetLineData;
        private bool _isSetLinePointParams;
        private bool _isSetTexts;
        private bool _isSetTextData;
        private bool _isSetTextPointParams;
        private bool _isSetSymbols;
        private bool _isSetSymbolData;
        private bool _isSetSymbolPointParams;
        private bool _isRedrawing;
        private bool _isSetInfoLabel;
        private bool _isDrawedHorizontalLine;
        private bool _isDrawedVerticalLine;
        private bool _isDrawedTrendLine;
        private bool _isMiddleButtonPressed_flg = false;
        private bool _crossDetailFlg = false;
        private bool _isGraphicMove = false;

        private bool _isSetLastPoint;
        private bool _isMouseDowned;
        private bool _isNotFill;
        private bool _isLineSelected;
        private bool _isMiddleButtonPressed; // +
        private bool _isChartMoving = false;
        #endregion

        #region Temperary Parameter
        private Panel _panParent;
        private PanelNF _panDrwing;
        private PanelNF _panValue;
        private PanelNF _panTime;
        private Panel _panXGuide;
        private Panel _panYGuide;
        private Panel _panTrendGuide;
        private Label _lblChatTitle;
        private Label _lblVersion;
        private Timer _internalTimer;
        private Label _lblUpdateValueText;
        private List<Label> _timeLabelList = new List<Label>();
        private List<Label> _valueLabelList = new List<Label>();
        private Label _lblUpdateTimeText;
        private Setting _frmSetting;
        private Label _cursorLabel;

        private Label _lblCurTime;
        private Label _lblCurHigh;
        private Label _lblCurLow;
        private Label _lblCurOpen;
        private Label _lblCurClose;
        private float _YValueMax = 0;
        private float _YvalueMin = 0;
        private float _mouseOldX = 0;
        private float _mouseOldY = 0;
        private float _selectThreshold = 10f;
        private int _startIndex;
        private float _XStartValue;
        private int count;
        private int autoHorizontalNumber = 0;

        private Point standardPoint;
        private HorizontalLineData autoHData = new HorizontalLineData(0,false);
        float _YstepLength;
        float _YStartValue;
        PointForFreeLine _lastSetPoint;    // Here X is x-axis index, not cooridination
        Point _curMousePoint;
        Point _oldMousePoint;
        Line _selectedLine;
        #endregion
        // Cursor control
        
        #region Public Methord
        public Chart(Panel _parent)
        {
            ForeGroundColor = ChartDefaults.DefaultForeGroundColor;
            BackGroundColor = ChartDefaults.DefaultBackGroundColor;
            UpGoingInsideColor = ChartDefaults.DefaultUpGoingInsideColor;
            UpGoingBorderColor = ChartDefaults.DefaultUpGoingBorderColor;
            DownGoingInsideColor = ChartDefaults.DefaultDownGoingInsideColor;
            DownGoingBorderColor = ChartDefaults.DefaultDownGoingBorderColor;
            RecentHorizontalColor = ChartDefaults.DefaultRecentHorizontalColor;
            SymbolPattern = ChartDefaults.DefaultSymbolPattern;

            _frmSetting = new Setting();
            _frmSetting.OnSubmit += settingChange;

            _panParent = _parent;
            _panParent.BackColor = BackGroundColor;

            _panDrwing = new PanelNF();
            _panDrwing.Dock = DockStyle.Fill;
            
            _panXGuide = new Panel();
            _panXGuide.Size = new Size(_panDrwing.Width - 80, 1);
            _panXGuide.BackColor = ForeGroundColor;
            _panXGuide.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _panXGuide.Visible = false;
            _panXGuide.MouseClick += new MouseEventHandler(showCrossLine);
            _panXGuide.MouseDown += new MouseEventHandler(onMouseDownEvent);
            _panXGuide.MouseUp += new MouseEventHandler(onMouseUpEvent);
            _panXGuide.MouseMove += new MouseEventHandler(crossMove);
            _panDrwing.Controls.Add(_panXGuide);

            _panYGuide = new Panel();
            _panYGuide.Size = new Size(1, _panDrwing.Height - 40);
            _panYGuide.BackColor = ForeGroundColor;
            _panYGuide.Anchor = AnchorStyles.Bottom | AnchorStyles.Top;
            _panYGuide.MouseClick += new MouseEventHandler(showCrossLine);
            _panYGuide.MouseDown += new MouseEventHandler(onMouseDownEvent);
            _panYGuide.MouseUp += new MouseEventHandler(onMouseUpEvent);
            _panYGuide.MouseMove += new MouseEventHandler(crossMove);
            _panYGuide.Visible = false;
            _panDrwing.Controls.Add(_panYGuide);

            _lblChatTitle = new Label();
            _lblChatTitle.Location = new Point(5, 3);
            _lblChatTitle.AutoSize = true;
            _lblChatTitle.Visible = false;
            _lblChatTitle.ForeColor = ForeGroundColor;
            _lblChatTitle.BackColor = Color.Transparent;
            _panDrwing.Controls.Add(_lblChatTitle);
            _panValue = new PanelNF();

            _lblVersion = new Label();
            _lblVersion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblVersion.Location = new Point(_panDrwing.Width - 220, 3);
            _lblVersion.Padding = new Padding(5);
            _lblVersion.AutoSize = true;
            _lblVersion.ForeColor = ForeGroundColor;
            _lblVersion.BackColor = Color.Transparent;
            _lblVersion.Text = "Version:" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            _panDrwing.Controls.Add(_lblVersion);
            _panValue = new PanelNF();

            _panValue.Dock = DockStyle.Right;
            _panValue.Width = YAxisWidth;
            _panParent.Controls.Add(_panValue);

            _panTime = new PanelNF();

            _lblUpdateValueText = new Label();
            _lblUpdateValueText.BackColor = ForeGroundColor;
            _lblUpdateValueText.ForeColor = BackGroundColor;
            _lblUpdateValueText.AutoSize = true;
            _panValue.Controls.Add(_lblUpdateValueText);

            _panTime.Dock = DockStyle.Bottom;
            _panTime.Height = XAxisHeight;
            _panParent.Controls.Add(_panTime);

            _internalTimer = new Timer();
            _internalTimer.Enabled = true;
            _internalTimer.Interval = 500;

            _panParent.Controls.Add(_panDrwing);

            _panDrwing.Resize += new EventHandler(resize);
            _panDrwing.MouseClick += new MouseEventHandler(showCrossLine);
            _panDrwing.MouseClick += new MouseEventHandler(openMenu);
            _panDrwing.Paint += new PaintEventHandler(paintChart);
            _panDrwing.MouseMove += new MouseEventHandler(crossMove);

            _panDrwing.MouseUp += new MouseEventHandler(setLine); 
            _panDrwing.MouseDown += new MouseEventHandler(mouseDown);

            _panValue.Paint += new PaintEventHandler(paintValue);
            _panValue.MouseMove += new MouseEventHandler(valueScaleChange);
            _panValue.DoubleClick += new EventHandler(valueScaleReset);
            _panValue.Resize += new EventHandler(resize);

            _panTime.Paint += new PaintEventHandler(paintTime);
            _panTime.Resize += new EventHandler(resize);
            _panTime.MouseMove += new MouseEventHandler(timeScaleChange);
            _panParent.MouseWheel += new MouseEventHandler(chartMove);
            _internalTimer.Tick += new EventHandler(tickHandler);

            _cursorLabel = new Label(); // detail
            _cursorLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _cursorLabel.Location = new Point(-10, -10);
            _cursorLabel.Padding = new Padding(2);
            _cursorLabel.AutoSize = true;
            _cursorLabel.ForeColor = ForeGroundColor;
            _cursorLabel.BackColor = Color.Transparent;
            _panDrwing.Controls.Add(_cursorLabel);
            _isAutoScroll = true;
            XSPace = 10;
        }
        public void AddLine(List<LinePointClass> lineData)
        {
            if(linesData == null)
            {
                linesData = new List<List<LinePointClass>>();
            }
            linesData.Add(lineData);
            _isSetLineData = true;
            if(_isSetLinePointParams) _isSetLine = true;
            Redraw(true);
        }
        public void SetText(List<TextPointClass> textList)
        {
            textData = textList;
            _isSetTexts = true;
            Redraw(true);
        }
        public void SetSymbol(List<SymbolPointClass> symbolList)
        {
            symbolData = symbolList;
            _isSetSymbols = true;
            Redraw(true);
        }
        public void SetLinePointParams(string timeValueName, string pointValueName, string colorValueName, string widthValueName)
        {
            ObjectExtension.LinePointTimeValueName = timeValueName;
            ObjectExtension.LinePointValueName = pointValueName;
            ObjectExtension.LineColorValueName = colorValueName;
            ObjectExtension.LineSizeValueName = widthValueName;
            _isSetLinePointParams = true;
            if (_isSetLineData) _isSetLine = true;
        }
        public void SetSymbolPointParams(string symbolName, string timeValueName, string pointValueName, string colorValueName, string widthValueName)
        {
            ObjectExtension.SymbolName = symbolName;
            ObjectExtension.SymbolPointTimeValueName = timeValueName;
            ObjectExtension.SymbolPointValueName = pointValueName;
            ObjectExtension.SymbolColorValueName = colorValueName;
            ObjectExtension.SymbolSizeValueName = widthValueName;
            _isSetLinePointParams = true;
            if (_isSetSymbolData) _isSetSymbols = true;
        }
        public void SetTextPointParams(string textName, string timeValueName, string pointValueName, string colorValueName, string widthValueName)
        {
            ObjectExtension.TextName = textName;
            ObjectExtension.TextPointTimeValueName = timeValueName;
            ObjectExtension.TextPointValueName = pointValueName;
            ObjectExtension.TextColorValueName = colorValueName;
            ObjectExtension.TextSizeValueName = widthValueName;
            _isSetLinePointParams = true;
            if (_isSetTextData) _isSetTexts = true;
        }
        public void SetData(List<BarClass> _data)
        {
            chartData = _data;
            _isSetBarData = true;
            if (_isSetBarParams) _isSetBar = true;

            Redraw(true);
        }
        public void SetBarParams(string timeValueName, string openValueName, string closeValueName, string highValueName, string lowValueName)
        {
            ObjectExtension.ChartTimeValueName = timeValueName;
            ObjectExtension.OpenValueName = openValueName;
            ObjectExtension.CloseValueName = closeValueName;
            ObjectExtension.HighValueName = highValueName;
            ObjectExtension.LowValueName = lowValueName;
            _isSetBarParams = true;
            if (_isSetBarData) _isSetBar = true;
        }
        public void SetInfoLabel(Label _timeLabel, Label _highLabel, Label _lowLabel, Label _openLabel, Label _closeLabel)
        {
            _lblCurTime = _timeLabel;
            _lblCurHigh = _highLabel;
            _lblCurLow = _lowLabel;
            _lblCurOpen = _openLabel;
            _lblCurClose = _closeLabel;
            _isSetInfoLabel = true;
        }
        public void SetTitle(string _title)
        {
            if (_title != "")
            {
                _lblChatTitle.Text = _title;
                _lblChatTitle.Visible = true;
            }
        }
        public void SetAxiSFormart(string _timeFormat)
        {
            timeFormart = _timeFormat;
            Redraw();
        }
        public void Redraw(bool isCalculateStartIndex = false)
        {
            if (!_isSetBar) return;
            if (isCalculateStartIndex)
            {
                if (_isAutoScroll)
                {
                    //_startIndex = 0;
                    xCurrentPadding = (_isXSpacing) ? xPadding : 0;
                }
                else if (chartData.Count != count && xCurrentPadding < xBarSpace)
                {
                    _startIndex += chartData.Count - count;
                }
            }
            if (_isXSpacing && count < chartData.Count)
            {
                if (!_isAutoScroll && xCurrentPadding >= xBarSpace)
                    xCurrentPadding = Math.Max(xCurrentPadding - xBarSpace, 0);
            }
            
            count = chartData.Count;
            float width = (IsXSpacing) ? _panDrwing.Width - 80 - xCurrentPadding : _panDrwing.Width - 80;
            int endIndexForDisplay = _startIndex + (int)(width / xBarSpace);
            if (endIndexForDisplay >= count) endIndexForDisplay = count - 1;
            float _min = chartData[count - _startIndex - 1].Low();
            float _max = 0;
            //foreach (Object d in chartData)
            for (int i = _startIndex; i <= endIndexForDisplay; i++)
            {
                Object d = chartData[count - i - 1];
                if (_min > d.Low()) _min = d.Low();
                if (_max < d.High()) _max = d.High();
            }
            _YValueMax = _max;
            _YvalueMin = _min;
            _XStartValue = (IsXSpacing) ? _panDrwing.Width - 80 - xCurrentPadding : _panDrwing.Width - 80;
            _isNotFill = false;
            if (xBarSpace * count < _XStartValue)
            {
                _XStartValue = xBarSpace * count;
                _isNotFill = true;
            }
            _isRedrawing = true;
            _panValue.Invalidate(true);
            _panParent.Invalidate(true);
        }
        public void Zoomin()
        {
            xBarSpace += 3;
            if (xBarSpace > 20) xBarSpace = 20;
            Redraw();
        }
        public void ZoomOut()
        {
            xBarSpace -= 3;
            if (xBarSpace < 1) xBarSpace = 1;
            Redraw();
        }
        public void SelectedLineDelete()
        {
            if (_isDrawedHorizontalLine)
            {
                for(int i = horizontalLines.Count - 1; i >=0; i--)
                    if (horizontalLines[i].IsSelected) horizontalLines.RemoveAt(i);
                if (horizontalLines.Count == 0) _isDrawedHorizontalLine = false;
            }
            if (_isDrawedVerticalLine)
            {
                for (int i = verticalLines.Count - 1; i >= 0; i--)
                    if (verticalLines[i].IsSelected) verticalLines.RemoveAt(i);
                if (verticalLines.Count == 0) _isDrawedVerticalLine = false;
            }
            if (_isDrawedTrendLine)
            {
                for (int i = trendLines.Count - 1; i >= 0; i--)
                    if (trendLines[i].IsSelected) trendLines.RemoveAt(i);
                if (trendLines.Count == 0) _isDrawedTrendLine = false;
            }
            Redraw();
        }
        public void SetYAxisWidth(int _width)
        {
            _panValue.Width = _width;
        }
        public void SetXAxisHeight(int _height)
        {
            _panTime.Height = _height;
        }
        #endregion
        
        #region EventHandler
        private void resize(object sender, EventArgs e)
        {
            _isRedrawing = true;
            ((Control)sender).Refresh();
        }
        private void paintChart(object sender, PaintEventArgs e)
        {
            if (!_isSetBar) return;
            if (_isRedrawing)
            {
                // broder paint
                Panel p = (Panel)sender;
                var g = e.Graphics;
                g.Clear(BackGroundColor);
                Pen pen = new Pen(ForeGroundColor);
                Brush brush;
                g.DrawRectangle(pen, new Rectangle(1, 1, p.Width - 80 - 2, p.Height - 40 - 2));

                // chart paint
                int xBarRadius = (int)(xBarSpace / 2 - 1);
                float distance = (_YValueMax - _YvalueMin) * yScale;
                float _mean = (_YValueMax + _YvalueMin) / 2f;
                _YStartValue = _mean + (_YValueMax - _mean) * yScale;
                int height = p.Height- 40 - 2 * yPadding;
                _YstepLength = height / (float)distance;

                Font font1 = new Font("Arial", 10);
                Brush brush1 = new SolidBrush(ForeGroundColor);
                Brush crossBrush1 = new SolidBrush(BackGroundColor);
                
                // Updating Status
                if (update_horizontalLines.Count > 0)
                {
                    pen = new Pen(RecentHorizontalColor);
                    float yy = 0;
                    if (update_horizontalLines[0].IsDash) pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    yy = update_horizontalLines[0].Value;

                    g.DrawLine(pen, 0, yy, p.Width - 80, yy);
                    if (update_horizontalLines[0].IsSelected)
                    {
                        g.DrawRectangle(pen, 0, yy - 2, 4, 4);
                        g.DrawRectangle(pen, p.Width - 80 - 4, yy - 1, 4, 4);
                    }
                    update_horizontalLines = new List<HorizontalLineData>();
                    float _uYValue = _YStartValue - (yy - yPadding) / _YstepLength;
                    _lblUpdateValueText.Location = new Point(15, (int)yy - _lblUpdateValueText.Height / 2);
                    _lblUpdateValueText.BackColor = RecentHorizontalColor;
                    _lblUpdateValueText.Text = _uYValue.ToString("n5");
                }
                for (int i = 1; i <= chartData.Count; i++)
                {
                    // calculate real location from value
                    float x = _XStartValue - (xBarSpace * i);
                    if (x < 0) break;
                    int index = count - i - _startIndex;
                    if (index < 0) break;
                    Object d = chartData[index];
                    float OpenY = yPadding + _YstepLength * (_YStartValue - d.Open());
                    float CloseY = yPadding + _YstepLength * (_YStartValue - d.Close());
                    float HighY = yPadding + _YstepLength * (_YStartValue - d.High());
                    float LowY = yPadding + _YstepLength * (_YStartValue - d.Low());
                    if (d.Open() > d.Close())
                    {
                        pen.Color = DownGoingInsideColor;
                        g.DrawLine(pen, x, HighY, x, LowY);
                        brush = new SolidBrush(DownGoingInsideColor);
                        g.FillRectangle(brush, new RectangleF(x - xBarRadius, OpenY, 2 * xBarRadius, CloseY - OpenY));
                        pen.Color = DownGoingBorderColor;
                        g.DrawRectangle(pen, new Rectangle((int)x - xBarRadius, (int)OpenY, 2 * xBarRadius, (int)(CloseY - OpenY)));
                    }
                    else
                    {
                        pen.Color = UpGoingInsideColor;
                        g.DrawLine(pen, x, HighY, x, LowY);
                        if (d.Open() == d.Close())
                        {
                            g.DrawLine(pen, x - xBarRadius, OpenY, x + xBarRadius, OpenY);
                        }
                        else
                        {
                            brush = new SolidBrush(UpGoingInsideColor);
                            g.FillRectangle(brush, new RectangleF(x - xBarRadius, CloseY, 2 * xBarRadius, OpenY - CloseY));
                            pen.Color = UpGoingBorderColor;
                            g.DrawRectangle(pen, new Rectangle((int)x - xBarRadius, (int)CloseY, 2 * xBarRadius, (int)(OpenY - CloseY)));
                        }
                    }

                    Object update_obj = chartData[count-1];
                    float UpdateOpenY = yPadding + _YstepLength * (_YStartValue - update_obj.Open());
                    float UpdateCloseY = yPadding + _YstepLength * (_YStartValue - update_obj.Close());
                    update_horizontalLines.Add(new HorizontalLineData(UpdateCloseY, IsDash));
                }
                
                _isRedrawing = false;
                // H, V, T Line Data paint
                if (_isSetLine)
                {
                    foreach(List<LinePointClass> line in linesData)
                    {
                        int pointCount = line.Count;
                        int lastIndex = -1;

                        for (int i = count - _startIndex ; i >= 0; i--)
                        {
                            if (i >= pointCount) continue;
                            if (line[i].LinePointTime() == null) continue;
                            if (lastIndex > 0)
                            {
                                pen = new Pen(line[lastIndex].LineColor());
                                pen.Width = line[lastIndex].LineSize();
                                float x1 = _XStartValue - (xBarSpace * (count - lastIndex - _startIndex));
                                float y1 = yPadding + _YstepLength * (_YStartValue - line[lastIndex].LinePointValue());
                                float x2 = _XStartValue - (xBarSpace * (count - i - _startIndex));
                                float y2 = yPadding + _YstepLength * (_YStartValue - line[i].LinePointValue());
                                g.DrawLine(pen, x1, y1, x2, y2);
                                if (x2 < 0) break;
                                lastIndex = i;
                            }
                            else
                            {
                                lastIndex = i;
                            }
                        }
                    }
                }
                // H, V, T Line draw
                if (_isMouseDowned)
                {
                    pen.Color = ForeGroundColor;
                    switch (_drawingType)
                    {
                        case LineType.horizontal:
                            g.DrawLine(pen, 0, _curMousePoint.Y, _panDrwing.Width- 80, _curMousePoint.Y);
                            break;
                        case LineType.vertical:
                            g.DrawLine(pen, _curMousePoint.X, 0, _curMousePoint.X, _panDrwing.Height - 40);
                            break;
                        case LineType.trend:
                            if (_isSetLastPoint)
                            {
                                float x = _XStartValue - (xBarSpace * (count - _lastSetPoint.Index + 1 - _startIndex));
                                float y = yPadding + _YstepLength * (_YStartValue - _lastSetPoint.Value);
                                g.DrawLine(pen, x, y, _curMousePoint.X, _curMousePoint.Y);
                            }
                            break;
                    }
                }
                if (_isLineSelected)
                {
                    int index;
                    float _YValue;
                    switch (_selectedLine.Type)
                    {
                        case LineType.horizontal:
                            _YValue = _YStartValue - (_curMousePoint.Y - yPadding) / _YstepLength;
                            ((HorizontalLineData)_selectedLine).Value = _YValue;
                            break;
                        case LineType.vertical:
                            index = chartData.Count - (int)((_XStartValue - _curMousePoint.X) / xBarSpace) - _startIndex;
                            ((VerticalLineData)_selectedLine).Index = index;
                            break;
                        case LineType.trend:
                            Point delta = _curMousePoint.Substract(_oldMousePoint);
                            float indexDelta = delta.X / xBarSpace;
                            _YValue = -delta.Y / _YstepLength;
                            _oldMousePoint = _curMousePoint;
                            break;
                    }
                }
                // CrossDetail show
                if (_crossDetailFlg)
                {
                    crossDetailPoint = new Point((int)_curMousePoint.X + 10, (int)_curMousePoint.Y);
                }
                // Horizontal Line Paint
                if (_isDrawedHorizontalLine)
                {
                    for(int i = 0; i < horizontalLines.Count; i++)
                    {
                        pen = new Pen(ForeGroundColor);
                        if (horizontalLines[i].IsDash) pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                        float y = yPadding + _YstepLength * (_YStartValue - horizontalLines[i].Value);
                        g.DrawLine(pen, 0, y, p.Width - 80, y);
                        if (horizontalLines[i].IsSelected)
                        {
                            g.DrawRectangle(pen, 0, y - 2, 4, 4);
                            g.DrawRectangle(pen, p.Width- 80 - 4, y - 1, 4, 4);
                        }
                        if (_valueLabelList != null)
                        {
                            for(int j =0; j < _valueLabelList.Count; j++) 
                            {
                                if (j == i)
                                {
                                    _valueLabelList[j].Location = new Point(_valueLabelList[j].Location.X, (int)(y));
                                }
                            }
                        }
                    }
                }
                if (_isDrawedVerticalLine)
                {
                    for(int i=0; i < verticalLines.Count; i++)
                    {
                        pen = new Pen(ForeGroundColor);
                        if (verticalLines[i].IsDash) pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        float x = _XStartValue - (xBarSpace * (count - verticalLines[i].Index + 1 - _startIndex));
                        g.DrawLine(pen, x, 0, x, p.Height- 40);
                        if (verticalLines[i].IsSelected)
                        {
                            g.DrawRectangle(pen, x - 2, 0, 4, 4);
                            g.DrawRectangle(pen, x - 2, p.Height -40 - 4, 4, 4);
                        }

                        if (_timeLabelList != null)
                        {
                            for (int j = 0; j < _timeLabelList.Count; j++)
                            {
                                if (j == i)
                                {
                                    _timeLabelList[j].Location = new Point((int)x, _timeLabelList[j].Location.Y);
                                }
                            }
                        }
                    }
                }
                // Trend Line Paint
                if (_isDrawedTrendLine)
                {
                    if (trendLines != null)
                    {
                        foreach (TrendLineData line in trendLines)
                        {
                            pen = new Pen(ForeGroundColor);
                            if (line.IsDash) pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            float x1 = _XStartValue - (xBarSpace * (count - line.FirstPoint.Index + 1 - _startIndex));
                            float x2 = _XStartValue - (xBarSpace * (count - line.EndPoint.Index + 1 - _startIndex));
                            float y1 = yPadding + _YstepLength * (_YStartValue - line.FirstPoint.Value);
                            float y2 = yPadding + _YstepLength * (_YStartValue - line.EndPoint.Value);
                            g.DrawLine(pen, x1, y1, x2, y2);
                            if (line.IsSelected)
                            {
                                g.DrawRectangle(pen, x1 - 2, y1 - 2, 4, 4);
                                g.DrawRectangle(pen, x2 - 2, y2 - 2, 4, 4);
                            }
                        }
                    }
                }
                // Text Paint
                if (_isSetTexts)
                {
                    foreach (TextPointClass text in textData)
                    {
                        DateTime time = text.TextPointTime();
                        int index = -1;
                        for(int i = count - 1 - _startIndex; i >= 0 ; i--)
                        {
                            if(time.CompareTo(chartData[i].ChartTime()) == 0)
                            {
                                index = i;
                                break;
                            }
                        }
                        if (index > 0)
                        {
                            brush = new SolidBrush(text.TextColor());
                            Font font = new Font("Arial", text.TextSize());
                            float x = _XStartValue - (xBarSpace * (count - index - _startIndex));
                            float y = yPadding + _YstepLength * (_YStartValue - text.TextPointValue());
                            g.DrawString(text.Text(), font, brush, x, y);
                            
                        }
                    }
                }
                // Symbol Paint
                if (_isSetSymbols)
                {
                    foreach (SymbolPointClass symbol in symbolData)
                    {
                        DateTime time = symbol.SymbolPointTime();
                        int index = -1;
                        for (int i = count - 1 - _startIndex; i >= 0; i--)
                        {
                            if (time.CompareTo(chartData[i].ChartTime()) == 0)
                            {
                                index = i;
                                break;
                            }
                        }
                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        if (index > 0)
                        {
                            brush = new SolidBrush(symbol.SymbolColor());
                            Font font = new Font("Arial", symbol.SymbolSize());
                            float x = _XStartValue - (xBarSpace * (count - index - _startIndex));
                            float y = yPadding + _YstepLength * (_YStartValue - symbol.SymbolPointValue());
                            g.DrawString(SymbolPattern[symbol.Symbol()], font, brush, x, y, sf);
                        }
                    }
                }
                for (int i = 1; i <= chartData.Count; i++)
                {
                    // calculate real location from value
                    float x = _XStartValue - (xBarSpace * i);
                    if (x < 0) break;
                    int index = count - i - _startIndex;
                    if (index < 0) break;
                    Object d = chartData[index];
                    float OpenY = yPadding + _YstepLength * (_YStartValue - d.Open());
                    float CloseY = yPadding + _YstepLength * (_YStartValue - d.Close());
                    float HighY = yPadding + _YstepLength * (_YStartValue - d.High());
                    float LowY = yPadding + _YstepLength * (_YStartValue - d.Low());

                    Object update_obj = chartData[count - 1];
                    float UpdateOpenY = yPadding + _YstepLength * (_YStartValue - update_obj.Open());
                    float UpdateCloseY = yPadding + _YstepLength * (_YStartValue - update_obj.Close());
                    update_horizontalLines.Add(new HorizontalLineData(UpdateCloseY, IsDash));
                    if (_curMousePoint.X >= (int)x - xBarRadius && (int)x - xBarRadius + 2 * xBarRadius >= _curMousePoint.X)
                    {
                        if (OpenY < CloseY)
                        {
                            if (_curMousePoint.Y >= (int)OpenY && _curMousePoint.Y <= CloseY)
                            {
                                Font font = new Font("Arial", 12);
                                int cur_index = chartData.Count - (int)((_XStartValue - _curMousePoint.X) / xBarSpace) - _startIndex;
                                if (cur_index < 0 || cur_index >= chartData.Count) return;
                                float _YValue = _YStartValue - (_curMousePoint.Y - yPadding) / _YstepLength;
                                float cur_OpenY = _YStartValue - (OpenY - yPadding) / _YstepLength;
                                float cur_CloseY = _YStartValue - (CloseY - yPadding) / _YstepLength;
                                float cur_LowY = _YStartValue - (LowY - yPadding) / _YstepLength;
                                float cur_HighY = _YStartValue - (HighY - yPadding) / _YstepLength;
                                String curStickData1 = "OpenY : " + cur_OpenY.ToString("n5") + " CloseY : " + cur_CloseY.ToString("n5");
                                String curStickData2 = "LowY : " + cur_LowY.ToString("n5") + " HighY : " + cur_HighY.ToString("n5");
                                g.DrawString(curStickData1, font, brush1, new Point(_curMousePoint.X - 120, _curMousePoint.Y + 25));
                                g.DrawString(curStickData2, font, brush1, new Point(_curMousePoint.X - 110, _curMousePoint.Y + 55));
                            }
                        }
                        else
                        {
                            if (_curMousePoint.Y <= (int)OpenY && _curMousePoint.Y >= CloseY)
                            {
                                Font font = new Font("Arial", 12);
                                int cur_index = chartData.Count - (int)((_XStartValue - _curMousePoint.X) / xBarSpace) - _startIndex;
                                if (cur_index < 0 || cur_index >= chartData.Count) return;
                                float _YValue = _YStartValue - (_curMousePoint.Y - yPadding) / _YstepLength;
                                float cur_OpenY = _YStartValue - (OpenY - yPadding) / _YstepLength;
                                float cur_CloseY = _YStartValue - (CloseY - yPadding) / _YstepLength;
                                float cur_LowY = _YStartValue - (LowY - yPadding) / _YstepLength;
                                float cur_HighY = _YStartValue - (HighY - yPadding) / _YstepLength;
                                String curStickData1 = "OpenY: " + cur_OpenY.ToString("n5") + " CloseY: " + cur_CloseY.ToString("n5");
                                String curStickData2 = "LowY: " + cur_LowY.ToString("n5") + " HighY: " + cur_HighY.ToString("n5");
                                //g.DrawRectangle(pen)
                                g.DrawString(curStickData1, font, brush1, new Point(_curMousePoint.X - 120, _curMousePoint.Y + 25));
                                g.DrawString(curStickData2, font, brush1, new Point(_curMousePoint.X - 110, _curMousePoint.Y + 55));
                            }
                        }
                    }
                }

                g.DrawString(crossDetailText, font1, brush1, crossDetailPoint);
            }

        }
        private void paintValue(object sender, PaintEventArgs e)
        {
            if (chartData == null) return;
            Panel p = (Panel)sender;
            var g = e.Graphics;
            g.Clear(BackGroundColor);
            Pen pen = new Pen(ForeGroundColor);
            Brush brush = new SolidBrush(ForeGroundColor);
            Brush crossBrush = new SolidBrush(BackGroundColor);
            Font font = new Font("Arial", 8);

            float distance = (_YValueMax - _YvalueMin) * yScale;
            float _mean = (_YValueMax + _YvalueMin) / 2f;
            _YStartValue = _mean + (_YValueMax - _mean) * yScale;
            float step = distance / yCount;
            int height = p.Height - 2 * yPadding;
            _YstepLength = height / (float)distance;

            for (float i = 0; ; i += step)
            {
                float y = yPadding + _YstepLength * i;
                if (y > (height + yPadding)) break;
                g.DrawString((_YStartValue - i).ToString("n5"), font, brush, new Point(15, (int)y - 4));
                g.DrawLine(pen, 0, y, 4, y);
            }
            if (IsCrossView)
            {
                g.FillRectangle(brush, new Rectangle(new Point(15, selectedValuePoint.Y), new Size(45, 15)));
                g.DrawString(selectedValueText, font, crossBrush, new Point(15, selectedValuePoint.Y));
            }
        }
        private void paintTime(object sender, PaintEventArgs e)
        {
            if (!_isSetBar) return;
           
            Panel p = (Panel)sender;
            var g = e.Graphics;
            g.Clear(BackGroundColor);
            Pen pen = new Pen(ForeGroundColor);
            Brush brush = new SolidBrush(ForeGroundColor);
            Font font = new Font("Arial", 8);
            int skip = (int)((_panDrwing.Width - 80) / xBarSpace / xCount);
            if (skip == 0) return;
            Brush crossBrush = new SolidBrush(BackGroundColor);
            for (int i = 1; i <= chartData.Count; i++)
            {
                float x = _XStartValue - (xBarSpace * i);
                if (x < 0) break;
                if (i % skip == 0)
                {
                    int index = count - i - _startIndex;
                    if (index < 0) break;
                    g.DrawString(chartData[index].ChartTime().ToString(timeFormart), font, brush, new Point((int)x, 10));
                    g.DrawLine(pen, x, 0, x, 5);
                }
                else
                {
                    g.DrawLine(pen, x, 0, x, 1);
                }
            }
            if (IsCrossView)
            {
                g.FillRectangle(brush, new Rectangle(new Point(selectedTimePoint.X, 10), new Size(105, 13)));
                g.DrawString(selectedTimeText, font, crossBrush, new Point(selectedTimePoint.X, 10));
            }
        }
        
        private void crossMove(object sender, MouseEventArgs e)
        {
            if (_isGraphicMove)
            {
                float moveX = e.Location.X - standardPoint.X;
                standardPoint = e.Location;
                _XStartValue -= moveX/2;

                if (_isNotFill) return;
                float width = (IsXSpacing) ? _panDrwing.Width - 80 - xCurrentPadding : _panDrwing.Width - 80;
                int max = chartData.Count - (int)(width / xBarSpace) + 1;
                int delta = (int)moveX/2;

                if (xCurrentPadding == 0 || !_isXSpacing)
                    _startIndex = Math.Min(Math.Max(_startIndex + delta, 0), max);
                if ((_isXSpacing && _startIndex == 0) || xCurrentPadding > 0)
                    xCurrentPadding = Math.Min(Math.Max(xCurrentPadding - e.Delta / xBarSpace, 0), xPadding);
                _cursorLabel.Location = new Point(e.Location.X - 80, e.Location.Y + 10);
                _oldMousePoint = _curMousePoint;
                float _YValue1 = _YStartValue - (_oldMousePoint.Y - yPadding) / _YstepLength;

                int index1 = chartData.Count - (int)((_XStartValue - _oldMousePoint.X) / xBarSpace) - _startIndex;

                _cursorLabel.Text = "(" + chartData[index1].ChartTime().ToString(timeFormart) + " , " + _YValue1.ToString("n5") + ")";
                Redraw();
            }
            _curMousePoint = _panDrwing.PointToClient(Cursor.Position);
            _panXGuide.Location = new Point(0, _curMousePoint.Y);
            _panYGuide.Location = new Point(_curMousePoint.X, 0);

            int index = chartData.Count - (int)((_XStartValue - _curMousePoint.X) / xBarSpace) - _startIndex;
            if (index < 0 || index >= chartData.Count) return;
            float _YValue = _YStartValue - (_curMousePoint.Y - yPadding) / _YstepLength;
            
            if (_isSetInfoLabel)
            {
                BarClass data = chartData[index];
                _lblCurTime.Text = data.ChartTime().ToString(timeFormart);
                _lblCurHigh.Text = data.High().ToString("n5");
                _lblCurLow.Text = data.Low().ToString("n5");
                _lblCurOpen.Text = data.Open().ToString("n5");
                _lblCurClose.Text = data.Close().ToString("n5");
            }
            if (_isCrossView)
            {
                _isRedrawing = true;
                _panDrwing.Invalidate();
                selectedTimePoint = new Point(_curMousePoint.X - 100 / 2, 10);
                selectedValuePoint = new Point(15, _curMousePoint.Y - 20 / 2);
                selectedTimeText = chartData[index].ChartTime().ToString(timeFormart);
                selectedValueText = _YValue.ToString("n5");
            }
            if (_isMouseDowned)
            {
                if (_drawingType == LineType.trend && _isSetLastPoint || _isLineSelected)
                {
                    //crossDetailText
                    int first_index = chartData.Count - (int)(((_XStartValue - _oldMousePoint.X) / xBarSpace) - _startIndex);
                    int count_CandleStick = chartData[index].ChartTime().Second - saveOldSecond;
                    if (count_CandleStick < 0) count_CandleStick += 80;
                    float _yDiffPercent = ((_YValue - _lastSetPoint.Value) / _YValue * 100);
                    crossDetailText = selectedValueText + " : " + count_CandleStick + " bars, " + (int)((_YValue - _lastSetPoint.Value) * 100000) + " points, " + (_yDiffPercent >= 0 ? "+" + _yDiffPercent.ToString("n2") : _yDiffPercent.ToString("n2")) + "%";

                }
            }
            Redraw();
        }
        
        private void showCrossLine(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                _isMiddleButtonPressed_flg = !(_isMiddleButtonPressed_flg);

                DrawingType = LineType.none;
                IsCrossView = !(IsCrossView);
                if (IsCrossView)
                {
                    DrawingType = LineType.trend;
                }
            }
        }
        private void onMouseDownEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown(sender, e);
            }
        }
        private void onMouseUpEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (IsCrossView)
                {
                    setLine(sender, e);
                }
            }
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!IsCrossView)
                {
                    if (DrawingType == LineType.none)
                    {
                        _isGraphicMove = true;
                        standardPoint = e.Location;
                        _oldMousePoint = _curMousePoint;
                        float _YValue = _YStartValue - (_oldMousePoint.Y - yPadding) / _YstepLength;

                        int index = chartData.Count - (int)((_XStartValue - _oldMousePoint.X) / xBarSpace) - _startIndex;

                        _cursorLabel.Text = "(" + chartData[index].ChartTime().ToString(timeFormart) + " , " + _YValue.ToString("n5") + ")";
                        _panDrwing.Cursor = Cursors.SizeWE;
                    }
                    else
                    {
                        _isMouseDowned = true;
                        _oldMousePoint = _panDrwing.PointToClient(Cursor.Position);
                        _isLineSelected = CheckOnSegment(_oldMousePoint, out _selectedLine);
                        if (DrawingType == LineType.trend)
                        {
                            int index = chartData.Count - (int)((_XStartValue - _oldMousePoint.X) / xBarSpace) - _startIndex;
                            float _YValue = _YStartValue - (_oldMousePoint.Y - yPadding) / _YstepLength;
                            _lastSetPoint = new PointForFreeLine(index, _YValue);
                            _isSetLastPoint = true;
                        }
                    }
                    Redraw();
                }
                else
                {
                    _isMouseDowned = true;
                    _oldMousePoint = _curMousePoint;
                    _isLineSelected = CheckOnSegment(_oldMousePoint, out _selectedLine);
                    float _YValue = _YStartValue - (_oldMousePoint.Y - yPadding) / _YstepLength;
                    int index = chartData.Count - (int)((_XStartValue - _oldMousePoint.X) / xBarSpace) - _startIndex;
                    horizontalLines.Add(new HorizontalLineData(_YValue, IsDash));
                    verticalLines.Add(new VerticalLineData(index, IsDash));
                    _isDrawedHorizontalLine = true;
                    _isDrawedVerticalLine = true;
                    if (DrawingType == LineType.trend)
                    {
                        _lastSetPoint = new PointForFreeLine(index, _YValue);
                        _isSetLastPoint = true;
                    }
                    float _uYValue = _YStartValue - (_YValue - yPadding) / _YstepLength;
                    
                    Label addValueLabel = new Label(); //add value label
                    addValueLabel.BackColor = ForeGroundColor;
                    addValueLabel.ForeColor = BackGroundColor;
                    addValueLabel.AutoSize = true;
                    addValueLabel.Location = new Point(15, _oldMousePoint.Y - addValueLabel.Height / 2);
                    addValueLabel.Text = _YValue.ToString("n5");
                    _valueLabelList.Add(addValueLabel);
                    _panValue.Controls.Add(addValueLabel);

                    Label addTimeLabel = new Label(); //add time label
                    addTimeLabel.BackColor = ForeGroundColor;
                    addTimeLabel.ForeColor = BackGroundColor;
                    addTimeLabel.AutoSize = true;
                    addTimeLabel.Location = new Point(_curMousePoint.X - addTimeLabel.Width / 2, 10);
                    addTimeLabel.Text = chartData[index].ChartTime().ToString(timeFormart);
                    _timeLabelList.Add(addTimeLabel);
                    _panTime.Controls.Add(addTimeLabel);

                    _crossDetailFlg = true;

                    if (saveOldSecond < 0)
                    {
                        saveOldSecond = chartData[index].ChartTime().Second;
                    }
                    Redraw();
                }
            }
        }
        private void tickHandler(object sender, EventArgs e)
        {
            Redraw(true);
        }
        private void valueScaleChange(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float _y = Control.MousePosition.Y;
                float delta = _y - _mouseOldY;
                if(Math.Abs(delta) < 5)
                {
                    yScale = Math.Max(Math.Min(yScale + delta / 10f, 10), 1.05f);
                    Redraw();
                }
                _mouseOldY = _y;
            }
        }
        private void valueScaleReset(object sender, EventArgs e)
        {
            yScale = 1.05f;
            Redraw();
        }
        private void timeScaleChange(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float _x = Control.MousePosition.X;
                float delta = _x - _mouseOldX;
                if (Math.Abs(delta) < 5)
                {
                    xBarSpace = Math.Max(Math.Min(xBarSpace + delta / 5f, 20), 1);
                    Redraw(true);
                }
                _mouseOldX = _x;
            }
        }
        private void chartMove(object sender, MouseEventArgs e)
        {
            _isChartMoving = true;
            _isRedrawing = false;
            if (_isNotFill) return;
            float width = (IsXSpacing) ? _panDrwing.Width - 80 - xCurrentPadding : _panDrwing.Width - 80;
            int max = chartData.Count - (int)(width / xBarSpace) + 1;
            int delta = e.Delta / 10;
            if (xCurrentPadding == 0 || !_isXSpacing)
                _startIndex = Math.Min(Math.Max(_startIndex + delta, 0), max);
            if ((_isXSpacing && _startIndex == 0) || xCurrentPadding > 0)
                xCurrentPadding = Math.Min(Math.Max(xCurrentPadding - e.Delta / xBarSpace, 0), xPadding);
            Redraw();
        }
        private void setLine(object sender, MouseEventArgs e)
        {
            _cursorLabel.Location = new Point(-100, -100);
            _panDrwing.Cursor = Cursors.Default;
            _isMouseDowned = false;
            _isLineSelected = false;_isGraphicMove = false;
            if ( _drawingType != LineType.none)
            {
                Point point = _panDrwing.PointToClient(Cursor.Position);
                switch (_drawingType)
                {
                    case LineType.horizontal:
                        if (!_isDrawedHorizontalLine)
                        {
                            horizontalLines = new List<HorizontalLineData>();
                            _isDrawedHorizontalLine = true;
                        }
                        float _YValue = _YStartValue - (point.Y - yPadding) / _YstepLength;
                        horizontalLines.Add(new HorizontalLineData(_YValue, IsDash));

                        Label addValueLabel = new Label();
                        addValueLabel.BackColor = ForeGroundColor;
                        addValueLabel.ForeColor = BackGroundColor;
                        addValueLabel.AutoSize = true;

                        addValueLabel.Location = new Point(15, point.Y - addValueLabel.Height / 2);
                        addValueLabel.Text = _YValue.ToString("n5");
                        _valueLabelList.Add(addValueLabel);

                        _panValue.Controls.Add(addValueLabel);
                        break;
                    case LineType.vertical:
                        if (!_isDrawedVerticalLine)
                        {
                            verticalLines = new List<VerticalLineData>();
                            _isDrawedVerticalLine = true;
                        }
                        int index = chartData.Count - (int)((_XStartValue - point.X) / xBarSpace) - _startIndex;
                        verticalLines.Add(new VerticalLineData(index, IsDash));

                        Label addTimeLabel = new Label(); //add time label
                        addTimeLabel.BackColor = ForeGroundColor;
                        addTimeLabel.ForeColor = BackGroundColor;
                        addTimeLabel.AutoSize = true;

                        addTimeLabel.Location = new Point(point.X - addTimeLabel.Width / 2, 10);
                        addTimeLabel.Text = chartData[index].ChartTime().ToString(timeFormart);
                        _timeLabelList.Add(addTimeLabel);

                        _panTime.Controls.Add(addTimeLabel);
                        break;
                    default:   // free
                        if (_isSetLastPoint)
                        {
                            if (!_isDrawedTrendLine)
                            {
                                trendLines = new List<TrendLineData>();
                                _isDrawedTrendLine = true;
                            }
                            index = chartData.Count - (int)((_XStartValue - point.X) / xBarSpace) - _startIndex;
                            if (index < 0 || index >= chartData.Count) return;
                            _YValue = _YStartValue - (point.Y - yPadding) / _YstepLength;
                            
                            horizontalLines.Add(new HorizontalLineData(_YValue, IsDash));
                            verticalLines.Add(new VerticalLineData(index, IsDash));

                            Label addValueLabel1 = new Label(); //add value label
                            addValueLabel1.BackColor = ForeGroundColor;
                            addValueLabel1.ForeColor = BackGroundColor;
                            addValueLabel1.AutoSize = true;

                            addValueLabel1.Location = new Point(15, point.Y - addValueLabel1.Height / 2);
                            addValueLabel1.Text = _YValue.ToString("n5");
                            _valueLabelList.Add(addValueLabel1);

                            _panValue.Controls.Add(addValueLabel1);

                            Label addTimeLabel1 = new Label(); //add time label
                            addTimeLabel1.BackColor = ForeGroundColor;
                            addTimeLabel1.ForeColor = BackGroundColor;
                            addTimeLabel1.AutoSize = true;

                            addTimeLabel1.Location = new Point(point.X - addTimeLabel1.Width / 2, 10);
                            addTimeLabel1.Text = chartData[index].ChartTime().ToString(timeFormart);
                            _timeLabelList.Add(addTimeLabel1);

                            _panTime.Controls.Add(addTimeLabel1);

                            trendLines.Add(new TrendLineData(_lastSetPoint, new PointForFreeLine(index, _YValue), IsDash));

                            if (_crossDetailFlg)
                            {
                                crossDetailPoint = new Point(-10, -10);
                                crossDetailText = "";
                                verticalLines.RemoveRange(0, verticalLines.Count);
                                horizontalLines.RemoveRange(0, horizontalLines.Count);
                                trendLines.RemoveRange(0, trendLines.Count);
                                _timeLabelList.RemoveRange(0, _timeLabelList.Count);
                                _panTime.Controls.Clear();
                                _panValue.Controls.Clear();
                                _panValue.Controls.Add(_lblUpdateValueText);
                                _valueLabelList = new List<Label>();
                                _crossDetailFlg = false;
                            }
                            if (IsCrossView)
                            {
                                saveOldSecond = -1;
                                DrawingType = LineType.none;
                                _crossDetailFlg = true;
                                IsCrossView = false;
                            }
                        }
                    break;
                }
                Redraw();
            }
            else
            {
                _isGraphicMove = false;
            }
        }
        private void openMenu(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _frmSetting.picBack.BackColor = BackGroundColor;
                _frmSetting.picFore.BackColor = ForeGroundColor;
                _frmSetting.picUpInSide.BackColor = UpGoingInsideColor;
                _frmSetting.picUpBorder.BackColor = UpGoingBorderColor;
                _frmSetting.picDownInside.BackColor = DownGoingInsideColor;
                _frmSetting.picDownBorder.BackColor = DownGoingBorderColor;
                _frmSetting.txtXSpace.Text = XSPace.ToString();
                _frmSetting.chkAutoScroll.Checked = _isAutoScroll;
                _frmSetting.Location = e.Location.Add(_panParent.Location).Add(_panParent.Parent.Location);
                _frmSetting.Focus();
                _frmSetting.Show();
            }
        }
        private void settingChange()
        {
            BackGroundColor = _frmSetting.picBack.BackColor;
            ForeGroundColor = _frmSetting.picFore.BackColor;
            DownGoingBorderColor = _frmSetting.picDownBorder.BackColor;
            RecentHorizontalColor = _frmSetting.picRecent.BackColor;
            DownGoingInsideColor = _frmSetting.picDownInside.BackColor;
            UpGoingBorderColor = _frmSetting.picUpBorder.BackColor;
            UpGoingInsideColor = _frmSetting.picUpInSide.BackColor;
            int _trySpace;
            int.TryParse(_frmSetting.txtXSpace.Text, out _trySpace);
            XSPace = _trySpace;
            _isAutoScroll = _frmSetting.chkAutoScroll.Checked;
            Redraw(true);
        }
        #endregion

        #region Private API
        private bool CheckOnSegment(Point _MousePt, out Line selected)
        {
            if (_isDrawedHorizontalLine)
            {
                float y;
                foreach(HorizontalLineData line in horizontalLines)
                {
                    y = yPadding + _YstepLength * (_YStartValue - line.Value);
                    if((y - _MousePt.Y) * (y - _MousePt.Y) < _selectThreshold)
                    {
                        line.IsSelected = !line.IsSelected;
                        selected = line;
                        return true;
                    }
                }
            }
            if (_isDrawedVerticalLine)
            {
                float x;
                foreach(VerticalLineData line in verticalLines)
                {
                    x = _XStartValue - (xBarSpace * (count - line.Index + 1 - _startIndex));
                    if((x - _MousePt.X) * (x - _MousePt.X) < _selectThreshold)
                    {
                        line.IsSelected = !line.IsSelected;
                        selected = line;
                        return true;
                    }
                }
            }
            if (_isDrawedTrendLine)
            {
                float x1, x2, y1, y2;
                if (trendLines != null)
                {
                    foreach (TrendLineData line in trendLines)
                    {
                        x1 = _XStartValue - (xBarSpace * (count - line.FirstPoint.Index + 1 - _startIndex));
                        x2 = _XStartValue - (xBarSpace * (count - line.EndPoint.Index + 1 - _startIndex));
                        y1 = yPadding + _YstepLength * (_YStartValue - line.FirstPoint.Value);
                        y2 = yPadding + _YstepLength * (_YStartValue - line.EndPoint.Value);
                        if (FindDistancSquarToSegment(_MousePt, x1, x2, y1, y2) < _selectThreshold)
                        {
                            line.IsSelected = !line.IsSelected;
                            selected = line;
                            return true;
                        }
                    }
                }
            }
            selected = null;
            return false;
        }
        private float FindDistancSquarToSegment(Point pt, float x1, float x2, float y1, float y2)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;
            PointF closest;
            // Calculate the t that minimizes the distance.
            float t = ((pt.X - x1) * dx + (pt.Y - y1) * dy) /
                (dx * dx + dy * dy);
            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                closest = new PointF(x1, y1);
                dx = pt.X - x1;
                dy = pt.Y - y1;
            }
            else if (t > 1)
            {
                closest = new PointF(x2, y2);
                dx = pt.X - x2;
                dy = pt.Y - y2;
            }
            else
            {
                closest = new PointF(x1 + t * dx, y1 + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }
            return dx * dx + dy * dy;
        }
        #endregion
    }
}
