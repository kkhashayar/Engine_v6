namespace Engine;

using System.Collections.Generic;
using System.Linq;

public static class PuzzleManager
{
    private static Dictionary<int, List<string>> puzzles = new Dictionary<int, List<string>>
    {
        {
            2, new List<string>
            {
                "r1b2k1r/ppp1bppp/8/1B1Q4/5q2/2P5/PPP2PPP/R3R1K1 w - - 1 0",
                "r4k2/6pp/p1n1p2N/2p5/1q6/6QP/PbP2PP1/1K1R1B2 w - - 1 0",
                "r2q3k/ppb3pp/2p1B3/2P1RQ2/8/6P1/PP1r3P/5RK1 w - - 1 0",
                "5rk1/1p1q2bp/p2pN1p1/2pP2Bn/2P3P1/1P6/P4QKP/5R2 w - - 1 0",
                "r3kb1r/1p3ppp/p1n2n2/4p1N1/2B3b1/1P2P3/P4PPP/RNBR2K1 w kq - 1 0",
                "2n2rk1/5p1p/6p1/1pQ5/2q5/2N1B1P1/1b3P1P/4R1K1 w - - 1 0",
                "6k1/3q1ppp/p2r4/1p6/4Q3/8/PPP3PP/3R3K w - - 1 0",
                "r1bqr1k1/pp3pbR/2n1p1p1/4PnN1/2Bp1P2/2N4Q/PP4P1/R1B1K3 w Q - 1 0"
            }
        },
        {
            3, new List<string>
            {
                "r2qkbnr/ppp2ppp/8/8/2PnP3/2N2P2/PPP3PP/R1BQKBNR w KQkq - 1 0",
                "r1bqkbnr/pppp1ppp/2n5/8/2PnP3/2N2P2/PPP3PP/R1BQKBNR w KQkq - 1 0",
                "r1bq1rk1/ppp2ppp/2n2n2/4N3/2P5/2N1P3/PP1P1PPP/R1BQKB1R w KQ - 1 0",
                "r2qk2r/1b1pbppp/pp2n3/1Bp1N3/1Q6/P3P3/1PP2PPP/R1B1K2R w KQkq - 1 0",
                "r4rk1/1b1qbppp/p1n1pn2/1pNp4/3P4/4P3/PP2BPPP/R1BQ1RK1 w - - 1 0",
                "r1bqk2r/1b1pbppp/pp2n3/1Bp1N3/1Q6/P3P3/1PP2PPP/R1B1K2R w KQkq - 1 0",
                "r1bq1rk1/ppp2ppp/2n2n2/4N3/2P5/2N1P3/PP1P1PPP/R1BQKB1R w KQ - 1 0"
            }
        },
        {
            4, new List<string>
            {
                "r5rk/2p1Nppp/3p3P/pp2p1P1/4P3/2qnPQK1/8/R6R w - - 1 0, hxg7+ Rxg7 Rxh7+ Rxh7 Qf6+ Rg7 Rh1#",
                "1r2k1r1/pbppnp1p/1b3P2/8/Q7/B1PB1q2/P4PPP/3R2K1 w - - 1 0, Qxd7+ Kxd7 Bf5+ Ke8 Bd7+ Kf8 Bxe7#",
                "Q7/p1p1q1pk/3p2rp/4n3/3bP3/7b/PP3PPK/R1B2R2 b - - 0 1, Bxg2 Qh8+ Kxh8 Bg5 Qxg5 Rfe1 Nf3#",
                "r1bqr3/ppp1B1kp/1b4p1/n2B4/3PQ1P1/2P5/P4P2/RN4K1 w - - 1 0, Qe5+ Kh6 g5+ Kh5 Bf3+ Bg4 Qh2#",
                "r1b3kr/3pR1p1/ppq4p/5P2/4Q3/B7/P5PP/5RK1 w - - 1 0, Rxg7+ Kxg7 Qe7+ Kg8 Qf8+ Kh7 Qf7#",
                "2k4r/1r1q2pp/QBp2p2/1p6/8/8/P4PPP/2R3K1 w - - 1 0, Qa8+ Rb8 Rxc6+ Qc7 Rxc7+ Kd8 Qxb8#",
                "2r1r3/p3P1k1/1p1pR1Pp/n2q1P2/8/2p4P/P4Q2/1B3RK1 w - - 1 0, f6+ Kh8 g7+ Kg8 f7+ Kxg7 Qf6#",
                "r1bk3r/pppq1ppp/5n2/4N1N1/2Bp4/Bn6/P4PPP/4R1K1 w - - 1 0, Nexf7+ Qxf7 Nxf7+ Kd7 Bb5+ c6 Re7#",
                "6kr/pp2r2p/n1p1PB1Q/2q5/2B4P/2N3p1/PPP3P1/7K w - - 1 0, Qg7+ Rxg7 e7+ Rf7 e8=Q+ Qf8 Bxf7#",
                "r3k3/pbpqb1r1/1p2Q1p1/3pP1B1/3P4/3B4/PPP4P/5RK1 w - - 1 0, Bxg6+ Rxg6 Qxg6+ Kd8 Rf8+ Qe8 Qxe8#"
            }
        }
    };

    static PuzzleManager()
    {
      // Maybe for later if I wanna load a puzzle from database? or something :| 
    }

    public static List<string> GetPuzzles(int mateIn, int numberOfPuzzles)
    {
        if (puzzles.ContainsKey(mateIn) && numberOfPuzzles > 0)
        {
            return puzzles[mateIn].Take(numberOfPuzzles).ToList();
        }
        return new List<string>();
    }
}
