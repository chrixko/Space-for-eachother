public static class PilloConfig
{
    //GENERAL===================================================================
    public const bool USE_DEBUG = true;
    public const string UNKNOWN_PORTNAME = "NOT CONNECTED";

    //PARSING&CONNECT===========================================================
    public const int INPUT_STRING_LENGTH = 9;
    public const char PARSE_SPLITCHAR = ',';
    public const string PARSE_NEWLINE = "\n";
    public const int READ_TIMEOUT = 350;
    public const int BAUD_RATE = 9600;

    //CALCULATE=================================================================
    public const int CAP_TOP = 1200;
    public const int CAP_BOT = 300;
    public const int CAP_RANGE = CAP_TOP-CAP_BOT;
}