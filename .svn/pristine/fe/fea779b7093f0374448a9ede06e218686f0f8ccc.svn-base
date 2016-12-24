using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using BeaconPoc.BeaconPocService;

namespace BeaconPoc
{
    internal class AppHandler
    {
        private readonly Context _context;

        internal AppHandler(Context context)
        {
            _context = context;
        }

        internal bool Install(AppInfo appInfo)
        {
            var appPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory + "/download/", appInfo.ApkFilename);

            try
            {
                var setupIntent = new Intent(Intent.ActionView);
                setupIntent.SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(appPath)), "application/vnd.android.package-archive");
                _context.StartActivity(setupIntent);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal bool StartApp(AppInfo appInfo)
        {
            try
            {
                var intent = _context.PackageManager.GetLaunchIntentForPackage(appInfo.AppIdentifier);
                _context.StartActivity(intent);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal List<string> GetPackageNames()
        {
            return (from package in _context.PackageManager.GetInstalledApplications(PackageInfoFlags.MetaData)
                    where _context.PackageManager.GetLaunchIntentForPackage(package.PackageName) != null
                    select package.PackageName).ToList();
        }

        internal bool IsAppRunning(AppInfo appInfo)
        {
            var activityManager = (ActivityManager)_context.GetSystemService(Context.ActivityService);
            return activityManager.RunningAppProcesses.Any(p => p.ProcessName.Equals(appInfo.AppIdentifier));
        }

        internal bool IsPackageInstalled(AppInfo appInfo)
        {
            var packageManager = _context.PackageManager;
            var intent = packageManager.GetLaunchIntentForPackage(appInfo.AppIdentifier);
            if (intent == null)
            {
                return false;
            }
            IList<ResolveInfo> list = packageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return list.Count > 0;
        }

        internal bool KillApp(AppInfo appInfo)
        {
            var activityManager = (ActivityManager)_context.GetSystemService(Context.ActivityService);
            activityManager.KillBackgroundProcesses(appInfo.AppIdentifier);  
            return true;
        }
    }
}