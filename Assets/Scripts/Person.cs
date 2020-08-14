public class Person
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string ID { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Species { get; set; }
    public bool isSpy { get; set; }
    public string databaseID { get; set; }
    public int Mistake { get; set; }
    public Person(string firstName, string lastName, string id, int age, string gender, string species, bool isSpy, string databaseID, int mistake)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.ID = id;
        this.Age = age;
        this.Gender = gender;
        this.Species = species;
        this.isSpy = isSpy;
        this.databaseID = databaseID;
        this.Mistake = mistake;
    }

    public string getName()
    {
        return firstName + " " + lastName;
    }

    public override string ToString()
    {
        return getName() + " #" + ID + " is a " + Age + "-year-old " + Gender + " of the species " + Species + ".";
    }
}
