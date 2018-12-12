using System.Text.RegularExpressions;

public static class Utils
{
    public static readonly Regex LOWER_UPPER_TRANSITION = new Regex("[a-z]()[A-Z]", RegexOptions.Compiled);

    public static string ToFriendly(this string s, bool titleCase = false)
    {
        Match match = LOWER_UPPER_TRANSITION.Match(s);

        while (match.Captures.Count >= 1)
        {
            s = s.Substring(0, match.Captures[0].Index + 1) + " " + s.Substring(match.Captures[0].Index + 1);

            match = LOWER_UPPER_TRANSITION.Match(s);
        }

        s = s.ToLower();

        string[] sfrag = s.Split(' ');

        for (int i = 0; i < (!titleCase ? 1 : sfrag.Length); i++)
            sfrag[i] = sfrag[i].Substring(0, 1).ToUpper() + sfrag[i].Substring(1);

        return string.Join(" ", sfrag);
    }
}
