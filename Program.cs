using System.Collections.Immutable;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftraySolutionsTestApp
{
    internal class Program
    {
        static List<int> initialParkingOrder = new List<int>();     // List<int> objekti su koristeni zbog automatizovane provjere tokom inputChecker() validacije unosa
        static List<int> desiredParkingOrder = new List<int>();
        static List<int[]> sortingSteps = new List<int[]>();    // Finalni list objekt koji sprema nizove tokom svakog swapa/koraka

        public static bool inputChecker() {  
            string[] stringBuffer;  // String array koristen tokom ucitavanja unosa
            int[] intBuffer;    // integer array koristen za parsiranje string-a unosa
            bool freeSpace = false; // Bool vrijednost koristen za provjeru postojanja "0" vrijednosti zbog potrebe za praznim mjestom

                while (true)    // Petlja za unos prvobitnog stanja parkinga
                {
                    Console.WriteLine("Unesite prvobitnu postavu parkinga: ");
                    stringBuffer = Console.ReadLine().Split(' '); // Split() funkcija koristena zbog prepoznavanja ' ' tokom unosenja

                    intBuffer = Array.ConvertAll(stringBuffer, Int32.Parse);    //Konverzija unosa

                    foreach (int i in intBuffer) {      // Dodavanje int vrijednosti u listu
                        initialParkingOrder.Add(i);
                        if (i == 0) freeSpace = true;       // Prepoznavanje potrebnog praznog mjesta unutar unesene postave   
                    }


                    if (initialParkingOrder.Distinct().Count() == initialParkingOrder.Count() && freeSpace) {   // Provjera da li u unosu postoje duplikati i prazno mjesto
                        freeSpace = false;
                        break;
                    }

                    initialParkingOrder.Clear(); // U slucaju da je unos neispravan, prazni se List objekt za novi unos 
                    Console.Clear();

                    Console.WriteLine((freeSpace) ? "Neispravan unos: Duplikati" : "Neispravan unos: Parking nema slobodnog mjesta!"); // Ispis razloga pogreske
                }

            while (true)    // Petlja za unos ciljanog stanja parkinga sa istom logikom kao prijasnja petlja
            {
                Console.WriteLine("\nUnesite ciljanu postavu parkinga: ");
                stringBuffer = Console.ReadLine().Split(' ');

                intBuffer = Array.ConvertAll(stringBuffer, Int32.Parse);

                foreach (int i in intBuffer) { 
                    desiredParkingOrder.Add(i);
                    if (i == 0) freeSpace |= true;
                }

                    if (desiredParkingOrder.Distinct().Count() == desiredParkingOrder.Count() && freeSpace) {  
                        freeSpace = false;
                        break;
                    }

                    desiredParkingOrder.Clear();
                    Console.Clear();
                    Console.WriteLine((freeSpace) ? "Neispravan unos: Duplikati" : "Neispravan unos: Parking nema slobodnog mjesta!");
                }

                List<int> tempInit = new List<int>(initialParkingOrder);    // Dodani novi List<> objekti radi provjere legitimnosti 
                List<int> tempFinal = new List<int>(desiredParkingOrder);

                tempInit.Sort();    // Sortirati privremene liste kako bi se usporedile sekvence unesenih brojeva 
                tempFinal.Sort();

                if (Enumerable.SequenceEqual(tempInit,tempFinal))    // Provjera slicnosti kroz if() i SequenceEqual(objekt1, objekt2) funkcije 
                    return true;
                return false;    
        }

        public static void parkingSorter() {    // Glavno jelo funkcija koja nakon provjere validnosti unosa zapocinje sortiranje unosa prema datom cilju
            List<int[]> steps = new List<int[]>();   // List<int[]> objekt koji sadrzi nizove spremljene tokom svake izmjene pozicija 
            
            int parkingSize = initialParkingOrder.Count();
            int openPos=0, finalPos=0;

            bool firstSwap = false;     // Varijabla koristena kao flag za prvi i najbrzi swap koji se izvrsava ukoliko je nula bas na poziciji jednog od ostalih brojeva

            List<int> currentPO = new List<int>(initialParkingOrder);    // Deklarisanje privremenog List<> objekta za obradu podataka
            List<int> finalPO = new List<int>(desiredParkingOrder);

            while(!Enumerable.SequenceEqual(currentPO, finalPO)) {  // Finalna petlja koja se izvrsava sve dok postava ne postigne ciljni redoslijed
                if (!firstSwap) {   // Uslov za prvu for petlju koja izvrsava najbrzu izmjenu ukoliko daljni uslovi budu ispunjeni
                    for (int i = 0; i < parkingSize; i++) {
                        finalPos = finalPO.IndexOf(currentPO[i]);    // Ciljna pozicija datog broja u nizu
                        openPos = currentPO.IndexOf(0);  // Pozicija praznog mjesta u nizu
                        if (finalPos == openPos) {  // Ukoliko je prazno mjesto ciljno za neki broj, slijedi zamjena vrijdnosti 
                            currentPO[openPos] = currentPO[i];
                            currentPO[i] = 0;
                            steps.Add(currentPO.ToArray()); // Proces dodavanja nizova u List<int[]> kolekciju objekata
                            firstSwap = true;   // Potvrda da je prva izmjena zavrsena i time se nastavlja sa preostalim brojevima
                            break;
                        }
                    }
                }
                else { // U slucaju da je prva izmjena zavrsena, slijede preostale izmjene 
                    for (int i = 0;i < parkingSize; i++) {
                        finalPos = finalPO.IndexOf(currentPO[i]);
                        openPos = currentPO.IndexOf(0);

                        if (finalPos != i) {    // Ukoliko je broj vec na ciljnom mjestu, promjene nisu potrebne i for petlja se nastavlja
                            if (finalPos != openPos)
                            {
                                currentPO[openPos] = currentPO[finalPos];
                                currentPO[finalPos] = 0;
                            }
                            else { 
                                currentPO[openPos] = currentPO[i];
                                currentPO[i] = 0;
                            }
                            steps.Add(currentPO.ToArray());    // Ukoliko je izmjena izvrsena, stanje nakon promjene se unosi u zavrsni List<int[]> objekt
                            break; // for petlja se nakon dodavanja stanja u List-u prekida te se ili nastavlja sa ostalim brojevima, ili funkcija zavrsava
                        }
                    }
                }
            }

            Console.Clear();

            Console.WriteLine("Ukupno koraka: " + steps.Count());   // Ispis ukupnog broja koraka

            foreach (int[] step in steps)   // Ispis koraka odnosno postave brojeva tokom iteracija ne ukljucujuci pocetno stanje
            {
                foreach (int carNum in step)
                {
                    Console.Write(carNum + " ");
                }
                Console.WriteLine();
            }

            sortingSteps = steps;   // Spremanje koraka u staticnu varijablu jer funkcija prekida i lokalne varijable se brisu
        }
        
        static void Main(string[] args)
        {
            while (true) { // While loop-ing je koristen kao soft-exception handling i validacijski metod, te se nudi prilika za ponovni unos po potrebi
                if (inputChecker()) // Unos parking postava zapocinje u inputChecker funkciji koja vraca bool vrijednost true ukoliko su kombinacije ispravne
                    break; // Izlaz iz while petlje u slucaju da je unos validan
                
                Console.WriteLine("Uneseni redoslijedi parking postava se ne podudaraju!"); 
                Console.ReadKey();

                initialParkingOrder.Clear(); // Objekti listi datih unosa se prazne i while petlja krece iznova
                desiredParkingOrder.Clear();
                Console.Clear();
            }

            parkingSorter();    // Poziv void funkcije parkingSorter() koja na kraju sortiranja ispisuje korake i sprema ih u globalnu varijablu

            Console.ReadKey();
        }
    }
}
