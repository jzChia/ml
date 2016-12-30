using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace MLCommon
{
    public class xmlSerialize
    {
        public static void Serialize<T>(T o, string filePath)
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                StreamWriter sw = new StreamWriter(filePath, false);
                formatter.Serialize(sw, o);
                sw.Flush();
                sw.Close();
            }
            catch (Exception) { }

        }

        public static string Serializer( object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(obj.GetType());
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        public static T DeSerializestr<T>(string s)
        {
            try
            {
                byte[] array = Encoding.UTF8.GetBytes(s);
                MemoryStream stream = new MemoryStream(array);
                StreamReader reader = new StreamReader(stream);
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                T o = (T)formatter.Deserialize(reader);
                reader.Close();
                return o;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return default(T);
        }

        public static T DeSerialize<T>(string filePath)
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("utf-8"));
                T o = (T)formatter.Deserialize(sr);
                sr.Close();
                return o;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return default(T);
        }
    }
}

