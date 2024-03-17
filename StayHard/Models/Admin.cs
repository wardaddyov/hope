namespace StayHard.Models;

// What if we want to create the login method in a way that both the admin and the student can login
// since we do not have many admins, we can first look for a match in admins and then if we didn't get any results
// we can search in the students

public class Admin
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string HashedPassword { get; set; }

    public int AccessLevel { get; set; }
}