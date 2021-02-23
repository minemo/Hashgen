using System;
using System.Collections.Generic;
using System.IO;
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
using System.Security.Cryptography;

namespace Hashgen
{

    public partial class MainWindow : Window
    {
        public string rootpath;
        public List<String> allhash;
        public MainWindow()
        {
            InitializeComponent();
        }

        public static List<String> ProcessDir(string targetdir)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetdir);
            List<String> hashlist = new List<string>();
            foreach (string fileName in fileEntries)
                hashlist.Add(ProcessFile(fileName));

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetdir);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDir(subdirectory);

            return hashlist;
        }

        public static string ProcessFile(string path)
        {
            MD5 alg = MD5.Create();
            using FileStream stream = File.OpenRead(path);
            byte[] hashcode = alg.ComputeHash(stream);
            return BitConverter.ToString(hashcode).Replace("-", "");
        }

        private void openfileselect(object sender, RoutedEventArgs e)
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            rootpath = "Der Festgelegte Pfad ist: " + '"' + dialog.SelectedPath + '"';
            Lbox.Items.Add(rootpath);
            allhash = ProcessDir(dialog.SelectedPath);
            foreach(String hash in allhash)
            {
                Lbox.Items.Add(hash);
            }

        }

        private void exportfile(object sender, RoutedEventArgs e)
        {
            Lbox.Items.Clear();
            Lbox.Items.Add("Exportpfad ist: C:\\Users\\" + Environment.UserName + "\\Downloads\\" + "Hashgen_export.txt");
            StreamWriter write = File.CreateText("C:\\Users\\"+ Environment.UserName +"\\Downloads\\" + "Hashgen_export.txt");
            foreach(String hash in allhash)
            {
                write.WriteLine(hash);
            }
            write.Close();
            Lbox.Items.Add("Export fertiggestellt");
        }
    }
}
