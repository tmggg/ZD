using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SgS.Model;

namespace SgS.Helper
{
     public class XmlReadWriter
    {
        public static void SaveToFile<T>(T item,string dataPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            try
            {
                Directory.CreateDirectory(Directory.GetParent(dataPath).FullName);
                using (var sw = new StreamWriter(dataPath))
                {
                    serializer.Serialize(sw, item);
                    sw.Close();
                }
            }
            catch (Exception )
            {

            }
        }

        public static ObservableCollection<T> LoadFromFile<T>(string dataPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<T>));

            try
            {
                using (Stream reader = new FileStream(dataPath, FileMode.Open))
                {
                    // Call the Deserialize method to restore the object's state.
                    return (ObservableCollection<T>)serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                return new ObservableCollection<T>();
            }
        }

    }
}
