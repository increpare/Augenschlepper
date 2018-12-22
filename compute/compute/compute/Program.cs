using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



static class MainClass
{

    public static readonly int[] dicefaces = {
    0b000_010_000,
    0b100_000_001,
    0b001_000_100,
    0b100_010_001,
    0b001_010_100,
    0b101_000_101,
    0b101_010_101,
    0b111_000_111,
    0b101_101_101
    };

    public static readonly int[] dicescores = { 1,2,2,3,3,4,5,6,6 };




    //-1 all bad, 0 mixed, 1 all good
    public static int scoreausrechnen(int[] state, int H, int B)
    {
        //elemente ändern a=3 wenn pie ein teil eines Musters ist
        int score = 0;

        for (var i = 0; i < B - 2; i++)
        {
            for (var j = 0; j < H - 2; j++)
            {
                var facemask =
                      ((state[i + 0 + B * (j + 0)] & 1) << 0)
                    + ((state[i + 1 + B * (j + 0)] & 1) << 1)
                    + ((state[i + 2 + B * (j + 0)] & 1) << 2)
                    + ((state[i + 0 + B * (j + 1)] & 1) << 3)
                    + ((state[i + 1 + B * (j + 1)] & 1) << 4)
                    + ((state[i + 2 + B * (j + 1)] & 1) << 5)
                    + ((state[i + 0 + B * (j + 2)] & 1) << 6)
                    + ((state[i + 1 + B * (j + 2)] & 1) << 7)
                    + ((state[i + 2 + B * (j + 2)] & 1) << 8);

                if (facemask == 0)
                {
                    continue;
                }

                for (var k = 0; k < dicefaces.Length; k++)
                {
                    if (dicefaces[k] == facemask)
                    {
                        score += dicescores[k];
                        break;
                    }
                }
            }
        }


        return score;
    }



