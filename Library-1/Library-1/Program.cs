using System;
using System.Collections.Generic;
using System.IO;

namespace BibliotekHantering
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }
        public string Password { get; set; }
    }

    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Subject { get; set; }
        public string ISBN { get; set; }
        public bool IsBorrowed { get; set; }
    }

    public class LibrarySystem
    {
        private List<User> users;
        private List<Book> books;
        private User currentUser;
        private string booksFilePath = "C:\\Users\\sajeel.ahmad\\source\\repos\\Library-1\\Library-1\\books.txt";

        public LibrarySystem()
        {
            users = new List<User>();
            books = new List<Book>();
            LoadUserData();
            LoadBooksData();

        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Välkommen till bibliotekssystemet!");
                Console.WriteLine("1. Logga in");
                Console.WriteLine("2. Skapa konto");
                Console.WriteLine("3. Avsluta");
                Console.Write("Välj en handling: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        CreateAccount();
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void Login()
        {
            Console.Write("Ange personnummer: ");
            string personalNumber = Console.ReadLine();
            Console.Write("Ange lösenord: ");
            string password = Console.ReadLine();

            currentUser = users.Find(user => user.PersonalNumber == personalNumber && user.Password == password);

            if (currentUser != null)
            {
                Console.WriteLine($"Inloggad som {currentUser.FirstName} {currentUser.LastName}");
                if (IsLibrarian(currentUser))
                {
                    
                    RunLibrarianActions();
                }
                else
                {
                   
                    RunMemberActions();
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt personnummer eller lösenord. Försök igen.");
            }
        }

        private void CreateAccount()
        {
            Console.Write("Ange förnamn: ");
            string firstName = Console.ReadLine();
            Console.Write("Ange efternamn: ");
            string lastName = Console.ReadLine();
            Console.Write("Ange personnummer: ");
            string personalNumber = Console.ReadLine();
            Console.Write("Ange lösenord: ");
            string password = Console.ReadLine();

            User newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                PersonalNumber = personalNumber,
                Password = password
            };

            users.Add(newUser);
            SaveUserData(); // Spara användarinformation till textfil

            Console.WriteLine("Kontot har skapats.");
        }

        private void RunLibrarianActions()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Välkommen!");
                Console.WriteLine("1. Sök efter böcker");
                Console.WriteLine("2. Ändra lösenord");
                Console.WriteLine("3. Låna ut bok");
                Console.WriteLine("4. Återlämna bok");
                Console.WriteLine("5. Logga ut");
                Console.Write("Välj en handling: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        SearchBooks();
                        break;
                    case "2":
                        ChangePassword();
                        break;
                    case "3":
                        BorrowBook();
                        break;
                    case "4":
                        ReturnBook();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void RunMemberActions()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine($"Välkommen, {currentUser.FirstName} {currentUser.LastName}!");
                Console.WriteLine("1. Sök efter böcker");
                Console.WriteLine("2. Ändra lösenord");
                Console.WriteLine("3. Låna bok");
                Console.WriteLine("4. Återlämna bok");
                Console.WriteLine("5. Logga ut");
                Console.Write("Välj en handling: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        SearchBooks();
                        break;
                    case "2":
                        ChangePassword();
                        break;
                    case "3":
                        BorrowBook();
                        break;
                    case "4":
                        ReturnBook();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void SearchBooks()
        {
            Console.WriteLine("Ange sökterm: ");
            string searchQuery = Console.ReadLine();

            List<Book> matchingBooks = books.FindAll(book =>
                book.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                book.Author.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                book.Subject.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                book.ISBN.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));

            if (matchingBooks.Count > 0)
            {
                Console.WriteLine("Hittade böcker:");
                foreach (var book in matchingBooks)
                {
                    Console.WriteLine($"Titel: {book.Title}");
                    Console.WriteLine($"Författare: {book.Author}");
                    Console.WriteLine($"Ämne: {book.Subject}");
                    Console.WriteLine($"ISBN: {book.ISBN}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Inga matchande böcker hittades.");
            }
        }

        private void ChangePassword()
        {
            Console.Write("Ange nytt lösenord: ");
            string newPassword = Console.ReadLine();
            currentUser.Password = newPassword;
            SaveUserData(); // Spara ändringen till textfil
            Console.WriteLine("Lösenordet har ändrats.");
        }

        private void BorrowBook()
        {
            Console.Write("Ange ISBN på boken att låna: ");
            string isbn = Console.ReadLine();

            Book book = books.Find(b => b.ISBN == isbn);

            if (book != null)
            {
                if (book.IsBorrowed)
                {
                    Console.WriteLine("Boken är redan utlånad.");
                }
                else
                {
                    book.IsBorrowed = true;
                    Console.WriteLine("Boken har lånats ut");
                }
            }
            else
            {
                Console.WriteLine("Boken hittades inte.");
            }
        }

        private void ReturnBook()
        {
            Console.Write("Ange ISBN på boken att lämna tillbaka: ");
            string isbn = Console.ReadLine();

            Book book = books.Find(b => b.ISBN == isbn);

            if (book != null)
            {
                if (book.IsBorrowed)
                {
                    book.IsBorrowed = false;
                    Console.WriteLine("Boken har lämnats tillbaka.");
                }
                else
                {
                    Console.WriteLine("Boken är inte utlånad.");
                }
            }
            else
            {
                Console.WriteLine("Boken hittades inte.");
            }
        }

        private void LoadUserData()
        {
            try
            {
                string[] lines = File.ReadAllLines("C:\\Users\\sajeel.ahmad\\source\\repos\\Library-1\\Library-1\\users.txt");

                foreach (string line in lines)
                {
                    string[] userData = line.Split(';');
                    User user = new User
                    {
                        FirstName = userData[0],
                        LastName = userData[1],
                        PersonalNumber = userData[2],
                        Password = userData[3]
                    };

                    users.Add(user);
                }
            }
            catch (FileNotFoundException)
            {
                // Hantera om filen inte hittas
            }
        }



        private void SaveUserData()
        {
            List<string> lines = new List<string>();

            foreach (User user in users)
            {
                string line = $"{user.FirstName};{user.LastName};{user.PersonalNumber};{user.Password}";
                lines.Add(line);
            }

            File.WriteAllLines("C:\\Users\\sajeel.ahmad\\source\\repos\\Library-1\\Library-1\\users.txt", lines);
        }

        private bool IsLibrarian(User user)
        {
 
            return user.PersonalNumber.StartsWith("123");
        }

        private void LoadBooksData()
        {
            try
            {
                string[] lines = File.ReadAllLines(booksFilePath);

                foreach (string line in lines)
                {
                    string[] bookData = line.Split(';');
                    Book book = new Book
                    {
                        Title = bookData[0],
                        Author = bookData[1],
                        Subject = bookData[2],
                        ISBN = bookData[3],
                        IsBorrowed = bool.Parse(bookData[4])
                    };

                    books.Add(book);
                }
            }
            catch (FileNotFoundException)
            {
                // Hantera om filen inte hittas
            }
        }
        private void SaveBooksData()
        {
            List<string> lines = new List<string>();

            foreach (Book book in books)
            {
                string line = $"{book.Title};{book.Author};{book.Subject};{book.ISBN};{book.IsBorrowed}";
                lines.Add(line);
            }

            File.WriteAllLines(booksFilePath, lines);
        }
      
    }

    class Program
    {
        static void Main(string[] args)
        {
            LibrarySystem librarySystem = new LibrarySystem();
            librarySystem.Run();
        }
    }


}
