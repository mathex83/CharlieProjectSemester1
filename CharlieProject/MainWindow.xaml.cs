using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using System.IO;
using System.Diagnostics;
using System.IO.Compression;
using System.ComponentModel;
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using MaterialDesignThemes.Wpf;
using CharlieProject.Model;
using CharlieProject.View.Windows;

namespace CharlieProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
#region variables for pulling csv files from web. Coded by Rezan.
		WebClient client;
        string fileSource = @"C:\Temp\Corona.zip";
        string fileExtract = @"C:\Temp";
		#endregion

#region variable to load data from csv-files. Function is not working though Coded by Martin.
		public string[][] dataFromMunicipalityCsvFile;
		#endregion

#region elements called for csv-download to work from MainWindow initialize. Coded by Rezan.
		public MainWindow()
        {
            InitializeComponent();
            website.Visibility = Visibility.Hidden;
            info.Visibility = Visibility.Hidden;
            KommuneName.Text = "";

            HomePage homepage = new HomePage();
            Home.Children.Add(homepage);
            
            client = new WebClient();

        }
#endregion

#region Method started to pull data from csv in to app and send to database. Coded by Martin, but will wait for a "perfect" version.

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            //Browse File function
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = fileExtract;
            //Filter files that will  be shown in files windows
            openFileDialog.Filter = "CSV  FILES|Municipality_cases_time_series.csv;Municipality_tested_persons_time_series.csv;";
            // Show open file dialog box
            Nullable<bool> FileOk = openFileDialog.ShowDialog();
            int testedRowArrayLength;
            // Process open file dialog box results
            if (FileOk == true)
            {
				SqlQueries mt = new SqlQueries();

				//Save full file  path  inside a string
				string CSVFilePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                //If file  name  is  Municipality_tested_persons_time_series do  ....
                if (CSVFilePath == openFileDialog.InitialDirectory + @"\Municipality_tested_persons_time_series.csv")
                {
                    //Read all lines in csv file
                    string[] testedRowArray = File.ReadAllLines(CSVFilePath);
                    dataFromMunicipalityCsvFile = new string[testedRowArray.Length][];                    

                    //Loop the data.
                    for (int i = 1; i < testedRowArray.Length; i++)
                    {
                        for (int j = 1; j < testedRowArray[1].Length; j++)
                        {
                            dataFromMunicipalityCsvFile[i] = testedRowArray[i].Split(';');
                        }
                    }

                    MessageBox.Show("Række 0:\n" + mt.infectedDataIncoming[0][1].ToString() + "\n" +
                        mt.infectedDataIncoming[0][2].ToString() + "\n" +
                        mt.infectedDataIncoming[0][3].ToString() + "\n" +
                        Convert.ToDateTime(mt.infectedDataIncoming[0][4]).ToString("yyyy-MM-dd") + "\n\nRække 1:\n" +
                        mt.infectedDataIncoming[1][1].ToString() + "\n" +
                        mt.infectedDataIncoming[1][2].ToString() + "\n" +
                        mt.infectedDataIncoming[1][3].ToString() + "\n" +
                        Convert.ToDateTime(mt.infectedDataIncoming[1][4]).ToString("yyyy-MM-dd") + "\n\nRække 2:\n" +
                        mt.infectedDataIncoming[2][1].ToString() + "\n" +
                        mt.infectedDataIncoming[2][2].ToString() + "\n" +
                        mt.infectedDataIncoming[2][3].ToString() + "\n" +
                        Convert.ToDateTime(mt.infectedDataIncoming[2][4]).ToString("yyyy-MM-dd") + "\n\nRække 3:\n" +
                        mt.infectedDataIncoming[3][1].ToString() + "\n" +
                        mt.infectedDataIncoming[3][2].ToString() + "\n" +
                        mt.infectedDataIncoming[3][3].ToString() + "\n" +
                        Convert.ToDateTime(mt.infectedDataIncoming[3][4]).ToString("yyyy-MM-dd"));

                    if (Convert.ToDateTime(mt.infectedDataIncoming[3][4]).ToString("yyyy-MM-dd") ==
                            Convert.ToDateTime(dataFromMunicipalityCsvFile[1][0]).ToString("yyyy-MM-dd")
                        && mt.infectedDataIncoming[3][3].ToString() == "5")
                    {

                        if (mt.infectedDataIncoming[3][1] == dataFromMunicipalityCsvFile[1][5])
                        {

                        }
                    }
                    else
                    {
                        MessageBox.Show("false\n" + mt.infectedDataIncoming[1][4] + "\n" + dataFromMunicipalityCsvFile[1][0]);
                    }

                }
                //If file name is Municipality_cases_time_series do  ....

                else if (CSVFilePath == openFileDialog.InitialDirectory + @"\Municipality_cases_time_series.csv")
                {
                    string[] infectedRowArray = File.ReadAllLines(CSVFilePath);
                    for (int i = 1; i < infectedRowArray.Length; i++)
                    {
                        //File2.AddRange(infectedRowArray[i].Split(';'));
                    }
                    testedRowArrayLength = infectedRowArray[1].Split(';').Length;
                }
            }
        }
        #endregion

