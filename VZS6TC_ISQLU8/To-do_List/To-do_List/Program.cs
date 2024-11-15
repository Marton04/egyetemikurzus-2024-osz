
using System.Text.Json;

using To_do_List;

class Program
{
    static List<TodoItem> todoList=new ();
    const string FilePath = "todo.json";

    static void Main(string[] args)
    {
        LoadTodoList();
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
        throw new NotImplementedException();
    }
}