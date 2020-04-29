using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Gas_chromatography_data_converter_ver._1
{
    public partial class parameter_window : Form
    {
        

        public parameter_window()
        {
            InitializeComponent();
            data_grid();
        }
        private void data_grid()
        {

            DataTable dt;
            dt = new DataTable();
            {   dt.Columns.Add("Parameter", typeof(string));
                dt.Columns.Add("current", typeof(string));
                dt.Columns.Add("value", typeof(string));
            }
            dt.Rows.Add("Temperature interval", Program.Temperature_Interval);
            dt.Rows.Add("temperature_per_hour", Program.Temperature_per_hr);
            dt.Rows.Add("Calbiration", Program.Calibration_Var);
            dt.Rows.Add("Boundar Temperature", Program.Boundary_Temperature);
            dt.Rows.Add("Sample Weight", Program.sample_weigh);

            dataGridView1.DataSource = dt;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows[0].Cells[2].Value.ToString() != "")
                Program.Temperature_Interval = Convert.ToDouble(dataGridView1.Rows[0].Cells[2].FormattedValue);
            if (dataGridView1.Rows[1].Cells[2].Value.ToString() != "")
                Program.Temperature_per_hr = Convert.ToDouble(dataGridView1.Rows[1].Cells[2].FormattedValue);
            if (dataGridView1.Rows[2].Cells[2].Value.ToString() != "")
                Program.Calibration_Var = Convert.ToDouble(dataGridView1.Rows[2].Cells[2].FormattedValue);
            if (dataGridView1.Rows[3].Cells[2].Value.ToString() != "")
                Program.Boundary_Temperature = Convert.ToDouble(dataGridView1.Rows[3].Cells[2].FormattedValue);
            if (dataGridView1.Rows[4].Cells[2].Value.ToString() != "")
                Program.sample_weigh = Convert.ToDouble(dataGridView1.Rows[4].Cells[2].FormattedValue);

            MessageBox.Show(Program.Temperature_Interval+"\n"+  
                Program.Temperature_per_hr + "\n"+ 
                Program.Calibration_Var + "\n"+
                Program.Boundary_Temperature +"\n"
                +Program.sample_weigh);

            using(StreamWriter fsout = new StreamWriter(Program.set_up_file_path))
            {
                fsout.Write("1.Temperature Interval for Table:: "+ Program.Temperature_Interval.ToString() + "\n" +
                        "2.Temperature per hour:: "+ Program.Temperature_per_hr.ToString() + "\n" +
                        "3.Calibration Value:: "+ Program.Calibration_Var.ToString() + "\n" +
                        "4.Boundary Temperature:: "+ Program.Boundary_Temperature.ToString() + "\n" +
                        "5.folder_browsing :: NV-F0101.D, NV-F0202.D, NV-F0303.D" +
                        ", NV-F0404.D, NV-F0505.D, NV-F0606.D, NV-F0707.D, NV-F0808.D" +
                        ", NV-F0909.D, NV-F1010.D\n" +
                        "6.Data_file_name :: intermediate.txt\n" +
                        "7.sample_weigh :: "+ Program.sample_weigh.ToString());
            }
                        
            data_grid();
        }
    }
}
