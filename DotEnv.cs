namespace uga_mpl_server;

using System;
using System.IO;

public static class DotEnv
{
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath)) return;
        foreach (string line in File.ReadAllLines(filePath))
        {
            var delimiters = new[] { "__=__" };
            string[] pairs = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            if (pairs.Length != 2) continue;
            Environment.SetEnvironmentVariable(pairs[0], pairs[1]);
        }
    }
}