using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Telekocsi
{
    class Program
    {
        struct autok    //1.feladat
        {
            public string indulas;
            public string cel;
            public string rendszam;                                       //Az adatok tárolásához szükséges 
            public string telefonszam;
            public int ferohely;
        }
        static autok[] adatok = new autok[1000];                        //max 1000 sor
        struct igenyek
        {
            public string azonosito;
            public string indulas;
            public string cel;                                          //Az adatok tárolásához szükséges 
            public int szemelyek;
            public string rendszam;
            public string telefonszam;
            public int ok;
        }
        static igenyek[] igenyekek = new igenyek[500];
        static void Main(string[] args)
        {
            string[] nyersadat = File.ReadAllLines("autok.csv");        // Adat beolvasása fájlból
            int soroksz = 0;                                            //sorok száma a fájlban
           
            
            for (int k = 1; k < nyersadat.Count(); k++)
            {
                string[] sorok = nyersadat[k].Split(';');                   //Az adatok elválasztása pontosvesszővel
                adatok[soroksz].indulas = sorok[0];
                adatok[soroksz].cel = sorok[1];
                adatok[soroksz].rendszam = sorok[2];                        //Változók feltöltése
                adatok[soroksz].telefonszam = sorok[3];
                adatok[soroksz].ferohely = Convert.ToInt32(sorok[4]);
                soroksz++;
            }

            int autoksz = soroksz;                                      //Ennyi autó van

            string[] nyersadat2 = File.ReadAllLines("igenyek.csv");
            soroksz = 0;                                                //sorok száma a fájlban
            for (int k = 1; k < nyersadat2.Count(); k++)   
            {
                string[] sorok2 = nyersadat2[k].Split(';');             //Az adatok elválasztása pontosvesszővel
                igenyekek[soroksz].azonosito = sorok2[0];
                igenyekek[soroksz].indulas = sorok2[1];               //Változók feltöltése
                igenyekek[soroksz].cel = sorok2[2];
                igenyekek[soroksz].szemelyek = Convert.ToInt32(sorok2[3]);
                soroksz++;
            }
            int igenyekszama = soroksz;                                  //Ennyi igény van
            //2.feladat
            Console.WriteLine("2. feladat");
            Console.WriteLine($"\t{autoksz}autós hirdet fuvart");
            //3.feladat
            int osszferohely = 0;
            for (int i = 0; i < autoksz; i++)
            {
                if (adatok[i].indulas == "Budapest" && adatok[i].cel == "Miskolc")
                {
                    osszferohely += adatok[i].ferohely;                         //férőhelyek számítása
                }
            }
            Console.WriteLine("3. feladat");
            Console.WriteLine($"\tÖsszesen {osszferohely} férőhelyet hirdettek az autósok Budapestről Miskolcra");
            //4.feladat           
            int j;
            int kulonbozo = 0;
            string[] indulasescel = new string[1000];                     //útvonalak adatok tárolása
            int[] ferohelyossz = new int[1000];
            for (int i = 0; i < 1000; i++) 
            { 
                ferohelyossz[i] = 0; 
            }
            for (int i = 0; i < autoksz; i++)
            {
                j = 0;
                while ((j <= kulonbozo) && (adatok[i].indulas + "-" + adatok[i].cel !=indulasescel[j]))
                {
                    j++;
                }
                if (j > kulonbozo)                                                              //különböző útvonalak megszámolása
                {
                    kulonbozo++;
                    indulasescel[kulonbozo] = adatok[i].indulas + "-" + adatok[i].cel;
                }
            }           
            for (int i = 0; i < autoksz; i++)
            {
                for (j = 0; j < kulonbozo; j++)
                {
                    if (indulasescel[j] == adatok[i].indulas + "-" + adatok[i].cel)         //megszámolja
                    {
                        ferohelyossz[j] += adatok[i].ferohely;
                    }
                }
            }           
            int max = ferohelyossz[0];
            int maxmax = 0;
            for (j = 0; j < kulonbozo; j++)
            {
                if (ferohelyossz[j] > max)                  //maximum keresés
                {
                    max = ferohelyossz[j];
                    maxmax = j;
                }
            }
            Console.WriteLine("4. feladat");
            Console.WriteLine($"\tA legtöbb férőhelyet({ferohelyossz[maxmax]}-t) a {indulasescel[maxmax]} útvonalon ajánlották fel a hirdetők");
            //5.feladat
            Console.WriteLine("5. feladat");
            for (int i = 0; i < igenyekszama; i++)
            {
                for (j = 0; j < autoksz; j++)
                {
                    if (igenyekek[i].indulas == adatok[j].indulas && igenyekek[i].cel == adatok[j].cel && igenyekek[i].szemelyek < adatok[j].ferohely)
                    {
                        Console.WriteLine($"{igenyekek[i].azonosito}=>{adatok[j].rendszam}");
                        igenyekek[i].telefonszam = adatok[j].telefonszam;
                        igenyekek[i].rendszam = adatok[j].rendszam;                                                     //Igényekhez találatok
                        igenyekek[i].ok = 1;
                    }
                }
            }
            //6.feladat
            Console.WriteLine("6. feladat");
            FileStream nev = new FileStream("utasuzenetek.txt", FileMode.Create);                       //Fájl létrehozása 
            StreamWriter kiirat = new StreamWriter(nev);
            for (int i = 0; i < igenyekszama; i++)
            {
                if (igenyekek[i].ok == 1)
                {
                    kiirat.WriteLine($"{igenyekek[i].azonosito}: Rendszám: {adatok[i].rendszam}, Telefonszám: {adatok[i].telefonszam}");
                    Console.WriteLine($"{igenyekek[i].azonosito}: Rendszám: {igenyekek[i].rendszam}, Telefonszám: {igenyekek[i].telefonszam}");              
                                                                                  
                                                                                                        //Fájl feltöltése adatokkal
                }
                else
                {
                    kiirat.WriteLine($"{igenyekek[i].azonosito}: Sajnos nem sikerült autóttalálni");                  
                    Console.WriteLine($"{igenyekek[i].azonosito}: Sajnos nem sikerült autót találni");
                }
            }
            kiirat.Close();                                                        
            nev.Close();
            Console.ReadKey();
        }
    }    
}
