
using To_do_List;

class Program
{
    static List<TodoItem> todoList=new ();
    const string FilePath = "";

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
        throw new NotImplementedException();
    }
    static void RemoveTodo()
    {
        throw new NotImplementedException();
    }

    static void ExitProgram()
    {
        throw new NotImplementedException();
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