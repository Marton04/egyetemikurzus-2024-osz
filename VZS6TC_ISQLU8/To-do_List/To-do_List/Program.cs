
using System.Text.Json;

using To_do_List;

class Program
{
    static List<TodoItem> todoList=new ();
    const string FilePath = "todo.json";
    static HashSet<string> categories = new();
    const string CategoriesFilePath = "categories.json";

    static void Main(string[] args)
    {
        LoadTodoList();
        LoadCategories();
        var commands = new Dictionary<string, Action>
        {
            { "1", AddTodo },
            { "2", ListTodos },
            { "3", RemoveTodo },
            { "4", ExitProgram }
        };
        while (true)
        {
            Console.Clear();
            Console.WriteLine("To-Do Lista Kezelő");
            Console.WriteLine("==================");
            Console.WriteLine("Parancsok:");
            Console.WriteLine("1 - Teendő hozzáadása");
            Console.WriteLine("2 - Teendők listázása");
            Console.WriteLine("3 - Teendő törlése");
            Console.WriteLine("4 - Kilépés");
            Console.Write("Válassz egy lehetőséget: ");

            var input = Console.ReadLine();

            if (commands.TryGetValue(input ?? string.Empty, out var action))
            {
                action.Invoke();
            }
            else
            {
                Console.WriteLine("Ismeretlen parancs. Próbáld újra!");
                Console.WriteLine("Nyomj egy gombot a folytatáshoz...");
                Console.ReadKey();
            }
        }
    }

    static void LoadTodoList()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                todoList = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Hozzáférési hiba: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Hiba a JSON feldolgozása során: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Bemeneti/kimeneti hiba: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ismeretlen hiba: {ex.Message}");
        }
    }
    static void RemoveTodo()
    {
        throw new NotImplementedException();
    }

    static void ExitProgram()
    {
        SaveTodoList();
        Console.WriteLine("Kilépés...");
        Environment.Exit(0);
    }

    private static void SaveTodoList()
    {
        try
        {
            var json = JsonSerializer.Serialize(todoList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Hozzáférési hiba: {ex.Message}");
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine($"A megadott könyvtár nem található: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Input/Output hiba: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON formázási hiba: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba a lista mentésekor: {ex.Message}");
        }
    }

    static void ListTodos()
    {
        throw new NotImplementedException();
    }

    static void AddTodo()
    {
        Console.Clear();
        Console.WriteLine("Új teendő hozzáadása");
        Console.WriteLine("====================");

        var description = GetDescription();
        var deadline = GetDeadline();
        var priority = GetPriority();
        var category = GetCategory();

        var newTodo = new TodoItem(description, deadline, priority, category);
        todoList.Add(newTodo);
        SaveTodoList();

        Console.WriteLine("Teendő sikeresen hozzáadva!");
        Console.WriteLine("Nyomj egy gombot a folytatáshoz...");
        Console.ReadKey();
    }

    static string GetDescription()
    {
        Console.Write("Leírás: ");
        var description = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("A leírás nem lehet üres.");
            Console.WriteLine("Nyomj egy gombot a folytatáshoz...");
            Console.ReadKey();
            return GetDescription(); // Újra bekérés
        }
        return description;
    }

    static DateTime GetDeadline()
    {
        Console.Write("Határidő (ÉÉÉÉ-HH-NN): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var deadline))
        {
            Console.WriteLine("Érvénytelen dátum formátum.");
            Console.WriteLine("Nyomj egy gombot a folytatáshoz...");
            Console.ReadKey();
            return GetDeadline(); // Újra bekérés
        }
        return deadline;
    }

    static int GetPriority()
    {
        Console.Write("Prioritás (1-5): ");
        if (!int.TryParse(Console.ReadLine(), out var priority) || priority < 1 || priority > 5)
        {
            Console.WriteLine("A prioritásnak 1 és 5 közötti számnak kell lennie.");
            Console.WriteLine("Nyomj egy gombot a folytatáshoz...");
            Console.ReadKey();
            return GetPriority(); // Újra bekérés
        }
        return priority;
    }

    static string GetCategory()
    {
        Console.WriteLine("Válaszd ki a kategóriát:");
        var categoriesList = categories.ToList();
        for (int i = 0; i < categoriesList.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {categoriesList[i]}");
        }
        Console.WriteLine("0 - Új kategória hozzáadása");

        Console.Write("Választás: ");
        if (!int.TryParse(Console.ReadLine(), out var categoryChoice) || categoryChoice < 0 || categoryChoice > categoriesList.Count)
        {
            Console.WriteLine("Érvénytelen választás.");
            Console.WriteLine("Nyomj egy gombot a folytatáshoz...");
            Console.ReadKey();
            return GetCategory(); // Újra bekérés
        }

        if (categoryChoice == 0)
        {
            return AddNewCategory();
        }
        return categoriesList[categoryChoice - 1];
    }

    static string AddNewCategory()
    {
        Console.Write("Add meg az új kategória nevét: ");
        var category = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(category))
        {
            Console.WriteLine("A kategória neve nem lehet üres.");
            Console.WriteLine("Nyomj egy gombot a folytatáshoz...");
            Console.ReadKey();
            return AddNewCategory(); // Újra bekérés
        }

        if (!categories.Add(category)) // Ha már létezik, nem kerül újra hozzáadásra
        {
            Console.WriteLine("Ez a kategória már létezik.");
        }
        SaveCategories();
        return category;
    }



    static void LoadCategories()
    {
        try
        {
            if (File.Exists(CategoriesFilePath))
            {
                var json = File.ReadAllText(CategoriesFilePath);
                categories = JsonSerializer.Deserialize<HashSet<string>>(json) ?? new HashSet<string>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba a kategóriák betöltésekor: {ex.Message}");
        }
    }

    static void SaveCategories()
    {
        try
        {
            var json = JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(CategoriesFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba a kategóriák mentésekor: {ex.Message}");
        }
    }


}