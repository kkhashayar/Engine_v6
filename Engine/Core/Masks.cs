﻿using Engine.Core;

namespace Engine;
public static class Masks
{
    public static readonly ulong[] King = new ulong[]
    {
      0x302, 0x705, 0xe0a, 0x1c14, 0x3828, 0x7050, 0xe0a0, 0xc040,
      0x30203, 0x70507, 0xe0a0e, 0x1c141c, 0x382838, 0x705070, 0xe0a0e0, 0xc040c0,
      0x3020300, 0x7050700, 0xe0a0e00, 0x1c141c00, 0x38283800, 0x70507000, 0xe0a0e000, 0xc040c000,
      0x302030000, 0x705070000, 0xe0a0e0000, 0x1c141c0000, 0x3828380000, 0x7050700000, 0xe0a0e00000, 0xc040c00000,
      0x30203000000, 0x70507000000, 0xe0a0e000000, 0x1c141c000000, 0x382838000000, 0x705070000000, 0xe0a0e0000000, 0xc040c0000000,
      0x3020300000000, 0x7050700000000, 0xe0a0e00000000, 0x1c141c00000000, 0x38283800000000, 0x70507000000000, 0xe0a0e000000000, 0xc040c000000000,
      0x302030000000000, 0x705070000000000, 0xe0a0e0000000000, 0x1c141c0000000000, 0x3828380000000000, 0x7050700000000000, 0xe0a0e00000000000, 0xc040c00000000000,
      0x203000000000000, 0x507000000000000, 0xa0e000000000000, 0x141c000000000000, 0x2838000000000000, 0x5070000000000000, 0xa0e0000000000000, 0x40c0000000000000,
    };

    public static readonly ulong KingMaskWhiteShortCastle = 0x4000000000000000;
    public static readonly ulong KingMaskWhiteLongCastle = 0x400000000000000;
    public static readonly ulong KingMaskBlackShortCastle = 0x4;
    public static readonly ulong KingMaskBlackLongCastle = 0x40;


    public static readonly ulong[] WhiatePawnSinglePush = new ulong[]
    {
        00,   00,  00,   00,   00,   00,   00,   00,
        0x1,  0x2, 0x4,  0x8,  0x10, 0x20, 0x40, 0x80,
        0x100, 0x200, 0x400, 0x800, 0x1000, 0x2000, 0x4000, 0x8000,
        0x10000, 0x20000, 0x40000, 0x80000, 0x100000, 0x200000, 0x400000, 0x800000,
        0x1000000, 0x2000000, 0x4000000, 0x8000000, 0x10000000, 0x20000000, 0x40000000, 0x80000000,
        0x100000000, 0x200000000, 0x400000000, 0x800000000, 0x1000000000, 0x2000000000, 0x4000000000, 0x8000000000,
        0x10000000000, 0x20000000000, 0x40000000000, 0x80000000000, 0x100000000000, 0x200000000000, 0x400000000000, 0x800000000000,
        00,   00,  00,   00,   00,   00,   00,   00,

    };

    public static readonly ulong[] WhiatePawnDoublePush = new ulong[]
    {
        00,   00,  00,   00,   00,   00,   00,   00,
        00,   00,  00,   00,   00,   00,   00,   00,
        00,   00,  00,   00,   00,   00,   00,   00,
        00,   00,  00,   00,   00,   00,   00,   00,
        00,   00,  00,   00,   00,   00,   00,   00,
        00,   00,  00,   00,   00,   00,   00,   00,
        0x100000000, 0x200000000, 0x400000000, 0x800000000, 0x1000000000, 0x2000000000, 0x4000000000, 0x8000000000,
        00,   00,  00,   00,   00,   00,   00,   00
    };

    public static readonly ulong[] WhitePawnCaptures = new ulong[]
    {
        00,   00,  00,   00,   00,   00,   00,   00,
        0x2,  0x5, 0xa,  0x14, 0x28, 0x50, 0xa0, 0x40,
        0x200, 0x500, 0xa00, 0x1400, 0x2800, 0x5000, 0xa000, 0x4000,
        0x20000, 0x50000, 0xa0000, 0x140000, 0x280000, 0x500000, 0xa00000, 0x400000,
        0x2000000, 0x5000000, 0xa000000, 0x14000000, 0x28000000, 0x50000000, 0xa0000000, 0x40000000,
        0x200000000, 0x500000000, 0xa00000000, 0x1400000000, 0x2800000000, 0x5000000000, 0xa000000000, 0x4000000000,
        0x20000000000, 0x50000000000, 0xa0000000000, 0x140000000000, 0x280000000000, 0x500000000000, 0xa00000000000, 0x400000000000,
        00,   00,  00,   00,   00,   00,   00,   00

    };

    public static readonly ulong[] WhitePawnEnPassant = new ulong[]
    {
        00,   00,  00,   00,   00,   00,   00,   00,
        00,   00,  00,   00,   00,   00,   00,   00,
        00,   00,  00,   00,   00,   00,   00,   00,
        0x20000,   0x50000,  0xa0000,   0x140000,   0x280000,   0x500000,   0xa00000,   0x400000,
        00,   00,  00,   00,   00,   00,   00,   00,
        00,   00,  00,   00,   00,   00,   00,   00,
        00,   00,  00,   00,   00,   00,   00,   00,
        00,   00,  00,   00,   00,   00,   00,   00,
    };



    public static readonly ulong[] Knight = new ulong[]
    {
        132096,329728,659712,1319424,2638848,5277696,10489856,4202496,
        33816580,84410376,168886289,337772578,675545156,1351090312,2685403152,1075839008,
        8657044482,21609056261,43234889994,86469779988,172939559976,345879119952,687463207072,275414786112,
        2216203387392,5531918402816,11068131838464,22136263676928,44272527353856,88545054707712,175990581010432,70506185244672,
        567348067172352,1416171111120896,2833441750646784,5666883501293568,11333767002587136,22667534005174272,45053588738670592,18049583422636032,
        145241105196122112,362539804446949376,725361088165576704,1450722176331153408,2901444352662306816,5802888705324613632,11533718717099671552,4620693356194824192,
        288234782788157440,576469569871282176,1224997833292120064,2449995666584240128,4899991333168480256,9799982666336960512,1152939783987658752,2305878468463689728,
        1128098930098176,2257297371824128,4796069720358912,9592139440717824,19184278881435648,38368557762871296,4679521487814656,9077567998918656
    };
}

