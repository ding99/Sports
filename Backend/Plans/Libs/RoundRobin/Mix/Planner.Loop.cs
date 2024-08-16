using Libs.RoundRobin.Mix.Models;


namespace Libs.RoundRobin.Mix; 

public partial class Planner {

    #region loop 6-5

    public void Chose65(int men, int women, int games, int loop) {
        int cn = 0, maxC2 = 6;
        log.Information("loop {l}, maxC2 {max}", loop, maxC2);
        for (int i = 0; i < loop; i++) {
            if (i > 0 && i % 1000 == 0) {
                log.Information("-- i {i}", i);
            }
            var result = CreateMix(men, women, games);

            if (result.IsSuccess) {
                var ps = result.Value.p;
                var c3 = Count3_65(ps);
                var c2 = Count2_65(ps);
                var p2 = Part2_65(ps);
                if (c3 < 1 && c2 < maxC2 && p2 < 1) {
                    cn++;
                    log.Information("-- {i,2}: C3 {c3} C2 {c2} P2 {p2}", cn, c3, c2, p2);
                    //log.Information("{i,2}: P2 {p2}", cn, p2);
                    log.Information("{d}", DTour(result.Value.t, $"{men}M/{women}W-{games}Games"));
                    log.Information("{d}", DPlayers(result.Value.p));
                }
            }
        }
        log.Information("Sum {sum}", cn);
    }

    #endregion

    #region util 6-5
    public int Count3_65(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 2) + p.OppoSame.Count(o => o > 2) + p.OppoDiff.Count(o => o > 2));
    }

    public int Count2_65(Player[] ps) {
        return ps.Sum(p => p.OppoSame.Count(o => o > 1) + p.OppoDiff.Count(o => o > 1));
    }

    public int Part2_65(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1));
    }
    #endregion

    #region loop 6-6

    public void Chose66(int men, int women, int games, int loop) {
        int cn = 0, maxC2 = 12;
        log.Information("loop {l}, maxC2 {max}", loop, maxC2);
        for (int i = 0; i < loop; i++) {
            if (i > 0 && i % 1000 == 0) {
                log.Information("-- i {i}", i);
            }
            var result = CreateMix(men, women, games);

            if (result.IsSuccess) {
                var ps = result.Value.p;
                var c3 = Count3_66(ps);
                var c2 = Count2_66(ps);
                var p2 = Part2_66(ps);
                if (c3 < 1 && p2 < 1 && c2 < maxC2) {
                    cn++;
                    log.Information("-- {i,2}: C3 {c3} C2 {c2} P2 {p2}", cn, c3, c2, p2);
                    log.Information("{d}", DTour(result.Value.t, $"{men}M/{women}W-{games}Games"));
                    log.Information("{d}", DPlayers(result.Value.p));
                }
            }
        }
        log.Information("Sum {sum}", cn);
    }

    #endregion

    #region util 6-6
    public int Count3_66(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 2) + p.OppoSame.Count(o => o > 2) + p.OppoDiff.Count(o => o > 2));
    }

    public int Count2_66(Player[] ps) {
        return ps.Sum(p => p.OppoSame.Count(o => o > 1) + p.OppoDiff.Count(o => o > 1));
    }

    public int Part2_66(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1));
    }
    #endregion

}
