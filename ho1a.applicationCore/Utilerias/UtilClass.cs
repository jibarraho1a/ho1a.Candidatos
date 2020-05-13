using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ho1a.applicationCore.Utilerias
{
    public static class UtilClass
    {
        public static T DeepClone<T>(T obj)
        {
            if (ReferenceEquals(obj, null))
            {
                throw new Exception("The source object must not be null");
            }

            var result = default(T);

            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                formatter.Serialize(memoryStream, obj);

                memoryStream.Seek(0, SeekOrigin.Begin);

                result = (T)formatter.Deserialize(memoryStream);

                memoryStream.Close();
            }

            return result;
        }
    }
}
