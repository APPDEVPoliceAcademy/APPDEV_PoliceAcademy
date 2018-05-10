using System;
using System.IO;
using Android.App;
using Android.Content;
using WorkshopScheduler.Droid;
using WorkshopScheduler.Logic;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileDownloader))]

namespace WorkshopScheduler.Droid
{
    public class FileDownloader : IFileDownloader
    {
        public bool DownloadFile(string uri)
        {
            
            var manager = (DownloadManager)Android.App.Application.Context.GetSystemService(Context.DownloadService);
            var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, "Workshops");
            var path = Path.Combine(directoryname, "file.mp4");
            var req = new DownloadManager.Request(Android.Net.Uri.Parse(uri));
            req.SetDestinationInExternalFilesDir(Android.App.Application.Context, null, path);
            var ret = manager.Enqueue(req);
            return true;
        }
    }

}