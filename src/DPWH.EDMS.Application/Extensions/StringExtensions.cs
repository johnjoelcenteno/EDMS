﻿namespace DPWH.EDMS.Application.Extensions;
public static class StringExtensions
{
    public static decimal SafeDecimalParse(this string value)
    {
        try
        {
            return decimal.Parse(value);
        }
        catch
        {
            return decimal.Zero;
        }
    }
}
