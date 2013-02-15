// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Windows.Storage;

namespace Kona.UILogic.Services
{
    public class TemporaryFolderCacheService : ICacheService
    {
        private static readonly StorageFolder _cacheFolder = ApplicationData.Current.TemporaryFolder;
        private static TimeSpan _expirationPolicy = new TimeSpan(0, 5, 0); // 5 minutes
        private IRequestService _requestService;

        public TemporaryFolderCacheService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        // <snippet515>
        public async Task<bool> DataExistsAndIsValidAsync(string cacheKey)
        {
            try
            {
                StorageFile file = await _cacheFolder.GetFileAsync(cacheKey);

                // Check if the file has expired
                var fileBasicProperties = await file.GetBasicPropertiesAsync();
                var expirationDate = fileBasicProperties.DateModified.Add(_expirationPolicy).DateTime;
                bool fileIsValid = DateTime.Now.CompareTo(expirationDate) < 0;

                return fileIsValid;
            }
            catch (FileNotFoundException)
            {
                // File does not exist
                return false;
            }
        }
        // </snippet515>

        public async Task<T> GetDataAsync<T>(string cacheKey)
        {
            StorageFile file = await _cacheFolder.GetFileAsync(cacheKey);

            if (file == null) throw new Exception("File does not exist");

            string text = await FileIO.ReadTextAsync(file);
            var toReturn = Deserialize<T>(text);

            return toReturn;
        }

        public async Task<Uri> SaveDataAsync<T>(string cacheKey, T content)
        {
            StorageFile file = await _cacheFolder.CreateFileAsync(cacheKey, CreationCollisionOption.ReplaceExisting);

            var textContent = Serialize<T>(content);
            await FileIO.WriteTextAsync(file, textContent);

            return new Uri(file.Path);
        }

        // <snippet516>
        public async Task<Uri> SaveExternalDataAsync(string cacheKey, Uri dataUrl)
        {
            StorageFile file = await _cacheFolder.CreateFileAsync(cacheKey, CreationCollisionOption.ReplaceExisting);
            Uri fullUrl = new Uri(new Uri(Constants.ServerAddress), dataUrl);
            var resourceBytes = await this._requestService.GetExternalResourceAsync(fullUrl);

            await FileIO.WriteBytesAsync(file, resourceBytes);
            
            return new Uri(file.Path, UriKind.Absolute);
        }
        // </snippet516>

        private static T Deserialize<T>(string json)
        {
            var jsonBytes = Encoding.Unicode.GetBytes(json);
            using (var jsonStream = new MemoryStream(jsonBytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var toReturn = (T)serializer.ReadObject(jsonStream);

                return toReturn;
            }
        }

        private static string Serialize<T>(T entity)
        {
            var stream = new MemoryStream();
            StreamReader streamReader = null;
            try
            {
                var serializer = new DataContractJsonSerializer(entity.GetType());
                serializer.WriteObject(stream, entity);

                // Rewind the stream
                stream.Seek(0, SeekOrigin.Begin);

                streamReader = new StreamReader(stream);
                return streamReader.ReadToEnd();
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Dispose();
                }
                else
                {
                    stream.Dispose();
                }
            }

        }
    }
}
