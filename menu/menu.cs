using BACKEND;

delegate bool tryparser<t>(string text, out t value);

class menu
{
    static void Main()
    {
        Console.WriteLine("Seleccione el tipo de dato de la lista:");
        Console.WriteLine("1.Entero");
        Console.WriteLine("2.Decimal");
        Console.WriteLine("3.Texto");
        Console.WriteLine("4.Caracter");
        Console.WriteLine("5.Salir");
        Console.Write("Opción: ");
        string choice = Console.ReadLine() ?? "";

        switch (choice.Trim())
        {
            case "1":
                runlist<int>(int.TryParse);
                break;
            case "2":
                runlist<double>(double.TryParse);
                break;
            case "3":
                runlist<string>((string text, out string value) =>
                {
                    value = text.Trim();
                    return value.Length > 0;
                });
                break;
            case "4":
                runlist<char>(char.TryParse);
                break;
            case "5":
                Console.WriteLine("Hasta luego.");
                return;
            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }

    static void runlist<t>(tryparser<t> parse) where t : IComparable<t>
    {
        var list = new doublyLinkedList<t>();
        int option;
        do
        {
            showmenu();
            if (!int.TryParse(Console.ReadLine(), out option)) option = -1;
            Console.WriteLine();

            switch (option)
            {
                case 1:
                    if (read("Elemento a adicionar: ", parse, out var toadd))
                    {
                        list.add(toadd);
                        Console.WriteLine("Elemento adicionado.");
                    }
                    break;
                case 2:
                    if (list.count == 0) Console.WriteLine("La lista está vacía.");
                    else Console.WriteLine("Hacia adelante: " + string.Join(", ", list.forward()));
                    break;
                case 3:
                    if (list.count == 0) Console.WriteLine("La lista está vacía.");
                    else Console.WriteLine("Hacia atrás: " + string.Join(", ", list.backward()));
                    break;
                case 4:
                    list.sortdescending();
                    Console.WriteLine("Lista ordenada descendentemente.");
                    break;
                case 5:
                    if (list.count == 0)
                    {
                        Console.WriteLine("La lista está vacía.");
                        break;
                    }
                    var occ = list.countduplicates();
                    int max = occ.Max(o => o.count);
                    var modes = occ.Where(o => o.count == max).Select(o => o.value).ToList();
                    if (modes.Count == 1)
                        Console.WriteLine($"La moda es {modes[0]}.");
                    else
                        Console.WriteLine("Las modas son " + string.Join(", ", modes.Take(modes.Count - 1)) + " y " + modes[^1] + ".");
                    break;
                case 6:
                    if (list.count == 0) Console.WriteLine("La lista está vacía.");
                    else
                        foreach (var item in list.countduplicates())
                            Console.WriteLine(value: $"{item.value} {new string('*', item.count)}");
                    break;
                case 7:
                    if (read("Elemento a buscar: ", parse, out var lookfor))
                        Console.WriteLine(list.exists(lookfor) ? $"Sí existe el {lookfor}." : $"No existe el {lookfor}.");
                    break;
                case 8:
                    if (read("Elemento a eliminar: ", parse, out var toremove))
                        Console.WriteLine(list.removefirst(toremove) ? $"Se eliminó una ocurrencia del {toremove}." : $"No existe el {toremove}.");
                    break;
                case 9:
                    if (read("Elemento a eliminar (todas): ", parse, out var toremoveall))
                    {
                        int removed = list.removeall(toremoveall);
                        Console.WriteLine(removed > 0 ? $"Se eliminaron {removed} ocurrencia(s) del {toremoveall}." : $"No existe el {toremoveall}.");
                    }
                    break;
                case 0:
                    Console.WriteLine("Hasta luego.");
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }
            Console.WriteLine();
        } while (option != 0);
    }

    static void showmenu()
    {
        Console.WriteLine("===== MENÚ =====");
        Console.WriteLine("1.Adicionar");
        Console.WriteLine("2.Mostrar hacia adelante");
        Console.WriteLine("3.Mostrar hacia atrás");
        Console.WriteLine("4.Ordenar descendentemente");
        Console.WriteLine("5.Mostrar la(s) moda(s)");
        Console.WriteLine("6.Mostrar gráfico");
        Console.WriteLine("7.Existe");
        Console.WriteLine("8.Eliminar una ocurrencia");
        Console.WriteLine("9.Eliminar todas las ocurrencias");
        Console.WriteLine("0.Salir");
        Console.Write("Opción: ");
    }

    static bool read<t>(string prompt, tryparser<t> parse, out t value)   // 1, 7, 8 y 9
    {
        Console.Write(prompt);
        if (parse(Console.ReadLine() ?? "", out value))
        {
            return true;
        }
        else
        {
            Console.WriteLine("Valor inválido para este tipo de lista.");
            return false;
        }
    }
}