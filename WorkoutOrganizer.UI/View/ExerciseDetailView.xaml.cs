using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorkoutOrganizer.UI.View
{
    /// <summary>
    /// Interaction logic for ExerciseDetailView.xaml
    /// </summary>
    public partial class ExerciseDetailView : UserControl
    {
        public ExerciseDetailView()
        {
            InitializeComponent();
        }

       
        //private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    Uri uri;
        //    if (Uri.TryCreate((sender as TextBox).Text, UriKind.Absolute, out uri))
        //    {
        //        Process.Start(new ProcessStartInfo(uri.AbsoluteUri));
        //    }
        //}
    }
}
