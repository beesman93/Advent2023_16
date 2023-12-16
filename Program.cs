using System.Drawing;
using System.Linq;

List<string> lines = new();
using (StreamReader reader = new(args[0]))
{
    while(!reader.EndOfStream)
        lines.Add(reader.ReadLine());
}

Point UP = new(-1, 0);
Point DOWN = new(1, 0);
Point LEFT = new(0, -1);
Point RIGHT = new(0, 1);

List<List<char>> map = new List<List<char>>();
foreach (string line in lines)
{
    map.Add(new());
    foreach (char c in line)
    {
        map.Last().Add(c);
    }

}

part1();
part2();
void part1()
{
    Console.WriteLine($"part1: {solvefor(new(0,-1),new(0,1))}");
}
void part2()
{
    int max = 0;
    int curr = 0;
    for (int x = 0; x < map.Count; x++)
    {
        curr = solvefor(new(x, -1), new(0, 1));
        if(curr>max)max = curr;
        curr = solvefor(new(x, map[x].Count), new(0, -1));
        if (curr > max) max = curr;
    }
    for (int y = 0; y < map[0].Count; y++)
    {
        curr = solvefor(new(-1, y), new(1, 0));
        if (curr > max) max = curr;
        curr = solvefor(new(map.Count, y), new(-1, 0));
        if (curr > max) max = curr;
    }
    Console.WriteLine($"part2: {max}");
}

int solvefor(Point coord, Point velocity)
{
    bool[][] energized = new bool[map.Count][];
    for (int i = 0; i < map.Count; i++)
        energized[i]= new bool[map[i].Count];
    move(coord, velocity, map, ref energized,new());
    int cnt = 0;
    foreach (var l in energized)
        foreach (var b in l)
            if (b)
                cnt++;
    return cnt;
}
void move(
    Point coord,
    in Point velocity,
    in List<List<char>> map,
    ref bool[][] ene,
    Dictionary<Point,List<Point>> visited)
{
    if (!visited.ContainsKey(coord))
        visited.Add(coord, new());
    if (visited[coord].Contains(velocity))
        return; //already bounced here before
    else
        visited[coord].Add(velocity);

    coord.Offset(velocity);

    if (outOfBounds(map, coord))
        return; // ray went out of grid

    ene[coord.X][coord.Y] = true;

    switch (map[coord.X][coord.Y])
    {
        case '/':
            if     (velocity == RIGHT)  move(coord, UP,     map, ref ene, visited);
            else if(velocity == LEFT)   move(coord, DOWN,   map, ref ene, visited);
            else if(velocity == DOWN)   move(coord, LEFT,   map, ref ene, visited);
            else if(velocity == UP)     move(coord, RIGHT,  map, ref ene, visited);
            break;
        case '\\':
            if     (velocity == RIGHT)  move(coord, DOWN,   map, ref ene, visited);
            else if(velocity == LEFT)   move(coord, UP,     map, ref ene, visited);
            else if(velocity == DOWN)   move(coord, RIGHT,  map, ref ene, visited);
            else if(velocity == UP)     move(coord, LEFT,   map, ref ene, visited);
            break;
        case '-':
            if (velocity.Y == 0)
            {
                move(coord, LEFT, map, ref ene, visited);
                move(coord, RIGHT, map, ref ene, visited);
                break;
            }
            else
            {
                move(coord, velocity, map, ref ene, visited);
            }
            break;
        case '|':
            if (velocity.X == 0)
            {
                move(coord, UP, map, ref ene, visited);
                move(coord, DOWN, map, ref ene, visited);
                break;
            }
            else
            {
                move(coord, velocity, map, ref ene, visited);
            }
            break;
        default:
            move(coord, velocity, map, ref ene, visited);
            break;
    }
}

bool outOfBounds(in List<List<char>> map,in Point coord)
{
    if (coord.X < 0) return true;
    if (coord.Y < 0) return true;
    if(coord.X>=map.Count) return true;
    if (coord.Y >= map[coord.X].Count) return true;
    return false;
}