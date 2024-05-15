namespace DPWH.EDMS.Application.Services;

public static class ShortCodeGeneratorService
{
    private const string Base32AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
    private static readonly uint[] _primeArray = { 68711, 57977, 47123, 66083, 3461, 94583, 86171, 59113, 72307, 79273, 59011, 22229, 46093, 95869, 56921, 99347 };

    private struct CodeComponent
    {
        public uint Range { get; }
        public ulong Value { get; }

        public CodeComponent(uint bitLength, ulong value)
        {
            Range = bitLength;
            Value = value;
        }
    }

    public static string GenerateCode8(Guid seedGuid, DateTimeOffset seedTime)
    {
        var userBits8 = (byte)(seedGuid.ToByteArray().Sum(x => x) % 8);
        var ts = seedTime.Subtract(DateTimeOffset.MinValue);
        var components = new CodeComponent[3];

        // Total bits = 35
        components[2] = new CodeComponent(397, (ulong)seedTime.Subtract(DateTimeOffset.MinValue).TotalDays % 397);
        components[1] = new CodeComponent(86400, (ulong)seedTime.TimeOfDay.TotalSeconds);
        components[0] = new CodeComponent(1000, (ulong)seedTime.TimeOfDay.Milliseconds);

        ulong v = 0;
        foreach (var t in components)
        {
            v *= t.Range;
            v += t.Value;
        }

        var timeBits4 = (byte)(ts.TotalMilliseconds % 4);
        var xpad = (ulong)4611686018427387847 * _primeArray[timeBits4] % ulong.MaxValue;
        v ^= xpad;

        // Add bits = 5 ==> 40
        v = v << 3 | userBits8;
        v = v << 2 | timeBits4;

        var vBytes = NumberToBytes(v, 5);   // Big-endian - most significant byte at [0]

        var vBytesShuffle = new byte[vBytes.Length];
        vBytesShuffle[0] = vBytes[2];
        vBytesShuffle[1] = vBytes[0];
        vBytesShuffle[2] = vBytes[3];
        vBytesShuffle[3] = ReverseBits(vBytes[4]);
        vBytesShuffle[4] = vBytes[1];

        var strLen = (int)Math.Ceiling(vBytesShuffle.Length * 8 / 5.0);

        return EncodeAsBase32String(vBytesShuffle, false).PadLeft(strLen, '0');
    }

    public static string GenerateCode10(Guid seedGuid, DateTimeOffset seedTime)
    {
        var userBits32 = (byte)(seedGuid.ToByteArray().Sum(x => x) % 32);
        var ts = seedTime.Subtract(DateTimeOffset.MinValue);

        var components = new CodeComponent[3];

        // Total bits = 36
        components[2] = new CodeComponent(397, (ulong)seedTime.Subtract(DateTimeOffset.MinValue).TotalDays % 397);
        components[1] = new CodeComponent(86400, (ulong)seedTime.TimeOfDay.TotalSeconds);
        components[0] = new CodeComponent(2000, (ulong)(seedTime.TimeOfDay.TotalMilliseconds % 2000));

        ulong v = 0;
        foreach (var t in components)
        {
            v *= t.Range;
            v += t.Value;
        }

        var timeBits16 = (byte)(ts.TotalMilliseconds % 16);
        var xpad = (ulong)4611686018427387847 * _primeArray[timeBits16] % ulong.MaxValue;
        v ^= xpad;

        // Add bits = 12 ==> 48
        v <<= 1;                       // Reserve 1 bit
        v = v << 5 | userBits32;
        v = v << 4 | timeBits16;
        v <<= 2;                       // Reserve 2 bits

        var vBytes = NumberToBytes(v, 6);   // Big-endian - most significant byte at [0]

        var vBytesShuffle = new byte[vBytes.Length];
        vBytesShuffle[0] = vBytes[2];
        vBytesShuffle[1] = vBytes[0];
        vBytesShuffle[2] = vBytes[4];
        vBytesShuffle[3] = ReverseBits(vBytes[5]);
        vBytesShuffle[4] = vBytes[1];
        vBytesShuffle[5] = vBytes[3];

        var strLen = (int)Math.Ceiling(vBytesShuffle.Length * 8 / 5.0);

        return EncodeAsBase32String(vBytesShuffle, false).PadLeft(strLen, '0');
    }

    private static byte[] NumberToBytes(ulong intValue, int numBytes)
    {
        var bytes = new byte[numBytes];
        var shft = 0;
        for (var i = numBytes - 1; i >= 0; i--)
        {
            bytes[i] = (byte)(intValue >> shft);
            shft += 8;
        }

        return bytes;
    }

    private static byte ReverseBits(byte v)
    {
        var r = v; // r will be reversed bits of v; first get LSB of v
        var s = 7; // extra shift needed at end
        for (v >>= 1; v != 0; v >>= 1)
        {
            r <<= 1;
            r |= (byte)(v & 1);
            s--;
        }
        r <<= s; // shift when v's highest bits are zero
        return r;
    }

    private static string EncodeAsBase32String(byte[] bytes, bool addPadding = true)
    {
        var result = ToBase32String(bytes, addPadding);
        return result;
    }

    private static string ToBase32String(IReadOnlyCollection<byte> input, bool addPadding = true)
    {
        if (input == null || input.Count == 0)
        {
            return string.Empty;
        }

        var bits = input.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).Aggregate((a, b) => a + b).PadRight((int)(Math.Ceiling(input.Count * 8 / 5d) * 5), '0');
        var result = Enumerable.Range(0, bits.Length / 5).Select(i => Base32AllowedCharacters.Substring(Convert.ToInt32(bits.Substring(i * 5, 5), 2), 1)).Aggregate((a, b) => a + b);
        if (addPadding)
        {
            result = result.PadRight((int)(Math.Ceiling(result.Length / 8d) * 8), '=');
        }

        return result;
    }
}