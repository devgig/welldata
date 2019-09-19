using System.Windows;
using System.Windows.Controls;
using WellData.Ui.MaterialDesign;

namespace WellData.Ui.Controls
{

    public class FontButton : Button
    {
        static FontButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FontButton), new FrameworkPropertyMetadata(typeof(FontButton)));
            
        }

      

        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        /// <value>
        /// The width of the image.
        /// </value>
        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        /// <summary>
        /// The image width property
        /// </summary>
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(FontButton), new UIPropertyMetadata(16.0));

        
        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <value>
        /// The height of the image.
        /// </value>
        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        /// <summary>
        /// The image height property
        /// </summary>
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(FontButton), new UIPropertyMetadata(16.0));
                     
        /// <summary>
        /// Gets or sets a value indicating whether [image align top].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [image align top]; otherwise, <c>false</c>.
        /// </value>
        public bool ImageAlignTop
        {
            get { return (bool)GetValue(ImageAlignTopProperty); }
            set { SetValue(ImageAlignTopProperty, value); }
        }

        /// <summary>
        /// The image align right property
        /// </summary>
        public static readonly DependencyProperty ImageAlignTopProperty =
            DependencyProperty.Register("ImageAlignTop", typeof(bool), typeof(FontButton), new UIPropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether [image align right].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [image align right]; otherwise, <c>false</c>.
        /// </value>
        public bool ImageAlignRight
        {
            get { return (bool)GetValue(ImageAlignRightProperty); }
            set { SetValue(ImageAlignRightProperty, value); }
        }

        /// <summary>
        /// The image align right property
        /// </summary>
        public static readonly DependencyProperty ImageAlignRightProperty =
            DependencyProperty.Register("ImageAlignRight", typeof(bool), typeof(FontButton), new UIPropertyMetadata(false));



        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public PackIconKind Icon
        {
            get { return (PackIconKind)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// The image property
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(PackIconKind), typeof(FontButton), new UIPropertyMetadata(null)); 
    }

}