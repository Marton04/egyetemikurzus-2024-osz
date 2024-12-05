
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
        Console.Clear();
        Console.WriteLine("Teendő törlése");
        Console.WriteLine("===============");

        Console.Write("Add meg a törlendő teendő leírását: ");
        var descriptionToRemove = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(descriptionToRemove))
        {
            Console.WriteLine("A leírás nem lehet üres.");
            Console.WriteLine("Nyomj egy gombot a visszatéréshez...");
            Console.ReadKey();
            return;
        }

        var todoToRemove = todoList.FirstOrDefault(todo => todo.Description.Equals(descriptionToRemove, StringComparison.OrdinalIgnoreCase));

        if (todoToRemove != null)
        {
            todoList.Remove(todoToRemove);
            SaveTodoList();
            Console.WriteLine("A teendő sikeresen törölve!");
        }
        else
        {
            Console.WriteLine("Nem található ilyen leírással rendelkező teendő.");
        }

        Console.WriteLine("Nyomj egy gombot a visszatéréshez...");
        Console.ReadKey();
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
        Console.Clear();
        Console.WriteLine("Teendők listázása");
        Console.WriteLine("==================");

        var filteredTodos = GetFilteredTodos();
        var sortedTodos = SortTodos(filteredTodos);
        DisplayTodos(sortedTodos, "Szűrt és rendezett teendők listája");
    }

    static List<TodoItem> GetFilteredTodos()
    {
        Console.WriteLine("Mit szeretnél megjeleníteni?");
        Console.WriteLine("1 - Az összes teendőt");
        Console.WriteLine("2 - Csak egy adott kategória teendőit");
        Console.Write("Választás: ");

        var selection = Console.ReadLine();

        if (selection == "1")
        {
            return todoList;
        }
        else if (selection == "2")
        {
            return GetFilteredTodosByCategory();
        }
        else
        {
            Console.WriteLine("Érvénytelen választás. Nyomj egy gombot a visszatéréshez.");
            Console.ReadKey();
            return new List<TodoItem>();
        }
    }

    private static List<TodoItem> GetFilteredTodosByCategory()
    {
        Console.WriteLine("Válaszd ki a kategóriát:");
        var categoriesList = categories.ToList();
        for (int i = 0; i < categoriesList.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {categoriesList[i]}");
        }

        Console.Write("Választás: ");
        if (!int.TryParse(Console.ReadLine(), out var categoryChoice) || categoryChoice <= 0 || categoryChoice > categoriesList.Count)
        {
            Console.WriteLine("Érvénytelen választás.");
            Console.WriteLine("Nyomj egy gombot a folytatáshoz...");
            Console.ReadKey();
            return GetFilteredTodosByCategory();
        }
        else
        {
            return todoList
            .Where(todo => todo.Category.Equals(categoriesList[categoryChoice-1], StringComparison.OrdinalIgnoreCase))
            .ToList();
        }
    }

    static List<TodoItem> SortTodos(List<TodoItem> todos)
    {
        Console.WriteLine("Hogyan szeretnéd rendezni a teendőket?");
        Console.WriteLine("1 - Határidő szerint");
        Console.WriteLine("2 - Prioritás szerint");
        Console.WriteLine("3 - Alapértelmezett sorrendben");
        Console.Write("Választás: ");

        var sortOption = Console.ReadLine();

        return sortOption switch
        {
            "1" => todos.OrderBy(todo => todo.Deadline).ToList(),
            "2" => todos.OrderByDescending(todo => todo.Priority).ToList(),
            "3" => todos, 
            _ => todos
        };
    }

    static void DisplayTodos(List<TodoItem> todos, string title)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine("====================");

        if (todos.Count == 0)
        {
            Console.WriteLine("Nincsenek megjeleníthető teendők.");
        }
        else
        {
            foreach (var todo in todos)
            {
                Console.WriteLine($"- Leírás: {todo.Description}");
                Console.WriteLine($"  Határidő: {todo.Deadline:yyyy-MM-dd}");
                Console.WriteLine($"  Prioritás: {todo.Priority}");
                Console.WriteLine($"  Kategória: {todo.Category}");
                Console.WriteLine();
            }
        }

        Console.WriteLine("Nyomj egy gombot a visszatéréshez...");
        Console.ReadKey();
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
            return GetDescription();
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
            return GetPriority();
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
            return GetCategory();
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
            return AddNewCategory();
        }

        if (!categories.Add(category))
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