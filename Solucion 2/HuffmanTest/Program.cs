using System;
using System.Linq;

namespace HuffmanTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Huffman Test");
            Console.WriteLine("Escriba el mensaje a comprimir:");
            string mensaje = Console.ReadLine();

            HuffmanFinal.Comprimir MHFC = new HuffmanFinal.Comprimir(mensaje);
            Console.WriteLine("El mensaje comprimido es:");
            Console.WriteLine(MHFC.mensajeComprimido);
            
            Console.Clear();

            Console.WriteLine("El mensaje original es:");
            HuffmanFinal.Descomprimir MHFD = new HuffmanFinal.Descomprimir(MHFC.mensajeComprimido);
            Console.WriteLine(MHFD.mensajeDescomprimido);
            Console.ReadKey();
        }
    }
}
