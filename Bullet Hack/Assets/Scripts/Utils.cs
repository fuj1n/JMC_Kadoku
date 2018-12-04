using System.Text.RegularExpressions;

public static class Utils
{
    public static readonly Regex LOWER_UPPER_TRANSITION = new Regex("[a-z]()[A-Z]", RegexOptions.Compiled);

    public static string ToFriendly(this string s)
    {
        Match match = LOWER_UPPER_TRANSITION.Match(s);

        while (match.Captures.Count >= 1)
        {
            s = s.Substring(0, match.Captures[0].Index + 1) + " " + s.Substring(match.Captures[0].Index + 1);

            match = LOWER_UPPER_TRANSITION.Match(s);
        }

        s = s.ToLower();



        return s;
    }
}
