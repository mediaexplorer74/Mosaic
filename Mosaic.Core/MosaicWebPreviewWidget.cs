// MosaicWebPreviewWidget

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net; // WebBrowser here =)
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using System.Windows.Media.Animation;
using Mosaic.Base;
using Mosaic.Core.Controls; // MosaicFriendWidgetControl here =)
using ContextMenu = System.Windows.Controls.ContextMenu;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
//using Image = System.Windows.Controls.Image;
using MenuItem = System.Windows.Controls.MenuItem;
using WebBrowser = System.Windows.Forms.WebBrowser;
using System.Windows.Controls;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;

// Mosaic.Core namespace
namespace Mosaic.Core
{
    // MosaicWebPreviewWidget class 
    public class MosaicWebPreviewWidget : MosaicWidget
    {
        // =)
        //private string file;
        private Grid root;
        private Image icon;
        private TextBlock title;

        // RnD
        private MosaicFriendWidgetControl control;
        private WebClient webClient;
        private string id;

        private Image previewControl;
        private System.Windows.Forms.WebBrowser browser;
        private string url;
        private string file;

        public override string Name
        {
            get 
            {
                //return "Name1"; 
                return string.Empty; 
            }
        }

        /*
        public override System.Windows.FrameworkElement WidgetControl
        {
            get { return previewControl; }
        }

        public override Uri IconPath
        {
            get { return null; }
        }

        public override int ColumnSpan
        {
            get { return 1; }
        }
        */
        public override FrameworkElement WidgetControl
        {
            get { return root; }
        }

        public override Uri IconPath
        {
            get { return null; }
        }

        public override int ColumnSpan
        {
            get { return 1; }
        }


        // Load(string path)
        public override void Load(string path)
        {
            
            url = path;

            /*
            previewControl = new Image();
            previewControl.Stretch = Stretch.UniformToFill;
            previewControl.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(PreviewControlMouseLeftButtonUp);
            RenderOptions.SetBitmapScalingMode(previewControl, BitmapScalingMode.HighQuality);
            previewControl.HorizontalAlignment = HorizontalAlignment.Left;

            file = ConvertUrlToFileName(path) + ".png";
            if (File.Exists(E.Root + "\\Thumbnails\\" + file))
            {
                var bi = new BitmapImage();

                bi.BeginInit();

                bi.CacheOption = BitmapCacheOption.OnLoad;

                bi.UriSource = new Uri(E.Root + "\\Thumbnails\\" + file);

                bi.EndInit();
                previewControl.Source = bi;
                //previewControl.Source = new BitmapImage(new Uri(E.Root + "\\Thumbnails\\" + file));
            }
            else
            {
                browser = new WebBrowser();
                browser.ScrollBarsEnabled = false;
                browser.ScriptErrorsSuppressed = true;
                //browser.Navigated += BrowserNavigated;
                browser.DocumentCompleted += BrowserDocumentCompleted;
                browser.Width = 1024;
                browser.Height = 768;
                browser.Navigate(path);
            }
            */

            // 1
            root = new Grid(); // !

            icon = new Image(); // !!
            icon.Width = 64;
            icon.Height = 64;

            var bgBrush = new LinearGradientBrush();

            bgBrush.StartPoint = new System.Windows.Point(0, 0);
            bgBrush.EndPoint = new System.Windows.Point(1, 0);
            bgBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#280a37"), 0));
            bgBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2f0d40"), 1));

            root.Background = bgBrush;

            //var source = GetIcon(file);
            //icon.Source = source;
            icon.VerticalAlignment = VerticalAlignment.Bottom;
            icon.HorizontalAlignment = HorizontalAlignment.Left;
            icon.Margin = new Thickness(12);

            RenderOptions.SetBitmapScalingMode(icon, BitmapScalingMode.HighQuality);

            //RnD
            root.Children.Add(icon);

            // 2
            //var f = FileVersionInfo.GetVersionInfo(file);

            title = new TextBlock();

            title.Foreground = System.Windows.Media.Brushes.White;
            title.Margin = new Thickness(12, 12, 12, 12); //new Thickness(12, 20, 12, 40);
            title.VerticalAlignment = VerticalAlignment.Top;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.Text = path;//f.FileDescription;
            title.FontSize = 16;//20;
            title.FontWeight = FontWeights.Light;
            title.TextWrapping = TextWrapping.Wrap;//TextWrapping.WrapWithOverflow;
            title.TextTrimming = TextTrimming.CharacterEllipsis;

            root.Children.Add(title);

            // 3
            root.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(PreviewControlMouseLeftButtonUp);//RootMouseLeftButtonUp;


        }//Load(string path) end


