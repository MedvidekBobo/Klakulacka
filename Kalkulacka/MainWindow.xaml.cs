using Kalkulator.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Kalkulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Kalkulacka kalk = new Kalkulacka();

        private bool bBylaOperace = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = kalk;
        }

        private void Cislo_Click(object sender, RoutedEventArgs e)
        {
            string sZmacknuto = ((Button)sender).Content.ToString();
            sZmacknuto = sZmacknuto.ToUpper();

            switch (sZmacknuto)
            {
                case ",":
                    if (!tblDisplay.Text.Contains(sZmacknuto[0]))
                        tblDisplay.Text += sZmacknuto;
                    break;

                case "C":
                    kalk.Reset();
                    tblDisplay.Text = "0";
                    break;

                case "CE":
                    kalk.ResetLast();
                    tblDisplay.Text = "0";
                    break;

                case "+":
                    if (kalk.JeRovnitko)
                    {
                        kalk.Reset();
                        bBylaOperace = false;
                    }
                    if (!bBylaOperace)
                    {
                        NastavCislo();
                        kalk.PridejOperaci(Operace.Scitani);
                        bBylaOperace = true;
                    }
                    break;

                case "-":
                    if (kalk.JeRovnitko)
                    {
                        kalk.Reset();
                        bBylaOperace = false;
                    }
                    if (!bBylaOperace)
                    {
                        NastavCislo();
                        kalk.PridejOperaci(Operace.Odecitani);
                        bBylaOperace = true;
                    }
                    break;

                case "×":
                    if (kalk.JeRovnitko)
                    {
                        kalk.Reset();
                        bBylaOperace = false;
                    }
                    if (!bBylaOperace)
                    {
                        NastavCislo();
                        kalk.PridejOperaci(Operace.Nasobeni);
                        bBylaOperace = true;
                    }
                    break;

                case "÷":
                    if (kalk.JeRovnitko)
                    {
                        kalk.Reset();
                        bBylaOperace = false;
                    }
                    if (!bBylaOperace)
                    {
                        NastavCislo();
                        kalk.PridejOperaci(Operace.Deleni);
                        bBylaOperace = true;
                    }
                    break;

                case "=":
                    if (!bBylaOperace)
                    {
                        NastavCislo();
                        kalk.PridejOperaci(Operace.RovnaSe);
                        bBylaOperace = true;

                        //pokud mám alespoň dvě číslice, tak zkusím výpočet, jinak reset
                        if (kalk.PocetCislic >= 2)
                        {
                            try
                            {
                                kalk.Spocitej();
                                tblDisplay.Text = ((decimal)kalk.Vysledek).ToString();
                                svDisplej.ScrollToHorizontalOffset(tblDisplay.ActualWidth);
                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                kalk.Reset();
                                tblDisplay.Text = "0";
                            }
                        }
                        else
                            kalk.Reset();
                    }
                    break;

                //jakékoliv zmáčknuté číslo
                default:
                    if ((tblDisplay.Text == "0") || bBylaOperace)
                        tblDisplay.Text = "";
                    bBylaOperace = false;
                    tblDisplay.Text += sZmacknuto;
                    svDisplej.ScrollToHorizontalOffset(tblDisplay.ActualWidth);
                    break;
            }
        }

        private void NastavCislo()
        {
            if (tblDisplay.Text != "")
            {
                double cislo = double.Parse(tblDisplay.Text, CultureInfo.GetCultureInfo("cs"));
                kalk.PridejCislo(cislo);
                svFronta.ScrollToHorizontalOffset(tblFronta.ActualWidth);
            }
        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad0:
                    Cislo_Click(cmd0, e);
                    break;
                case Key.NumPad1:
                    Cislo_Click(cmd1, e);
                    break;
                case Key.NumPad2:
                    Cislo_Click(cmd2, e);
                    break;
                case Key.NumPad3:
                    Cislo_Click(cmd3, e);
                    break;
                case Key.NumPad4:
                    Cislo_Click(cmd4, e);
                    break;
                case Key.NumPad5:
                    Cislo_Click(cmd5, e);
                    break;
                case Key.NumPad6:
                    Cislo_Click(cmd6, e);
                    break;
                case Key.NumPad7:
                    Cislo_Click(cmd7, e);
                    break;
                case Key.NumPad8:
                    Cislo_Click(cmd8, e);
                    break;
                case Key.NumPad9:
                    Cislo_Click(cmd9, e);
                    break;
                case Key.Decimal:
                    Cislo_Click(cmdComma, e);
                    break;
                case Key.C:
                    Cislo_Click(cmdC, e);
                    break;
                case Key.Delete:
                    Cislo_Click(cmdCE, e);
                    break;
                case Key.Enter:
                    Cislo_Click(cmdRovnaSe, e);
                    break;
                case Key.Add:
                    Cislo_Click(cmdPlus, e);
                    break;
                case Key.Subtract:
                    Cislo_Click(cmdMinus, e);
                    break;
                case Key.Multiply:
                    Cislo_Click(cmdKrat, e);
                    break;
                case Key.Divide:
                    Cislo_Click(cmdDeleno, e);
                    break;
                default:
                    break;
            }

        }

    }
}
