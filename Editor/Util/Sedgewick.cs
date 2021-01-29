namespace TypeReferences.Editor.Util
{
    using System.Runtime.CompilerServices;
    using TypeDropdown;

    /// <summary>
    /// Multi-key quicksort algorithm written by Robert Sedgewick and translated to C# by Stefan Savev.
    /// Modified to sort TypeItems instead of strings.
    /// </summary>
    /// <remarks> Source: https://www.codeproject.com/Articles/146086/Fast-String-Sort-in-C-and-F </remarks>
    internal static class Sedgewick
    {
        public static void SortInPlace(TypeItem[] input)
        {
            SortInPlace(input, 0, input.Length, 0);
        }

        private static void SortInPlace(TypeItem[] input, int a, int n, int depth)
        {
            if (n < 10)
            {
                InsertionSort(input, a, n, depth);
                return;
            }

            int pl = a;
            int pm = a + n / 2;
            int pn = a + (n - 1);

            if (n > 30)
            {
                // On big arrays, pseudomedian of 9
                int d = n / 8;
                pl = MedianOf3(input, pl, pl + d, pl + 2 * d, depth);
                pm = MedianOf3(input, pm - d, pm, pm + d, depth);
                pn = MedianOf3(input, pn - 2 * d, pn - d, pn, depth);
            }

            pm = MedianOf3(input, pl, pm, pn, depth);
            (input[a], input[pm]) = (input[pm], input[a]);

            int pa = a + 1;
            int pb = a + 1;

            int pd = a + n - 1;
            int pc = a + n - 1;

            char partVal = CharOrNull(input[a].Path, depth);
            int len = input[a].Path.Length;
            bool empty = len == depth;

            int r;

            while (true)
            {
                while (pb <= pc && (r = empty ? (input[pb].Path.Length - depth) : ((depth == input[pb].Path.Length) ? -1 : (input[pb].Path[depth] - partVal))) <= 0)
                {
                    if (r == 0)
                    {
                        (input[pa], input[pb]) = (input[pb], input[pa]);
                        pa++;
                    }

                    pb++;
                }

                while (pb <= pc && (r = empty ? (input[pc].Path.Length - depth) : ((depth == input[pc].Path.Length) ? -1 : (input[pc].Path[depth] - partVal))) >= 0)
                {
                    if (r == 0)
                    {
                        (input[pc], input[pd]) = (input[pd], input[pc]);
                        pd--;
                    }

                    pc--;
                }

                if (pb > pc)
                    break;

                (input[pb], input[pc]) = (input[pc], input[pb]);
                pb++;
                pc--;
            }

            pn = a + n;
            if (pa - a < pb - pa)
            {
                r = pa - a;
            }
            else
            {
                r = pb - pa;
            }

            // Swapping pointers to strings
            VecSwap(input, a, pb - r, r);
            if (pd - pc < pn - pd - 1)
            {
                r = pd - pc;
            }
            else
            {
                r = pn - pd - 1;
            }

            VecSwap(input, pb, pn - r, r);
            r = pb - pa;

            // By definition x[a + r] has at least one element
            if (pa - a + pn - pd - 1 > 1 && input[a + r].Path.Length > depth)
            {
                SortInPlace(input, a + r, pa - a + pn - pd - 1, depth + 1);
            }

            if (r > 1)
            {
                SortInPlace(input, a, r, depth);
            }

            if ((r = pd - pc) > 1)
            {
                SortInPlace(input, a + n - r, r, depth);
            }
        }

        // Pathological case is: strings with long common prefixes will cause long running times
        private static void InsertionSort(TypeItem[] x, int a, int n, int depth)
        {
            for (int pi = a + 1; --n > 0; pi++)
            {
                for (int pj = pi; pj > a; pj--)
                {
                    string s = x[pj - 1].Path;
                    string t = x[pj].Path;
                    int d = depth;

                    int sLength = s.Length;
                    int tLength = t.Length;

                    while (d < sLength && d < tLength && s[d] == t[d]) { d++; }

                    if (d == sLength || (d < tLength && s[d] <= t[d]))
                        break;

                    int previousPj = pj - 1;
                    (x[pj], x[previousPj]) = (x[previousPj], x[pj]);
                }
            }
        }

        private static int MedianOf3(TypeItem[] x, int a, int b, int c, int depth)
        {
            char va = CharOrNull(x[a].Path, depth);
            char vb = CharOrNull(x[b].Path, depth);

            if (va == vb)
                return a;

            char vc = CharOrNull(x[c].Path, depth);

            if (vc == va || vc == vb)
                return c;

            return va < vb
                ? (vb < vc ? b : (va < vc ? c : a))
                : (vb > vc ? b : (va < vc ? a : c));
        }

        private static void VecSwap(TypeItem[] x, int a, int b, long n)
        {
            while (n-- > 0)
            {
                (x[a], x[b]) = (x[b], x[a]);
                a++;
                b++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char CharOrNull(string s, int pos) => pos < s.Length ? s[pos] : (char) 0;
    }
}
