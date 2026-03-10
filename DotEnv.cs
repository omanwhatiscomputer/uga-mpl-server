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
            string[] pairs = line.Split("__=__", StringSplitOptions.RemoveEmptyEntries);
            if (pairs.Length != 2) continue;
            Environment.SetEnvironmentVariable(pairs[0], pairs[1]);
        }
    }
}