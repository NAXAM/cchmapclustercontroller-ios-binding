// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MapClusteringQs
{
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem btnCancel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell cellSizeTableViewCell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell debuggingEnabledTableViewCell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell groupingEnabledTableViewCell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell marginFactorTableViewCell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell maxZoomLevelTableViewCell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell minUniqueLocationsTableViewCell { get; set; }

        [Action ("cancel:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void cancel (UIKit.UIBarButtonItem sender);

        [Action ("done:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void done (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnCancel != null) {
                btnCancel.Dispose ();
                btnCancel = null;
            }

            if (cellSizeTableViewCell != null) {
                cellSizeTableViewCell.Dispose ();
                cellSizeTableViewCell = null;
            }

            if (debuggingEnabledTableViewCell != null) {
                debuggingEnabledTableViewCell.Dispose ();
                debuggingEnabledTableViewCell = null;
            }

            if (groupingEnabledTableViewCell != null) {
                groupingEnabledTableViewCell.Dispose ();
                groupingEnabledTableViewCell = null;
            }

            if (marginFactorTableViewCell != null) {
                marginFactorTableViewCell.Dispose ();
                marginFactorTableViewCell = null;
            }

            if (maxZoomLevelTableViewCell != null) {
                maxZoomLevelTableViewCell.Dispose ();
                maxZoomLevelTableViewCell = null;
            }

            if (minUniqueLocationsTableViewCell != null) {
                minUniqueLocationsTableViewCell.Dispose ();
                minUniqueLocationsTableViewCell = null;
            }
        }
    }
}