        // Load(string path, string name, int seed)
        public override void Load(string path, string name, int seed)
        {
            url = path;

            /*
            // Good block!
                        
            previewControl = new Image(); // Image object
            //previewControl.UserName.Text = name;

            previewControl.Stretch = Stretch.UniformToFill;
            previewControl.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(PreviewControlMouseLeftButtonUp);
            RenderOptions.SetBitmapScalingMode(previewControl, BitmapScalingMode.HighQuality);
            previewControl.HorizontalAlignment = HorizontalAlignment.Left;

            file = ConvertUrlToFileName(path) + ".png";
            
            if (File.Exists(E.Root + "\\Thumbnails\\" + file))
            {
                var bi = new BitmapImage();

                bi.BeginInit();

                bi.CacheOption = BitmapCacheOption.OnLoad;

                bi.UriSource = new Uri(E.Root + "\\Thumbnails\\" + file);

                bi.EndInit();
                previewControl.Source = bi;
                //previewControl.Source = new BitmapImage(new Uri(E.Root + "\\Thumbnails\\" + file));
            }
            else
            {
                browser = new WebBrowser();
                browser.ScrollBarsEnabled = false;
                browser.ScriptErrorsSuppressed = true;
                //browser.Navigated += BrowserNavigated;
                browser.DocumentCompleted += BrowserDocumentCompleted;
                browser.Width = 1024;
                browser.Height = 768;
                browser.Navigate(path);
            }
            */


            // 1
            root = new Grid(); // !

            icon = new Image(); // !!
            icon.Width = 64;
            icon.Height = 64;

            var bgBrush = new LinearGradientBrush();

            bgBrush.StartPoint = new System.Windows.Point(0, 0);
            bgBrush.EndPoint = new System.Windows.Point(1, 0);
            bgBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#280a37"), 0));
            bgBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2f0d40"), 1));

            root.Background = bgBrush;

            //var source = GetIcon(file);
            //icon.Source = source;
            icon.VerticalAlignment = VerticalAlignment.Bottom;
            icon.HorizontalAlignment = HorizontalAlignment.Left;
            icon.Margin = new Thickness(12);

            RenderOptions.SetBitmapScalingMode(icon, BitmapScalingMode.HighQuality);

            //RnD
            root.Children.Add(icon);

            // 2
            //var f = FileVersionInfo.GetVersionInfo(file);

            title = new TextBlock();

            title.Foreground = System.Windows.Media.Brushes.White;
            title.Margin = new Thickness(12, 12, 12, 12); //new Thickness(12, 20, 12, 40);
            title.VerticalAlignment = VerticalAlignment.Top;
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.Text = path;//f.FileDescription;
            title.FontSize = 16;//20;
            title.FontWeight = FontWeights.Light;
            title.TextWrapping = TextWrapping.Wrap;//TextWrapping.WrapWithOverflow;
            title.TextTrimming = TextTrimming.CharacterEllipsis;

            root.Children.Add(title);

            // 3
            root.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(PreviewControlMouseLeftButtonUp);//RootMouseLeftButtonUp;


            /*
            //Experimental =)
            string id = "me";//path;
            this.id = id;

            //RnD
            var hub = new MosaicFriendWidgetHub(id);
            hub.Show();

            
            control = new MosaicFriendWidgetControl(seed); // MosaicFriendWidgetControl

            control.UserName.Text = name;
            control.MouseLeftButtonUp += ControlMouseLeftButtonUp;
            control.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#0f92d6"));

            if (1==0)//(!File.Exists(E.Root + "\\Cache\\" + id + ".png"))
            {
                webClient = new WebClient();
                webClient.DownloadFileCompleted += WebClientDownloadFileCompleted;
                webClient.DownloadFileAsync(new Uri(string.Format("https://graph.facebook.com/{0}/picture?type=large", id)), E.Root + "\\Cache\\" + id + ".png");
            }
            else
            {
                var bi = new BitmapImage();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.BeginInit();
                bi.UriSource = new Uri(E.Root + "\\Cache\\" + id + ".png");
                bi.EndInit();
                control.UserPic.Source = bi;
            }
            */

        }//Load(string path, string name, int seed) end



        //RootMouseLeftButtonUp
        /*
        void RootMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WinAPI.ShellExecute(IntPtr.Zero, "open", file, null, null, 3);
        }//RootMouseLeftButtonUp end
        */

        // !
        private MemoryStream iconStream;

        //GetIcon
        private BitmapSource GetIcon(string file)
        {
            if (File.Exists(E.Root + "\\AppIcons\\" + Path.GetFileNameWithoutExtension(file) + ".png"))
            {
                return new BitmapImage(new Uri(E.Root + "\\AppIcons\\" + Path.GetFileNameWithoutExtension(file) + ".png"));
            }

            Icon icon = null;
            Icon[] splitIcons = null;

            try
            {
                using (var extractor = new IconExtractor(file))
                {
                    icon = extractor.GetIcon(0);
                }
            }
            catch (Exception)
            {
                return null;
            }
            splitIcons = IconExtractor.SplitIcon(icon);
            if (splitIcons == null)
                return null;
            foreach (var splitIcon in splitIcons)
            {
                if (splitIcon.Width == 256 && splitIcon.Height == 256)
                {
                    icon = splitIcon;
                    continue;
                }
                if (icon == null && splitIcon.Width == 32 && splitIcon.Height == 32)
                {
                    icon = splitIcon;
                    continue;
                }
                splitIcon.Dispose();
            }
            iconStream = new MemoryStream();
            icon.Save(iconStream);
            iconStream.Seek(0, SeekOrigin.Begin);
            return BitmapFrame.Create(iconStream);

        }//GetIcon

