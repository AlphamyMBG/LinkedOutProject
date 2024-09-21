


using BackendApp.auth;
using BackendApp.Data;
using BackendApp.Model;

namespace Util.DataFeeding;


public static class DummyDataFeeder
{
    public static void FillWithDummyData(ApiContext context)
    {
        var adminUser = new AdminUser("a@emailer.com", EncryptionUtility.HashPassword("bigchungusplayer6969f12"));
        var user1 = new RegularUser(
                "b@emailer.com",
                EncryptionUtility.HashPassword("bigchungusplayer6969f12"), 
                "name", "poop", null, new(
                    "6900000000", "My House :D", 
                    ["Fullstack Dotnet Developer at Microsoft: 4 years", "Game Developer at Microsoft: 2 years"], 
                    "Dead inside", 
                    ["Senior Software Developer", "Junior Software Engineer"], 
                    ["Bachelor's degree in Computer Science from National Kapodistrian University of Athens"]));
        var user2 = new RegularUser(
                "c@emailer.gr",
                EncryptionUtility.HashPassword("bigchungusplayer6969f12"), 
                "Ninja", "Blevins", null, new(
                    "6950000769", "His House :D", 
                    ["Fullstack Dotnet Developer at Microsoft: 4 years", "Game Developer at Microsoft: 2 years"], 
                    "Dead inside", 
                    ["Senior Software Developer", "Junior Software Engineer"], 
                    ["Bachelor's degree in Computer Science from National Kapodistrian University of Athens"]));
        var user3 = new RegularUser(
                "d@emailer.com",
                EncryptionUtility.HashPassword("bigchungusplayer6969f12"), 
                "namasteel", "babies", null, new(
                    "6900000000", "In hiding (I am convicted of illegal marijuana possession)", 
                    ["Hitman for the Russian Government: 7 years experience"], 
                    "Unemployed", 
                    ["Senior Hitman", "Junior Software Developer"], 
                    ["Certificate of Excellence in Service to the People"]));
        var user4 = new RegularUser(
                "e@emailer.com",
                EncryptionUtility.HashPassword("bigchungusplayer6969f12"), 
                "Person", "Eater", null, new(
                    "6900000000", "Belarus", 
                    ["Personal Chef for Hannibal Lecter: 3 years"], 
                    "Unemployed", 
                    ["Chef", "Cook", "Head Chef"], 
                    ["Doctorate In Cooking Somehow I don't know."]));
        var user5 = new RegularUser(
                "f@emailer.com",
                EncryptionUtility.HashPassword("bigchungusplayer6969f12"), 
                "Jean", "Schena", null, new(
                    "6900000000", "Florida", 
                    ["Professional Wrestling: 10 years"], 
                    "Women's World Heavyweight Champion in WUUE", 
                    ["Karate", "Olympic Wrestler", "Professional Wrestler"], 
                    ["Community Colledge Graduate"]));

        var post1 = new Post(
            user1, [user2, user3], 
            DateTime.Now, 
            [], 
            "I'm starting to have a lot of fun building posts in Rust.",
            [],
            false
        );
        var post2 = new Post(
            user1, [], 
            DateTime.Now, 
            [], 
            "I hate Javascript. It's the worst language ever!",
            [],
            false
        );
        var post3 = new Post(
            user1, [user2], 
            DateTime.Now, 
            [], 
            "Elixir is starting to really grow on me...",
            [],
            false
        );
        var post4 = new Post(
            user3, [user1, user2],
            DateTime.Now,
            [],
            "Killed the president of Cuba today. Hard times ahead, have to be on the lookout for Cuban Intelligence.... But hard times make better men.",
            [],
            false
        );
        var jobPost1 = new JobPost(
            user1, [], 
            DateTime.Now, 
            [],
            "Needed: ASP .NET developer",
            "Our company needs an ASP .NET developer for a project. Could you help?",
            ["ASP .NET Backend Developer", "Junior Software Developer", "Senior Software Developer"]
        );
        var jobPost2 = new JobPost(
            user2, [], 
            DateTime.Now, 
            [],
            "Searching for Moring Shift Security Officer",
            "I have noticed strange men with pagers walking around the front of my apartment complex and I have begun to worry about the safety of my neightbours. " 
            + "I need someone to cover the morning shift (4AM to 12PM)",
            ["Just be like, very big. Like very very big. And know how to use a gun, hopefully"]
        );


        
        var connection1 = new Connection(user1, user3, false, DateTime.Now);
        var connection2 = new Connection(user1, user2, true, DateTime.Now);

        context.AdminUsers.Add(adminUser);
        context.RegularUsers.Add(user1);
        context.RegularUsers.Add(user2);
        context.RegularUsers.Add(user3);
        context.RegularUsers.Add(user4);
        context.RegularUsers.Add(user5);
        context.SaveChanges();
        context.Posts.Add(post1);
        context.Posts.Add(post2);
        context.Posts.Add(post3);
        context.Posts.Add(post4);
        context.JobPosts.Add(jobPost1);
        context.JobPosts.Add(jobPost2);
        context.Connections.Add(connection1);
        context.Connections.Add(connection2);
        context.SaveChanges();
    }
}