using Libs.RoundRobin.Mix.Models;

namespace Libs.RoundRobin.Mix;

public partial class Planner {

    public void ReviewTour(int men, int women, int games) {
        Tour? rr = null;

        switch (men) {
        case 6:
            switch (women) {
            case 5:
                //if ()
                break;
            case 6:
                switch (games) {
                case 12:
                    rr = GetSample060612();
                    break;
                }
                break;
            }
            break;
        case 10:
            switch (women) {
            case 8:
                break;
            case 9:
                break;
            }
            break;
        }

        if (rr != null) {
            log.Information(DTour(rr, $"Sample{men}Men-{women}Women-{games}Games"));
            log.Information(DMixPlayers(rr));
        } else {
            log.Error("Not found review data for {m}-men, {w}-women, {g}-games!", men, women, games);
        }
    }

    #region data
    //6 men, 6 women, 12 games
    private static Tour GetSample060612() {
        return new Tour([
            new Round([
                new Court(new Team(3, 2), new Team(1, 0)),
                new Court(new Team(5, 3), new Team(4, 1)),
                new Court(new Team(0, 5), new Team(2, 4))
            ]),
            new Round([
                new Court(new Team(3, 0), new Team(5, 2)),
                new Court(new Team(0, 3), new Team(1, 5)),
                new Court(new Team(4, 4), new Team(2, 1))
            ]),
            new Round([
                new Court(new Team(3, 3), new Team(0, 4)),
                new Court(new Team(5, 1), new Team(1, 2)),
                new Court(new Team(4, 0), new Team(2, 5))
            ]),
            new Round([
                new Court(new Team(3, 1), new Team(2, 3)),
                new Court(new Team(1, 4), new Team(4, 5)),
                new Court(new Team(5, 0), new Team(0, 2))
            ])
        ]);
    }
    #endregion

}
