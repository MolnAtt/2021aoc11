using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021aoc11
{
    class Program
    {
        class Barlang
        {
            class Dumbo
            {
                public int e { get; private set; }
                private Barlang barlang;
                int x, y;
                public Dumbo(int e, Barlang barlang, int x, int y) 
                { 
                    this.e = e; 
                    this.barlang = barlang;
                    (this.x, this.y) = (x, y);
                }
                public void Tölt() { if (e!=-1) e++; }
                bool Már_feltöltődött { get => 10 <= e; }
                public void Bök() { if (Már_feltöltődött) Villan(); }
                public void Feltámaszt() { if (e < 0) e = 0; }
                void Villan()
                {
                    barlang.villanásszámláló++;
                    e = -1;
                    foreach (Dumbo d in Szomszédai)
                    {
                        d.Tölt();
                        d.Bök();
                    }
                }
                static (int, int)[] Nyolc_szomszéd_helyei = new (int,int)[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
                public IEnumerable<Dumbo> Szomszédai 
                {
                    get => Nyolc_szomszéd_helyei
                       .Select(pár => (x + pár.Item1, y + pár.Item2))
                       .Where(pár => barlang.Létező_pozíció(pár.Item1, pár.Item2))
                       .Select(pár => barlang.rács[pár.Item1, pár.Item2]);
                }
                public void Kiír()
                {
                    if (0 < e && e <= 9)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    if (0 == e )
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (9 < e)
                        Console.ForegroundColor = ConsoleColor.Red;
                    if (e < 0)
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write(e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            
            public int t { get; private set; }
            public int villanások_száma_az_előző_körig { get; private set; }
            int N, M;
            Dumbo[,] rács;
            public int villanásszámláló;
            public Barlang(string path)
            {
                villanások_száma_az_előző_körig = 0;
                t = 0;
                string[] sorok = System.IO.File.ReadAllLines(path);
                N = sorok.Length;
                M = sorok.First().Length;
                rács = new Dumbo[N,M];
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < M; j++)
                        rács[i, j] = new Dumbo(int.Parse(sorok[i][j].ToString()), this, i,j);
            }
            bool Létező_pozíció(int x, int y) => 0 <= x && x < N && 0 <= y && y < M;
            void Összes_feltöltése() { foreach (Dumbo dumbo in rács) dumbo.Tölt(); }
            void Összes_megbökése() { foreach (Dumbo dumbo in rács) dumbo.Bök(); }
            void Összes_feltámasztása() { foreach (Dumbo dumbo in rács) dumbo.Feltámaszt(); }
            public void Szimulál(Func<Barlang, bool> predicate, bool debug = false, int speed =0)
            {
                Diagnosztika($"Beolvasáskor:");
                while (predicate(this))
                {
                    villanások_száma_az_előző_körig = villanásszámláló;
                    t++;
                    /*
                    */
                    Összes_feltöltése();
                    //Diagnosztika($"Az {i}. töltés után:", debug, speed);
                    Összes_megbökése();
                    //Diagnosztika($"Az {i}. böködés után:", debug, speed);
                    Összes_feltámasztása();
                    Diagnosztika($"Az idő: {t}", debug, speed);
                }
            }
            void Diagnosztika(string megj = "", bool debug=false, int speed=0)
            {
                if (!debug)
                {
                    System.Threading.Thread.Sleep(speed);
                    Console.Clear();
                }
                Console.WriteLine($" --------- {megj} ---------");
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                        rács[i, j].Kiír();
                    Console.WriteLine();
                }
                Console.WriteLine($" --------- villanások száma: {villanásszámláló} db ---------");
            }
        }
        static void Main(string[] args)
        {
            /** /
            Barlang barlang = new Barlang("teszt.txt");
            /*/
            Barlang barlang = new Barlang("input.txt");
            /**/

            /** /
            barlang.Szimulál(x => barlang.t<100 , false, 200);
            /*/
            barlang.Szimulál(x => barlang.villanások_száma_az_előző_körig + 100 != barlang.villanásszámláló, false, 200);
            /**/

            Console.ReadKey();
        }
    }
}
