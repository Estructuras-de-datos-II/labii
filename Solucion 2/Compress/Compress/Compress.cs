using System;
using System.Collections.Generic;
using System.Text;

namespace Compress
{
    public interface Compresss
    {
        byte[] compress(byte[] data);

        byte[] descompress(byte[] data);
    }
}
