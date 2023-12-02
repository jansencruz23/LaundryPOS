using Guna.Charts.WinForms;

namespace LaundryPOS.Helpers
{
    public class LightChartConfig
    {
        public static ChartConfig Config()
        {
            Color gridColor = Color.FromArgb(214, 219, 224);
            Color foreColor = Color.FromArgb(105, 121, 139);
            Color[] colors = new Color[] { Color.FromArgb(255, 48, 162) };

            var font = new Guna.Charts.WinForms.ChartFont()
            {
                FontName = "Segoe UI",
                Size = 10,
                Style = ChartFontStyle.Normal
            };

            ChartConfig config = new ChartConfig()
            {
                Title =
                {
                    ForeColor = foreColor
                },
                Legend =
                {
                    LabelFont = font,
                    LabelForeColor = foreColor
                },
                XAxes =
                {
                    GridLines =
                    {
                        Color = gridColor,
                        ZeroLineColor = gridColor
                    },
                    Ticks =
                    {
                        Font = font,
                        ForeColor = foreColor
                    }
                },
                YAxes =
                {
                    GridLines =
                    {
                        Color = gridColor,
                        Display = false,
                        ZeroLineColor = gridColor
                    },
                    Ticks =
                    {
                        Font = font,
                        ForeColor = foreColor
                    }
                },
                ZAxes =
                {
                    GridLines =
                    {
                        Color = gridColor,
                        ZeroLineColor = gridColor
                    },
                    Ticks =
                    {
                        Font = font,
                        ForeColor = foreColor
                    },
                    PointLabels =
                    {
                        Font = font,
                        ForeColor = foreColor
                    }
                },
                PaletteCustomColors =
                {
                    FillColors = new ColorCollection(colors),
                    BorderColors = new ColorCollection(colors),
                    PointFillColors = new ColorCollection(colors),
                    PointBorderColors = new ColorCollection(colors)
                }
            };
            return config;
        }

        public static ChartConfig PieChartConfig()
        {
            Color gridColor = Color.FromArgb(214, 219, 224);
            Color foreColor = Color.FromArgb(105, 121, 139);
            Color[] colors = new Color[] { Color.FromArgb(140, 81, 165), Color.FromArgb(203, 94, 152), Color.FromArgb(244, 123, 138), Color.FromArgb(255, 163, 127), Color.FromArgb(255, 210, 133) };

            var font = new Guna.Charts.WinForms.ChartFont()
            {
                FontName = "Segoe UI",
                Size = 10,
                Style = ChartFontStyle.Normal
            };

            ChartConfig config = new ChartConfig()
            {
                Title =
                {
                    ForeColor = foreColor
                },
                Legend =
                {
                    LabelFont = font,
                    LabelForeColor = foreColor
                },
                XAxes =
                {
                    GridLines =
                    {
                        Color = gridColor,
                        ZeroLineColor = gridColor
                    },
                    Ticks =
                    {
                        Font = font,
                        ForeColor = foreColor
                    }
                },
                YAxes =
                {
                    GridLines =
                    {
                        Color = gridColor,
                        Display = false,
                        ZeroLineColor = gridColor
                    },
                    Ticks =
                    {
                        Font = font,
                        ForeColor = foreColor
                    }
                },
                ZAxes =
                {
                    GridLines =
                    {
                        Color = gridColor,
                        ZeroLineColor = gridColor
                    },
                    Ticks =
                    {
                        Font = font,
                        ForeColor = foreColor
                    },
                    PointLabels =
                    {
                        Font = font,
                        ForeColor = foreColor
                    }
                },
                PaletteCustomColors =
                {
                    FillColors = new ColorCollection(colors),
                    BorderColors = new ColorCollection(colors),
                    PointFillColors = new ColorCollection(colors),
                    PointBorderColors = new ColorCollection(colors)
                }
            };
            return config;
        }
    }
}