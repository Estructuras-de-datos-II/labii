using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Huffman Test");
            Console.WriteLine("Escriba el mensaje a comprimir:");
            string mensaje = "";
            Libreria_ED_II_F.HuffmanFinal.Comprimir MHFC = new Libreria_ED_II_F.HuffmanFinal.Comprimir(mensaje);

            Console.WriteLine("El mensaje comprimido es:");
            Console.WriteLine(MHFC.mensajeComprimido);

            Console.Clear();

            Console.WriteLine("El mensaje original es:");
            Libreria_ED_II_F.HuffmanFinal.Descomprimir MHFD = new Libreria_ED_II_F.HuffmanFinal.Descomprimir(MHFC.mensajeComprimido);
            Console.WriteLine(MHFD.mensajeDescomprimido);
        }
    }
}
