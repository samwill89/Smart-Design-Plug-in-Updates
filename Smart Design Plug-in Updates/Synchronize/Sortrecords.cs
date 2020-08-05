using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;
using NaturalComparer;

namespace Smart_Design_Plug_in_Updates.Synchronize
{
    class Sortrecords
    {
        public List<WpfApp1.Models.Item> RecordsSort(List<WpfApp1.Models.Item> RecordsUnsorted)
        {
            List<string> ItemNumbers = new List<string>();
            List<string> OriginalItemNumbers = new List<string>();
            List<int> Index = new List<int>();
            List<Item> RecordSorted = new List<Item>();
            foreach(Item item in RecordsUnsorted)
            {
                ItemNumbers.Add(item.ItemNumber);

            }
            foreach (string itemnum in ItemNumbers)
            {
                OriginalItemNumbers.Add(itemnum);

            }
            NaturalComparer.NaturalComparer Comparer = new NaturalComparer.NaturalComparer();
            ItemNumbers.Sort(Comparer);
            foreach (string itemnum in ItemNumbers)
            {
                int idx = OriginalItemNumbers.IndexOf(itemnum);
                Index.Add(idx);
            }
            int Counter = 0;
            foreach( Item item in RecordsUnsorted)
            {
                RecordSorted.Add(RecordsUnsorted[Index[Counter]]);
                Counter = Counter + 1;
            }


            return RecordSorted;
        }
    }
}
