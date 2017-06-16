using System;
using CoreGraphics;
using Foundation;
using MapKit;
using UIKit;

namespace MapClusteringQs
{
    [Register("ClusterAnnotationView")]
    public class ClusterAnnotationView : MKAnnotationView
    {
        uint count;
        public uint Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
                countLabel.Text = count.ToString();
                SetNeedsLayout();
            }
        }

        bool isBlue;
        public bool IsBlue
        {
            get
            {
                return isBlue;
            }

            set
            {
                isBlue = value;
                SetNeedsLayout();
            }
        }

        bool uniqueLocation;
        public bool UniqueLocation
        {
            get
            {
                return uniqueLocation;
            }

            set
            {
                uniqueLocation = value;
                SetNeedsLayout();
            }
        }

        UILabel countLabel;

        public ClusterAnnotationView(IMKAnnotation annotation, string reuseIdentifier) : base(annotation, reuseIdentifier)
        {
            BackgroundColor = UIColor.Clear;
            SetUpLabel();
            Count = 1;
        }

        void SetUpLabel()
        {
            countLabel = new UILabel(Bounds)
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
                TextAlignment = UITextAlignment.Center,
                BackgroundColor = UIColor.Clear,
                TextColor = UIColor.White,
                AdjustsFontSizeToFitWidth = true,
                MinimumScaleFactor = 2,
                Lines = 1,
                Font = UIFont.SystemFontOfSize(12),
                BaselineAdjustment = UIBaselineAdjustment.AlignCenters
            };

            AddSubview(countLabel);
        }

        void UpdateCountLabel()
        {
            countLabel.Text = Count.ToString();
            SetNeedsLayout();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            UIImage image;
            CGPoint centerOffset;
            CGRect countLabelFrame;

            if (UniqueLocation)
            {
                var imageName = IsBlue ? "SquareBlue" : "SquareRed";
                image = UIImage.FromBundle(imageName);
                centerOffset = new CGPoint(0, image.Size.Height / 2);

                countLabelFrame = new CGRect(Bounds.X, Bounds.Y - 2, Bounds.Width, Bounds.Height);
            }
            else
            {
                string suffix;

                if (Count > 1000)
                {
                    suffix = @"39";
                }
                else if (Count > 500)
                {
                    suffix = @"38";
                }
                else if (Count > 200)
                {
                    suffix = @"36";
                }
                else if (Count > 100)
                {
                    suffix = @"34";
                }
                else if (Count > 50)
                {
                    suffix = @"31";
                }
                else if (Count > 20)
                {
                    suffix = @"28";
                }
                else if (Count > 10)
                {
                    suffix = @"25";
                }
                else if (Count > 5)
                {
                    suffix = @"24";
                }
                else
                {
                    suffix = @"21";
                }

                var imageName = IsBlue ? "CircleBlue" : "CircleRed";
                image = UIImage.FromBundle($"{imageName}{suffix}");

                centerOffset = CGPoint.Empty;
                countLabelFrame = Bounds;
            }

            countLabel.Frame = countLabelFrame;
            Image = image;
            CenterOffset = centerOffset;
        }

    }
}
