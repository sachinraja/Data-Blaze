using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DataCard : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText = null;
    [SerializeField] private TMP_Text genderText = null;
    [SerializeField] private TMP_Text ageText = null;
    [SerializeField] private TMP_Text speciesText = null;
    [SerializeField] private TMP_Text idText = null;

    [SerializeField] private TextAsset firstNameFile = null;
    [SerializeField] private TextAsset lastNameFile = null;
    [SerializeField] private TextAsset speciesFile = null;

    public string[] firstNames;
    public string[] lastNames;
    string[] speciesList;

    public Person person;
    public List<Person> people = new List<Person>();

    private void Start()
    {
        //randomize name
        firstNames = firstNameFile.text.Split("\n"[0]);
        lastNames = lastNameFile.text.Split("\n"[0]);
        speciesList = speciesFile.text.Split("\n"[0]);

        person = GeneratePerson();

        nameText.text = person.getName();
        idText.text = "ID: " + person.ID;
        ageText.text = "Age: " + person.Age.ToString();
        genderText.text = person.Gender;
        speciesText.text = "Species: " + person.Species;
    }

    private Person GeneratePerson()
    {
        string randomFirstName = string.Empty;
        string randomLastName = string.Empty;

        bool uniqueName = false;

        //keep running until they have unique name
        while (uniqueName == false)
        {
            randomFirstName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
            randomLastName = lastNames[UnityEngine.Random.Range(0, lastNames.Length)];

            uniqueName = true;

            foreach (var person in people)
            {
                if (randomFirstName + " " + randomLastName == person.getName())
                {
                    uniqueName = false;
                }
            }
        }

        //randomize databaseID
        string databaseID = GetRandomString(8);

        //randomize gender
        int genderRandom = UnityEngine.Random.Range(0, 10);
        string gender = "";

        if (genderRandom >= 0 && genderRandom > 4)
            gender = "M";
        else if (genderRandom >= 4 && genderRandom >= 8)
            gender = "F";
        else
            gender = "Non-binary";

        //randomize species
        string species = speciesList[UnityEngine.Random.Range(0, speciesList.Length)];

        //randomize spy
        bool isSpy = UnityEngine.Random.Range(0, 3) == 0;

        string id = databaseID;

        //spy makes an error
        int mistake = UnityEngine.Random.Range(0, 3);

        if (isSpy)
        {
            if (mistake == 0)
            {
                //redo if databaseID somehow equals id after change
                while (id == databaseID)
                {
                    //random chance of letters/numbers changing
                    for (int i = 0; i < id.Length; i++)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0)
                        {
                            char charReplacement = chars[UnityEngine.Random.Range(0, chars.Length)];
                            id = id.Substring(0, i) + charReplacement + id.Substring(i + 1);
                        }
                    }
                }
            }
        }

        Person new_person = new Person(randomFirstName, randomLastName, id, UnityEngine.Random.Range(20, 80), gender, species, isSpy, databaseID, mistake);

        //add to list for reference later
        people.Add(new_person);
        return new_person;
    }

    public void NextPerson()
    {
        person = GeneratePerson();

        nameText.text = person.getName();
        idText.text = "ID: " + person.ID;
        ageText.text = "Age: " + person.Age.ToString();
        genderText.text = "Gender: " + person.Gender;
        speciesText.text = "Species: " + person.Species;
    }

    System.Random randomID = new System.Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public string GetRandomString(int length)
    {
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[randomID.Next(s.Length)]).ToArray());
    }
}
