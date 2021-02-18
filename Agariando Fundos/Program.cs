using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Guest> Guests = new List<Guest>();
            int inputs = Int32.Parse(Console.ReadLine());

            int[,] donateMatrix = new int[inputs,3];
            for (int x = 0; x < inputs; x++)
            {
                string[] line = Console.ReadLine().Split(' ');
                Guests.Add(new Guest
                {
                    guestId = x,
                    beauty = Int32.Parse(line[0]),
                    richesness = Int32.Parse(line[1]),
                    donation = Int32.Parse(line[2])
                });
            }
            Guests.ForEach(x =>
            {
                x.guestRestriction = Guests.Where(y => (x.beauty < y.beauty && x.richesness >= y.richesness) ||
                                                       (x.beauty >= y.beauty && x.richesness < y.richesness) ||
                                                       (y.beauty < x.beauty && y.richesness >= x.richesness) ||
                                                       (y.beauty >= x.beauty && y.richesness < x.richesness)).Select(y => y.guestId).ToList();
               // Console.WriteLine("ID : " + x.guestId + "\n Beleza : " + x.beauty + "\n Riqueza : " + x.richesness + "\n Doação : " + x.donation + "\n Restricao : " + String.Join("|",x.guestRestriction) + "__________________________________");
            });
            Console.WriteLine(Guests.Select(x => getBestDonationByFixedGuest(x, Guests)).Max());
        }
        static int getBestDonationByFixedGuest(Guest curr, List<Guest> possibleGests) {
            possibleGests = possibleGests.Where(x => !curr.guestRestriction.Contains(x.guestId)).ToList();
            List<int> possibleDonations = new List<int>();
            if (possibleGests.Count() == 1)
                return curr.donation;

            possibleGests.Remove(curr);
            for (int x = 0;x < possibleGests.Count(); x++)
            {
                possibleDonations.Add(getBestDonationByFixedGuest(possibleGests[x], possibleGests));
            }
            return curr.donation + possibleDonations.Max();
        }
    }
    class Guest {
        public List<int> guestRestriction { get; set; }
        public int guestId { get; set; }
        public int beauty { get; set; }
        public int richesness { get; set; }
        public int donation { get; set; }
    }
}
