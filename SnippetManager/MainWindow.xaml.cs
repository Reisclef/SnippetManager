using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Linq;

namespace SnippetManager {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private bool Changed = false;

        List<Literal> Literals = new List<Literal>();

        public MainWindow() {
            InitializeComponent();
            literalsDataGrid.ItemsSource = Literals; 
        }

        #region ClickEvents

        private void saveMenuItem_Click(object sender, RoutedEventArgs e) {
            SnippetXML xml = new SnippetXML(new HeaderInfo().SetHeaderData(
                titleTextBox.Text,
                authorTextBox.Text,
                descriptionTextBox.Text,
                GetSelectedRadioButton()
                ),
                new SnippetInfo().SetCode(new TextRange(codeRichTextBox.Document.ContentStart, codeRichTextBox.Document.ContentEnd).Text)
            );

            try {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.DefaultExt = ".snippet";
                dialog.Filter = "Code Snippets (.snippet)|*.snippet";

                if (dialog.ShowDialog() == true) {
                    xml.Xml.Save(dialog.FileName);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error saving file:" + Environment.NewLine + ex.Message.ToString());
            }
        }

        private void quitMenuItem_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to quit?", "Confirm Quit", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) {
                Application.Current.MainWindow.Close();
            }
        }

        private void newMenuItem_Click(object sender, RoutedEventArgs e) {
            if (Changed && MessageBox.Show("Create new snippet and lose all current changes?", "Confirm New", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                titleTextBox.Text = "";
                descriptionTextBox.Text = "";
                authorTextBox.Text = "";
                expansionRadioButton.IsChecked = true;
                codeRichTextBox.Document.Blocks.Clear();
                Changed = false;
            }
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e) {

            //Open the XML Document, and attempt to populate the GUI with its values
            try {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "Code Snippets (.snippet)|*.snippet";
                if (dialog.ShowDialog() == true) {
                    SnippetXML xdoc = new SnippetXML(XDocument.Load(dialog.FileName));
                    HeaderInfo headerInfo = xdoc.GetHeaderDataFromFile();
                    titleTextBox.Text = headerInfo.Title;
                    authorTextBox.Text = headerInfo.Author;
                    descriptionTextBox.Text = headerInfo.Description;
                    if (GetSelectedRadioButton() == "Expansion") {
                        expansionRadioButton.IsChecked = true;
                    }
                    else {
                        surroundsWithRadioButton.IsChecked = true;
                    }

                    SnippetInfo snippetInfo = xdoc.GetSnippetInfoFromFile();
                    codeRichTextBox.Document.Blocks.Clear();
                    codeRichTextBox.Document.Blocks.Add(new Paragraph(new Run(snippetInfo.Code)));
                    literalsDataGrid.ItemsSource = snippetInfo.Literals;
                }
            }
            catch (System.IO.IOException ex) {
                MessageBox.Show("Error loading file:" + Environment.NewLine + ex.Message.ToString());
            }
            catch (System.Xml.XmlException ex) {
                MessageBox.Show("Error parsing XML in file. " + Environment.NewLine + ex.Message.ToString());
            }
            catch (NullReferenceException ex) {
                MessageBox.Show("There was a problem locating the necessary elements within the XML." + Environment.NewLine + ex.Message.ToString());
            }
            catch (Exception ex) {
                MessageBox.Show("An Unknown Error occurred." + Environment.NewLine + ex.Message.ToString());
            }
        }

        #endregion

        #region TextChangedEvents

        private void titleTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            Changed = true;
        }

        private void authorTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            Changed = true;
        }

        private void descriptionTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            Changed = true;
        }

        private void codeRichTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            //The RichTextBox fires a TextChanged event when window is loaded, make sure it's visible 
            if (codeRichTextBox.IsVisible) {
                Changed = true;
            }
            
        }

        #endregion

        #region OtherEvents

        private string GetSelectedRadioButton() {
            if (expansionRadioButton.IsChecked == true) {
                return "Expansion";
            }
            else {
                return "SurroundsWith";
            }
        }

        #endregion

        private void codeRichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //Refresh the literals with any new literals (based on a text search of $literal$)

            //Prompt the user for any literals being removed
        }
    }
}
