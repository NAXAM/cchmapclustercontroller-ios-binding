using System;
using Foundation;

namespace MapClusteringQs
{
    public class Settings
    {
        public bool DebuggingEnabled { get; set; }

        public double CellSize { get; set; }

        public double MarginFactor { get; set; }

        public SettingsDataSet SettingsDataSet { get; set; }

        public bool GroupingEnabled { get; set; }

        public SettingsClusterer SettingsClusterer { get; set; }

        public double MaxZoomLevelForClustering { get; set; }

        public uint MinUniqueLocationsForClustering { get; set; }

        public SettingsAnimator SettingsAnimator { get; set; }

        public Settings()
        {
            CellSize = 60;
            MarginFactor = 0.5f;
            MaxZoomLevelForClustering = 16;
            MinUniqueLocationsForClustering = 3;
        }

        public Settings Clone()
        {
            return new Settings
            {
                DebuggingEnabled = DebuggingEnabled,
                CellSize = CellSize,
                SettingsDataSet = SettingsDataSet,
                MarginFactor = MarginFactor,
                GroupingEnabled = GroupingEnabled,
                SettingsAnimator = SettingsAnimator,
                SettingsClusterer = SettingsClusterer,
                MinUniqueLocationsForClustering = MinUniqueLocationsForClustering,
                MaxZoomLevelForClustering = MaxZoomLevelForClustering
            };
        }
    }

    public enum SettingsDataSet
    {
        Berlin,
        Usa
    }

    public enum SettingsClusterer
    {
        CenterOfMass,
        NearCenter
    }

    public enum SettingsAnimator
    {
        FadeInOut
    }
}
