//using Compress;
using Compression;
using System;
using System.IO;

namespace ConsoleCompressHuffman
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Compresor y Descompresor de Huffman\n\n");
                Console.WriteLine("Escribe el path y el nombre del archivo a comprimir");
                string filePath = Console.ReadLine();

                byte[] data = File.ReadAllBytes(filePath);
                HuffmanCompression huffmanCOmpressor = new HuffmanCompression();
                byte[] compressedData = huffmanCOmpressor.Compress(data);

                string fileName = filePath.Substring(0, filePath.IndexOf('.'));
                createFileE(compressedData, fileName);

                byte[] descompressedData = huffmanCOmpressor.Decompress(compressedData);
                createFileD(descompressedData, fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ubicacion o nombre del archivo incorrectos");
            }
            Console.WriteLine("Compresion Completada. Presione una tecla para continuar...");
            Console.ReadKey();
            Console.WriteLine("Desompresion Completada. Presione una tecla para terminar...");
            Console.ReadKey();
        }

        static void createFileE(byte[] data, string pathName)
        {
            using (FileStream fs = File.Create(pathName + "-compress-h.txt"))
            {
                // Add some text to file    
                fs.Write(data, 0, data.Length);

            }
        }

        static void createFileD(byte[] data, string pathName)
        {
            using (FileStream fs = File.Create(pathName + "-descompress-h.txt"))
            {
                fs.Write(data, 0, data.Length);

            }
        }


    }
}
