﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelingSalesmanProblem.Engine;
using TravelingSalesmanProblem.Interface;

namespace TravelingSalesmanProblem.Core
{
    class Core
    {
        #region Fields
        List<Path> Listpath;
        MainForm form;
        public bool running = false;
        private Path current_best;
        private int size;
        #endregion

        #region Constructors
        public Core(List<Path> Lp, MainForm main)
        {
            this.Listpath = Lp;
            form = main;
            size = Lp.Count; /* Set the size. At the end of each turn, the list must have this size in order to always continue the computation */
        }
        #endregion

        #region Public Methods
        public void Init()
        {
            running = true;
            while (running)
            {
                Do();
                Application.DoEvents();
            }
                
        }
        
        public void Stop()
        {
            running = false;
        }
        #endregion

        #region Private Methods

        private void Do()
        {
            Tuple<Path, int> best = BestDistance();
            SetBestDistance(best.Item1, best.Item2);
            List<Path> lp = BestOne(Listpath, 10); /* Get the actual ten bests path */
            List<Path> Childs = DoCrossOver(lp);   /* Best paths fucks and do childs */
            Listpath.RemoveRange(0, 9);
            Mutate(Listpath);                      /* Mutate the paths (without the best) */
            Listpath.AddRange(Childs);             /* Add the childs */
            Listpath.AddRange(lp);                 /* Re-add the best one */
            if (Listpath.Count < size) /* If the list doesn't have enough path */
                Listpath.AddRange(InitGen.Gen(size - Listpath.Count, Listpath[0].Locations)); /* Add random path to complete */
        }

        private List<Path> BestOne(List<Path> lp, int howmany)
        {
            /* Return the best chromosomes */
            List<Path> result = new List<Path>();

            lp.Sort(new PathSorter());
            for (int i = 0; i<howmany;i++)
            {
                result.Add(lp[i]);
            }

            return result;
        }

        private List<Path> DoCrossOver(List<Path> lp)
        {
            /* Reproduce chromosomes */
            List<Path> result = new List<Path>();
            Path temp = null;
            foreach (Path p in lp)
            {         
                if (temp != null)
                    result.Add(Breed(p, temp));
                temp = p;
            }

            /* Check and correct mistakes */
            foreach(Path p in result)
            {
                foreach (Point point in lp[0].Locations)
                {
                    if (!p.Locations.Contains(point))
                    {
                        int i = DoublonIndex(p);
                        if (i != -1)
                            p.Locations[i] = point;
                    }
                }
            }

            return result;
        }

        private List<Path> Mutate(List<Path> lp)
        {
            foreach (Path p in lp)
            {
                p.Mute();
            }
            return lp;
        }

        private Tuple<Path, int> BestDistance()
        {
            /* Get the best best distance between all the path */
            Tuple<Path, int> temp = new Tuple<Path, int>(null, 0);

            foreach (Path p in Listpath)
            {
                int t = p.Distance();
                if (t < temp.Item2 || temp.Item2 == 0)
                {
                    temp = new Tuple<Path, int>(p, p.Distance());
                }
                   
            }
            return temp;
        }

        private void SetBestDistance( Path p, int dist)
        {
            /* Display the best actual distance */
            if (Convert.ToInt32(form.BestDistLb.Text) > dist || form.BestDistLb.Text == "Unknown")
            {
                form.BestDistLb.Text = dist.ToString();
                current_best = p;
            }
                
            
        }

        private Path Breed(Path father, Path mother)
        {
            Random rand = new Random();
            Point[] genes = new Point[father.Locations.Count];
            Point[] map = new Point[father.Locations.Count + 1]; //create a map to map the indices
            int crossoverPoint1 = rand.Next(1, father.Locations.Count - 2); //select 2 crossoverpoints, without the first and last nodes, cuz they are always the same
            int crossoverPoint2 = rand.Next(1, father.Locations.Count - 2);
            father.Locations.CopyTo(genes, 0); //give child all genes from the father
            for (int i = 0; i < genes.Length; i++) //create the map
            {
                map[i] = genes[i];
            }
            //int[] genesToCopy = new int[Math.Abs(crossoverPoint1 - crossoverPoint2)]; //allocate space for the mother genes to copy
            if (crossoverPoint1 > crossoverPoint2) //if point 1 is bigger than point 2 swap them
            {
                int temp = crossoverPoint1;
                crossoverPoint1 = crossoverPoint2;
                crossoverPoint2 = temp;
            }
            //Console.WriteLine("copy mother genes into father genes from {0} to {1}", crossoverPoint1, crossoverPoint2);
            for (int i = crossoverPoint1; i <= crossoverPoint2; i++) //from index one to the 2nd
            {
                Point value = mother.Locations[i];
                Point t = genes[map.ToList().IndexOf(value)]; //swap the genes in the child
                genes[map.ToList().IndexOf(value)] = genes[i];
                genes[i] = t;
                t = map[genes.ToList().IndexOf(map[map.ToList().IndexOf(value)])]; //swap the indices in the map
                map[genes.ToList().IndexOf(map[map.ToList().IndexOf(value)])] = map[genes.ToList().IndexOf(map[i])];
                map[genes.ToList().IndexOf(map[i])] = t;
            }
            Path child = new Path(genes.ToList());
            return child;
        }

        private int DoublonIndex(Path p)
        {
            /* Return the index of the element which is x2 */
            foreach (Point point in p.Locations)
            {
                if (p.Locations.Count(x=>x == point) > 1)
                    return p.Locations.IndexOf(point);
            }
            return -1;
        }
        #endregion
    }
}
