using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    [SerializeField] DataCard dataCard;
    [SerializeField] End_Round Stats;
    [SerializeField] GameObject consoleGameObject = null;
    [SerializeField] TMP_InputField consoleInputField = null;
    [SerializeField] TMP_Text consoleText = null;
    string[] keywords = {"help", "clear", "accept", "deny", "id", "ask"};
    private bool commandCalled = false;
    private Regex rgxFindColorStart = new Regex(@"<color=.*?>");
    private Regex rgxFindColorEnd = new Regex(@"</color>");

    private string helpString = "Call <color=#00ffffff>help</color> to see this list again." + "\n\n" +
            "<color=#00ffffff>id</color> [name] - Searches the database for the id of the person with that name.\nEx: <color=#00ffffff>id</color> John Doe." + "\n\n" +
        "<color=#00ffffff>ask</color> [question] - Asks the person a question about their card. The valid questions are name and age.\nEx: <color=#00ffffff>ask</color> name.";

    private void Start()
    {
        consoleText.text += helpString;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x < Screen.width - Screen.width / 3)
            {
                consoleGameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            consoleInputField.text += dataCard.person.getName();
        }
    }

    public void SendCommand()
    {
        if (string.IsNullOrWhiteSpace(consoleInputField.text))
        {
            return;
        }

        consoleText.text += "\n" + consoleInputField.text;
        Commands(consoleInputField.text);

        consoleInputField.text = string.Empty;
    }

    public void ColorKeywords()
    {
        if (commandCalled == false)
        {
            commandCalled = true;
            string text = consoleInputField.text.ToLower();

            //replace colors with empty text
            text = rgxFindColorStart.Replace(text, string.Empty);
            text = rgxFindColorEnd.Replace(text, string.Empty);

            foreach (var keyword in keywords)
            {
                //only find one occurence of each keyword
                text = new Regex(@keyword).Replace(text, $"<color=#00ffffff>{keyword}</color>", 1);
            }

            consoleInputField.text = text;
            consoleInputField.caretPosition = consoleInputField.text.Length;

            commandCalled = false;
        }
    }

    private void Commands(string text)
    {
        //get each word in text
        string[] words = new Regex(@" ").Split(text);
        string output = "";

        for (int i = 0; i <  words.Length; i++)
        {
            if (words[i] == "<color=#00ffffff>help</color>")
            {
                output += "\n" + helpString;
            }

            else if (words[i] == "<color=#00ffffff>clear</color>")
            {
                consoleText.text = string.Empty;
                return;
            }

            else if (words[i] == "<color=#00ffffff>accept</color>")
            {
                if (dataCard.person.isSpy == false)
                {
                    Stats.correctAmount++;
                }

                Stats.processedAmount++;
                Stats.acceptedAmount++;
                dataCard.NextPerson();
            }

            else if (words[i] == "<color=#00ffffff>deny</color>")
            {
                if (dataCard.person.isSpy == true)
                {
                    Stats.correctAmount++;
                }


                Stats.processedAmount++;
                Stats.deniedAmount++;
                dataCard.NextPerson();
            }

            else if (words[i] == "<color=#00ffffff>id</color>")
            {
                //see if personName is formatted correctly
                string personName = "";

                try
                {
                    personName = words[i + 1] + " " + words[i + 2];
                }

                catch
                {
                    ErrorMessage("Enter a valid name with this format: id John Doe.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(personName))
                {
                    ErrorMessage("Enter a valid name with this format: id John Doe.");
                    return;
                }

                //if person hasn't been seen before then show random string
                string id = dataCard.GetRandomString(8);

                foreach (var person in dataCard.people)
                {
                    if (personName == person.getName().ToLower())
                    {
                        id = person.databaseID;
                        break;
                    }
                }

                output += "\n" + id;
            }

            else if (words[i] == "<color=#00ffffff>ask</color>")
            {
                string question = "";

                try
                {
                    question = words[i + 1];
                }

                catch
                {
                    ErrorMessage("Enter a valid question with this format: ask age.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(question))
                {
                    ErrorMessage("Enter a valid question with this format: ask age.");
                    return;
                }

                if (question == "name")
                {
                    if (dataCard.person.isSpy && dataCard.person.Mistake == 1)
                    {
                        List<string> availableFirstNames = new List<string>();
                        List<string> availableLastNames = new List<string>();

                        foreach (var firstName in dataCard.firstNames)
                        {
                            //if the name starts with the same letter as the name on the card
                            if (firstName.StartsWith(dataCard.person.firstName[0].ToString()))
                            {
                                availableFirstNames.Add(firstName);
                            }
                        }

                        foreach (var lastNames in dataCard.lastNames)
                        {
                            //if the name starts with the same letter as the name on the card
                            if (lastNames.StartsWith(dataCard.person.lastName[0].ToString()))
                            {
                                availableLastNames.Add(lastNames);
                            }
                        }

                        string fakeName = "";
                        bool uniqueName = false;

                        while (uniqueName == false)
                        {
                            fakeName = availableFirstNames[UnityEngine.Random.Range(0, availableFirstNames.Count)] +
                            " " + availableLastNames[UnityEngine.Random.Range(0, availableLastNames.Count)];

                            uniqueName = true;

                            foreach (var person in dataCard.people)
                            {
                                if (fakeName == person.getName())
                                {
                                    uniqueName = false;
                                }
                            }
                        }

                        output += "\n" + fakeName;
                    }

                    else
                    {
                        output += "\n" + dataCard.person.getName();
                    }
                }   
                
                else if (question == "age")
                {
                    if (dataCard.person.isSpy && dataCard.person.Mistake == 2)
                    {
                        int fakeAge = 0;
                        bool uniqueAge = false;

                        //make sure it doesn't actually equal the age
                        while (uniqueAge == false)
                        {
                            //fakeAge is within 15 of the person's age and is clamped to realistic values
                            fakeAge = UnityEngine.Random.Range(dataCard.person.Age - 15, dataCard.person.Age + 16);
                            fakeAge = Mathf.Clamp(fakeAge, 15, 100);

                            uniqueAge = true;

                            if (fakeAge == dataCard.person.Age)
                            {
                                uniqueAge = false;
                            }
                        }

                        output += "\n" + fakeAge;
                    }

                    else
                    {
                        output += "\n" + dataCard.person.Age;
                    }
                }

                else
                {
                    ErrorMessage("That is not a valid question. The valid questions are name and age.");
                }
            }
        }

        consoleText.text += output;
    }

    public void ErrorMessage(string message)
    {
        consoleText.text += "\n" + message;
    }
}
