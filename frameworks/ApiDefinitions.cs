using System;
using CCHMapClusterController;
using CoreLocation;
using Foundation;
using MapKit;
using ObjCRuntime;

namespace MapClustering
{
	// @protocol CCHMapClusterer
	[Protocol, Model]
	interface CCHMapClusterer
	{
		// @required -(CLLocationCoordinate2D)mapClusterController:(CCHMapClusterController *)mapClusterController coordinateForAnnotations:(NSSet *)annotations inMapRect:(MKMapRect)mapRect;
		[Abstract]
		[Export ("mapClusterController:coordinateForAnnotations:inMapRect:")]
		CLLocationCoordinate2D CoordinateForAnnotations (CCHMapClusterController mapClusterController, NSSet annotations, MKMapRect mapRect);
	}

	// @interface CCHCenterOfMassMapClusterer : NSObject <CCHMapClusterer>
	[BaseType (typeof(NSObject))]
	interface CCHCenterOfMassMapClusterer : ICCHMapClusterer
	{
		// -(CLLocationCoordinate2D)mapClusterController:(CCHMapClusterController *)mapClusterController coordinateForAnnotations:(NSSet *)annotations inMapRect:(MKMapRect)mapRect;
		[Export ("mapClusterController:coordinateForAnnotations:inMapRect:")]
		CLLocationCoordinate2D MapClusterController (CCHMapClusterController mapClusterController, NSSet annotations, MKMapRect mapRect);
	}

	// @protocol CCHMapAnimator <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface CCHMapAnimator
	{
		// @required -(void)mapClusterController:(CCHMapClusterController *)mapClusterController didAddAnnotationViews:(NSArray *)annotationViews;
		[Abstract]
		[Export ("mapClusterController:didAddAnnotationViews:")]
		[Verify (StronglyTypedNSArray)]
		void DidAddAnnotationViews (CCHMapClusterController mapClusterController, NSObject[] annotationViews);

		// @required -(void)mapClusterController:(CCHMapClusterController *)mapClusterController willRemoveAnnotations:(NSArray *)annotations withCompletionHandler:(void (^)())completionHandler;
		[Abstract]
		[Export ("mapClusterController:willRemoveAnnotations:withCompletionHandler:")]
		[Verify (StronglyTypedNSArray)]
		void WillRemoveAnnotations (CCHMapClusterController mapClusterController, NSObject[] annotations, Action completionHandler);
	}

	// @interface CCHFadeInOutMapAnimator : NSObject <CCHMapAnimator>
	[BaseType (typeof(NSObject))]
	interface CCHFadeInOutMapAnimator : ICCHMapAnimator
	{
		// @property (assign, nonatomic) NSTimeInterval duration;
		[Export ("duration")]
		double Duration { get; set; }

		// -(void)mapClusterController:(CCHMapClusterController *)mapClusterController didAddAnnotationViews:(NSArray *)annotationViews;
		[Export ("mapClusterController:didAddAnnotationViews:")]
		[Verify (StronglyTypedNSArray)]
		void MapClusterController (CCHMapClusterController mapClusterController, NSObject[] annotationViews);

		// -(void)mapClusterController:(CCHMapClusterController *)mapClusterController willRemoveAnnotations:(NSArray *)annotations withCompletionHandler:(void (^)())completionHandler;
		[Export ("mapClusterController:willRemoveAnnotations:withCompletionHandler:")]
		[Verify (StronglyTypedNSArray)]
		void MapClusterController (CCHMapClusterController mapClusterController, NSObject[] annotations, Action completionHandler);
	}

	// @interface CCHMapClusterAnnotation : NSObject <MKAnnotation>
	[BaseType (typeof(NSObject))]
	interface CCHMapClusterAnnotation : IMKAnnotation
	{
		// @property (nonatomic, weak) CCHMapClusterController * mapClusterController;
		[Export ("mapClusterController", ArgumentSemantic.Weak)]
		CCHMapClusterController MapClusterController { get; set; }

		// @property (copy, nonatomic) NSString * title;
		[Export ("title")]
		string Title { get; set; }

		// @property (copy, nonatomic) NSString * subtitle;
		[Export ("subtitle")]
		string Subtitle { get; set; }

