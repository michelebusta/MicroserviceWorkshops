using System;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Utils
{
    public class NamedGuidCreator
    {
        public static Guid CreateFrom(Guid @namespace, string name)
        {
            return CreateFrom(@namespace, name, 5);
        }

        //Implements rfc 4122
        public static Guid CreateFrom(Guid @namespace, string name, int version)
        {

            var dest = new byte[16 + name.Length];
            var ns = @namespace.ToByteArray();

            var bytes = Encoding.UTF8.GetBytes(name);
            //Guid structure is int, short, short, int8, int8, byte[5]
            //switch all values to network byte order for consistent hashing
            dest[0] = ns[3];
            dest[1] = ns[2];
            dest[2] = ns[1];
            dest[3] = ns[0];
            dest[4] = ns[5];
            dest[5] = ns[4];
            dest[6] = ns[7];
            dest[7] = ns[6];
            //Copy remaining bytes and then the name bytes
            Array.Copy(ns, 8, dest, 8, 8);
            Array.Copy(bytes, 0, dest, 16, bytes.Length);
            byte[] hashed;
            using (var hash = version == 3 ? (HashAlgorithm)MD5.Create() : SHA1.Create())
                hashed = hash.ComputeHash(dest);

            //Set the four most significant bits (bits 12 through 15) of the
            //time_hi_and_version field to the appropriate 4-bit version number
            hashed[6] = (byte)((hashed[6] & 0x0F) | (version << 4));

            //Set the two most significant bits (bits 6 and 7) of the
            //clock_seq_hi_and_reserved to zero and one, respectively
            hashed[8] = (byte)((hashed[8] & 0x3F) | 0x80);

            var guid = new Guid(new[]
            {
                hashed[3], hashed[2], hashed[1], hashed[0], //time low
                hashed[5], hashed[4], //time mid
                hashed[7], hashed[6], //time hi and version
                hashed[8], hashed[9], //clk_seq_high, clq_seq_low
                hashed[10], hashed[11], hashed[12], hashed[13], hashed[14], hashed[15]
            } //node 0-5
                );
            return guid;
        }
    }
}
