using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using MessageBox = System.Windows.MessageBox;
using Size = System.Drawing.Size;

namespace XmlAndJsonConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.ConvertButton.Click += delegate
            {
                this.Convert();
            };
            this.SimpleXml.Click += delegate
            {
                string xmlExample = "<?xml version='1.0'?>" +
                                    "<catalog>" +
                                    "<book id='bk'>" +
                                    "<author>Gambardella, Matthew</author>" +
                                    "<title>XML Developer's Guide</title>" +
                                    "<genre>Computer</genre>" +
                                    "<price>44.95</price>" +
                                    "<publish_date>2000-10-01</publish_date>" +
                                    "<description>An in-depth look at creating applications with XML.</description>" +
                                    "</book>" +
                                    "<book id='bk102'>" +
                                    "<author>Ralls, Kim</author>" +
                                    "<title>Midnight Rain</title>" +
                                    "<genre>Fantasy</genre>" +
                                    "<price>5.95</price>" +
                                    "<publish_date>2000-12-16</publish_date>" +
                                    "<description>A former architect battles corporate zombies, an evil sorceress," +
                                    " and her own childhood to become queen of the world.</description>" +
                                    "</book>" +
                                    "</catalog>";
                (this.InputScintillaHost.Child as Scintilla).Text = xmlExample;
            };
            this.SimpleJson.Click += delegate
            {
                string jsonExample = "{'glossary': " +
                                     "{'title': 'example glossary','GlossDiv': " +
                                     "{'title': 'S','GlossList': " +
                                     "{'GlossEntry': " +
                                     "{'ID': 'SGML','SortAs': 'SGML','GlossTerm': 'Standard Generalized Markup Language','Acronym': 'SGML','Abbrev': 'ISO 8879:1986','GlossDef': " +
                                     "{'para': 'A meta-markup language, used to create markup languages such as DocBook.','GlossSeeAlso': ['GML', 'XML']},'GlossSee': 'markup'}}}}}";
                (this.InputScintillaHost.Child as Scintilla).Text = jsonExample;
            };
        }

        private void ScintillaHostLoaded(object sender, RoutedEventArgs e)
        {
            Scintilla scintilla = new Scintilla();
            scintilla.ConfigurationManager.Language = "xml";
            scintilla.BorderStyle = BorderStyle.FixedSingle;
            scintilla.Margins.Margin0.Width = 40;
            scintilla.Margins.Margin2.Width = 20;
            scintilla.LineWrapping.Mode = LineWrappingMode.Char;
            scintilla.AutoSize = true;
            scintilla.Margin = System.Windows.Forms.Padding.Empty;
            (sender as WindowsFormsHost).Child = scintilla;
        }
        public void Convert()
        {
            Scintilla input = this.InputScintillaHost.Child as Scintilla;
            Scintilla output = this.OutputScintillaHost.Child as Scintilla;

            if (input != null && output != null && !string.IsNullOrEmpty(input.Text.Trim()))
            {
                string result = null;
                try
                {
                    // index 0 - Xml->Json
                    // index 1 - Json->Xml
                    if (this.ConvertType.SelectedIndex == 0)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(input.Text.Trim());
                        result = JsonConvert.SerializeXmlNode(doc);
                        input.ConfigurationManager.Language = "xml";
                        output.ConfigurationManager.Language = "Css";
                    }
                    else
                    {
                        XNode node = JsonConvert.DeserializeXNode(input.Text.Trim(), "Root");
                        result = node.ToString();
                        input.ConfigurationManager.Language = "Css";
                        output.ConfigurationManager.Language = "xml";
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Invalid input data");
                    return;
                }
                output.Text = result;
            }
        }
    }
}
