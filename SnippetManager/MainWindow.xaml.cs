using System;
using System.Collections.Generic;
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

namespace SnippetManager {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e) {
            SnippetXML xml = new SnippetXML(SetHeaderInfo());

            xml.Xml.Save(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\test.snippet");
        }

        private HeaderInfo SetHeaderInfo() {
            HeaderInfo info = new HeaderInfo();

            info.Title = titleTextBox.Text;
            info.Description = descriptionTextBox.Text;
            info.Author = authorTextBox.Text;
            if (expansionRadioButton.IsChecked ?? false) {
                info.SnippetType = "Expansion";
            }
            else {
                info.SnippetType = "SurroundsWith";
            }

            return info;
        }
    }
}
