using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using Mosaic.Base;

namespace Music
{
    public class Widget : MosaicWidget
    {
        private MusicWidget widgetControl;
        //public static Settings Settings;

        public override string Name
        {
            get { return "Music"; }
        }

        public override FrameworkElement WidgetControl
        {
            get { return widgetControl; }
        }

        public override Uri IconPath
        {
            get { return  new Uri("/Music;component/Resources/music2.png", UriKind.Relative);; }
        }

        public override int ColumnSpan
        {
            get { return 2; }
        }

        public override void Load()
        {
            //Settings = (Settings)XmlSerializable.Load(typeof(Settings), E.WidgetsRoot + "\\Music\\Music.config") ?? new Settings();
            try
            {
                widgetControl = new MusicWidget();
            }
            catch 
            {
                Debug.WriteLine("new MusicWidget fails");
            }

            try
            {
                widgetControl.Load();
            }
            catch 
            {
                Debug.WriteLine("widgetControl.Load fails");
            }
        }

        public override void Unload()
        {
            //Settings.Save(E.WidgetsRoot + "\\Music\\Music.config");
            widgetControl.Unload();
        }

        public override void Notify(string message)
        {
            switch (message)
            {
                case "IsMediaLoaded":

                    // patch : null widgetControl generates exception
                    if (widgetControl != null)
                    {
                        if (widgetControl.IsMediaLoaded)
                            Share.SendMessage("Mosaic.Widgets", "Clock:MediaLoaded");
                    }

                    break;
                case "Next":
                    widgetControl.NextTrack();
                    break;
                case "PlayPause":
                    widgetControl.PlayPauseTrack();
                    break;
                case "Prev":
                    widgetControl.PreviousTrack();
                    break;
            }
        }
    }
}
