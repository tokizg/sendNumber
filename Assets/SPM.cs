public class SPM
{
    public static string convSpr(string str)
    {
        string ret = "";
        for (int i = 0; i <= str.Length - 1; i++)
        {
            switch (str[i])
            {
                case ' ':
                    ret += " ";
                    break;
                default:
                    ret += "<sprite=" + (System.Convert.ToInt32(str[i]) - 48) + ">";

                    break;
            }
        }
        return ret;
    }
}
