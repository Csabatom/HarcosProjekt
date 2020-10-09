using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HarcosProjekt
{
    class Program
    {
        static int menuPont;
        static int menu()
        {
            do {
                Console.Clear();
                Console.WriteLine("Menü");
                Console.WriteLine("1 - Megküzdés egy harcossal");
                Console.WriteLine("2 - Gyógyulni");
                Console.WriteLine("3 - Kilépni");

                Console.Write("Válasszon menüpontot: ");
                Console.ResetColor();
                try {
                    while (!int.TryParse(Console.ReadLine(), out menuPont) || menuPont < 1 || menuPont > 6) {
                        MessageBox.Show("Hiba, nincs ilyen menüpont!");
                        break;
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            while (menuPont < 1 || menuPont > 6);
            return menuPont;
        }

        static void Main(string[] args)
        {
            var harcosLista = new List<Harcos>();

            try {
                StreamReader r = new StreamReader("harcosok 1.csv", Encoding.GetEncoding("iso-8859-1"));
                while (!r.EndOfStream) {
                    string[] st = r.ReadLine().Split(';');

                    harcosLista.Add(new Harcos(st[0], int.Parse(st[1])));
                }
                r.Close();
            }
            catch (FileNotFoundException) {
                Console.WriteLine("A fájl nem található!");
            }
            catch (Exception ex) {
                Console.WriteLine("Hiba: " + ex);
            }

            Console.Write("Mi legyen a harcos neve? : ");
            string bekertHarcosNev = Console.ReadLine();
            int bekertStatuszSablon = 0;
            bool bevitel;
            do {
                try {
                    Console.Write("Milyen státusz sablona legyen a  harcosának? (1, 2, 3): ");
                    bevitel = int.TryParse(Console.ReadLine(), out bekertStatuszSablon);
                    while (!bevitel)
                    {
                        MessageBox.Show("Hibás érték!");
                        break;
                    }
                    if (bekertStatuszSablon != 1 && bekertStatuszSablon != 2 && bekertStatuszSablon != 3 && bevitel)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("A megadott érték csak 1, 2 vagy 3 lehet!\nNyomja meg az ENTER-t!");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                    Console.Clear();
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            while (bekertStatuszSablon != 1 && bekertStatuszSablon != 2 && bekertStatuszSablon != 3);

            harcosLista.Add(new Harcos(bekertHarcosNev, bekertStatuszSablon));
            int korSzamlalo = 0;
            do {
                Console.Clear();
                Console.WriteLine("A többi harcos: \n");
                for (int i = 0; i < harcosLista.Count - 1; i++) {
                    Console.WriteLine((i + 1) + ". " + harcosLista[i]);
                }
                Console.WriteLine();
                Console.WriteLine("Az Ön harcosa: \n" + (harcosLista.Count) + ". " + harcosLista[harcosLista.Count - 1]);
                Console.WriteLine("\nNyomjon egy ENTER-t a menü megjelenítéséhez!");
                Console.ReadKey();

                int menuPont;
                do
                {
                    menuPont = menu();
                    switch (menuPont)
                    {
                        case 1:
                            int sorszam = 0;
                            bool beker;
                            do {
                                try {
                                    Console.Write("Melyik harcossal szeretne megküzdeni? Sorszáma: ");
                                    beker = int.TryParse(Console.ReadLine(), out sorszam);
                                    while (!beker) {
                                        MessageBox.Show("Hiba, csak számot adhat meg!");
                                        Console.Write(new string(' ', Console.BufferWidth));
                                        break;
                                    }
                                    if ((sorszam < 1 || sorszam > harcosLista.Count) && beker) {
                                        MessageBox.Show("Nem létező sorszámú harcost választott!");
                                        Console.Write(new string(' ', Console.BufferWidth));
                                    }
                                }
                                catch (Exception e) {
                                    Console.WriteLine(e);
                                }
                            }
                            while (sorszam < 1 || sorszam > harcosLista.Count);
                            harcosLista[harcosLista.Count - 1].Megkuzd(harcosLista[sorszam - 1]);
                            break;
                        case 2:
                            harcosLista[harcosLista.Count - 1].Gyogyul();
                            break;
                        case 3:
                            MessageBox.Show("Köszönjük a játékot");
                            Environment.Exit(0);
                            break;
                    }
                }
                while (menuPont == 3);
                korSzamlalo++;
                if (korSzamlalo % 3 == 0) {
                    Random r = new Random();

                    int kivelKuzdMeg = r.Next(harcosLista.Count - 1);

                    harcosLista[harcosLista.Count - 1].Megkuzd(harcosLista[kivelKuzdMeg]);

                    Console.Clear();
                    Console.WriteLine("Ebben a körben az Ön harcosa fog megküzdeni egy véletlenül kiválasztott harcossal");
                    Console.WriteLine("\nAz ellenfél: " + harcosLista[kivelKuzdMeg]);

                    Console.WriteLine("\nA csata után mindeegyik harcos gyógyul.");
                    foreach (var item in harcosLista)
                    {
                        item.Gyogyul();
                    }
                    Console.WriteLine("\nNyomjon egy ENTER-t a menü megjelenítéséhez!");
                    Console.ReadKey();
                }
            }
            while (menuPont != 3);


            Console.ReadKey();
        }
    }
}
