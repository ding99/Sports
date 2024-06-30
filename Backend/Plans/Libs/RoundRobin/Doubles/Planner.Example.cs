namespace Libs.RoundRobin.Doubles; 

public partial class Planner {

    public void ShowSample() {
        var rr10 = GetNet10();

        var orig = DTour(rr10, "Net10");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(orig);

        var stat = STour(rr10);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(stat);
    }

    private static Tour GetNet10() {
        return new Tour(new([
            new(new([
                new(new([0, 6]), new([5, 8])),
                new(new([9, 4]), new([3, 2]))
            ])),
            new(new([
                new(new([0, 2]), new([3, 6])),
                new(new([1, 7]), new([8, 4]))
            ])),
            new(new([
                new(new([7, 3]), new([6, 2])),
                new(new([9, 5]), new([1, 8]))
            ])),
            new(new([
                new(new([9, 6]), new([4, 5])),
                new(new([1, 0]), new([7, 2]))
            ])),
            new(new([
                new(new([1, 4]), new([0, 8])),
                new(new([5, 7]), new([3, 9]))
            ])),
            new(new([
                new(new([4, 6]), new([9, 1])),
                new(new([5, 2]), new([7, 8]))
            ])),
            new(new([
                new(new([3, 0]), new([1, 5])),
                new(new([9, 2]), new([6, 7]))
            ])),
            new(new([
                new(new([3, 8]), new([0, 4]))
            ]))
        ]));
    }

}
