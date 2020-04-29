using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Data;
using ZedGraph;

namespace Gas_chromatography_data_converter_ver._1
{
    /*1.Temperature Interval for Table:: 8.55
      2.Temperature per hour:: 100
      3.Calibration Value:: 1
      4.Boundary Temperature :: 1050
      5.folder_browsing :: NV - F0101.D, NV - F0202.D, NV - F0303.D, NV - F0404.D, NV - F0505.D, NV - F0606.D, NV - F0707.D, NV - F0808.D, NV - F0909.D, NV - F1010.D
      6.Data_file_name :: intermediate.txt*/
    class Program
    {
        public static double Temperature_Interval = 1;
        public static double Temperature_per_hr = 200;
        public static double Calibration_Var = 1;
        public static double Boundary_Temperature = 1050;
        public static string[] folder_list = { "NV-F0101.D",
            "NV-F0202.D", "NV-F0303.D",
            "NV-F0404.D", "NV-F0505.D",
            "NV-F0606.D", "NV-F0707.D",
            "NV-F0808.D", "NV-F0909.D",
            "NV-F1010.D" };
        public static string Data_file_name = "intermediate.txt";
        public static double sample_weigh = 7;
        public static int setup_exist = 0;
        public static string set_up_file_path = null;

        public static string user_selected_path = null;
        public static string[] file_path=new string[11];
        public static string[] file_path_2=new string[11];

        public static double[] Area_data = new double[100];
        public static double[] temperature = new double[100];


