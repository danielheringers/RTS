public class ItemIDGenerator
{
    private static int currentID = 0;

    public static int GenerateUniqueID()
    {
        return currentID++;
    }
}