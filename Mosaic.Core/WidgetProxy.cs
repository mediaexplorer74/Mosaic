// WidgetProxy

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Mosaic.Base;

// Mosaic.Core namespace
namespace Mosaic.Core
{
    // WidgetProxy class
    public class WidgetProxy
    {
        public readonly string Path; // 

        private Assembly assembly; // 
        public MosaicWidget WidgetComponent { get; private set; } //

        //private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger(); //

        public bool IsLoaded { get; private set; } // 
        public string Name { get; private set; } // 
        public bool HasErrors { get; private set; } // 
        public int Column { get; set; } // 
        public int Row { get; set; } // 
        public WidgetType WidgetType { get; set; } // 


        // WidgetProxy ? isSocial ??
        public WidgetProxy(string path, string name = null, bool isGenerated = false, bool isSocial = false)
        {
            Path = path;
            Column = -1;
            Row = -1;

            // генерируемый? :)
            if (isGenerated)
            {
                InitializeGenerated();
                return;
            }

            // социальный? :)
            if (isSocial)
            {
                Name = name;
                InitializeSocial();
                return;
            }

            // инициализируем!
            Initialize();
        }

        // инициализация виджетов (провайдеров)
        private void Initialize()
        {
            Type widgetType = null;

            try
            {
                assembly = Assembly.LoadFrom(Path);
                widgetType = assembly.GetTypes().FirstOrDefault(type => typeof(MosaicWidget).IsAssignableFrom(type));
            }
            catch (Exception ex)
            {
                // что-то не так? пишем Failed to load provider
                Debug.WriteLine("WidgetProxy - Initialize - Exception: " + ex.Message);
                //logger.Error("Failed to load provider from " + Path + ".\n" + ex);
                Debug.WriteLine("Failed to load provider from " + Path + ".\n" + ex);
                
                HasErrors = true;

                return;
            }

            if (widgetType == null)
            {
                // Failed to find IWeatherProvider 
                //logger.Error("Failed to find IWeatherProvider in " + Path);
                Debug.WriteLine("Failed to find IWeatherProvider in " + Path);
                
                HasErrors = true; // имеет некие ошибки
                return;
            }

            WidgetComponent = Activator.CreateInstance(widgetType) as MosaicWidget;
            if (WidgetComponent == null)
            {
                HasErrors = true; // имеет некие ошибки
                return;
            }

            Name = WidgetComponent.Name;
            WidgetType = WidgetType.Native;

        }//Initialize end 

        // InitializeGenerated
        private void InitializeGenerated()
        {
            if ( Path.StartsWith("http://") || Path.StartsWith("https://") )
            {
                WidgetComponent = new MosaicWebPreviewWidget();
                //Name = "Web Widget";
            }
            else
            {
                WidgetComponent = new MosaicAppWidget();
                //Name = "App Widget";
            }

            WidgetType = WidgetType.Generated;

            // RnD
            //Name = Path; 
            Name = string.Empty;
        }

        // InitializeSocial
        private void InitializeSocial()
        {
            WidgetComponent = new MosaicFriendWidget();
            WidgetType = WidgetType.Generated;
        }


        // Load
        public void Load()
        {
            if (WidgetType == WidgetType.Generated)
            {
                if (string.IsNullOrEmpty(Name))
                {
                    // simple load
                    WidgetComponent.Load(Path);

                }
                else
                {
                    //RnD
                    WidgetComponent.Load(Path, Name, Environment.TickCount * Row * Column);
                 
                }
            }
            else
            {
                try
                {
                    // RnD -- uncomment if needed
                    if (WidgetComponent.Name != "Music")
                    {
                        WidgetComponent.Load();
                    }
                }
                catch (Exception E5)
                {
                    System.Diagnostics.Debug.WriteLine(E5.Message);
                }
            }


            IsLoaded = true;
        }

        // Unload
        public void Unload()
        {
            if (WidgetComponent.Name != "Music")
            {
                WidgetComponent.Unload();
            }
            
            IsLoaded = false;

        }// Unload end
    }
}
