using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalloBlueSafe
{
    public partial class HalloBinding : UserControl
    {
        public HalloBinding()
        {
            InitializeComponent();

            //nameof ab VS2015
            textBox2.DataBindings.Add(nameof(TextBox.Text), textBox1, nameof(TextBox.Text), false, DataSourceUpdateMode.OnPropertyChanged);
            textBox2.DataBindings.Add(nameof(TextBox.BackColor), textBox1, nameof(TextBox.Text), true, DataSourceUpdateMode.OnPropertyChanged);


            trackBar1.Maximum = 200;
            var bind = new Binding(nameof(Label.Text), trackBar1, nameof(TrackBar.Value), true);
            bind.Format += Bind_Format;
            label1.DataBindings.Add(bind);

            label1.DataBindings.Add(nameof(Label.Top), trackBar1, nameof(TrackBar.Value), true);
            label1.DataBindings.Add(nameof(Label.Left), trackBar1, nameof(TrackBar.Value), true);

        }

        private void Bind_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value is int wert)
            {
                //e.Value = wert.ToString("0,000.00");
                //e.Value = "Wert: " + e.Value + "als Währung " + e.Value;
                //e.Value = string.Format("Wert: {0:##.0000} als Währung {0:c} am {1:D}", wert, DateTime.Now);
                e.Value = $"Wert: {wert:##.0000} als Währung {wert:c} am {DateTime.Now:D}";
            }
        }
    }
}
