using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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

namespace RSA_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int numPrimes = 20000;
        public static RSA rsa = new RSA(numPrimes);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PrimeNumbersButton_Click(object sender, RoutedEventArgs e)
        {
            PrimeNumbers window = new PrimeNumbers();
            window.Show();
        }

        private void ButtonGeneratKey_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(PTxtBox.Text) || string.IsNullOrEmpty(QTxtBox.Text))
            {
                MessageBox.Show("Nie podano poprawnego P i Q");
                return;
            }else
            {
                int p= Convert.ToInt32(PTxtBox.Text);
                int q = Convert.ToInt32(QTxtBox.Text);
                if(!rsa.PrimeNumbersDict.TryGetValue(p, out int resultP))
                {
                    MessageBox.Show("P nie jest liczbą pierwszą lub nie mieści się w podanym przedziale("+ numPrimes+")");
                    return;
                }
                if(!rsa.PrimeNumbersDict.TryGetValue(q, out int resultQ))
                {
                    MessageBox.Show("Q nie jest liczbą pierwszą lub nie mieści się w podanym przedziale(" + numPrimes + ")");
                    return;
                }

                if(p < 100)
                {
                    MessageBox.Show("P powinno być conajmniej 3 cyfrowe");
                    return;
                }

                if (q < 100)
                {
                    MessageBox.Show("Q powinno być conajmniej 3 cyfrowe");
                    return;
                }

                rsa.GenerateKey(p,q);
                privKeyGenTxtBox.Text = rsa.D + ", " + rsa.N;
                publicKeyGenTxtBox.Text = rsa.E + ", " + rsa.N;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] delimiter = { ",", " " };
                var key = PublicKeyTxtBox.Text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                int E = Convert.ToInt32(key[0]);
                int N = Convert.ToInt32(key[1]);

                string input = MessageTxtBox.Text;

                List<string> list = new List<string>();

                foreach (var item in input)
                {
                    var result = rsa.Encrypt(item, E, N);
                    list.Add(result.ToString());
                }
                CipherTxtBox.Text= string.Join(", ", list);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] delimiter = { ",", " " };
                var key = PrivateKeyTxtBox.Text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                int D = Convert.ToInt32(key[0]);
                int N = Convert.ToInt32(key[1]);
                
                string[] input = CipherInputTxtBox.Text.Split(delimiter,StringSplitOptions.RemoveEmptyEntries);

                List<byte> list = new List<byte>();

                foreach (var item in input)
                {
                    if(BigInteger.TryParse(item, out BigInteger res))
                    {
                        var result = rsa.Decrypt(res, D, N);
                        list.Add((byte)result);
                    }else
                    {
                        MessageBox.Show("Wystąpił błąd przy odszfyrowaniu!");
                        return;
                    }               
                }
                MessageOutTxtBox.Text = Encoding.ASCII.GetString(list.ToArray());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SaveKeysBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(publicKeyGenTxtBox.Text))
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    {
                        File.WriteAllText(openFileDialog.FileName, privKeyGenTxtBox.Text);
                    }

                }
            }
            else
            {
                MessageBox.Show("Nie wygenerowano klucza!");
                return;
            }
        }

        private void SavePublicKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(publicKeyGenTxtBox.Text))
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    {
                        File.WriteAllText(openFileDialog.FileName, publicKeyGenTxtBox.Text);
                    }
           
                }
            }
            else
            {
                MessageBox.Show("Nie wygenerowano klucza!");
                return;
            }
        }

        private void LoadPrivKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
               privKeyGenTxtBox.Text= File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void LoadPublicKeyBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                publicKeyGenTxtBox.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void SaveCipherBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CipherTxtBox.Text))
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    {
                        File.WriteAllText(openFileDialog.FileName, CipherTxtBox.Text);
                    }

                }
            }
            else
            {
                MessageBox.Show("Nie wygenerowano szyfrogramu!");
                return;
            }
        }

        private void LoadCipherBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                CipherInputTxtBox.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                MessageTxtBox.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageOutTxtBox.Text))
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    {
                        File.WriteAllText(openFileDialog.FileName, MessageOutTxtBox.Text);
                    }

                }
            }
            else
            {
                MessageBox.Show("Nie podano wiadomośći!");
                return;
            }
        }
    }
}
