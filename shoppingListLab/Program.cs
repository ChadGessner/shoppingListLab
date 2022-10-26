
using shoppingListLab;
using System.Linq;

List<string> shoppingList = new List<string>();
Dictionary<string, double> menu = GetMenu();


GetTotal();

void GetTotal()
{
    bool isValid = true;
    string item;
    Console.WriteLine
        (
        "Welcome to the item store where we sell items! \n" +
        "Please select from the following item menu full of items! \n" +
        "or press <c> to checkout..."
        ); ;
    DisplayMenu();
    while (true)
    {
        item = ValidateInputString(Console.ReadLine()); 
        if(item == "invalid")
        {
            Console.WriteLine("That was not a valid selection press <c> to checkout or a valid selection... \n");
            continue;
        }
        if(item == "c")
        {       
            Console.Clear();
            
            break;
        }
        Console.Clear();
        Console.WriteLine($"mmm {FormatMenuKeys(-1, item)} excellent choice!");
        shoppingList.Add(item);
        DisplayMenu(GetSum(shoppingList));
        
    }
    try
    {
        Console.WriteLine(ItemizedRecipt()); 
    }
    catch(IndexOutOfRangeException)
    {
        Console.WriteLine("No sale, have a great day!");
    }
}

Dictionary<string,int> GetShoppingListItemsQuantity()
{
    Dictionary<string, int> output = new Dictionary<string, int>();
    foreach (string item in shoppingList)
    {
        if (!output.ContainsKey(item))
        {
            output.Add(item, 1);
            continue;
        }
        output[item]++;
    }
    return output;
}

string ItemizedRecipt()
{
    Console.Clear();
    Dictionary<string, int> ShoppingListItemQuantity = GetShoppingListItemsQuantity();
    int magicNumber = shoppingList.OrderByDescending(s => s.Length).ToArray()[0].Length + 19; //14
    string total = $"{shoppingList.Select(item => menu[item]).Sum():c}";
    string stars = new String('*', (magicNumber / 2) - 3); // -3
    string recipt = $"{stars}RECIPT{stars}\n";
    foreach(KeyValuePair<string,int> item in ShoppingListItemQuantity)
    {
        bool isOnlyOne = item.Value == 1;
        string currentItem = isOnlyOne ? FormatMenuKeys(-1, item.Key) : $"{FormatMenuKeys(-1, item.Key)} x {item.Value} ";
        string priceToString = isOnlyOne ? $"{menu[item.Key]:c}" : $"{(menu[item.Key] * item.Value):c}";
        int spaceMod = currentItem.Length + priceToString.Length; // problem
        recipt += $"{currentItem}{new string(' ', Math.Abs(magicNumber - spaceMod))}{priceToString}\n";
    }
    
    recipt += $"\nTOTAL:{new string(' ', magicNumber - (6 + total.Length))}{total}";
    recipt += "\n" + new string('*', magicNumber) + "\n";
    recipt += $"Your cheapest item was {FormatMenuKeys(-1, GetMin(shoppingList))} at the low low price of {menu[GetMin(shoppingList)]:c}... \n";
    recipt += $"Your most expensive item was {FormatMenuKeys(-1, GetMax(shoppingList))} at the outrageous price of {menu[GetMax(shoppingList)]:c}... \n";
    recipt += $"Your total is {GetSum(shoppingList):c} thank you for shopping with us today!";
    return recipt;
}

double GetSum(List<string> shoppingList)
{
    return shoppingList.Select(item => menu[item]).Sum();
}
string GetMin(List<string> shoppingList)
{
    return shoppingList.OrderBy(item => menu[item]).ToArray()[0];
}
string GetMax(List<string> shoppingList)
{
    return shoppingList.OrderByDescending(item => menu[item]).ToArray()[0];
}


Dictionary<string, double> GetMenu()
{
    Dictionary<string, double> menu = new Dictionary<string, double>();
    for (int i = 0; i < 8; i++)
    {
        menu[$"{(Item)i}"] = (i + .09) * 2;
    }
    return menu;
}

void DisplayMenu(double total = 0.00)
{
    Console.WriteLine("");
    Console.WriteLine($"   ***MENU******TOTAL*** \n{new string(' ', 16)}{total:c}");
    Console.WriteLine($"    ");
    for (int i = 0; i < 8; i++)
    {
        Console.WriteLine($"{i + 1}) {FormatMenuKeys(i)} {menu[$"{(Item)i}"]:c}");
    }
}

string FormatMenuKeys(int e = -1, string f = "") // looking for a better way to deal with this, help?
{
    string item = e != -1 ? $"{(Item)e}" : f;
    string output = string.Empty;
    for(int i = 0; i < item.Length; i++)
    {
        if (i != 0 && item[i].ToString() == item[i].ToString().ToUpper())
        {
            output += " " + item[i];
            continue;
        }
        output += item[i];
    }
    return output.Trim();
}

string ValidateInputString(string input)
{
    bool isInt = true;
    try
    {
        int number = int.Parse(input);
        if(number > -1 && number < 9)
        {
            return $"{(Item)number - 1}";
        }
        
    }
    catch
    {
        isInt = false;
    }
    try
    {
        input = input.Split().Length > 1 ? String.Join("", input.Split()) : input;
        foreach(KeyValuePair<string,double> kv in menu)
        {
            if(input.ToLower() == kv.Key.ToLower())
            {
                return kv.Key;
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        return "henlo frend";
    }
    if (input.ToLower() == "c")
    {
        return "c";
    }
    return "invalid";
}