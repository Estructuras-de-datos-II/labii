using System;
using System.Collections.Generic;
using System.Text;

namespace Compress
{
    public class HuffmanDescompress : Compresss
    {
        private const int MAX_TREE_NODES = 511;

        public class BitStream
        {
            public byte[] BytePointer;
            public uint BitPosition;
            public uint Index;
        }

        public struct Symbol
        {
            public int Sym;
            public uint Count;
            public uint Code;
            public uint Bits;
        }

        public class DecodeNode
        {
            public DecodeNode ChildA;
            public DecodeNode ChildB;
            public int Symbol;
        }

        private static void initBitstream(ref BitStream stream, byte[] buffer)
        {
            stream.BytePointer = buffer;
            stream.BitPosition = 0;
        }

        private static uint readBit(ref BitStream stream)
        {
            byte[] buffer = stream.BytePointer;
            uint bit = stream.BitPosition;

            uint x = (uint)(Convert.ToBoolean((buffer[stream.Index] & (1 << (int)(7 - bit)))) ? 1 : 0);
            bit = (bit + 1) & 7;

            if (!Convert.ToBoolean(bit))
            {
                ++stream.Index;
            }

            stream.BitPosition = bit;

            return x;
        }

        private static uint read8Bits(ref BitStream stream)
        {
            byte[] buffer = stream.BytePointer;
            uint bit = stream.BitPosition;
            uint x = (uint)((buffer[stream.Index] << (int)bit) | (buffer[stream.Index + 1] >> (int)(8 - bit)));
            ++stream.Index;

            return x;
        }

        private static DecodeNode recoverTree(DecodeNode[] nodes, ref BitStream stream, ref uint nodenum)
        {
            DecodeNode thisNode;

            thisNode = nodes[nodenum];
            nodenum = nodenum + 1;
            thisNode.Symbol = -1;
            thisNode.ChildA = null;
            thisNode.ChildB = null;

            if (Convert.ToBoolean(readBit(ref stream)))
            {
                thisNode.Symbol = (int)read8Bits(ref stream);
                return thisNode;
            }

            thisNode.ChildA = recoverTree(nodes, ref stream, ref nodenum);
            thisNode.ChildB = recoverTree(nodes, ref stream, ref nodenum);

            return thisNode;
        }

        public static void Decompress(byte[] input, byte[] output, uint inputSize, uint outputSize)
        {
            DecodeNode[] nodes = new DecodeNode[MAX_TREE_NODES];

            for (int counter = 0; counter < nodes.Length; ++counter)
            {
                nodes[counter] = new DecodeNode();
            }

            DecodeNode root, node;
            BitStream stream = new BitStream();
            uint i, nodeCount;
            byte[] buffer;

            if (inputSize < 1)
                return;

            initBitstream(ref stream, input);
            nodeCount = 0;
            root = recoverTree(nodes, ref stream, ref nodeCount);
            buffer = output;

            for (i = 0; i < outputSize; ++i)
            {
                node = root;

                while (node.Symbol < 0)
                {
                    if (Convert.ToBoolean(readBit(ref stream)))
                        node = node.ChildB;
                    else
                        node = node.ChildA;
                }

                buffer[i] = (byte)node.Symbol;
            }
        }

        public byte[] compress(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] descompress(byte[] data)
        {
            byte[] descompressedData = new byte[data.Length];
            Decompress(data, descompressedData, (uint)descompressedData.Length, (uint)(data.Length));
            return descompressedData;
        }
    }
}
