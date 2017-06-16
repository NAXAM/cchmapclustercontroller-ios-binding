using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreFoundation;
using Foundation;
using MapKit;

namespace MapClusteringQs
{
    public class DataReader
    {
        public event EventHandler<MKPointAnnotation[]> AnnotationsAvailable;

        readonly int BATCH_COUNT = 500;
        readonly float DELAY_BETWEEN_BATCHES = 0.3f;

        NSOperationQueue operationQueue;

        public DataReader()
        {
            operationQueue = new NSOperationQueue
            {
                MaxConcurrentOperationCount = 1
            };
        }

        public void StartReadingBerlinData()
        {
            DispatchQueue.GetGlobalQueue(DispatchQueuePriority.Low).DispatchAsync(() =>
            {
                var file = NSBundle.MainBundle.PathForResource(@"Berlin-Data", "json");
                var inputStream = new NSInputStream(file);
                inputStream.Open();

                NSError error;
                var dataAsJson = (NSArray) NSJsonSerialization.Deserialize(inputStream, 0, out error);
                var annotations = new List<(double lat, double lon, string title)>(BATCH_COUNT);

                for (uint i = 0; i < dataAsJson.Count; i++)
                {
                    var annotationAsJson =  dataAsJson.GetItem<NSDictionary>(i);

                    var lat = (NSNumber)annotationAsJson.ValueForKeyPath(new NSString("location.coordinates.latitude"));
                    var lon = (NSNumber)annotationAsJson.ValueForKeyPath(new NSString("location.coordinates.longitude"));
                    var title = (NSString)annotationAsJson.ValueForKeyPath(new NSString("person.lastName"));

                    annotations.Add((lat.DoubleValue, lon.DoubleValue, title.ToString()));

                    if (annotations.Count == BATCH_COUNT) {
                        DispatchAnnotations(annotations.ToArray());
                        annotations.Clear();
                    }
                }

                DispatchAnnotations(annotations.ToArray());
            });
        }

        public void StartReadingUSData()
        {
            DispatchQueue.GetGlobalQueue(DispatchQueuePriority.Low).DispatchAsync(() =>
            {
                var file = NSBundle.MainBundle.PathForResource(@"USA-HotelMotel", "csv");
                var lines = File.ReadAllLines(file);
                var annotations = new List<(double lat, double lon, string title)>(BATCH_COUNT);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i]))
                    {
                        continue;
                    }
                    var line = lines[i].Trim();
                    var components = line.Split(',');
                    var lat = double.Parse(components[1]);
                    var lon = double.Parse(components[0]);

                    annotations.Add((lat, lon, components[2]));

                    if (annotations.Count == BATCH_COUNT)
                    {
                        DispatchAnnotations(annotations.ToArray());
                        annotations.Clear();
                    }
                }

                DispatchAnnotations(annotations.ToArray());
            });
        }

        public void StopReadingData () {
            operationQueue.CancelAllOperations();
        }

        void DispatchAnnotations(IEnumerable<(double lat, double lon, string title)> annotations) {

            operationQueue.AddOperation(() => {
				DispatchQueue.MainQueue.DispatchSync(() =>
				{
					var temp = annotations
						.Select(x => new MKPointAnnotation
						{
							Coordinate = new CoreLocation.CLLocationCoordinate2D(x.lat, x.lon),
							Title = x.title
						})
						.ToArray();
                    AnnotationsAvailable?.Invoke(this, temp);
                });

                NSThread.SleepFor(DELAY_BETWEEN_BATCHES);
            });
        }
    }
}