        static void setup_file()
        {
            try
            {
                string set_up = System.IO.Directory.GetCurrentDirectory();
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(set_up);
                foreach (System.IO.FileInfo file in di.GetFiles())
                {
                    /*if(file.Extension.ToLower().CompareTo(".txt")==0)
                    {
                        string filenameonly = file.Name.Substring(0, file.Name.Length - 4);
                        string fullfilename = file.FullName;
                        MessageBox.Show(fullfilename + "     " + filenameonly);
                    }*/
                    if (file.Name.IndexOf("setup.txt") != -1)
                    {
                        setup_exist = 1;
                        set_up_file_path = file.FullName;

                        using (StreamReader fsource = new StreamReader(set_up_file_path))
                        {
                            string read_line = null;
                            while ((read_line = fsource.ReadLine()) != null)
                            {
                                /*1.Temperature Interval for Table:: 8.55
                               2.Temperature per hour:: 100
                               3.Calibration Value:: 1
                               4.Boundary Temperature:: 1050
                               5.folder_browsing :: NV - F0101.D, NV - F0202.D, NV - F0303.D, NV - F0404.D, NV - F0505.D, NV - F0606.D, NV - F0707.D, NV - F0808.D, NV - F0909.D, NV - F1010.D
                               6.Data_file_name :: intermediate.txt*/
                                if (read_line.IndexOf("Temperature Interval for Table") != -1)
                                    Temperature_Interval = Convert.ToDouble(read_line.Substring((read_line.IndexOf("::") + 2)));
                                if (read_line.IndexOf("Temperature per hour") != -1)
                                    Temperature_per_hr = Convert.ToDouble(read_line.Substring((read_line.IndexOf("::") + 2)));
                                if (read_line.IndexOf("Calibration Value") != -1)
                                    Calibration_Var = Convert.ToDouble(read_line.Substring((read_line.IndexOf("::") + 2)));
                                if (read_line.IndexOf("Boundary Temperature") != -1)
                                    Boundary_Temperature = Convert.ToDouble(read_line.Substring((read_line.IndexOf("::") + 2)));
                                if (read_line.IndexOf("folder_browsing") != -1)
                                {
                                    string line_folder_names = read_line.Substring((read_line.IndexOf("::") + 2));
                                    for (int i = 0; i < 9; i++)
                                    {
                                        folder_list[i] = (line_folder_names.Substring(((line_folder_names.IndexOf(",")) - 10), 10)).Trim();
                                        line_folder_names = line_folder_names.Remove((line_folder_names.IndexOf(",") - 10), 11);
                                    }
                                    /*int i = 0;
                                    while (line_folder_names.IndexOf(",")!=-1)
                                    {
                                        folder_list[i] = line_folder_names.Substring(((line_folder_names.IndexOf(",")) - 10), 10);
                                        //Substring(line_folder_names.IndexOf(",") - 10, 10);
                                        //line_folder_names = line_folder_names.Substring(line_folder_names.IndexOf(",")+1);
                                        MessageBox.Show(folder_list[i], "1");
                                        i++;
                                    }*/
                                }

                                if (read_line.IndexOf("Data_file_name") != -1)
                                {
                                    Data_file_name = read_line.Substring((read_line.IndexOf("::") + 2));
                                    Data_file_name = Data_file_name.Trim();
                                }
                                if (read_line.IndexOf("sample_weigh") != -1)
                                {
                                    sample_weigh = Convert.ToDouble(read_line.Substring((read_line.IndexOf("::") + 2)));
                                }
                            }

                            /*
                            MessageBox.Show(sample_weigh.ToString());
                            MessageBox.Show(Temperature_Interval.ToString()+"\n"+
                            Temperature_per_hr.ToString()+ "\n" +
                            Calibration_Var.ToString()+ "\n" +
                            Boundary_Temperature.ToString()+ "\n" +
                            Data_file_name+"\n"+sample_weigh.ToString());*/
                            //////////////////////////////////////////////////////////
                            int count_2 = 0;
                            foreach (string element in folder_list)
                            {
                                file_path[count_2] = element.Trim();
                                count_2++;
                            }
                        }
                    }
                }
                if (setup_exist == 0)
                {
                    using (StreamWriter fsout = new StreamWriter(set_up + "\\setup.txt"))
                    {
                        fsout.Write("1.Temperature Interval for Table:: 8.55\n" +
                            "2.Temperature per hour:: 100\n" +
                            "3.Calibration Value:: 1\n" +
                            "4.Boundary Temperature:: 1050\n" +
                            "5.folder_browsing :: NV-F0101.D, NV-F0202.D, NV-F0303.D" +
                            ", NV-F0404.D, NV-F0505.D, NV-F0606.D, NV-F0707.D, NV-F0808.D" +
                            ", NV-F0909.D, NV-F1010.D\n" +
                            "6.Data_file_name :: intermediate.txt\n" +
                            "7.sample_weigh :: 1");
                    }
                }
                using (FolderBrowserDialog folder_di = new FolderBrowserDialog())
                {
                    if (folder_di.ShowDialog() == DialogResult.OK)
                    {
                        string folder_main = folder_di.SelectedPath;
                        user_selected_path = folder_main;
                        int count_3 = 0;
                        foreach (string element in file_path)
                        {
                            file_path_2[count_3] = folder_main + "\\" + element + "\\" + Data_file_name;
                            count_3++;
                        }
                    }
                }
                show_table();

                Data_table_2();
            }
            catch
            {
                ;
            }
            
        }

