using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomGenerator
{
    const ulong InitialSeed = 64829450ul;
    static ulong seed = InitialSeed;
    public static double GaussianRand()
    {
        double sum = 0;
        for (int i = 0; i < 3; ++i)
        {
            ulong holdseed = seed;
            // 固定方式生成种子序列
            seed ^= seed << 13;
            seed ^= seed >> 17;
            seed ^= seed << 5;
            long r = (Int64)(holdseed + seed);
            sum += (double)r * (1.0 / 0x7FFFFFFFFFFFFFFF);
        }
        return sum;
    }
}
