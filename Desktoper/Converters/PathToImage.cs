using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Data;
using Desktoper.Model;

namespace Desktoper.Converters
{
    class PathToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                FileInfo ds = new FileInfo(value as string);
                if (ds.Extension == ".siteIco") // if need load site icon
                {
                    System.Drawing.Bitmap br = new System.Drawing.Bitmap(value as string);
                    return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                               br.GetHbitmap(),
                               IntPtr.Zero,
                               Int32Rect.Empty,
                               BitmapSizeOptions.FromEmptyOptions());
                } 
                System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(value as string);
                return ToImageSource(icon);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        private ImageSource ToImageSource(System.Drawing.Icon icon)
        {
            ImageSource imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
}
