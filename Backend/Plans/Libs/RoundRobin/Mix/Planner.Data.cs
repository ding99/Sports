using Libs.RoundRobin.Mix.Models;


namespace Libs.RoundRobin.Mix;

public partial class Planner {

    public Master CreateMaster(int men, int women, int games) {
        //fixed data for test
        //var master = new Master {
        //    Men = [3, 1, 2, 5, 0, 0, 5, 3, 1, 4, 5, 2, 5, 0, 2, 2, 2, 4, 1, 5, 2, 4, 0, 0, 5, 3, 0, 4, 1, 4, 4, 3, 1, 3, 1, 3],
        //    Women = [2, 2, 4, 1, 0, 1, 3, 0, 4, 5, 0, 3, 2, 5, 0, 1, 2, 3, 0, 4, 4, 5, 2, 2, 4, 5, 0, 3, 1, 5, 4, 3, 1, 1, 5, 3]
        //};

        var master = new Master {
            Men = NewMaster(men, games),
            Women = NewMaster(women, games)
        };

        //log.Debug("Men   {men} [{ct}]", master.Men.Count, GroupMaster(master.Men));
        //log.Debug("  {list}", string.Join(",", master.Men));
        //log.Debug("Women {women} [{ct}]", master.Women.Count, GroupMaster(master.Women));
        //log.Debug("  {list}", string.Join(",", master.Women));

        return master;
    }

    public Tour[] Excellent66() {
        return [
            new Tour([ //10 C2
                new Round([
                    new Court(new Team(0,5), new Team(5,4)),
                    new Court(new Team(2,3), new Team(4,2)),
                    new Court(new Team(1,0), new Team(3,1))
                ]),
                new Round([
                    new Court(new Team(5,2), new Team(1,1)),
                    new Court(new Team(4,4), new Team(3,0)),
                    new Court(new Team(0,3), new Team(2,5))
                ]),
                new Round([
                    new Court(new Team(5,1), new Team(4,0)),
                    new Court(new Team(0,2), new Team(3,3)),
                    new Court(new Team(2,4), new Team(1,5))
                ]),
                new Round([
                    new Court(new Team(0,4), new Team(4,1)),
                    new Court(new Team(2,2), new Team(5,0)),
                    new Court(new Team(3,5), new Team(1,3))
                ]),
                new Round([
                    new Court(new Team(3,4), new Team(2,1)),
                    new Court(new Team(4,5), new Team(5,3)),
                    new Court(new Team(0,0), new Team(1,2))
                ]),
                new Round([
                    new Court(new Team(3,2), new Team(5,5)),
                    new Court(new Team(0,1), new Team(2,0)),
                    new Court(new Team(1,4), new Team(4,3))
                ])
            ])
        ];
    }

    //TODO correct data
    public Tour[] Excellent65() {
        return [
            new Tour([ //6 C2
                new Round([
                    new Court(new Team(0,5), new Team(5,4)),
                    new Court(new Team(1,0), new Team(3,1))
                ]),
                new Round([
                    new Court(new Team(0,3), new Team(2,5)),
                    new Court(new Team(4,4), new Team(3,0))
                ]),
                new Round([
                    new Court(new Team(0,2), new Team(3,3)),
                    new Court(new Team(5,1), new Team(4,0))
                ]),
                new Round([
                    new Court(new Team(0,4), new Team(4,1)),
                    new Court(new Team(3,5), new Team(1,3))
                ]),
                new Round([
                    new Court(new Team(0,0), new Team(1,2)),
                    new Court(new Team(4,5), new Team(5,3))
                ]),
                new Round([
                    new Court(new Team(0,4), new Team(4,1)),
                    new Court(new Team(3,5), new Team(1,3))
                ]),
                new Round([
                    new Court(new Team(0,0), new Team(1,2)),
                    new Court(new Team(4,5), new Team(5,3))
                ]),
                new Round([
                    new Court(new Team(3,2), new Team(5,5))
                ])
            ])
        ];
    }

    public string GroupMaster(List<int> players) {
        var groups = players.GroupBy(x => x, (k, g) => new { Key = k, Count = g.Count() }).OrderBy(x => x.Key);
        return string.Join(", ", groups.Select(x => $"{x.Key}-{x.Count}"));
    }

    public List<int> NewMaster(int players, int maxGames) {
        List<int> list = [];
        Random rd = new();
        int rounds = maxGames / players, n;

        while (list.Count < rounds * players) {
            n = rd.Next(players);
            if (list.Count(x => x == n) < rounds) {
                list.Add(n);
            }
        }

        while (list.Count < maxGames) {
            n = rd.Next(players);
            if (list.Count(x => x == n) < rounds + 1) {
                list.Add(n);
            }
        }

        return list;
    }

}