#region Download and extract csv-files. Coded by Rezan
        private void CallDownload_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(fileExtract) == false)
            {
                Directory.CreateDirectory(fileExtract);
			}
			else
			{
				DirectoryInfo dir = new DirectoryInfo(fileExtract);
				foreach (FileInfo fi in dir.GetFiles())
				{
					fi.Delete();
				}
			}
            KommuneName.Text = "";
            var hw = new HtmlWeb();
            HtmlDocument doc = hw.Load("https://covid19.ssi.dk/overvagningsdata/download-fil-med-overvaagningdata");
            string url = doc.DocumentNode.SelectSingleNode("//blockquote[@class='factbox']//p//a").Attributes["href"].Value.ToString();

            if (!string.IsNullOrEmpty(url))
            {
                Uri uri = new Uri(url);
                client.DownloadFileAsync(uri, @"C:\Temp\Corona.zip");

            }
            //hejhej
            Thread.Sleep(500);

            ExtractFile();
            popup p = new popup();
            p.popupWindow.Text = "Files downloaded!";
            p.Show();
        }

        private void ExtractFile()
        {
            ZipFile.ExtractToDirectory(fileSource, fileExtract);
        }
		#endregion

#region Clickevent to pass data from database to fields on MainWindow. Coded by Martin.
		//Created by Martin Nørholm
		private void Municipality_Click(object sender, RoutedEventArgs e)
        {
            Home.Visibility = Visibility.Hidden;
            info.Visibility = Visibility.Visible;
            website.Visibility = Visibility.Hidden;
            MenuItem bt = sender as MenuItem;
            SqlQueries mt = new SqlQueries();
            string[] dataforuse = new string[1];
            //knap funktion
            switch (bt.Name)
            {                
                case "a":
                    dataforuse = mt.SelectAMunicipality(4);
                    KommuneName.Text = "Tårnby kommune";
                    break;
                case "b":
                    dataforuse = mt.SelectAMunicipality(5);
                    KommuneName.Text = "Albertslund kommune";
                    break;
                case "c":
                    dataforuse = mt.SelectAMunicipality(6);
                    KommuneName.Text = "Ballerup kommune";
                    break;
            }

            tested.Text = dataforuse[0];
            infected.Text = dataforuse[1];
            Procent.Text = dataforuse[2] + "%";
            InfectedLevel.Text = dataforuse[3] + " - " + dataforuse[4];

            LockCompanies.Text = dataforuse[5];
        }
		#endregion

#region Exit the app. Coded by Martin and Rezan.
		private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
		#endregion

#region Open view to select buttons to open sites for the industry groups. Coded by Rezan.
		private void info_Click(object sender, RoutedEventArgs e)
        {
            KommuneName.Text = "";
            Home.Visibility = Visibility.Hidden;
            info.Visibility = Visibility.Hidden;
            website.Visibility = Visibility.Visible;
        }
#endregion

#region A little about window. Coded by Rezan.
        private void CodedBy_Click(object sender, RoutedEventArgs e)
        {
            popup win = new popup();
            win.popupWindow.Text = "Coded by Team Charlie:\nDitte Slyngborg\nJanus B. Reedtz\nMartin Nørholm\nRezan Razoul";
            win.Show();            
        }
#endregion

#region buttonclick-events to open browser on our industry groups. Coded by Janus (JBD) and links adapted by Rezan and Martin.
        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            var uri = "https://virksomhedsguiden.dk/erhvervsfremme/content/temaer/coronavirus_og_din_virksomhed/ydelser/krav-og-anbefalinger-til-detailhandlen/59cef21f-a199-42a8-bea0-fdc01ed6cf5f/";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            Process.Start(psi);
        }

        private void production_Click(object sender, RoutedEventArgs e)
        {
            var uri = "https://virksomhedsguiden.dk/erhvervsfremme/content/temaer/coronavirus_og_din_virksomhed/ydelser/krav-og-anbefalinger-til-produktionsvirksomheder/de3e13b6-038d-46c9-8488-fd5d7518b0b6/";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            Process.Start(psi);
        }

        private void service_Click(object sender, RoutedEventArgs e)
        {
            var uri = "https://virksomhedsguiden.dk/erhvervsfremme/content/temaer/coronavirus_og_din_virksomhed/ydelser/krav-og-anbefalinger-til-serviceerhverv/e8172f4a-3366-4392-a166-f917dd50a65e/";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            Process.Start(psi);
        }

        private void restaurants_Click(object sender, RoutedEventArgs e)
        {
            var uri = "https://virksomhedsguiden.dk/erhvervsfremme/content/temaer/coronavirus_og_din_virksomhed/ydelser/krav-og-anbefalinger-til-restauranter-cafeer-og-overnatningssteder/bc5c379d-8ee0-4f1a-890e-317c3e20a45e/";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            Process.Start(psi);
        }

        private void office_Click(object sender, RoutedEventArgs e)
        {
            var uri = "https://virksomhedsguiden.dk/erhvervsfremme/content/temaer/coronavirus_og_din_virksomhed/ydelser/krav-og-anbefalinger-til-kontorarbejdspladser/940fd004-bf50-4529-8533-fc2f49217d4c/";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            Process.Start(psi);
        }
		#endregion

#region Click to show home page view. Coded by Rezan.
		private void Homepage_Click(object sender, RoutedEventArgs e)
        {
            KommuneName.Text = "";
            info.Visibility = Visibility.Hidden;
            website.Visibility = Visibility.Hidden;
            Home.Visibility = Visibility.Visible;
            HomePage home = new HomePage();
            Home.Children.Add(home);
        }
#endregion

#region buttonclickevent to add some data to database for new infected people. Coded by Martin.
        private void AddDataDB_Click(object sender, RoutedEventArgs e)
		{
            SqlQueries mt = new SqlQueries();
            mt.InsertSomeData();
        }
#endregion

    }
}
