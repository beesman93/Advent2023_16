using System.Linq;

List<string> lines = new();
using (StreamReader reader = new(args[0]))
{
    while(!reader.EndOfStream)
        lines.Add(reader.ReadLine());
}

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
    Console.WriteLine($"part1: {solvefor(0,-1,0,1)}");
}
void part2()
{
    int max = 0;
    int curr = 0;
    for (int x = 0; x < map.Count; x++)
    {
        curr = solvefor(x, -1, 0, 1);
        if(curr>max)max = curr;
        curr = solvefor(x, map[x].Count, 0, -1);
        if (curr > max) max = curr;
    }
    for (int y = 0; y < map[0].Count; y++)
    {
        curr = solvefor(-1, y, 1, 0);
        if (curr > max) max = curr;
        curr = solvefor(map.Count, y, -1, 0);
        if (curr > max) max = curr;
    }
    Console.WriteLine($"part2: {max}");
}

int solvefor(int x, int y,int velX, int velY)
{
    string header = lines[0];

    bool[][] energized = new bool[map.Count][];
    for (int i = 0; i < map.Count; i++)
        energized[i]= new bool[map[i].Count];

    move(x, y, velX, velY, map, ref energized,new());

    int cnt = 0;
    foreach (var l in energized)
        foreach (var b in l)
            if (b)
                cnt++;
    return cnt;
}
void move(int x, int y, int velX, int velY,List<List<char>> map, ref bool[][] ene, List<int> d)
{
    int key = x + 1_000 * y + 1_000_000 * velX + 2_000_000 * velY;
    if (d.Contains(key))
        return;
    d.Add(key);
    x = x + velX;
    y = y + velY;
    //Console.WriteLine($"{x},{y}");
    if (x < 0 || x >= map.Count || y < 0 || y >= map[0].Count)
        return;

    char c = map[x][y];
    ene[x][y] = true;

    switch (c)
    {
        case '-':
            if (velY == 0)
            {
                move(x, y, 0, 1, map, ref ene,d);
                move(x, y, 0, -1, map, ref ene, d);
                break;
            }
            else
                move(x, y, velX, velY, map, ref ene, d);
            break;
        case '|':
            if (velX == 0)
            {
                move(x, y, 1, 0, map, ref ene, d);
                move(x, y, -1, 0, map, ref ene, d);
                break;
            }
            else
                move(x, y, velX, velY, map, ref ene, d);
            break;
        case '/':
            if(velY==1)
                move(x, y, -1, 0, map, ref ene, d);
            if(velY==-1)
                move(x, y, 1, 0, map, ref ene, d);
            if (velX == 1)
                move(x, y, 0, -1, map, ref ene, d);
            if (velX == -1)
                move(x, y, 0, 1, map, ref ene, d);
            break;
        case '\\':
            if (velY == 1)
                move(x, y, 1, 0, map, ref ene,d);
            if (velY == -1)
                move(x, y, -1, 0, map, ref ene,d);
            if (velX == 1)
                move(x, y, 0, 1, map, ref ene, d);
            if (velX == -1)
                move(x, y, 0, -1, map, ref ene, d);
            break;
        case '.':
            move(x, y, velX, velY, map, ref ene, d);
            break;
        default:
            throw new Exception("nope");
    }
    return;
}