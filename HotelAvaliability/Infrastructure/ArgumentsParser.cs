public static class ArgumentsParser 
{
    static public Dictionary<string, string> Parse(string[] args) 
    {
        Dictionary<string, string> argsDict = new Dictionary<string, string>();

        string currentTag = "";
        for (int i = 0; i < args.Length; i++) 
        {
            if (args[i].StartsWith("--")) 
            {
                currentTag = string.Join("", args[i].Skip(2).ToArray());
            } else if (currentTag != "") 
            {
                argsDict[currentTag] = args[i];
                currentTag = "";
            }
        }

        return argsDict;
    }
}