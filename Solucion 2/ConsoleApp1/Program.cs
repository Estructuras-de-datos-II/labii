﻿using System;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Escriba mensaje a comprimir:");
            string mensaje = Console.ReadLine();


            Libreria_ED_II_F.HuffmanFinal.Comprimir MHFC = new Libreria_ED_II_F.HuffmanFinal.Comprimir(mensaje);
            Console.WriteLine("El mensaje comprimido es:");
            Console.WriteLine(MHFC.mensajeComprimido);

            Console.Read();
            Console.ReadKey();
            Console.WriteLine("El mensaje descomprimido es:");
            Libreria_ED_II_F.HuffmanFinal.Descomprimir MHFD = new Libreria_ED_II_F.HuffmanFinal.Descomprimir(MHFC.mensajeComprimido);
            Console.WriteLine(MHFD.mensajeDescomprimido);
            
            
            
        }
    }
}
