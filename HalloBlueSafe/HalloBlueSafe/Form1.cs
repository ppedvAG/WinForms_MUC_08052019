using HalloBlueSafe.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace HalloBlueSafe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-EN");
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-CH");


            Text = Properties.Resources.appTitle;
            demodatenErstellenToolStripMenuItem.Text = Properties.Resources.menuDemo;
            openButton.Text = Properties.Resources.btnOpen;
            saveButton.Text = Properties.Resources.btnSave;


            dataGridView1.AutoGenerateColumns = false;
        }

        private void demodatenErstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var blaues = new List<Blue>();
            blaues.Add(new Blue()
            {
                Titel = "Alien",
                ReleaseDate = new DateTime(1979, 10, 25),
                Speicher = 243763485.45m,
                RegionCode = RegionCode.SouthAmerica
            });
            blaues.Add(new Blue()
            {
                Titel = "Alien vs. Predator",
                ReleaseDate = new DateTime(2004, 11, 4),
                Speicher = 50,
                RegionCode = RegionCode.America
            });

            blaues.Add(new Blue()
            {
                Titel = "Avengers: Endgame",
                ReleaseDate = new DateTime(2019, 4, 24),
                Speicher = 150,
                RegionCode = RegionCode.Spacecraft
            });
            dataGridView1.DataSource = blaues;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog()
            {
                Title = "Blaue Zieldatei auswählen",
                Filter = "Blaue XML-Datei|*.xml|Alles und mit scharf!|*.*"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {

                using (var sw = new StreamWriter(dlg.FileName))
                {
                    var serial = new XmlSerializer(typeof(List<Blue>));
                    serial.Serialize(sw, dataGridView1.DataSource as List<Blue>);
                }
                MessageBox.Show("Fertig");
                textBox1.Text = dlg.FileName;
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog()
            {
                Title = "Blaue Quelldatei auswählen",
                Filter = "Blaue XML-Datei|*.xml|Alles und mit scharf!|*.*"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {

                using (var sr = new StreamReader(dlg.FileName))
                {
                    var serial = new XmlSerializer(typeof(List<Blue>));
                    dataGridView1.DataSource = serial.Deserialize(sr);
                }
                textBox1.Text = dlg.FileName;
            }
        }
    }
}
