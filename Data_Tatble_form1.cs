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
using System.Threading;
using ZedGraph;

namespace Gas_chromatography_data_converter_ver._1
{
    public partial class Data_Tatble_form1 : Form
    {
        static public DataTable dt_2;
        static public double mass_ppm = 0;

        public Data_Tatble_form1()
        {
            InitializeComponent();
            datagrid2();
            Gridview_2.DataSource = dt_2;
            graph();

        }

        static public void datagrid2()
        {
            dt_2 = new DataTable { TableName = "DATA TABLE" };
            dt_2.Columns.AddRange(new DataColumn[] {new DataColumn{ColumnName="temperature",Caption="온도",DataType=typeof(double)},
            new DataColumn { ColumnName = "area", Caption = "영역", DataType = typeof(double) },
            new DataColumn { ColumnName = "mass ppm", Caption = "PPM", DataType = typeof(double) },
            new DataColumn { ColumnName = "mass ppm/sec", Caption = "초당 발생 PPM", DataType = typeof(double) },
            new DataColumn { ColumnName = "boundary", Caption = "경계온도", DataType = typeof(double) },
            new DataColumn { ColumnName = "수소량", Caption = "경계온도", DataType = typeof(double) }});

            int t = 0;
            mass_ppm = 0;
            foreach( double temp in Program.temperature)
            {
                if (temp < Program.Boundary_Temperature)
                    mass_ppm += (Program.Area_data[t] * Program.Calibration_Var / Program.sample_weigh);
                t++; 
            }

            int i = 0;
            foreach (double temp in Program.temperature)
            {
                if (i<1)
                dt_2.Rows.Add(temp, Program.Area_data[i], (Program.Area_data[i] * Program.Calibration_Var / Program.sample_weigh), (Program.Area_data[i] * Program.Calibration_Var / Program.sample_weigh) / 300, Program.Boundary_Temperature,mass_ppm );
                else
                {
                 dt_2.Rows.Add(temp, Program.Area_data[i], (Program.Area_data[i] * Program.Calibration_Var / Program.sample_weigh), (Program.Area_data[i] * Program.Calibration_Var / Program.sample_weigh) / 300);
                }
                i++;
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parameter_window form2 = new parameter_window();
            form2.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mass_ppm = 0;
            datagrid2();
            Gridview_2.DataSource = dt_2;
            graph();
        }
        private void graph()
        {
            GraphPane graphpane;
            graphpane = zedGraphControl1.GraphPane;

            PointPairList _pointpairlist = new PointPairList();
            int i = 0;
            graphpane.XAxis.Title.Text = "temperature (oC)";
            graphpane.YAxis.Title.Text = "mass PPM/sec";
            graphpane.XAxis.MajorGrid.IsVisible = true;
            graphpane.XAxis.MinorGrid.IsVisible = true;

            graphpane.YAxis.MajorGrid.IsVisible = true;
            graphpane.YAxis.MinorGrid.IsVisible = true;

            foreach (double temp in Program.temperature)
            {       
                dt_2.Rows.Add(temp, Program.Area_data[i], 
                    (Program.Area_data[i] * Program.Calibration_Var / Program.sample_weigh),
                    (Program.Area_data[i] * Program.Calibration_Var / Program.sample_weigh) / 300);

                PointPair _pointpair = new PointPair(temp, (Program.Area_data[i] * Program.Calibration_Var / Program.sample_weigh) / 300);
                //(Program.Area_data[i] * Program.Calibration_Var / Program.sample_weigh) / 300

                _pointpairlist.Add(_pointpair);
                i++;
            }
            LineItem _lineitem = graphpane.AddCurve("mass PPM/sec",_pointpairlist,Color.Red,SymbolType.None);
            zedGraphControl1.AxisChange();      
        }
        private void button2_Click(object sender, EventArgs e)
        {

            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;

            worksheet = workbook.Sheets["sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "CustomerDetail";

            for (int i = 1; i < Gridview_2.Columns.Count+1; i++)
            {
                worksheet.Cells[1,i] = Gridview_2.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < Gridview_2.Rows.Count-1; i++)
            {
                for (int j = 0; j < Gridview_2.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = Gridview_2.Rows[i].Cells[j].Value.ToString();
                }
            }

            SaveFileDialog save_excel  = new SaveFileDialog();
            save_excel.FileName = "output";
            save_excel.DefaultExt = ".xlsx";
            if (save_excel.ShowDialog()==DialogResult.OK)
            {
                workbook.SaveAs(save_excel.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            app.Quit();
        }
    }
}
