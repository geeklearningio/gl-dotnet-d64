namespace GeekLearning.D64
{
    using System.Linq;
    using System.Text;

    public class D64Convert
    {
        private static char[] alphabet;
        private static int[] codeToIndex;

        static D64Convert()
        {
            alphabet = ".PYFGCRLAOEUIDHTNSQJKXBMWVZ_pyfgcrlaoeuidhtnsqjkxbmwvz1234567890".ToCharArray().OrderBy(x => x).ToArray();

            codeToIndex = new int[128];

            for (int i = 0; i < 64; i++)
            {
                var code = alphabet[i];
                codeToIndex[(int)code] = i;
            }
        }

        public static string Encode(byte[] data)
        {
            var s = new StringBuilder(data.Length);
            var l = data.Length;
            var hang = 0;
            for (var i = 0; i < l; i++)
            {
                var v = data[i];

                switch (i % 3)
                {
                    case 0:
                        s.Append(alphabet[v >> 2]);
                        hang = (v & 3) << 4;
                        break;
                    case 1:
                        s.Append(alphabet[hang | v >> 4]);
                        hang = (v & 0xf) << 2;
                        break;
                    case 2:
                        s.Append(alphabet[hang | v >> 6]);
                        s.Append(alphabet[v & 0x3f]);
                        hang = 0;
                        break;
                }

            }

            if (l % 3 != 0)
            {
                s.Append(alphabet[hang]);
            }

            return s.ToString();
        }

        public static byte[] Decode(string str)
        {
            var l = (double)str.Length;
            var j = 0;
            var b = new byte[(int)((l / 4) * 3)];
            var hang = 0;

            for (var i = 0; i < l; i++)
            {
                var v = codeToIndex[str[i]];

                switch (i % 4)
                {
                    case 0:
                        hang = v << 2;
                        break;
                    case 1:
                        b[j++] = (byte)(hang | v >> 4);
                        hang = (v << 4) & 0xff;
                        break;
                    case 2:
                        b[j++] = (byte)(hang | v >> 2);
                        hang = (v << 6) & 0xff;
                        break;
                    case 3:
                        b[j++] = (byte)(hang | v);
                        break;
                }

            }

            return b;
        }
    }
}
