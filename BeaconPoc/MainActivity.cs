﻿using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using BeaconPoc.BeaconPocService;
using RadiusNetworks.IBeaconAndroid;

namespace BeaconPoc
{
    [Activity(Label = "BeaconPoc", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IBeaconConsumer
    {
        private const string Uuid = "60ABF5CD-9D72-4524-824E-27031F5E6F76";
        private const string BeaconId = "myBeacon";
        private const string DownloadUrl = "http://ax2012r3-demo-krissjessen2-2caf9e58513e877c.cloudapp.net/";

        private readonly IBeaconManager _beaconManager;
        private readonly BeaconHandler _beaconHandler;
        private readonly ServiceHandler _serviceHandler;
        private readonly DownloadHandler _downloadHandler;
        private readonly AppHandler _appHandler;

        private ListView _resultListView;
        private ArrayAdapter _adapter;
        private Button _startButton;
        private string _savedStatus;

        public MainActivity()
        {
            _beaconManager = IBeaconManager.GetInstanceForApplication(this);

            _beaconHandler = new BeaconHandler(_beaconManager, Uuid, BeaconId);
            _beaconHandler.Alert += (alert) => DisplayStatus(alert);
            _beaconHandler.OnRangingBeaconsInRegion += Main;

            _serviceHandler = new ServiceHandler();

            _appHandler = new AppHandler(this);

            _downloadHandler = new DownloadHandler(DownloadUrl);
            _downloadHandler.DovnloadDataCompleted += DownloadHandlerDataCompleted;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _startButton = FindViewById<Button>(Resource.Id.StartButton);
            _startButton.Text = "Start";
            _startButton.Click += (sender, e) => Listen();

            _resultListView = FindViewById<ListView>(Resource.Id.listView1);
            _adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, new List<string>());
            _resultListView.Adapter = _adapter;

            _beaconManager.Bind(this);
        }

        #region IBeaconConsumer members

        public void OnIBeaconServiceConnect()
        {
            _beaconHandler.BeaconManagerConnect();
        }

        protected override void OnPause()
        {
            _beaconHandler.OnPause(this);
            base.OnPause();
        }

        protected override void OnResume()
        {
            _beaconHandler.OnResume(this);
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            _beaconHandler.OnDestroy(this);
            base.OnDestroy();
        }

        #endregion

        private void Listen()
        {
            ClearStatus();

            if (_beaconHandler.Listening)
            {
                _startButton.Text = "Press to listen for Beacons";
                DisplayStatus("Listening stopped", true);
                _beaconHandler.Listen(false);
            }
            else
            {
                _startButton.Text = "Press to stop listening";
                DisplayStatus("Listening started", true);
                _savedStatus = "";
                _beaconHandler.Listen(true);
            }
        }

        private void Main(BeaconInfo beaconInfo)
        {
            // We don't want to handle the same Beacon more than once (testing only for 21000!)
            if (_beaconHandler.IsKnownBeacon(beaconInfo)) return;

            DisplayStatus($"Found Beacon {beaconInfo.Major}/{beaconInfo.Minor}");

            // Get information from webservice depending on the beacon info received
            var appInfo = _serviceHandler.GetAppInfo(beaconInfo);

            DisplayStatus($"Webservice says: {appInfo.AppIdentifier}");

            // Failure. The 'Comment' field in appInfo holds the error message
            if (appInfo.Id == "-1")
            {
                DisplayStatus(appInfo.Comment);
                return;
            }

            // Check if app is already running
            if (_appHandler.IsAppRunning(appInfo))
            {
                DisplayStatus("App is currently running");
                MainContinued(appInfo);
            }
            else
            {
                DisplayStatus("App is not currently running");

            }

            // Check if the app is already installed
            if (_appHandler.IsPackageInstalled(appInfo))
            {
                DisplayStatus("App is currently installed");
                MainContinued(appInfo);
            }
            else
            {
                DisplayStatus("App is not currently installed");
                DisplayStatus("Starting download from webservice. Please be patient...");

                // Download the app from AppStorage and save it locally
                _downloadHandler.Download(appInfo);
            }
        }

        private void MainContinued(AppInfo appInfo)
        {
            DisplayStatus("Starting app");

            // Start the app
            var success = _appHandler.StartApp(appInfo);

            if (success == false)
            {
                DisplayStatus($"Unable to start {appInfo.ApkFilename}");
                return;
            }

            DisplayStatus("App was started with success");
        }

        private void DownloadHandlerDataCompleted(bool success, AppInfo appInfo)
        {
            if (success == false)
            {
                DisplayStatus($"Download of {appInfo.ApkFilename} failed");
                return;
            }

            DisplayStatus($"{appInfo.ApkFilename} was downloaded with success");

            DisplayStatus("Starting installation");

            // Install the app
            success = _appHandler.Install(appInfo);

            if (success == false)
            {
                DisplayStatus($"Installation of {appInfo.ApkFilename} failed");
                return;
            }

            DisplayStatus($"Successfully installed {appInfo.ApkFilename}");

            MainContinued(appInfo);
        }

        internal void DisplayStatus(string status, bool clear = false)
        {
            RunOnUiThread(() =>
            {
                string newStatus;
                if ((_savedStatus != null) && (status == _savedStatus.TrimEnd('.')))
                {
                    newStatus = _savedStatus.Length < 40 ? _savedStatus + "." : _savedStatus.TrimEnd('.');
                    _adapter.Remove(_adapter.GetItem(_adapter.Count - 1));
                    _adapter.NotifyDataSetChanged();
                }
                else
                {
                    newStatus = status;
                }
                if (clear) ClearStatus();
                _adapter.Add(newStatus);
                _savedStatus = newStatus;
                _adapter.NotifyDataSetChanged();
                _resultListView.SetSelection(_adapter.Count);
            });
        }

        private void ClearStatus()
        {
            _adapter.Clear();
            _adapter.NotifyDataSetChanged();
        }
    }
}

