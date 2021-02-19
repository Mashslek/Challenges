using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Guest> Guests = new List<Guest>();
            
            StreamReader testFile = new StreamReader("D:\\InputFilePath");
            int inputs = Int32.Parse(testFile.ReadLine());
            for (int x = 0; x < inputs; x++)
            {
                string[] line = testFile.ReadLine().Split(' ');
                Guests.Add(new Guest
                {
                    guestId = x,
                    beauty = Int32.Parse(line[0]),
                    richesness = Int32.Parse(line[1]),
                    donation = Int32.Parse(line[2])
                });
            }
            Guests = Guests.GroupBy(x => x.richesness.ToString() + "|" + x.beauty)
                          .Select((x, index) => {
                              return new Guest {
                                  guestId = index,
                                  donation = x.Sum(y => y.donation),
                                  beauty = x.First().beauty,
                                  richesness = x.First().richesness,
                              };
                          }).ToList();
            Guests.ForEach(x =>
            {
                x.guestRestriction = Guests.Where(y => (x.beauty < y.beauty && x.richesness >= y.richesness) ||
                                                       (x.beauty >= y.beauty && x.richesness < y.richesness) ||
                                                       (y.beauty < x.beauty && y.richesness >= x.richesness) ||
                                                       (y.beauty >= x.beauty && y.richesness < x.richesness)).Select(y => y.guestId).ToList();
               // Console.WriteLine("ID : " + x.guestId + "\n Beleza : " + x.beauty + "\n Riqueza : " + x.richesness + "\n Doação : " + x.donation + "\n Restricao : " + String.Join("|",x.guestRestriction) + "__________________________________");
            });
            
            int solution = Guests.Select(x => getBestDonationByFixedGuest(x, Guests.OrderBy(y => y.guestId).ToList())).Max();
            Console.WriteLine(solution);
            StreamWriter streamWriter = new StreamWriter("D:\\OutputFilePath");            
            streamWriter.WriteLine("Resposta : " + solution);
            streamWriter.WriteLine(String.Join("\n", calculatedSubTrees.Select(x => String.Join("|", x.Key+ "\t\t" + x.Value))));
            streamWriter.Flush();
        }
        static int getBestDonationByFixedGuest(Guest curr, List<Guest> possibleGests) {

            possibleGests = possibleGests.Where(x => !curr.guestRestriction.Contains(x.guestId)).ToList();
            int subTreeMax;
            List<int> possibleDonations = new List<int>();

            if (possibleGests.Count() == 1)
                return curr.donation;

            possibleGests.Remove(curr);

            if (calculatedSubTrees.TryGetValue(String.Join("|", possibleGests.Select(x => x.guestId.ToString())), out subTreeMax))
            {
                return curr.donation + subTreeMax;
            }

            for (int x = 0;x < possibleGests.Count(); x++)
                possibleDonations.Add(getBestDonationByFixedGuest(possibleGests[x], possibleGests));

            calculatedSubTrees.Add(String.Join("|",possibleGests.Select(x=>x.guestId.ToString())), possibleDonations.Max());
            return curr.donation + possibleDonations.Max();
        }
        static Dictionary<string, int> calculatedSubTrees = new Dictionary<string, int>();
    }
    class Guest {
        public List<int> guestRestriction { get; set; }
        public int guestId { get; set; }
        public int beauty { get; set; }
        public int richesness { get; set; }
        public int donation { get; set; }
    }
}
