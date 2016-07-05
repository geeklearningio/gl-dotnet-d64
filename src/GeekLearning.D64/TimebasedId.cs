namespace GeekLearning.D64
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;

    public class TimebasedId
    {
        private RandomNumberGenerator rng;
        private bool reverseOrder;

        public TimebasedId(bool reverseOrder)
        {
            this.rng = RandomNumberGenerator.Create();
            this.reverseOrder = reverseOrder;
        }

        private long GetDateTicks(DateTimeOffset date)
        {
            if (this.reverseOrder)
            {
                return long.MaxValue - date.UtcTicks;
            }

            return date.UtcTicks;
        }

        private byte[] GetDateBytes(DateTimeOffset date)
        {
            var utcBytes = BitConverter.GetBytes(GetDateTicks(date));
            if (BitConverter.IsLittleEndian)
            {
                utcBytes = utcBytes.Reverse().ToArray();
            }

            return utcBytes;
        }

        /// <summary>
        /// Generates a new Id using based on UTCNow
        /// </summary>
        /// <returns></returns>
        public string NewId()
        {
            return NewId(DateTimeOffset.UtcNow);
        }

        public string NewId(DateTimeOffset datetime)
        {
            var utcBytes = GetDateBytes(datetime);

            var rngId = new byte[8];
            this.rng.GetBytes(rngId);

            return D64Convert.Encode(utcBytes) + D64Convert.Encode(rngId);
        }

        public string DatePrefix(DateTimeOffset datetime)
        {
            var utcBytes = GetDateBytes(datetime);

            return D64Convert.Encode(utcBytes);
        }

        public string DateBoundary(DateTimeOffset datetime)
        {
            var utcBytes = GetDateBytes(datetime);
            var rngId = new byte[8];

            return D64Convert.Encode(utcBytes) + D64Convert.Encode(rngId);
        }
    }
}
