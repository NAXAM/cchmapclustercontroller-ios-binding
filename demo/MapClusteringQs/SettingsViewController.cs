using Foundation;
using System;
using UIKit;

namespace MapClusteringQs
{
    public partial class SettingsViewController : UITableViewController
    {
        public event EventHandler<Settings> Completed;
        public Settings Settings { get; set; }

        readonly int SECTION_GENERAL = 0;
        readonly int SECTION_DATA_SET = 1;
        readonly int SECTION_CLUSTERER = 2;
        readonly int SECTION_ANIMATOR = 3;

        UISwitch debuggingEnabledSwitch;
        UIStepper cellSizeStepper;
        UIStepper marginFactorStepper;
        UIStepper maxZoomLevelStepper;
        UIStepper minUniqueLocationsStepper;
        UISwitch groupingEnabledSwitch;

        public SettingsViewController(IntPtr handle) : base(handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            debuggingEnabledSwitch = new UISwitch
            {
                On = Settings.DebuggingEnabled
            };

            debuggingEnabledTableViewCell.AccessoryView = debuggingEnabledSwitch;

            cellSizeStepper = new UIStepper
            {
                MinimumValue = 20,
                MaximumValue = 200,
                StepValue = 10
            };
            cellSizeStepper.Value = Math.Min(Math.Max(Settings.CellSize, cellSizeStepper.MinimumValue), cellSizeStepper.MaximumValue);
            cellSizeTableViewCell.AccessoryView = cellSizeStepper;

            marginFactorStepper = CreateStepper();


            marginFactorStepper.MinimumValue = -0.2f;
            marginFactorStepper.MaximumValue = 1.5f;
            marginFactorStepper.StepValue = 0.1f;
            marginFactorStepper.Value = Math.Min(Math.Max(Settings.MarginFactor, marginFactorStepper.MinimumValue), marginFactorStepper.MaximumValue);
            marginFactorTableViewCell.AccessoryView = marginFactorStepper;
            marginFactorTableViewCell.DetailTextLabel.Text = string.Format("{0:F1}", marginFactorStepper.Value);

            var row = (int)Settings.SettingsDataSet;
            var dataSetIndexPath = NSIndexPath.FromRowSection(row, SECTION_DATA_SET);
            SelectIndexPath(dataSetIndexPath);

            groupingEnabledSwitch = new UISwitch
            {
                On = Settings.GroupingEnabled,
            };
            groupingEnabledTableViewCell.AccessoryView = groupingEnabledSwitch;
            var clustererIndexPath = NSIndexPath.FromRowSection((int)Settings.SettingsClusterer, SECTION_CLUSTERER);
            SelectIndexPath(clustererIndexPath);

            maxZoomLevelStepper = CreateStepper();
            maxZoomLevelStepper.MinimumValue = 5;
            maxZoomLevelStepper.MaximumValue = 25;
            maxZoomLevelStepper.StepValue = 1;
            maxZoomLevelStepper.Value = Math.Min(Math.Max(Settings.MaxZoomLevelForClustering, maxZoomLevelStepper.MinimumValue), maxZoomLevelStepper.MaximumValue);
            maxZoomLevelTableViewCell.AccessoryView = maxZoomLevelStepper;
            maxZoomLevelTableViewCell.DetailTextLabel.Text = string.Format("{0:F1}", maxZoomLevelStepper.Value);

            minUniqueLocationsStepper = CreateStepper();
            minUniqueLocationsStepper.MinimumValue = 2;
            minUniqueLocationsStepper.MaximumValue = 10;
            minUniqueLocationsStepper.StepValue = 1;
            minUniqueLocationsStepper.Value = Math.Min(Math.Max(Settings.MinUniqueLocationsForClustering, minUniqueLocationsStepper.MinimumValue), minUniqueLocationsStepper.MaximumValue);
            minUniqueLocationsTableViewCell.AccessoryView = minUniqueLocationsStepper;

            minUniqueLocationsTableViewCell.DetailTextLabel.Text = string.Format("{0:F1}", minUniqueLocationsStepper.Value);

            var animatorIndexPath = NSIndexPath.FromRowSection((int)Settings.SettingsClusterer, SECTION_ANIMATOR);
            SelectIndexPath(animatorIndexPath);
        }

        UIStepper CreateStepper()
        {
            var stepper = new UIStepper();

            stepper.ValueChanged += StepperValueChanged;

            return stepper;
        }

        void StepperValueChanged(object sender, EventArgs e)
        {
            var point = TableView.ConvertPointFromView(new CoreGraphics.CGPoint(0, 0), sender as UIView);
            var indexPath = TableView.IndexPathForRowAtPoint(point);
            var cell = TableView.CellAt(indexPath);
            cell.DetailTextLabel.Text = string.Format("{0:F1}", (sender as UIStepper).Value);
        }

        void SelectIndexPath(NSIndexPath indexPath)
        {
            var numberOfRows = TableView.NumberOfRowsInSection(indexPath.Section);

            for (int i = 0; i < numberOfRows; i++)
            {
                var rowIndexPath = NSIndexPath.FromRowSection(i, indexPath.Section);
                var cell = TableView.CellAt(rowIndexPath);

                if (i == indexPath.Row)
                {
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
                }
                else
                {
                    cell.Accessory = UITableViewCellAccessory.None;
                }
            }
        }

        int SelectedRowForSection(int section)
        {
            int selectedRow = 0;

            int numberOfRows = (int)TableView.NumberOfRowsInSection(section);

            for (int i = 0; i < numberOfRows; i++)
            {
                var rowIndexPath = NSIndexPath.FromRowSection(i, section);
                var cell = TableView.CellAt(rowIndexPath);

                if (cell?.Accessory == UITableViewCellAccessory.Checkmark)
                {
                    selectedRow = i;
                    break;
                }
            }

            return selectedRow;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == SECTION_DATA_SET)
            {
                SelectIndexPath(indexPath);
            }
            else if (indexPath.Section == SECTION_CLUSTERER)
            {
                SelectIndexPath(indexPath);
            }
            else if (indexPath.Section == SECTION_ANIMATOR)
            {
                SelectIndexPath(indexPath);
            }
        }

        partial void cancel(UIKit.UIBarButtonItem sender)
        {
            PresentingViewController.DismissViewController(true, null);
        }

        partial void done(UIKit.UIBarButtonItem sender)
        {
            Settings.DebuggingEnabled = debuggingEnabledSwitch.On;
            Settings.CellSize = cellSizeStepper.Value;
            Settings.MarginFactor = marginFactorStepper.Value;
            Settings.GroupingEnabled = groupingEnabledSwitch.On;
            Settings.SettingsDataSet = (SettingsDataSet)SelectedRowForSection(SECTION_DATA_SET);
            Settings.SettingsClusterer = (SettingsClusterer)SelectedRowForSection(SECTION_CLUSTERER);
            Settings.MaxZoomLevelForClustering = maxZoomLevelStepper.Value;
            Settings.MinUniqueLocationsForClustering = (uint)minUniqueLocationsStepper.Value;
            Settings.SettingsAnimator = (SettingsAnimator)SelectedRowForSection(SECTION_ANIMATOR);

            Completed?.Invoke(this, Settings.Clone());

            PresentingViewController.DismissViewController(true, null);
        }
    }
}