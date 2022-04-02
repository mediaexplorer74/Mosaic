using System;
using System.IO;
using System.Security.AccessControl;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Mosaic.Base
{
    public abstract class XmlSerializable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public virtual void Save(string path)
        {
            //var w = new StreamWriter(path);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                return;
            var w = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
            var s = new XmlSerializer(this.GetType());
            s.Serialize(w, this);
            w.Close();
        }

        // Загрузчик XML (из mosaic.config и не только)
        public static object Load(Type t, string path)
        {
            // если файл по пути path существует... 
            if (File.Exists(path))
            {
                // считываем потоки
                var sr = new StreamReader(path);
                var xr = new XmlTextReader(sr);
                var xs = new XmlSerializer(t);
                object result = null;
                try
                {
                    // делаем десериализацию
                    result = xs.Deserialize(xr);
                }
                catch (Exception ex)
                {
                    // возник сбой! пишем о нем в лог и сообщаем об ошибке на экран!
                    logger.Error("Can't read xml file: " + path + ". Deserialization failed.\n" + ex);
                    MessageBox.Show("Can't read xml file: " + path + ". See details in log.");
                }
                xr.Close();
                sr.Close();
                return result;
            }
            return null;
        }
    }
}
