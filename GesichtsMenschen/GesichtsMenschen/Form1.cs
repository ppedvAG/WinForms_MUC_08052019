using GesichtsMenschen.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GesichtsMenschen
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        BindingSource bs = new BindingSource();
        DataSet1 ds = new DataSet1();
        EmployeesTableAdapter ada = new EmployeesTableAdapter();

        public Form1()
        {
            InitializeComponent();

            metroGrid1.AutoGenerateColumns = false;

            var nameCol = new DataGridViewTextBoxColumn
            {
                HeaderText = "Vorname",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DataPropertyName = ds.Employees.FirstNameColumn.ColumnName
            };
            metroGrid1.Columns.Add(nameCol);

            metroGrid1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nachname",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DataPropertyName = ds.Employees.LastNameColumn.ColumnName
            });

            bs.DataSource = ds.Employees;
            metroGrid1.DataSource = bs;
            ada.Fill(ds.Employees); //SELECT *

            metroTextBox1.DataBindings.Add(nameof(metroTextBox1.Text), bs, ds.Employees.FirstNameColumn.ColumnName);
            metroTextBox2.DataBindings.Add(nameof(metroTextBox1.Text), bs, ds.Employees.LastNameColumn.ColumnName);

            var dateBind = new Binding(nameof(metroDateTime1.Value), bs, ds.Employees.BirthDateColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged);
            dateBind.Format += (s, e) =>
            {
                if (e.Value == DBNull.Value)
                    e.Value = new DateTime(2000, 1, 1);
            };
            metroDateTime1.DataBindings.Add(dateBind);

            pictureBox1.DataBindings.Add("Image", bs, "Photo", true);
            //pictureBox1.DataBindings.Add(nameof(pictureBox1.Image), bs, ds.Employees.PhotoColumn.ColumnName, true);

            metroButton1.Click += (object s, EventArgs e) =>
            {
                bs.MovePrevious();
            };

            metroButton1.Click += MetroButton1_Click;
            metroButton2.Click += (s, e) => bs.MoveNext();

            bs.CurrentChanged += Bs_CurrentChanged;
        }

        private void Bs_CurrentChanged(object sender, EventArgs e)
        {
        }

        private void MetroButton1_Click(object sender, EventArgs e)
        {
            bs.MovePrevious();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            bs.EndEdit();

            try
            {
                ada.Update(ds);
            }
            catch (DBConcurrencyException dbex)
            {
                var dlg = MessageBox.Show("Die Daten wurden zwischenzeitlich geändert", "", MessageBoxButtons.AbortRetryIgnore, 
                    MessageBoxIcon.Warning);
                if (dlg == DialogResult.Abort) //lokale änderungen verwerfen
                {
                    //todo reload row: dbex.Row.
                }
                else if (dlg == DialogResult.Ignore) //db änderungen überschreiben
                {
                    //todo reload original Values
                }
        }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}");
            }
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            var newEmp = ds.Employees.NewEmployeesRow();
            newEmp.LastName = "NEU";
            newEmp.FirstName = "NEU";

            ds.Employees.AddEmployeesRow(newEmp);

        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            if (bs.Current == null)
            {
                MessageBox.Show("Es ist kein Datensatz ausgewählt");
                return;
            }

            if (bs.Current is DataRowView drv)
            {
                //casting = unsicher, weil laufzeitfehler falls 'current' nicht EmployeesRow
                var empRow = (DataSet1.EmployeesRow)drv.Row;

                //boxing = gut, weil NULL wenn 'current' nicht EmployeesRow ist
                var empRow2 = drv.Row as DataSet1.EmployeesRow;
                if (empRow2 != null) { }

                //pattern-matching ab VS2015 = cool!
                if (drv.Row is DataSet1.EmployeesRow empRow3)
                {
                    if (MessageBox.Show($"Soll das Gesicht mit dem Namen {empRow3.FirstName} {empRow3.LastName} wirklich gelöscht werden?",
                                        "Titel der MessageBox",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button2)
                                        == DialogResult.Yes)
                    {
                        empRow3.Delete(); //DB
                    }
                }
            }

        }
    }
}