        static void show_table()
        {
            /*
            bool test111 = File.Exists("C:\\Users\\matar\\Desktop\\Data\\200316_1ea\\NV-F0101.D\\intermediate.txt");
            bool test112 = File.Exists(file_path_2[0]);
            MessageBox.Show(test111.ToString());
            MessageBox.Show(test112.ToString());*/
            string path_data = user_selected_path + "\\data.txt";
            int file_number = 0;

            for (int i = 0; i < 100; i++) Area_data[i] = 0;

            foreach (string element in file_path_2)
            {   
                if (File.Exists(element)==true)
                {
                    if (!File.Exists(path_data))
                    {
                        using (StreamReader fsource = new StreamReader(element))
                        {
                            using (StreamWriter fsout = File.AppendText(path_data))
                            {
                                for (int i = 0; i < 12; i++)
                                {
                                    if ((Convert.ToInt32(file_number * 12 + i)) < 100)
                                    {
                                        temperature[Convert.ToInt32(file_number * 12 + i)] = (200 * file_number) + (i * 16.6666) + 25;
                                        Area_data[Convert.ToInt32(file_number * 12 + i)] = 0;
                                    }
                                }
                                string line1 = null;
                                while ((line1 = fsource.ReadLine()) != null)
                                {   
                                    if((line1.IndexOf("MM"))!=-1)
                                    {
                                        double time_min=Convert.ToDouble(line1.Substring(0,5).Trim());
                                        double area_data_raw_1 = Convert.ToDouble(line1.Substring(line1.IndexOf("MM")+2, 12).Trim());
                                        temperature[Convert.ToInt32(file_number*12+(time_min/5))]= (200 * file_number) + ((time_min/5) * 16.6666) + 25;
                                        Area_data[Convert.ToInt32(file_number * 12 + (time_min / 5))] = area_data_raw_1;
                                        
                                        //Console.WriteLine(temperature[Convert.ToInt32(file_number * 12 + (time_min / 5))] + "\t" + Area_data[Convert.ToInt32(file_number * 12 + (time_min / 5))]);
                                    }        
                                }
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader fsource = new StreamReader(element))
                        {
                            using (StreamWriter fsout = File.AppendText(path_data))
                            {
                                string line1 = null; 
                                for (int i = 0; i < 12; i++)
                                {
                                    if ((Convert.ToInt32(file_number * 12 + i)) < 100)
                                    {
                                        temperature[Convert.ToInt32(file_number * 12 + i)] = (200 * file_number) + (i * 16.6666) + 25;
                                        Area_data[Convert.ToInt32(file_number * 12 + i)] = 0;
                                    }
                                }
                                while ((line1 = fsource.ReadLine()) != null)
                                {
                                    
                                    if ((line1.IndexOf("MM")) != -1)
                                    {
                                        double time_min = Convert.ToDouble(line1.Substring(0, 5).Trim());
                                        double area_data_raw_1 = Convert.ToDouble(line1.Substring(line1.IndexOf("MM") + 2, 12).Trim());
                                        temperature[Convert.ToInt32(file_number * 12 + (time_min / 5))] = (200 * file_number) + ((time_min / 5) * 16.6666)+25;
                                        Area_data[Convert.ToInt32(file_number * 12 + (time_min / 5))] = area_data_raw_1;

                                        //Console.WriteLine(temperature[Convert.ToInt32(file_number * 12 + (time_min / 5))] + "\t" + Area_data[Convert.ToInt32(file_number * 12 + (time_min / 5))]);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {                 
                        using (StreamWriter fsout = File.AppendText(path_data))
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                if((Convert.ToInt32(file_number * 12 + i))<100)
                                {
                                    temperature[Convert.ToInt32(file_number * 12 + i)] = (200 * file_number) + (i * 16.6666) + 25;
                                    Area_data[Convert.ToInt32(file_number * 12 + i)] = 0;

                                    //Console.WriteLine(temperature[Convert.ToInt32(file_number * 12 + i)] + "\t" + Area_data[Convert.ToInt32(file_number * 12 + i)]);
                                }
                            }
                        }
                }
                file_number++;
            }
            for(int i=0;i<100;i++)
            {
                Console.WriteLine(temperature[i] + "\t" + Area_data[i]);
            }
            MessageBox.Show("data 확인");
            //data processing 
        }
        static async void Data_table_2()
        {
            Thread th = new Thread(graph);
            th.SetApartmentState(ApartmentState.STA);
            //var task_1 = Task.Run(() => graph());

            th.Start();
            //await task_1;
        }
        static void graph()
        {
            Data_Tatble_form1 form1 = new Data_Tatble_form1();
            form1.ShowDialog();
        }

        [STAThread]
        static void Main(string[] args)
        {
            char function_number = '0';
            do
            {
                Console.WriteLine("1.Load_Table_From_Raw_Data...\n2.Graph from Table...\n3.Exit...");
                Thread.Sleep(500);
                Console.Clear();
                if(Console.KeyAvailable==true)
                {
                    function_number = Console.ReadKey(true).KeyChar;
                    switch(function_number)
                    {
                        case '1':
                            {
                                setup_file();
                                break;
                            }
                        case '2':
                            {
                                break;
                            }
                    }
                }    
            } while (function_number != '3');
        }
    }
}
