using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

// Define the Contact record
public record Contact(string Name, string Phone);

class Program
{
    static void Main()
    {
        // File used to save contacts
        string filePath = "contacts.json";

        // Create an empty contact list
        List<Contact> contacts = new List<Contact>();

        // LOAD CONTACTS WHEN PROGRAM STARTS
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            contacts = JsonSerializer.Deserialize<List<Contact>>(json)
                       ?? new List<Contact>();

            Console.WriteLine("Contacts loaded successfully!");
        }
        else
        {
            Console.WriteLine("No saved contacts found. Starting a new list.");
        }

        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== CONTACT MANAGER =====");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. List Contacts");
            Console.WriteLine("3. Search Contact");
            Console.WriteLine("4. Save Contacts");
            Console.WriteLine("5. Quit");

            Console.Write("Choose an option (1-5): ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // ADD CONTACT
                    Console.Write("Enter contact name: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("Enter phone number: ");
                    string phone = Console.ReadLine() ?? "";

                    if (!string.IsNullOrWhiteSpace(name) &&
                        !string.IsNullOrWhiteSpace(phone))
                    {
                        contacts.Add(new Contact(name, phone));

                        Console.WriteLine("Contact added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Name and phone cannot be empty!");
                    }
                    break;

                case "2":
                    // LIST CONTACTS
                    if (contacts.Count == 0)
                    {
                        Console.WriteLine("No contacts available.");
                    }
                    else
                    {
                        Console.WriteLine("\n===== YOUR CONTACTS =====");

                        int index = 1;

                        foreach (Contact contact in contacts)
                        {
                            Console.WriteLine(
                                $"{index}. {contact.Name} - {contact.Phone}");

                            index++;
                        }
                    }
                    break;

                case "3":
                    // SEARCH CONTACTS USING LINQ
                    Console.Write("Enter name to search: ");
                    string searchText = Console.ReadLine() ?? "";

                    var results = contacts
                        .Where(c => c.Name.Contains(
                            searchText,
                            StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (results.Count == 0)
                    {
                        Console.WriteLine("No matching contacts found.");
                    }
                    else
                    {
                        Console.WriteLine("\n===== SEARCH RESULTS =====");

                        foreach (Contact contact in results)
                        {
                            Console.WriteLine(
                                $"{contact.Name} - {contact.Phone}");
                        }
                    }
                    break;

                case "4":
                    // SAVE CONTACTS TO JSON
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };

                    string jsonOutput =
                        JsonSerializer.Serialize(contacts, options);

                    File.WriteAllText(filePath, jsonOutput);

                    Console.WriteLine("Contacts saved successfully!");
                    break;

                case "5":
                    // QUIT PROGRAM
                    Console.WriteLine("Goodbye!");
                    running = false;
                    break;

                default:
                    Console.WriteLine(
                        "Invalid option! Please choose 1-5.");
                    break;
            }
        }
    }
}