		// @property (nonatomic) CLLocationCoordinate2D coordinate;
		[Export ("coordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D Coordinate { get; set; }

		[Wrap ("WeakDelegate")]
		CCHMapClusterControllerDelegate Delegate { get; set; }

		// @property (nonatomic, weak) id<CCHMapClusterControllerDelegate> delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		// @property (copy, nonatomic) NSSet * annotations;
		[Export ("annotations", ArgumentSemantic.Copy)]
		NSSet Annotations { get; set; }

		// -(BOOL)isCluster;
		[Export ("isCluster")]
		[Verify (MethodToProperty)]
		bool IsCluster { get; }

		// -(BOOL)isUniqueLocation;
		[Export ("isUniqueLocation")]
		[Verify (MethodToProperty)]
		bool IsUniqueLocation { get; }

		// -(BOOL)isOneLocation __attribute__((deprecated("")));
		[Export ("isOneLocation")]
		[Verify (MethodToProperty)]
		bool IsOneLocation { get; }

		// -(MKMapRect)mapRect;
		[Export ("mapRect")]
		[Verify (MethodToProperty)]
		MKMapRect MapRect { get; }
	}

	// @interface CCHMapClusterController : NSObject
	[BaseType (typeof(NSObject))]
	interface CCHMapClusterController
	{
		// @property (readonly, copy, nonatomic) NSSet * annotations;
		[Export ("annotations", ArgumentSemantic.Copy)]
		NSSet Annotations { get; }

		// @property (readonly, nonatomic) MKMapView * mapView;
		[Export ("mapView")]
		MKMapView MapView { get; }

		// @property (nonatomic) double marginFactor;
		[Export ("marginFactor")]
		double MarginFactor { get; set; }

		// @property (nonatomic) double cellSize;
		[Export ("cellSize")]
		double CellSize { get; set; }

		// @property (readonly, nonatomic) double zoomLevel;
		[Export ("zoomLevel")]
		double ZoomLevel { get; }

		// @property (nonatomic) double maxZoomLevelForClustering;
		[Export ("maxZoomLevelForClustering")]
		double MaxZoomLevelForClustering { get; set; }

		// @property (nonatomic) NSUInteger minUniqueLocationsForClustering;
		[Export ("minUniqueLocationsForClustering")]
		nuint MinUniqueLocationsForClustering { get; set; }

		[Wrap ("WeakDelegate")]
		CCHMapClusterControllerDelegate Delegate { get; set; }

		// @property (nonatomic, weak) id<CCHMapClusterControllerDelegate> delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		// @property (nonatomic, weak) id<CCHMapClusterer> clusterer;
		[Export ("clusterer", ArgumentSemantic.Weak)]
		CCHMapClusterer Clusterer { get; set; }

		// @property (nonatomic) BOOL reuseExistingClusterAnnotations;
		[Export ("reuseExistingClusterAnnotations")]
		bool ReuseExistingClusterAnnotations { get; set; }

		// @property (nonatomic, weak) id<CCHMapAnimator> animator;
		[Export ("animator", ArgumentSemantic.Weak)]
		CCHMapAnimator Animator { get; set; }

		// @property (getter = isDebuggingEnabled, nonatomic) BOOL debuggingEnabled;
		[Export ("debuggingEnabled")]
		bool DebuggingEnabled { [Bind ("isDebuggingEnabled")] get; set; }

		// -(instancetype)initWithMapView:(MKMapView *)mapView;
		[Export ("initWithMapView:")]
		IntPtr Constructor (MKMapView mapView);

		// -(void)addAnnotations:(NSArray *)annotations withCompletionHandler:(void (^)())completionHandler;
		[Export ("addAnnotations:withCompletionHandler:")]
		[Verify (StronglyTypedNSArray)]
		void AddAnnotations (NSObject[] annotations, Action completionHandler);

		// -(void)removeAnnotations:(NSArray *)annotations withCompletionHandler:(void (^)())completionHandler;
		[Export ("removeAnnotations:withCompletionHandler:")]
		[Verify (StronglyTypedNSArray)]
		void RemoveAnnotations (NSObject[] annotations, Action completionHandler);

		// -(void)selectAnnotation:(id<MKAnnotation>)annotation andZoomToRegionWithLatitudinalMeters:(CLLocationDistance)latitudinalMeters longitudinalMeters:(CLLocationDistance)longitudinalMeters;
		[Export ("selectAnnotation:andZoomToRegionWithLatitudinalMeters:longitudinalMeters:")]
		void SelectAnnotation (MKAnnotation annotation, double latitudinalMeters, double longitudinalMeters);
	}

	// @protocol CCHMapClusterControllerDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface CCHMapClusterControllerDelegate
	{
		// @optional -(NSString *)mapClusterController:(CCHMapClusterController *)mapClusterController titleForMapClusterAnnotation:(CCHMapClusterAnnotation *)mapClusterAnnotation;
		[Export ("mapClusterController:titleForMapClusterAnnotation:")]
		string MapClusterController (CCHMapClusterController mapClusterController, CCHMapClusterAnnotation mapClusterAnnotation);

		// @optional -(NSString *)mapClusterController:(CCHMapClusterController *)mapClusterController subtitleForMapClusterAnnotation:(CCHMapClusterAnnotation *)mapClusterAnnotation;
		[Export ("mapClusterController:subtitleForMapClusterAnnotation:")]
		string MapClusterController (CCHMapClusterController mapClusterController, CCHMapClusterAnnotation mapClusterAnnotation);

		// @optional -(void)mapClusterController:(CCHMapClusterController *)mapClusterController willReuseMapClusterAnnotation:(CCHMapClusterAnnotation *)mapClusterAnnotation;
		[Export ("mapClusterController:willReuseMapClusterAnnotation:")]
		void MapClusterController (CCHMapClusterController mapClusterController, CCHMapClusterAnnotation mapClusterAnnotation);
	}

	// @interface CCHNearCenterMapClusterer : NSObject <CCHMapClusterer>
	[BaseType (typeof(NSObject))]
	interface CCHNearCenterMapClusterer : ICCHMapClusterer
	{
		// -(CLLocationCoordinate2D)mapClusterController:(CCHMapClusterController *)mapClusterController coordinateForAnnotations:(NSSet *)annotations inMapRect:(MKMapRect)mapRect;
		[Export ("mapClusterController:coordinateForAnnotations:inMapRect:")]
		CLLocationCoordinate2D MapClusterController (CCHMapClusterController mapClusterController, NSSet annotations, MKMapRect mapRect);
	}

	[Static]
	[Verify (ConstantsInterfaceAssociation)]
	partial interface Constants
	{
		// extern double CCHMapClusterControllerVersionNumber;
		[Field ("CCHMapClusterControllerVersionNumber", "__Internal")]
		double CCHMapClusterControllerVersionNumber { get; }

		// extern const unsigned char [] CCHMapClusterControllerVersionString;
		[Field ("CCHMapClusterControllerVersionString", "__Internal")]
		byte[] CCHMapClusterControllerVersionString { get; }
	}
}
