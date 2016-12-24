using System;
using System.IO;
using System.Net;
using BeaconPoc.BeaconPocService;

namespace BeaconPoc
{
    internal class DownloadHandler
    {
        private readonly string _downloadUrl;
        public delegate void DovnloadDataCompletedEventHandler(bool success, AppInfo appInfo);
        public event DovnloadDataCompletedEventHandler DovnloadDataCompleted;

        public DownloadHandler(string downloadUrl)
        {
            _downloadUrl = downloadUrl;
        }

        internal void Download(AppInfo appInfo)
        {
            var appPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory + "/download/", appInfo.ApkFilename);

            var webClient = new WebClient();
            
            webClient.DownloadDataCompleted += (s, e) =>
            {
                try
                {
                    var bytes = e.Result;

                    File.WriteAllBytes(appPath, bytes);

                    DovnloadDataCompleted?.Invoke(true, appInfo);
                }
                catch (Exception )
                {
                    // ToDo: Implement proper error handling
                    throw;
                }
                finally
                {
                    webClient.Dispose();
                }
            };

            try
            {
                webClient.DownloadDataAsync(new Uri(Path.Combine(_downloadUrl, appInfo.ApkFilename)));
            }
            catch (Exception){

                // ToDo: Implement proper error handling
                throw;
            }
        }
    }
}