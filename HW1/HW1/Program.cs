using System.Diagnostics;
using System.Text.RegularExpressions;


string path = "./assets/";
System.Console.WriteLine("First time instructions: \nCreate a folder called \"assets\" inside this program\'s directory" + 
"\nThe path file should look like this \'HW1\'/\'HW1\'/\'assets\'" + 
"\nAfter that place the file you want to analyse. It should be a txt file.");
string book = string.Empty;
bool flag = false;
StreamReader streamReader;
do{
    try{
    System.Console.Write("Enter the name of the file:");
    string name = Console.ReadLine();
    name = name.EndsWith(".txt") ? name : name + ".txt";
    streamReader = new StreamReader(path + name);
    book = streamReader.ReadToEnd().ToLower();
    streamReader.Close();
    flag = true;
    }
    catch (Exception c){
        System.Console.WriteLine(c);
    }
}while(flag == false);


string pattern = @"[а-яА-Я]{3,}";
Regex rex = new Regex(pattern, RegexOptions.Compiled);
MatchCollection matches_book = rex.Matches(book);
string[] words = new string[matches_book.Count];
for(int i = 0; i< matches_book.Count; i++) words[i] = matches_book[i].Value;

Stopwatch stopwatch = new Stopwatch();

stopwatch.Start();
Results.numOfWords = NumberOfWords(words);
Results.shortest_words = ShortestWords(words);
Results.longest_words = LongestWords(words);
Results.average_word_length = AverageWordLength(words);
Dictionary<string, int> dict = ClassifyWords(words);
Results.most_common_words = Find5MostCommonWords(dict);
Results.least_common_words = Find5LeastCommonWords(dict);
stopwatch.Stop();

/*stopwatch.Start();
Thread[] threads = {new Thread(ThreadMethod1),
                    new Thread(ThreadMethod2),
                    new Thread(ThreadMethod3),
                    new Thread(ThreadMethod4),
                    new Thread(ThreadMethod5),
                    new Thread(ThreadMethod6)};
for (int i = 0;  i< 4; i++) threads[i].Start(words);
Dictionary<string, int> dict = ClassifyWords(words);
threads[4].Start(dict);
threads[5].Start(dict);
foreach (Thread thread in threads) thread.Join();
stopwatch.Stop();*/

System.Console.WriteLine(stopwatch.ElapsedMilliseconds);
Console.ReadLine();

/*System.Console.WriteLine($"Number of words: {Results.numOfWords}");
System.Console.WriteLine($"Shortest words: {Results.shortest_words}");
System.Console.WriteLine($"Longest words: {Results.longest_words}");
System.Console.WriteLine($"Average word length: {Results.average_word_length}");
System.Console.WriteLine($"5 most common words: {Results.most_common_words}");
System.Console.WriteLine($"5 least common words: {Results.least_common_words}");*/



int NumberOfWords(string[] words)
{
    return words.Length;
}

double AverageWordLength(string[] words)
{
    int length = 0;
    foreach (string s in words) length += s.Length;
    return length / NumberOfWords(words);
}

string LongestWords(string[] words)
{
    List<string> longest_words = new List<string>();
    int max_letters = 3;
    foreach (string s in words)
    {
        if (s.Length == max_letters && !longest_words.Contains(s)) longest_words.Add(s);
        else if (s.Length > max_letters)
        {
            max_letters = s.Length;
            longest_words.Clear();
            longest_words.Add(s);
        }
    }
    return string.Join(", ", longest_words);
}
string ShortestWords(string[] words)
{
    List<string> shortest_words = new List<string>();
    int min_letters = 10_000;
    foreach (string s in words)
    {
        if (s.Length == min_letters && !shortest_words.Contains(s)) shortest_words.Add(s);
        else if (s.Length < min_letters)
        {
            min_letters = s.Length;
            shortest_words.Clear();
            shortest_words.Add(s);
        }
    }
    return string.Join(", ", shortest_words);
}

Dictionary<string, int> ClassifyWords(string[] words)
{
    Dictionary<string, int> dict = new Dictionary<string, int>();
    foreach (string s in words)
    {
        if (dict.ContainsKey(s)) dict[s] += 1;
        else dict.Add(s, 1);
    }
    return dict;
}

string Find5MostCommonWords(Dictionary<string, int> dict)
{
    int max_count = 0;
    string temp_word = string.Empty;
    List<string> common_words = new List<string>();
    List<int> common_words_count = new List<int>();
    for (int i = 0; i < 5; i++)
    {
        foreach (KeyValuePair<string, int> kv in dict)
        {
            if (max_count < kv.Value && !common_words.Contains(kv.Key))
            {
                max_count = kv.Value;
                temp_word = kv.Key;
            }
        }
        common_words.Add(temp_word);
        common_words_count.Add(max_count);
        max_count = 0;
        temp_word = string.Empty;
    }
    return $"{string.Join(", ", common_words)} ---> {string.Join(", ", common_words_count)}";
}

string Find5LeastCommonWords(Dictionary<string, int> dict)
{
    int min_count = 500;
    string temp_word = string.Empty;
    List<string> uncommon_words = new List<string>();
    List<int> uncommon_words_count = new List<int>();
    for (int i = 0; i < 5; i++)
    {
        foreach (KeyValuePair<string, int> kv in dict)
        {
            if (min_count > kv.Value && !uncommon_words.Contains(kv.Key))
            {
                min_count = kv.Value;
                temp_word = kv.Key;
            }
        }
        uncommon_words.Add(temp_word);
        uncommon_words_count.Add(min_count);
        min_count = 500;
        temp_word = string.Empty;
    }
    return $"{string.Join(", ", uncommon_words)} ---> {string.Join(", ", uncommon_words_count)}";
}

void ThreadMethod1(object p)
{
    Results.numOfWords = NumberOfWords((string[] )p);
    //System.Console.WriteLine("Thread 1 complete");
}

void ThreadMethod2(object p)
{
    Results.shortest_words = ShortestWords((string[] )p);
    //System.Console.WriteLine("Thread 2 complete");
}

void ThreadMethod3(object p)
{
    Results.longest_words = LongestWords((string[] )p);
    //System.Console.WriteLine("Thread 3 complete");
}

void ThreadMethod4(object p)
{
    Results.average_word_length = AverageWordLength((string[] )p);
    //System.Console.WriteLine("Thread 4 complete");
}

/* void ThreadMethod5_6(object p)
{
    Dictionary<string, int> dict = ClassifyWords((string[] )p);
    Results.least_common_words = Find5LeastCommonWords(dict);
    Results.most_common_words = Find5MostCommonWords(dict);
    //System.Console.WriteLine("Thread 5 complete.");
}*/
void ThreadMethod5(object p)
{
    Results.least_common_words = Find5LeastCommonWords((Dictionary<string,int>) p);
    //System.Console.WriteLine("Thread 5 complete.");
}
void ThreadMethod6(object p)
{
    Results.most_common_words = Find5MostCommonWords((Dictionary<string, int>)p);
    //System.Console.WriteLine("Thread 6 complete.");
}

static class Results
{
    static public int numOfWords;
    static public string shortest_words;
    static public string longest_words;
    static public double average_word_length;
    static public string most_common_words;
    static public string least_common_words;
}