using Foundation;
using System;
using UIKit;
using MapClustering;
using MapKit;
using CoreLocation;
using System.Linq;

namespace MapClusteringQs
{
    public partial class MapViewController : UIViewController, ICCHMapClusterControllerDelegate, IMKMapViewDelegate
    {
        DataReader dataReader;
        Settings settings;
        CCHMapClusterController mapClusterControllerRed;
        CCHMapClusterController mapClusterControllerBlue;
        int count;
        ICCHMapClusterer mapClusterer;
        ICCHMapAnimator mapAnimator;

        public MapViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            mapClusterControllerRed = new CCHMapClusterController(mapView);
            mapClusterControllerRed.WeakDelegate = this;

            dataReader = new DataReader();
            dataReader.AnnotationsAvailable += DataReader_AnnotationsAvailable;

            resetSettings();
        }

        partial void resetSettings()
        {
            count = 0;
            var settings = new Settings();
            UpdateWithSettings(settings);
        }

        void UpdateWithSettings(Settings xsettings)
        {
            this.settings = xsettings;

            // Map cluster controller settings
            mapClusterControllerRed.DebuggingEnabled = settings.DebuggingEnabled;
            mapClusterControllerRed.CellSize = settings.CellSize;
            mapClusterControllerRed.MarginFactor = settings.MarginFactor;

            if (settings.SettingsClusterer == SettingsClusterer.CenterOfMass)
            {
                mapClusterer = new CCHCenterOfMassMapClusterer();// [[CCHCenterOfMassMapClusterer alloc] init];
            }
            else if (settings.SettingsClusterer == SettingsClusterer.NearCenter)
            {
                mapClusterer = new CCHNearCenterMapClusterer();// [[CCHNearCenterMapClusterer alloc] init];
            }

            mapClusterControllerRed.Clusterer = mapClusterer;
            mapClusterControllerRed.MaxZoomLevelForClustering = settings.MaxZoomLevelForClustering;
            mapClusterControllerRed.MinUniqueLocationsForClustering = settings.MinUniqueLocationsForClustering;

            if (settings.SettingsAnimator == SettingsAnimator.FadeInOut)
            {
                mapAnimator = new CCHFadeInOutMapAnimator();//[[CCHFadeInOutMapAnimator alloc] init];
            }
            mapClusterControllerRed.Animator = mapAnimator;

            // Similar settings for second cluster controller
            if (settings.GroupingEnabled)
            {
                if (mapClusterControllerBlue == null)
                {
                    mapClusterControllerBlue = new CCHMapClusterController(mapView);
                    mapClusterControllerBlue.WeakDelegate = this;
                }

                mapClusterControllerBlue.DebuggingEnabled = settings.DebuggingEnabled;
                mapClusterControllerBlue.CellSize = settings.CellSize + 20;
                mapClusterControllerBlue.MarginFactor = settings.MarginFactor;
                mapClusterControllerBlue.Clusterer = mapClusterer;
                mapClusterControllerBlue.MaxZoomLevelForClustering = settings.MaxZoomLevelForClustering;
                mapClusterControllerBlue.MinUniqueLocationsForClustering = settings.MinUniqueLocationsForClustering;
                mapClusterControllerBlue.Animator = mapAnimator;
            }
            else
            {
                mapClusterControllerBlue = null;
            }

            // Restart data reader
            count = 0;
            dataReader.StopReadingData();


            mapView.RemoveAnnotations(mapView.Annotations);

            if (mapView.Overlays != null)
            {
                foreach (var overlay in mapView.Overlays)
                {
                    mapView.RemoveOverlay(overlay);
                }
            }

            MKCoordinateRegion region;
            if (settings.SettingsDataSet == SettingsDataSet.Berlin)
            {
                // 5000+ items near Berlin
                var location = new CLLocationCoordinate2D(52.516221, 13.377829);
                region = MKCoordinateRegion.FromDistance(location, 45000, 45000);

                dataReader.StartReadingBerlinData();
            }
            else
            {
                // 80000+ items in the US
                var location = new CLLocationCoordinate2D(39.833333, -98.583333);
                region = MKCoordinateRegion.FromDistance(location, 7000000, 7000000);

                dataReader.StartReadingUSData();
            }
            mapView.Region = region;
        }