        //RnD
        void WebClientDownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            webClient.DownloadFileCompleted -= WebClientDownloadFileCompleted;
            if (e.Error != null)
                return;
            var bi = new BitmapImage();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.BeginInit();
            bi.UriSource = new Uri(E.Root + "\\Cache\\" + id + ".png");
            bi.EndInit();
            control.UserPic.Source = bi;
            //control.UserPic.Source = new BitmapImage(new Uri(E.Root + "\\Cache\\" + id + ".png"));
        }

        /*
        void ControlMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var hub = new MosaicFriendWidgetHub(id);
            hub.Show();
        }
        */



        // Refresh
        public override void Refresh()
        {
            browser = new WebBrowser();
            browser.ScrollBarsEnabled = false;
            browser.ScriptErrorsSuppressed = true;
            browser.DocumentCompleted += BrowserDocumentCompleted;
            browser.Width = 1024;
            browser.Height = 768;
            browser.Navigate(url);
        }//Refresh end


        // PreviewControlMouseLeftButtonUp
        void PreviewControlMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WinAPI.ShellExecute(IntPtr.Zero, "open", url, null, null, 1);
        }//PreviewControlMouseLeftButtonUp end


        // BrowserDocumentCompleted
        void BrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (browser.ReadyState != WebBrowserReadyState.Complete)
                return;
            
            browser.DocumentCompleted -= BrowserDocumentCompleted;
            
            var bitmap = new Bitmap(browser.Width, browser.Height);
            
            browser.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, browser.Width, browser.Height));
            
            try
            {
                previewControl.Source = null;
                if (File.Exists(E.Root + "\\Thumbnails\\" + file))
                    File.Delete(E.Root + "\\Thumbnails\\" + file);
                bitmap.Save(E.Root + "\\Thumbnails\\" + file, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MosaicWebPreviewWidget - BrowserDocumentCompleted - Ex: " + ex.Message);
            }

            if (File.Exists(E.Root + "\\Thumbnails\\" + file))
            {
                var bi = new BitmapImage();

                bi.BeginInit();

                bi.CacheOption = BitmapCacheOption.OnLoad;

                bi.UriSource = new Uri(E.Root + "\\Thumbnails\\" + file);

                bi.EndInit();

                previewControl.Source = bi;

                /*previewControl.Source = new BitmapImage(new Uri(E.Root + "\\Thumbnails\\" + file));
                    //ToBitmapSource(bitmap);*/
            }

            bitmap.Dispose();
            browser.Dispose();
        }//

        // Unload()
        public override void Unload()
        {
            if (browser != null && !browser.IsDisposed)
                browser.Dispose();

            // patch : null previewControl generates exception
            if (previewControl != null)
            {
                previewControl.MouseLeftButtonUp -= PreviewControlMouseLeftButtonUp;
            }

            // RnD
            //control.MouseLeftButtonUp -= ControlMouseLeftButtonUp;

            // =)
            if (iconStream != null)
            {
                iconStream.Dispose();
            }

            // patch : null root generates exception
            if (root != null)
            {
                root.MouseLeftButtonUp -= PreviewControlMouseLeftButtonUp;//RootMouseLeftButtonUp;
            }

        }//Unload() end

        // ConvertUrlToFileName
        private static string ConvertUrlToFileName(string url)
        {
            return Path.GetFileName(Uri.UnescapeDataString(url).Replace("/", "\\").Replace("?", "-").Replace(":", "-"));
        }// ConvertUrlToFileName end


        // ToBitmap
        public Bitmap ToBitmap(BitmapSource source)
        {
            var bmp = new Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            var data = bmp.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }//ToBitmap end


        // CalcAverageColor
        private static System.Drawing.Color CalcAverageColor(Bitmap image)
        {
            var bmp = new Bitmap(1, 1);
            var orig = image;
            using (var g = Graphics.FromImage(bmp))
            {
                // the Interpolation mode needs to be set to 
                // HighQualityBilinear or HighQualityBicubic or this method
                // doesn't work at all.  With either setting, the results are
                // slightly different from the averaging method.
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(orig, new System.Drawing.Rectangle(0, 0, 1, 1));
            }
            var pixel = bmp.GetPixel(0, 0);
            orig.Dispose();
            bmp.Dispose();
            return pixel;

        }//CalcAverageColor end

    }//MosaicWebPreviewWidget class end

}//Mosaic.Core namespace end
