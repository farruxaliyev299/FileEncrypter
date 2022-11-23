using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using Path = System.IO.Path;
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
using System.Threading;

namespace EncryptionApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string InitialStr { get; set; } = String.Empty;
        private string InitialKey { get; set; } = String.Empty;
        private bool isEncrypted { get; set; }
        private bool isCancelled { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true && Path.GetExtension(openFileDialog.FileName) == ".txt")
            {
                filePath.Text = Path.GetFullPath(openFileDialog.FileName);
            }

            isEncrypted = false;
            isCancelled = false;
            prBar.Value = 0;

            InitialStr = File.ReadAllText(openFileDialog.FileName);

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InitialKey = keyBox.Text;
        }

        private async void startBtn_Click(object sender, RoutedEventArgs e)
        {
            //Validation for empty imputs
            if (InitialStr.Length != 0 && InitialKey.Length != 0)
            {
                prBar.Maximum = 1000;
                startBtn.IsEnabled = false;
                await Task.Delay(1000);
                for (int i = 0; i <= 1000; i += 8)
                {
                    /*In case of Cancel button is pressed,
                     *it will stop the proccess and return elements to their initial forms */
                    if (isCancelled)
                    {
                        isCancelled = false;
                        return;
                    }
                    prBar.Value = i;
                    await Task.Delay(1);
                }

                byte[] textBytes = Encoding.UTF8.GetBytes(InitialStr);
                byte[] keyBytes = Encoding.UTF8.GetBytes(InitialKey);

                string encodedStr = ByteToString(textBytes, keyBytes);
                byte[] encodedBytes = Encoding.UTF8.GetBytes(encodedStr);

                //Checking if file is already encrypted or not
                if (!isEncrypted)
                {
                    resBox.Text = encodedStr;
                    isEncrypted = true;
                }
                else
                {

                    resBox.Text = ByteToString(encodedBytes, keyBytes);
                    isEncrypted = false;
                }

                startBtn.Content = "Start";
                startBtn.IsEnabled = true;
            }
            else
            {
                resBox.Text = "Fill out the form correctly!";
            }
        }

        private string ByteToString(byte[] textBytes, byte[] keyBytes)
        {
            byte[] encodedBytes = FileEncrypt.EncryptOrDecrypt(textBytes, keyBytes);
            string encodedString = Encoding.UTF8.GetString(encodedBytes);

            return encodedString;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            isCancelled = true;
            prBar.Value = 0;
            startBtn.IsEnabled = true;
        }
    }
}
