using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;

namespace InventoryManagement.Helpers
{
    public class Settings<T>
    {  
        private XmlSerializer serializer;
        public Settings()
        {
            serializer = new XmlSerializer(typeof(T));
        }
        private string FileName(T Obj, string Handle)
        {
            var str = String.Concat(Handle, String.Format("{0}", Obj.GetType().ToString()));
            return str;
        }

        public async Task SaveAsync(string Key, T Obj)
        {
            string fileName = FileName(Activator.CreateInstance<T>(), Key);
            try
            {
                if (Obj != null)
                {
                    StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                    using (Stream outStream = Task.Run(() => writeStream.AsStreamForWrite()).Result)
                    {
                        serializer.Serialize(outStream, Obj);
                        await outStream.FlushAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete a stored instance of T from Windows.Storage.ApplicationData
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
            string fileName = FileName(Activator.CreateInstance<T>(), String.Empty);
            await DeleteAsync(fileName);
        }

        /// <summary>
        /// Delete a stored instance of T with a specified handle from Windows.Storage.ApplicationData.
        /// Specification of a handle supports storage and deletion of different instances of T.
        /// </summary>
        /// <param name="Handle">User-defined handle for the stored object</param>
        public async Task DeleteAsync(string Key)
        {
            if (Key == null)
                throw new ArgumentNullException("Handle");
            string fileName = FileName(Activator.CreateInstance<T>(), Key);
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                if (file != null)
                {
                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    
        /// <summary>
        /// Retrieve a stored instance of T with a specified handle from Windows.Storage.ApplicationData.
        /// Specification of a handle supports storage and deletion of different instances of T.
        /// </summary>
        /// <param name="Handle">User-defined handle for the stored object</param>
        public async Task<T> LoadAsync(string Key)
        {
            string fileName = FileName(Activator.CreateInstance<T>(), Key);

            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);
                using (Stream inStream = Task.Run(() => readStream.AsStreamForRead()).Result)
                {
                    return (T)serializer.Deserialize(inStream);
                }
            }
            catch (FileNotFoundException)
            {
                //file not existing is perfectly valid so simply return the default 
                return default(T);
                //Interesting thread here: How to detect if a file exists (http://social.msdn.microsoft.com/Forums/en-US/winappswithcsharp/thread/1eb71a80-c59c-4146-aeb6-fefd69f4b4bb)
                //throw;
            }
            catch (Exception)
            {
                //Unable to load contents of file
                throw;
            }
        }
    }
}
