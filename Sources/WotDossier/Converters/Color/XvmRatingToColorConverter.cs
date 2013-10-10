﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace WotDossier.Converters.Color
{
    public class XvmRatingToColorConverter : IValueConverter
    {
        private static readonly XvmRatingToColorConverter defaultInstance = new XvmRatingToColorConverter();

        public static XvmRatingToColorConverter Default { get { return defaultInstance; } }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? eff = value as double?;
            if (eff != null)
            {
                if (eff >= 93)
                    return EffRangeBrushes.Purple;
                if (eff >= 76)
                    return EffRangeBrushes.Blue;
                if (eff >= 53)
                    return EffRangeBrushes.Green;
                if (eff >= 34)
                    return EffRangeBrushes.Yellow;
                if (eff >= 17)
                    return EffRangeBrushes.Orange;
            }
            return EffRangeBrushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
