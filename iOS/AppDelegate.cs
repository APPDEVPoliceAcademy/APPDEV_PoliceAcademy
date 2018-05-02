using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using UIKit;

namespace WorkshopScheduler.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            UITabBar.Appearance.SelectedImageTintColor = UIColor.Red;
            UISwitch.Appearance.OnTintColor = UIColor.Red;
            UIButton.Appearance.SetTitleColor(UIColor.Red,UIControlState.Normal);

            // Code for starting up the Xamarin Test Cloud Agent
#if DEBUG
			Xamarin.Calabash.Start();
#endif

            LoadApplication(new App());

            CrossDownloadManager.Current.PathNameForDownloadedFile = new System.Func<IDownloadFile, string>(file => {
                string fileName = (new NSUrl(file.Url, false)).LastPathComponent;
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            });

            return base.FinishedLaunching(app, options);
        }

        public override void HandleEventsForBackgroundUrl(UIApplication application, string sessionIdentifier, Action completionHandler)
        {
            CrossDownloadManager.BackgroundSessionCompletionHandler = completionHandler;
        }
    }
}
