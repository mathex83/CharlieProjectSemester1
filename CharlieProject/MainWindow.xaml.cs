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
using CharlieProject.ViewModel;
using MaterialDesignThemes.Wpf;

namespace CharlieProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebClient client;
        string fileSource = @"C:\Temp\Corona.zip";
        string fileExtract = @"C:\Temp";


        public MainWindow()
        {

            InitializeComponent();
            info.Visibility = Visibility.Hidden;
            Home.Visibility = Visibility.Visible;

            client = new WebClient();
            website.Visibility = Visibility.Hidden;
            KommuneName.Text = "";

        }

        private void CallDownload_Click(object sender, RoutedEventArgs e)
        {



            var hw = new HtmlWeb();
            HtmlDocument doc = hw.Load("https://covid19.ssi.dk/overvagningsdata/download-fil-med-overvaagningdata");
            string url = doc.DocumentNode.SelectSingleNode("//blockquote[@class='factbox']//p//a").Attributes["href"].Value.ToString();

            DirectoryInfo dir = new DirectoryInfo(@"C:\Temp");

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            if (!string.IsNullOrEmpty(url))
            {
                Uri uri = new Uri(url);
                client.DownloadFileAsync(uri, @"C:\Temp\Corona.zip");

            }
            if (Directory.Exists(fileExtract) == true)
            {
                Directory.CreateDirectory(fileExtract);
            }

            Thread.Sleep(500);

            ExtractFile();
        }

        private void ExtractFile()
        {
            ZipFile.ExtractToDirectory(fileSource, fileExtract);
        }

        private void Ballerup_Click(object sender, RoutedEventArgs e)
        {
            Home.Visibility = Visibility.Hidden;
            website.Visibility = Visibility.Hidden;

            info.Visibility = Visibility.Visible;

            KommuneName.Text = "Ballerup Kommune";
            tested.Text = "hello";
            infected.Text = "World";
            Procent.Text = "100%";
            LockCompanies.Text = "Resturanter";
            LockCompanies.Text = "Resturanter og alle barer";


        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void tarnby_click(object sender, RoutedEventArgs e)
        {
            Home.Visibility = Visibility.Hidden;
            KommuneName.Text = "Tårnby Kommune";
            tested.Text = "blablabla";
            infected.Text = "blablabladfgdfgdgf";
            Procent.Text = "324234100%";
            LockCompanies.Text = "sdfsdfsdfsdf";
            LockCompanies.Text = "sdfsdfsdfsdf";
        }
        

        private void info_Click(object sender, RoutedEventArgs e)
        {
            Home.Visibility = Visibility.Hidden;
            info.Visibility = Visibility.Hidden;
            website.Visibility = Visibility.Visible;
        }

        private void CodedBy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coded  By  Team Charlie");
        }

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

        private void Homepage_Click(object sender, RoutedEventArgs e)
        {
            Home.Visibility = Visibility.Visible;
            info.Visibility = Visibility.Hidden;
            website.Visibility = Visibility.Hidden;
        }










        /*private void BottomBar_Loaded(object sender, RoutedEventArgs e)
{

}

private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
{

}*/


        //The following 5 event handlers are the button clicks that opens the respective web pages,
        //which apparently have been changed in the way eventhandlers link to webpages in .net 5.0
        //and c# core. /Janus
        /* private void Button1_Click(object sender, RoutedEventArgs e)
         {
             
         }

         

         private void Button3_Click(object sender, RoutedEventArgs e)
         {
             
         }

         private void Button4_Click(object sender, RoutedEventArgs e)
         {
             
         }

         private void Button5_Click(object sender, RoutedEventArgs e)
         {
             var uri = "https://virksomhedsguiden.dk/erhvervsfremme/content/temaer/coronavirus_og_din_virksomhed/ydelser/krav-og-anbefalinger-til-virksomheder-med-vandrende-arbejdskraft/ff9c2363-fd26-4feb-a390-53e48534994b/";
             var psi = new System.Diagnostics.ProcessStartInfo();
             psi.UseShellExecute = true;
             psi.FileName = uri;
             Process.Start(psi);
         }

         private void btnDownload_Click(object sender, RoutedEventArgs e)
         {
             var hw = new HtmlWeb();
             HtmlDocument doc = hw.Load("https://covid19.ssi.dk/overvagningsdata/download-fil-med-overvaagningdata");
             string url = doc.DocumentNode.SelectSingleNode("//blockquote[@class='factbox']//p//a").Attributes["href"].Value.ToString();

             DirectoryInfo dir = new DirectoryInfo(@"C:\Temp");

             foreach (FileInfo fi in dir.GetFiles())
             {
                 fi.Delete();
             }

             if (!string.IsNullOrEmpty(url))
             {
                 Uri uri = new Uri(url);
                 client.DownloadFileAsync(uri, @"C:\Temp" + @"\Corona.zip");

             }
             if (Directory.Exists(fileExtract) == true)
             {
                 Directory.CreateDirectory(fileExtract);
             }

             Thread.Sleep(500);

             ExtractFile();
         }


         private void ExtractFile()
         {
             ZipFile.ExtractToDirectory(fileSource, fileExtract);

         }*/

        /*private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            TryTry municipality = new TryTry();
            municipality.FindFile();

        }*/
    }

}
