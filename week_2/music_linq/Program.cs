using System;
using System.Collections.Generic;
using System.Linq;
using JsonData;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Collections to work with
            List<Artist> Artists = JsonToFile<Artist>.ReadJson();
            List<Group> Groups = JsonToFile<Group>.ReadJson();

            //========================================================
            //Solve all of the prompts below using various LINQ queries
            //========================================================

            //There is only one artist in this collection from Mount Vernon, what is their name and age?
           
           Console.WriteLine("**Solution One:");
           var solution1 = Artists.Where((Artist) => 
                {
                    return Artist.Hometown == "Mount Vernon";
                }).ToList();
            foreach(Artist person in solution1){
                Console.WriteLine(person.ArtistName + " : "  + person.Age);
            }

            //Who is the youngest artist in our collection of artists?

            Console.WriteLine("**Solution Two:");
            var solution2 = Artists.OrderBy((Artist) => Artist.Age).First();
            Console.WriteLine(solution2.ArtistName + " : " + solution2.Age);

            //Display all artists with 'William' somewhere in their real name

           Console.WriteLine("**Solution Three:");
           var solution3 = Artists.Where((Artist) => 
                {
                    return Artist.RealName.Contains("William");
                }).ToList();
            foreach(Artist person in solution3){
                Console.WriteLine(person.ArtistName + " : "  + person.RealName);
            }

            //Display all groups with names less than 8 characters in length

            Console.WriteLine("**Solution Four:");
            var solution4 = Groups.Where((Group) => 
                {
                    return Group.GroupName.Length < 8;
                }).ToList();
            foreach(Group thing in solution4){
                Console.WriteLine(thing.GroupName);
            }
            //Display the 3 oldest artist from Atlanta

           Console.WriteLine("**Solution Five:");
           var solution5 = Artists.Where((Artist) => 
                {
                    return Artist.Hometown == "Atlanta";
                })
                .OrderByDescending((Artist) => Artist.Age)
                .Take(3);
            foreach(Artist person in solution5){
                Console.WriteLine(person.ArtistName + " : " + person.Age);
            }

            //(Optional) Display the Group Name of all groups that have members that are not from New York City

            Console.WriteLine("**Solution Six:");
            var solution6 = Artists.Where((Artist) => Artist.Hometown != "New York City").Join(
                Groups,
                (Artist) => Artist.GroupId,
                (Group) => Group.Id,
                (Artist, Group) => {Artist.Group = Group; return Group;}
            ).Distinct().ToList();

            foreach(Group thing in solution6){
                Console.WriteLine(thing.GroupName);
            }

            //(Optional) Display the artist names of all members of the group 'Wu-Tang Clan'

            Console.WriteLine("**Solution Seven:");
            var solution7a = Groups.Where((Group) => Group.GroupName == "Wu-Tang Clan").First();
            var solution7b = Artists.Where((Artist) => 
            {
                return Artist.GroupId ==  solution7a.Id;
            })
            .ToList();

            foreach(Artist person in solution7b){
                Console.WriteLine(person.ArtistName);
            }
        }
    }
}


//SOLUTION

        // namespace ConsoleApplication
        // {
        //     public class Program
        //     {
        //         public static void Main(string[] args)
        //         {
        //             //Collections to work with
        //             List<Artist> Artists = JsonToFile<Artist>.ReadJson();
        //             List<Group> Groups = JsonToFile<Group>.ReadJson();

        //             //========================================================
        //             //Solve all of the prompts below using various LINQ queries
        //             //========================================================

        //             //There is only one artist in this collection from Mount Vernon, what is their name and age?
        //             Artist FromMtVernon = Artists.Where(artist => artist.Hometown == "Mount Vernon").Single();
        //             Console.WriteLine($"The artist {FromMtVernon.ArtistName} from Mt Vernon is {FromMtVernon.Age} years old");
        //             //Who is the youngest artist in our collection of artists?
        //             Artist Youngest = Artists.OrderBy(artist => artist.Age).First();
        //             Console.WriteLine($"The Youngest artist is {Youngest.ArtistName}");
        //             //Display all artists with 'William' somewhere in their real name
        //             List<Artist> Williams = Artists.Where(artist => artist.RealName.Contains("William")).ToList();
        //             Console.WriteLine("The Following artists have William in their real names:");
        //             foreach(var artist in Williams) {
        //                 Console.WriteLine(artist.ArtistName + " - " + artist.RealName);
        //             }

        //             //Display the names of all groups less than 8 characters in length
        //             List<Group> groupByLength = Groups.Where(group => group.GroupName.Length < 8).ToList();
        //             Console.WriteLine("All groups with names less than 8 in length:");
        //             foreach(var group in groupByLength){
        //                 Console.WriteLine(group.GroupName);
        //             }

        //             //Display the 3 oldest artist from Atlanta
        //             List<Artist> Oldest = Artists.Where(artist=> artist.Hometown == "Atlanta")
        //                                     .OrderByDescending(artist => artist.Age)
        //                                     .Take(3)
        //                                     .ToList();
        //             Console.WriteLine("The three oldest artists from Atlanta are:");
        //             foreach(var artist in Oldest) {
        //                 Console.WriteLine(artist.ArtistName + " - " + artist.Age);
        //             }
        //             //(Optional) Display the Group Name of all groups that have members that are not from New York City
        //             List<string> NonNewYorkGroups = Artists
        //                                 .Join(Groups, artist => artist.GroupId, group => group.Id, (artist, group) => { artist.Group = group; return artist;})
        //                                 .Where(artist => (artist.Hometown != "New York City" && artist.Group != null))
        //                                 .Select(artist => artist.Group.GroupName)
        //                                 .Distinct()
        //                                 .ToList();
        //             Console.WriteLine("All groups with members not from New York City:");
        //             foreach(var group in NonNewYorkGroups){
        //                 Console.WriteLine(group);
        //             }

        //             //(Optional) Display the artist names of all members of the group 'Wu-Tang Clan'
        //             Group WuTang = Groups.Where(group => group.GroupName == "Wu-Tang Clan")
        //                                     .GroupJoin(Artists, 
        //                                         group => group.Id,
        //                                         artist => artist.GroupId,
        //                                         (group, artists) => { group.Members = artists.ToList(); return group;})
        //                                     .Single();
        //             Console.WriteLine("List of Artist in the Wu-Tang Clan:");
        //             foreach(var artist in WuTang.Members){
        //                 Console.WriteLine(artist.ArtistName);
        //             }
        //         }
        //     }
        // }