    //-1 all bad, 0 mixed, 1 all good
    public static int geltend(int[] state, int H, int B)
    {
        //elemente ändern a=3 wenn pie ein teil eines Musters ist


        for (var i = 0; i < B - 2; i++)
        {
            for (var j = 0; j < H - 2; j++)
            {
                var facemask =
                      ((state[i + 0 + B * (j + 0)] & 1) << 0)
                    + ((state[i + 1 + B * (j + 0)] & 1) << 1)
                    + ((state[i + 2 + B * (j + 0)] & 1) << 2)
                    + ((state[i + 0 + B * (j + 1)] & 1) << 3)
                    + ((state[i + 1 + B * (j + 1)] & 1) << 4)
                    + ((state[i + 2 + B * (j + 1)] & 1) << 5)
                    + ((state[i + 0 + B * (j + 2)] & 1) << 6)
                    + ((state[i + 1 + B * (j + 2)] & 1) << 7)
                    + ((state[i + 2 + B * (j + 2)] & 1) << 8);

                if (facemask == 0)
                {
                    continue;
                }

                for (var k = 0; k < dicefaces.Length; k++)
                {
                    if (dicefaces[k] == facemask)
                    {
                        //setzen Augen -> 3
                        for (var i2 = i; i2 < i + 3; i2++)
                        {
                            for (var j2 = j; j2 < j + 3; j2++)
                            {
                                var index = i2 + B * j2;
                                if (state[index] == 1)
                                {
                                    state[index] = 3;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }



        bool allgood = true;
        bool allbad = true;

        for (var i = 0; i < state.Length; i++) {
            var e = state[i];
            if (e == 3)
            {
                allbad = false;
                if (allgood == false)
                {
                    break;
                }
            }
            if (e == 1)
            {
                allgood = false;
                if (allbad==false)
                {
                    break;
                }
            }
        }

        if (allgood)
        {
            return 1;
        } else if (allbad)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }




    //-1 all bad, 0 mixed, 1 all good
    public static int[] zählAugen(int[] state, int H, int B)
    {
        var tally = new int[state.Length];

        //elemente ändern a=3 wenn pie ein teil eines Musters ist


        for (var i = 0; i < B - 2; i++)
        {
            for (var j = 0; j < H - 2; j++)
            {
                var facemask =
                      ((state[i + 0 + B * (j + 0)] & 1) << 0)
                    + ((state[i + 1 + B * (j + 0)] & 1) << 1)
                    + ((state[i + 2 + B * (j + 0)] & 1) << 2)
                    + ((state[i + 0 + B * (j + 1)] & 1) << 3)
                    + ((state[i + 1 + B * (j + 1)] & 1) << 4)
                    + ((state[i + 2 + B * (j + 1)] & 1) << 5)
                    + ((state[i + 0 + B * (j + 2)] & 1) << 6)
                    + ((state[i + 1 + B * (j + 2)] & 1) << 7)
                    + ((state[i + 2 + B * (j + 2)] & 1) << 8);

                if (facemask == 0)
                {
                    continue;
                }

                for (var k = 0; k < dicefaces.Length; k++)
                {
                    if (dicefaces[k] == facemask)
                    {
                        //setzen Augen -> 3
                        for (var i2 = i; i2 < i + 3; i2++)
                        {
                            for (var j2 = j; j2 < j + 3; j2++)
                            {
                                var index = i2 + B * j2;
                                if (state[index] >0)
                                {
                                    state[index] = 3;
                                    tally[index]++;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }


        return tally;
    }



    public static int[][] par_generateSubsets(int remaining, int firstpossible, int[] state, int H, int B)
    {
        ConcurrentDictionary<int, int[]> stuff = new ConcurrentDictionary<int, int[]>();

        Parallel.For(firstpossible, state.Length + 1 - remaining, i =>
       {
           var clone = (int[])state.Clone();
           clone[i] = 1;
           generateSubsets(remaining - 1, i + 1, clone, stuff,H,B);
       }
       );

       return stuff.Values.ToArray();

    }



    public static void generateSubsets(int remaining, int firstpossible, int[] state, ConcurrentDictionary<int,int[]> result, int H, int B)
    {

        for(int i=firstpossible;i< state.Length + 1 - remaining; i++) 
        {
            var clone = (int[])state.Clone();
            clone[i] = 1;
            if (remaining == 1)
            {
                if (geltend(clone,H,B) == 1)
                {
                    oneify(clone);

                    var reduced = normalize(clone,H,B);
                    if (reduced != null)
                    {
                        var reducedhash = hash(reduced);

                        result.TryAdd(reducedhash, reduced);
                    }
                }
            }
            else
            {
                generateSubsets(remaining - 1, i + 1, clone, result,H,B);
            }
        }
    
    }

    public static int boundshash(int[] ar, int H, int B)
    {
        int obereZeile = 0;
        for (var j = 0; j < H; j++)
        {
            var leerezeile = true;
            for (var i = 0; i < B; i++)
            {
                if (ar[i + B * j] > 0)
                {
                    leerezeile = false;
                    break;
                }
            }
            if (leerezeile == false)
            {
                break;
            }
            obereZeile++;
        }


        int untereZeile = H-1;
        for (var j = H-1; j >=0; j--)
        {
            var leerezeile = true;
            for (var i = 0; i < B; i++)
            {
                if (ar[i + B * j] > 0)
                {
                    leerezeile = false;
                    break;
                }
            }
            if (leerezeile == false)
            {
                break;
            }
            untereZeile--;
        }

        int linkeSpalte = 0;
        for (var i = 0; i < B; i++)
        {
            var leereSpalte = true;
            for (var j = 0; j < H; j++)
            {
                if (ar[i + B * j] > 0)
                {
                    leereSpalte = false;
                    break;
                }
            }
            if (leereSpalte == false)
            {
                break;
            }
            linkeSpalte++;
        }

        int rechteSpalte = B-1;
        for (var i = B-1; i >=0; i--)
        {
            var leereSpalte = true;
            for (var j = 0; j < H; j++)
            {
                if (ar[i + B * j] > 0)
                {
                    leereSpalte = false;
                    break;
                }
            }
            if (leereSpalte == false)
            {
                break;
            }
            rechteSpalte--;
        }

        //Console.WriteLine($"Bounds: ({linkeSpalte},{obereZeile}) -> ({rechteSpalte},{untereZeile})");

        var g_b = rechteSpalte - linkeSpalte + 1;
        var g_h = untereZeile - obereZeile + 1;


        var subrect = new int[g_b * g_h];

        for (var x = 0; x < g_b; x++)
        {
            for (var y = 0; y < g_h; y++)
            {
                var i_local = x + g_b * y;
                var i_global = linkeSpalte + x + B*(obereZeile+y );
                subrect[i_local] = ar[i_global];
            }
        }
        subrect = normalize(subrect, g_h, g_b);
        return hash(subrect);
    }

    public static int hash(int[] ar)
    {
        int result = 0;
        for (var i = 0; i < ar.Length; i++)
        {
            result += (ar[i] & 1) << i;
        }
        return result;
    }

    public static void oneify(int[] ar)
    {
        for (var i = 0; i < ar.Length; i++)
        {
            if (ar[i] == 3)
            {
                ar[i] = 1;
            }
        }
    }

    public static List<int[]> perturbations(int[] orig, int H, int B)
    {
        List<int[]> result = new List<int[]>();
        for (var i = 0; i < B; i++) {
            for (var j = 0; j < H; j++)
            {
                if (orig[i + B * j] != 0)
                {
                    //horizontal shove
                    //pushed from left
                    if (i > 1)
                    {
                        if (orig[i - 1 + B * j]==0 && orig[i - 2 + B * j] == 0)
                        {
                            var prestate = (int[])orig.Clone();
                            prestate[i - 1 + B * j] = 5;
                            prestate[i + B * j] = 0;
                            result.Add(prestate);
                        }
                    }
                    //von rechts verschoben
                    if (i < B-2)
                    {
                        if (orig[i + 1 + B * j] == 0 && orig[i + 2 + B * j] == 0)
                        {
                            //push left
                            var prestate = (int[])orig.Clone();
                            prestate[i + 1 + B * j] = 5;
                            prestate[i + B * j] = 0;
                            result.Add(prestate);
                        }
                    }
                    //vertical shove
                    //from above
                    if (j > 1)
                    {
                        if (orig[i + B * (j - 1)] == 0 && orig[i + B * (j -2)] == 0)
                        {
                            //push up
                            var prestate = (int[])orig.Clone();
                            prestate[i + B * (j - 1)] = 5;
                            prestate[i + B * (j)] = 0;
                            result.Add(prestate);

                        }
                    }
                    //from below
                    if ( j < H - 2)
                    {
                        if (orig[i + B * (j + 1)] == 0 && orig[i + B * (j + 2)] == 0)
                        {
                            //push up
                            var prestate = (int[])orig.Clone();
                            prestate[i + B * (j + 1)] = 5;
                            prestate[i + B * (j)] = 0;
                            result.Add(prestate);

                        }
                    }
                }
            }
        }

        result = result.Where(r => geltend(r,H,B) == -1).ToList();
        return result;
    }

    public static bool filledupperleft(int[] orig, int H, int B)
    {

        var leerezeilen = true;
        for (var i = 0; i < B; i++)
        {
            if (orig[i + B * (0)] == 1 || orig[i + B * (1)] == 1 || orig[i + B * (H - 1)] == 1)
            {
                leerezeilen = false;
                break;
            }
        }
        if (leerezeilen)
        {
            return false;
        }

        var leerespalten = true;
        for (var j = 2; j < H; j++)
        {
            if (orig[0 + B * (j)] == 1 || orig[1 + B * (j)] == 1 || orig[B-1 + B * (j)] == 1)
            {
                leerespalten = false;
                break;
            }
        }

        if (leerespalten)
        {
            return false;
        }


        return true;
    }

    public static int[] normalize(int[] orig, int H, int B)
    {
        int[] cur = cur = (int[])orig.Clone();
        int curhash = 0;
        if (filledupperleft(orig,H,B))
        {
            curhash = hash(cur);
        }

        int[] buffer = new int[orig.Length];

        //h flip
        for (var i = 0; i < B; i++)
        {
            for (var j = 0; j < H; j++)
            {
                buffer[ i + B * j ] = orig[ (B - 1 - i) + B * j ];
            }
        }

        if (filledupperleft(buffer,H,B))
        {
            var candhash = hash(buffer);
            if (candhash > curhash)
            {
                curhash = candhash;
                Array.Copy(buffer, cur, cur.Length);
            }
        }

        //v flip
        for (var i = 0; i < B; i++)
        {
            for (var j = 0; j < H; j++)
            {
                buffer[ i + B * j ] = orig[ i + B * (H - 1 - j) ];
            }
        }

        if (filledupperleft(buffer, H, B))
        {
            var candhash = hash(buffer);
            if (candhash > curhash)
            {
                curhash = candhash;
                Array.Copy(buffer, cur, cur.Length);
            }
        }

        //vh flip
        for (var i = 0; i < B; i++)
        {
            for (var j = 0; j < H; j++)
            {
                buffer[i + B * j] = orig[(B - 1 - i) + B * (H - 1 - j)];
            }
        }

        if (filledupperleft(buffer,H,B))
        {
            var candhash = hash(buffer);
            if (candhash > curhash)
            {
                curhash = candhash;
                Array.Copy(buffer, cur, cur.Length);
            }
        }

        if (B == H)
        {
            //r
            for (var i = 0; i < B; i++)
            {
                for (var j = 0; j < H; j++)
                {
                    buffer[(i) + B * (j)] = orig[(j) + B * (i)];
                }
            }

            if (filledupperleft(buffer,H,B))
            {
                var candhash = hash(buffer);
                if (candhash > curhash)
                {
                    curhash = candhash;
                    Array.Copy(buffer, cur, cur.Length);
                }
            }


            //rv
            for (var i = 0; i < B; i++)
            {
                for (var j = 0; j < H; j++)
                {
                    buffer[(i) + B * (j)] = orig[(B-1-j) + B * (i)];
                }
            }

            if (filledupperleft(buffer,H,B))
            {
                var candhash = hash(buffer);
                if (candhash > curhash)
                {
                    curhash = candhash;
                    Array.Copy(buffer, cur, cur.Length);
                }
            }

            //hr
            for (var i = 0; i < B; i++)
            {
                for (var j = 0; j < H; j++)
                {
                    buffer[(i) + B * (j)] = orig[(B-1-j) + B * (H-1-i)];
                }
            }

            if (filledupperleft(buffer,H,B))
            {
                var candhash = hash(buffer);
                if (candhash > curhash)
                {
                    curhash = candhash;
                    Array.Copy(buffer, cur, cur.Length);
                }
            }


            //rv
            for (var i = 0; i < B; i++)
            {
                for (var j = 0; j < H; j++)
                {
                    buffer[(i) + B * (j)] = orig[( j) + B * (H-1-i)];
                }
            }

            if (filledupperleft(buffer,H,B))
            {
                var candhash = hash(buffer);
                if (candhash > curhash)
                {
                    curhash = candhash;
                    Array.Copy(buffer, cur, cur.Length);
                }
            }

        }

        if (curhash == 0)
        {
            return null;
        }
        else
        {
            return cur;
        }
    }

    public static string printMap(int[] arr,  int H, int B, bool maskmode=false)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
    
    
        for (var j = 0; j < H; j++)
        {
            for (var i = 0; i < B; i++)
            {
                var v = arr[i + B * j];

                if (v == 0)
                {
                    sb.Append('.');
                } else if (maskmode)
                {
                    sb.Append('O');
                } else 
                {
                    sb.Append(v);
                }
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }

    public static string printMapPair(int[] arr, int[] arr2, int H, int B)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();


        for (var j = 0; j < H; j++)
        {
            for (var i = 0; i < B; i++)
            {
                var v = arr[i + B * j];

                if (v == 0)
                {
                    sb.Append('.');
                }
                else 
                {
                    sb.Append('O');
                }
            }
            sb.Append("    ");

            for (var i = 0; i < B; i++)
            {
                var v = arr2[i + B * j];

                if (v == 0)
                {
                    if (arr[i + B * j] == 5)
                    {
                        sb.Append('*');
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }
                else
                {
                    sb.Append(v);
                }
            }

            sb.Append('\n');
        }
        return sb.ToString();
    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    public static void Main()
    {


        const int H = 6;
        const int B = 6;
        const int AUGE_ZAHL = 4;


        var init = new int[H * B];
        //var subsets = new List<int[]>();
        //Console.WriteLine("generating subsets series");

        Console.WriteLine("generating subsets parallel");
        var watch2 = System.Diagnostics.Stopwatch.StartNew();

        var subsets = par_generateSubsets(AUGE_ZAHL, 0, init,H,B);


        watch2.Stop();
        var elapsedMs2 = watch2.ElapsedMilliseconds;
        Console.WriteLine("timed: " + elapsedMs2 / 1000.0f);

        Console.WriteLine("generated");
        //1 generate final states

        Console.WriteLine($"ss length before {subsets.Length}.");
        subsets = subsets.DistinctBy(ss => boundshash(ss,H,B)).ToArray();
        Console.WriteLine($"ss length after {subsets.Length}.");

        var scoreset = subsets
                        .Where(ss => perturbations(ss,H,B).Count > 0)
                        .Select(o => scoreausrechnen(o,H,B))
                        .OrderBy(n=>n).Distinct().ToArray();

        subsets = subsets.OrderBy(o => scoreausrechnen(o, H, B)).ToArray();

        var score_0 = subsets.Count(s => scoreausrechnen(s, H, B) == scoreset[0]);
        var score_1 = subsets.Count(s => scoreausrechnen(s, H, B) == scoreset[1]);


        var score_nm1 = subsets.Count(s => scoreausrechnen(s, H, B) == scoreset[scoreset.Length - 2]);
        var score_n = subsets.Count(s => scoreausrechnen(s, H, B) == scoreset[scoreset.Length-1]);

        var count = subsets.Count();
        Console.WriteLine($"{count} configurations found, with scores from {scoreset[0]} to {scoreset[scoreset.Length-1]}.");
        Console.WriteLine($"{score_0} have score of {scoreset[0]}.");
        Console.WriteLine($"{score_1} have score of {scoreset[1]}.");
        Console.WriteLine($"{score_nm1} have score of {scoreset[scoreset.Length - 2]}.");
        Console.WriteLine($"{score_n} have score of {scoreset[scoreset.Length - 1]}.\n");

        for (var i = 0; i < subsets.Length; i++)
        {
            var subset = subsets[i];
            var perturbs = perturbations(subset, H, B);
            if (perturbs.Count == 0)
            {
                continue;
            }
            Console.WriteLine("\n\n----------\n\n");
            Console.WriteLine(printMapPair(perturbs[0], zählAugen(subset, H, B),H,B));
            var score = scoreausrechnen(subset,H,B);
            Console.WriteLine("Score:"+score);

        }
    }
}
