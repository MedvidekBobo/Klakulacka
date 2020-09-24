using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Kalkulator.Classes
{
    public class Kalkulacka : INotifyPropertyChanged
    {

        /// <summary>
        /// Definice atributu změny vlastnosti
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// List čísel pro výpočty
        /// </summary>
        private List<double?> lstCisla = new List<double?>();

        /// <summary>
        /// Vrátí počet číslic ve vzorci (aby bylo možné použít rovnítko, musejí být alespoň dvě čísla)
        /// </summary>
        public int PocetCislic { get; private set; }

        /// <summary>
        /// List operací pro výpočty (nebude obsahovat rovnítko na konci)
        /// </summary>
        private List<Operace> lstOperace = new List<Operace>();

        /// <summary>
        /// Výsledek výpočtu
        /// </summary>
        public double? Vysledek { get; private set; }

        /// <summary>
        /// Obsahuje textovou prezentaci listu čísel a operací
        /// </summary>
        public string Fronta { get => this.ToString(); }

        /// <summary>
        /// Vrátí informaci o tom, jestli je ve vzorci rovnítko
        /// </summary>
        public bool JeRovnitko { get => lstOperace.Contains(Operace.RovnaSe); }

        public bool RemovedLast { get; private set; }

        /// <summary>
        /// Inicializace kalkulačky
        /// </summary>
        public Kalkulacka()
        {
            this.Reset();
        }

        /// <summary>
        /// Metoda pro smazání kalkulačky
        /// </summary>
        public void Reset()
        {
            lstCisla.Clear();
            lstOperace.Clear();
            Vysledek = null;
            RemovedLast = false;
            VyvolejZmenu(nameof(Fronta));
        }

        /// <summary>
        /// Metoda pro smazání poslední operace a posledního čísla
        /// </summary>
        public void ResetLast()
        {
            if (!RemovedLast)
            {
                if (lstOperace.Contains(Operace.RovnaSe))
                    this.Reset();
                RemovedLast = true;
                VyvolejZmenu(nameof(Fronta));
            }
        }

        /// <summary>
        /// Přidání čísla do seznamu čísel
        /// </summary>
        /// <param name="cislo"></param>
        public void PridejCislo(double? cislo)
        {
            if (!(cislo is null))
            {
                lstCisla.Add(cislo);
                PocetCislic++;
                lstOperace.Add(Operace.Nedefinovana);
                VyvolejZmenu(nameof(Fronta));
            }
        }

        /// <summary>
        /// Přidání typu operace do seznamu operací
        /// </summary>
        /// <param name="o"></param>
        public void PridejOperaci(Operace o)
        {
            if (o != Operace.Nedefinovana)
            {
                lstCisla.Add(null);
                lstOperace.Add(o);
                RemovedLast = false;
                VyvolejZmenu(nameof(Fronta));
            }
        }

        /// <summary>
        /// Metoda pro provedení výpočtů
        /// </summary>
        public void Spocitej()
        {

            List<double?> lstPomCisla = new List<double?>();
            List<Operace> lstPomOperace = new List<Operace>();
            double? mezivypocet;

            /*
            //ověření typu objektu
             
            if (zvire.GetType() == typeof(Delfin))
                (zvire as Delfin).Vyskoc();
            */

            //musím zajistit prioritu matematických operací (násobení/dělení dřív)
            int i = 0;

            if (lstOperace[lstOperace.Count - 1] == Operace.RovnaSe)
            {


                //procházím čísla 
                do
                {

                    //pokud je operace na aktuálním indexu+1 jiná než Nedefinovaná, pak jsem na pozici čísla
                    switch (lstOperace[i + 1])
                    {
                        case Operace.Scitani:
                            lstPomCisla.Add(lstCisla[i]);
                            lstPomOperace.Add(lstOperace[i]);
                            i++;
                            break;
                        case Operace.Odecitani:
                            lstPomCisla.Add(lstCisla[i]);
                            lstPomOperace.Add(lstOperace[i]);
                            i++;
                            break;
                        case Operace.Nasobeni:
                            //mám násobení 
                            //musím provést mezivýpočet aktuálního čísla a následujícího čísla
                            mezivypocet = lstCisla[i] * lstCisla[i + 2];
                            lstCisla[i + 2] = mezivypocet;
                            i += 2;
                            break;
                        case Operace.Deleni:
                            //mám dělení
                            //musím provést mezivýpočet čísel vlevo a vpravo od znaménka
                            if (lstCisla[i + 2] == 0)
                                throw new Exception("Nulou nelze dělit!");
                            else
                            {
                                mezivypocet = lstCisla[i] / lstCisla[i + 2];
                                lstCisla[i + 2] = mezivypocet;
                                i += 2;
                            }
                            break;
                        case Operace.RovnaSe:
                            lstPomCisla.Add(lstCisla[i]);
                            lstPomOperace.Add(lstOperace[i]);
                            lstPomCisla.Add(null);
                            lstPomOperace.Add(Operace.RovnaSe);
                            i++;
                            break;
                        case Operace.Nedefinovana:
                            //zde to znamená, že jsem na pozici operace
                            lstPomCisla.Add(null);
                            lstPomOperace.Add(lstOperace[i]);
                            i++;
                            break;
                    }

                } while (i < lstCisla.Count - 1); //&& (lstOperace[i + 1]) != Operace.RovnaSe);

            }
            else
                throw new ArithmeticException("Nelze provést výpočet, protože poslední operací není rovnítko!");

            //nyní bych měl mít v lstPomCisla a lstPomOperace jen sčítání a odečítání
            i = 1;

            double? vypocet;
            vypocet = 0;
            vypocet += lstPomCisla[0];

            do
            {

                if (lstPomOperace[i] == Operace.Scitani)
                    vypocet += lstPomCisla[i + 1];
                else if (lstPomOperace[i] == Operace.Odecitani)
                    vypocet -= lstPomCisla[i + 1];
                i++;

            } while (i < lstPomCisla.Count - 1);

            Vysledek = vypocet;

        }

        public string OperaceToString(Operace o)
        {
            switch (o)
            {
                case Operace.Scitani:
                    return " + ";
                case Operace.Odecitani:
                    return " - ";
                case Operace.Nasobeni:
                    return " × ";
                case Operace.Deleni:
                    return " ÷ ";
                case Operace.RovnaSe:
                    return " = ";
                default:
                    return "";
            }

        }

        /// <summary>
        /// Metoda pro vyvolání údálosti změny vlastnosti
        /// </summary>
        /// <param name="vlastnost">jméno vlastnosti</param>
        protected void VyvolejZmenu(string vlastnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }

        /// <summary>
        /// Textový výpis kalkulačky
        /// </summary>
        /// <returns>Vrátí seznam čísel a operací s nimi</returns>
        public override string ToString()
        {
            string vypis = "";

            for (int i = 0; i <= lstCisla.Count - 1; i++)
            {
                if (!(lstCisla[i] is null))
                    vypis += lstCisla[i].ToString();
                else
                    vypis += OperaceToString(lstOperace[i]);
            }
            return vypis;

        }
    }
}
