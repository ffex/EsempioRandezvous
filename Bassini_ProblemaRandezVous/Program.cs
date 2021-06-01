using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bassini_ProblemaRandezVous
{
    
    class Program
    {
        static SemaphoreSlim SemMetodo1 = new SemaphoreSlim(0);// inizializzo i semafori a rossi
        static SemaphoreSlim SemMetodo2 = new SemaphoreSlim(0);// inizializzo i semafori a rossi
        //i Vettori sono globali
        static int[] V = new int[1000];
        static int[] W = new int[1000];
        static void Main(string[] args)
        {
            while (true) { 
            Thread t1 = new Thread(() => Metodo1());
            Thread t2 = new Thread(() => Metodo2());
            t1.Start();
            t2.Start();
            Console.ReadLine();
            }
        }
        static void Metodo1()
        {
            Random r = new Random();
            // inizializzo a 1001 perchè so che al primo ciclo verra sovrascritto dal primo valore
            int minV =1001;
            int minW = 1001;
            for (int i = 0; i < V.Length; i++)
            {
                V[i] = r.Next(0,1000);
                if (V[i] < minV)
                {
                    minV = V[i];
                }
            }
            //arrivato qua io ho V riempito e su min ho il più piccolo valore di V
            //DA QUI HO UNA ZONA CRITICA perchè devo intervenire anche su W.
            // la domanda che mi pongo è: sarà già pieno W?
            SemMetodo1.Release(); // Rilascio il semaforo che mi permette di continuare sul METODO2
            SemMetodo2.Wait(); //Aspetto che il METODO2 mi dica di continuare
            for (int i = 0; i < W.Length; i++)
            {
                
                if (W[i] < minW)
                {
                    minW = W[i];
                }
            }
            int min;
            if (minW < minV)
            {
                min = minW;
            }else
            {
                min = minV;
            }
            Console.WriteLine("minimo: " +min);
        }
        static void Metodo2()
        {
            Random r = new Random();

            
            int sommaW=0;
            int sommaV = 0;
            for (int i = 0; i < W.Length; i++)
            {
                W[i] = r.Next(0, 1000);
                sommaW = sommaW + W[i];
            }
            //arrivato qua ho W pieno e la somma di tutti i suoi valori

            //DA QUI HO UNA ZONA CRITICA perchè devo intervenire anche su V.
            // la domanda che mi pongo è: sarà già pieno V?
            SemMetodo2.Release(); // Rilascio il semaforo che mi permette di continuare sul METODO1
            SemMetodo1.Wait(); //Aspetto che il METODO1 mi dica di continuare

            //Quando son qua son sicuro che anche V è pieno
            for (int i = 0; i < V.Length; i++)
            {
                
                sommaV = sommaV + V[i];
            }

            int media = (sommaV + sommaW) / (W.Length + V.Length);
            Console.WriteLine("Media: " + media);
        }
    }
}