        [Export("mapClusterController:titleForMapClusterAnnotation:")]
        public string TitleForMapClusterAnnotation(CCHMapClusterController mapClusterController, CCHMapClusterAnnotation mapClusterAnnotation)
        {
            var annotationsCount = mapClusterAnnotation.Annotations.Count;
            var unit = annotationsCount > 1 ? "annotations" : "annotation";

            return $"{annotationsCount} {unit}";
        }

        [Export("mapClusterController:subtitleForMapClusterAnnotation:")]
        public string SubtitleForMapClusterAnnotation(CCHMapClusterController mapClusterController, CCHMapClusterAnnotation mapClusterAnnotation)
        {
            System.Diagnostics.Debug.WriteLine($"clusterAnnotation.Annotations.Count: {mapClusterAnnotation.Annotations.Count}");
            var numAnnotations = (int)Math.Min(mapClusterAnnotation.Annotations.Count, 5);
            var titleKey = new NSString("title");
            var annotations = mapClusterAnnotation.Annotations.Skip(0).Take(numAnnotations)
                                                  .Select(x => x.ValueForKey(titleKey));

            return string.Join(", ", annotations);
        }

        [Export("mapClusterController:willReuseMapClusterAnnotation:")]
        public void WillReuseMapClusterAnnotation(CCHMapClusterController mapClusterController, CCHMapClusterAnnotation mapClusterAnnotation)
        {
            var clusterAnnotationView = (ClusterAnnotationView)mapView.ViewForAnnotation(mapClusterAnnotation);

            if (clusterAnnotationView == null) return;

            System.Diagnostics.Debug.WriteLine($"WillReuseMapClusterAnnotation: {mapClusterAnnotation.Annotations.Count}");
            clusterAnnotationView.Count = (uint)mapClusterAnnotation.Annotations.Count;
            clusterAnnotationView.UniqueLocation = mapClusterAnnotation.IsUniqueLocation;
        }

        [Export("mapView:viewForAnnotation:")]
        public MKAnnotationView GetViewForAnnotation(MKMapView mapView, NSObject annotation)
        {
            MKAnnotationView annotationView = null;

            System.Diagnostics.Debug.WriteLine($"{annotation.GetType().FullName}");
            if (annotation is CCHMapClusterAnnotation clusterAnnotation)
            {
                var identifier = @"clusterAnnotation";

                var clusterAnnotationView = (ClusterAnnotationView)mapView.DequeueReusableAnnotation(identifier);

                if (clusterAnnotationView != null)
                {
                    clusterAnnotationView.Annotation = clusterAnnotation;
                }
                else
                {
                    clusterAnnotationView = new ClusterAnnotationView(clusterAnnotation, identifier)
                    {
                        CanShowCallout = true
                    };
                }

                System.Diagnostics.Debug.WriteLine($"ClusterAnnotationView: {clusterAnnotation.Annotations.Count}");
                clusterAnnotationView.Count = (uint)clusterAnnotation.Annotations.Count;
                clusterAnnotationView.IsBlue = clusterAnnotation.MapClusterController == mapClusterControllerBlue;
                clusterAnnotationView.UniqueLocation = clusterAnnotation.IsUniqueLocation;
                annotationView = clusterAnnotationView;
            }

            return annotationView;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier != "mapToSettings")
            {
                return;
            }

            var navigationViewController = (UINavigationController)segue.DestinationViewController;
            var settingsViewController = (SettingsViewController)navigationViewController.TopViewController;
            settingsViewController.Settings = settings;
            settingsViewController.Completed += (s, settings) =>
            {
                UpdateWithSettings(settings);
            };
        }

        void DataReader_AnnotationsAvailable(object sender, MapKit.MKPointAnnotation[] annotations)
        {
            if (settings.GroupingEnabled)
            {
                if (count++ % 2 == 0)
                {

                    mapClusterControllerRed.AddAnnotations(annotations, null);
                }
                else
                {
                    mapClusterControllerBlue.AddAnnotations(annotations, null);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Annotations: {annotations.Length}");

                mapClusterControllerRed.AddAnnotations(annotations, null);
            }
        }
    }
}