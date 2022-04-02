// WidgetManager 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Mosaic.Base;

namespace Mosaic.Core
{
    // WidgetManager class 
    public class WidgetManager
    {
        //private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public delegate void WidgetLoadedEventHandler(WidgetProxy widget);
        public delegate void WidgetUnloadedEventHandler(WidgetProxy widget);
        public event WidgetLoadedEventHandler WidgetLoaded;
        public event WidgetUnloadedEventHandler WidgetUnloaded;

        public List<WidgetProxy> Widgets { get; private set; }

        public WidgetManager()
        {
            Widgets = new List<WidgetProxy>();
        }//WidgetManager end


        //FindWidgets : Функция НайтиВиджеты
        public void FindWidgets()
        {
            // Если нет папки  WidgetsRoot ("C:\\Users\\zaharov\\Documents\\Visual Studio 2015\\Projects\\Mosaic\\bin\\Debug\\Widgets")
            if (!Directory.Exists(E.WidgetsRoot))
                return; // то выходим из процедуры

            // на выходе массив вида [0] = "C:\\Users\\zaharov\\Documents\\Visual Studio 2015\\Projects\\Mosaic\\bin\\Debug\\Widgets\\Clock\\Clock.dll"
            var files = from x in Directory.GetDirectories(E.WidgetsRoot)
                        where File.Exists(x + "\\" + Path.GetFileNameWithoutExtension(x) + ".dll")
                        select x + "\\" + Path.GetFileNameWithoutExtension(x) + ".dll";

            // цикл по файликам
            foreach (var f in files)
            {
                var w = new WidgetProxy(f);

                //w.Load();

                // если ошибки, то обходим (но не завершаем цикл)
                if (w.HasErrors)
                    continue;
                Widgets.Add(w); // добавляем виджеты
            }

            // прибираемся... стираем *.bak - файлы
            foreach (var file in Directory.GetFiles(E.WidgetsRoot, "*.bak", SearchOption.AllDirectories))
            {
                File.Delete(file); 
            }

            //if (!Directory.Exists(E.AppsRoot))
            //    return;

            //var files = from x in Directory.GetDirectories(E.AppsRoot)
            //            where Directory.GetFiles(x, "*Widget.dll", SearchOption.AllDirectories).Length == 1
            //            /*File.Exists(x + "\\" + Path.GetFileNameWithoutExtension(x) + "\\Widget\\" + ".dll")*/
            //            select Directory.GetFiles(x, "*Widget.dll", SearchOption.AllDirectories).First();


            //foreach (var f in files)
            //{
            //    var w = new WidgetProxy(f);
            //    //w.Load();
            //    if (w.HasErrors)
            //        continue;
            //    Widgets.Add(w);
            //}

            // сортировочка имен виджетов
            Widgets = Widgets.OrderBy(x => x.Name).ToList();

        }//FindWidgets end 


        public bool IsWidgetLoaded(string name)
        {
            return Widgets.Where(widget => widget.Name == name).Select(widget => widget.IsLoaded).FirstOrDefault();
        }//IsWidgetLoaded end


        public bool HasWidget(string name)
        {
            return Widgets.Any(widget => widget.Name == name);
        }//HasWidget end


        // LoadWidget
        public void LoadWidget(string name)
        {
            foreach (var widget in Widgets.Where(widget => widget.Name == name))
            {
                //widget.Load();
                if (WidgetLoaded != null)
                    WidgetLoaded(widget);
            }

        }//LoadWidget end



        // LoadExternalWidget
        public void LoadExternalWidget(string file, int row = 0, int column = 0)
        {
            if (!File.Exists(file))
                return;

            var w = new WidgetProxy(file);

            if (w.HasErrors)
                return;

            Widgets.Add(w);

            w.Row = row;
            w.Column = column;

            w.Load();

            if (WidgetLoaded != null)
            {
                WidgetLoaded(w);
            }

        }//LoadExternalWidget end



        // LoadWidget(WidgetProxy widget)
        public void LoadWidget(WidgetProxy widget)
        {
            if (WidgetLoaded != null)
                WidgetLoaded(widget);

        }//LoadWidget(WidgetProxy widget) end



        // UnloadWidget(string name)
        public void UnloadWidget(string name)
        {
            foreach (var widget in Widgets.Where(widget => widget.Name == name))
            {
                widget.Unload();

                if (WidgetUnloaded != null)
                {
                    WidgetUnloaded(widget);
                }

                break;
            }
        }//UnloadWidget(string name) end


        // UnloadWidget(WidgetProxy widget)
        public void UnloadWidget(WidgetProxy widget)
        {

            if (widget.WidgetType == WidgetType.Generated)
            {
                //RnD
                //Widgets.Remove(widget);
            }
            
            
            widget.Unload();

            if (WidgetUnloaded != null)
            {
                WidgetUnloaded(widget);
            }

        }//UnloadWidget(WidgetProxy widget) end

        // CreateWidget
        public WidgetProxy CreateWidget(string url)
        {
            var widget = new WidgetProxy(url, null, true);

            Widgets.Add(widget);

            return widget;
        }//CreateWidget end


        // CreateFriendWidget
        public WidgetProxy CreateFriendWidget(string id, string name)
        {
            var widget = new WidgetProxy(id, name, false, true);

            Widgets.Add(widget);
            
            return widget;
        }//CreateFriendWidget end


        //puts widget from source folder to the Mosaic\Widgets folder
        public void InstallWidget(string source, string name)
        {
            if (!Directory.Exists(source))
                return;
            
            if (!Directory.Exists(E.WidgetsRoot + "\\" + name))
                Directory.CreateDirectory(E.WidgetsRoot + "\\" + name);
            
            foreach (var file in Directory.GetFiles(source))
            {
                File.Copy(file, E.WidgetsRoot + "\\" + name + "\\" + Path.GetFileName(file));
            }

            if (HasWidget(name))
                return;
            
            string widgetDll = E.WidgetsRoot + "\\" + name + "\\" + name + ".dll";
            
            if (File.Exists(widgetDll))
            {
                var w = new WidgetProxy(widgetDll);

                //w.Load();

                if (w.HasErrors)
                    return;

                Widgets.Add(w);
            }
        }//InstallWidget end


        // InstallWidgetFromZip
        public void InstallWidgetFromZip(string zipFile, string name)
        {
            if (!File.Exists(zipFile))
            {
                //logger.Info("Install widget from zip failed. File " + zipFile + " doesn't exists.");
                Debug.WriteLine("Install widget from zip failed. File " + zipFile + " doesn't exists.");
                return;
            }

            if (!Directory.Exists(E.WidgetsRoot + "\\" + name))
                Directory.CreateDirectory(E.WidgetsRoot + "\\" + name);
            
            PackageManager.Unpack(zipFile, E.WidgetsRoot + "\\" + name);
            
            string widgetDll = E.WidgetsRoot + "\\" + name + "\\" + name + ".dll";
            
            if (File.Exists(widgetDll))
            {
                var w = new WidgetProxy(widgetDll);
                
                //w.Load();
                
                if (w.HasErrors)
                    return;

                Widgets.Add(w);
            }
        }//InstallWidgetFromZip end


        public WidgetProxy GetWidgetByName(string name)
        {
            return Widgets.Find(widget => widget.Name == name);
        }//GetWidgetByName end

    }//WidgetManager class end

}// Mosaic.Core namespace end
