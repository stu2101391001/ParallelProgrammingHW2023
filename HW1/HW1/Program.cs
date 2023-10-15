using System.Diagnostics;
using System.Text.RegularExpressions;

StreamReader streamReader = new StreamReader("long_book.txt");
string book = streamReader.ReadToEnd().ToLower();
streamReader.Close();
string pattern = @"[а-яА-Я]{3,}";
Regex rex = new Regex(pattern, RegexOptions.Compiled);
MatchCollection matches_book = rex.Matches(book);

Stopwatch stopwatch = new Stopwatch();

/*stopwatch.Start();
Results.numOfWords = NumberOfWords(matches_book);
Results.shortest_words = ShortestWords(matches_book);
Results.longest_words = LongestWords(matches_book);
Results.average_word_length = AverageWordLength(matches_book);
Dictionary<string, int> dict = ClassifyWords(matches_book);
Results.most_common_words = Find5MostCommonWords(dict);
Results.least_common_words = Find5LeastCommonWords(dict);
stopwatch.Stop();*/

stopwatch.Start();
Thread[] threads = {new Thread(ThreadMethod1), 
                    new Thread(ThreadMethod2), 
                    new Thread(ThreadMethod3),
                    new Thread(ThreadMethod4),
                    new Thread(ThreadMethod5_6)};
foreach(Thread thread in threads) thread.Start(matches_book);
foreach(Thread thread in threads) thread.Join();
stopwatch.Stop();

System.Console.WriteLine(stopwatch.ElapsedMilliseconds);

/*System.Console.WriteLine($"Number of words: {Results.numOfWords}");
System.Console.WriteLine($"Shortest words: {Results.shortest_words}");
System.Console.WriteLine($"Longest words: {Results.longest_words}");
System.Console.WriteLine($"Average word length: {Results.average_word_length}");
System.Console.WriteLine($"5 most common words: {Results.most_common_words}");
System.Console.WriteLine($"5 least common words: {Results.least_common_words}");*/



int NumberOfWords(MatchCollection book_matches){
    return book_matches.Count;
}

double AverageWordLength(MatchCollection book_matches){
    int length = 0;
    foreach(Match match in book_matches) length += match.Value.Length;
    return length/ NumberOfWords(book_matches);
}

string LongestWords(MatchCollection book_matches){
    List<string> longest_words = new List<string>();
    int max_letters = 3;
    foreach(Match match in book_matches){
        if (match.Value.Length == max_letters && !longest_words.Contains(match.Value)) longest_words.Add(match.Value);
        else if(match.Value.Length > max_letters){
            max_letters = match.Value.Length;
            longest_words.Clear();
            longest_words.Add(match.Value);
        }
    }
    return string.Join(", ", longest_words);
}
string ShortestWords(MatchCollection book_matches){
    List<string> shortest_words = new List<string>();
    int min_letters = 10_000;
    foreach(Match match in book_matches){
        if (match.Value.Length == min_letters && !shortest_words.Contains(match.Value)) shortest_words.Add(match.Value);
        else if(match.Value.Length < min_letters){
            min_letters = match.Value.Length;
            shortest_words.Clear();
            shortest_words.Add(match.Value);
        }
    }
    return string.Join(", ", shortest_words);
}

Dictionary<string, int> ClassifyWords(MatchCollection matches_book){
    Dictionary<string, int> dict = new Dictionary<string, int>();
    foreach (Match match in matches_book){
    if(dict.ContainsKey(match.Value)) dict[match.Value] +=1;
    else dict.Add(match.Value, 1);
}
    return dict;
}

string Find5MostCommonWords(Dictionary<string,int> dict){
    int max_count = 0;
    string temp_word = string.Empty;
    List<string> common_words = new List<string>();
    List<int> common_words_count = new List<int>();
    for (int i = 0; i< 5; i++){
        foreach(KeyValuePair<string, int> kv in dict){
            if (max_count < kv.Value && !common_words.Contains(kv.Key)) {
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

string Find5LeastCommonWords(Dictionary<string,int> dict){
    int min_count = 500;
    string temp_word = string.Empty;
    List<string> uncommon_words = new List<string>();
    List<int> uncommon_words_count = new List<int>();
    for (int i = 0; i< 5; i++){
        foreach(KeyValuePair<string, int> kv in dict){
            if (min_count > kv.Value && !uncommon_words.Contains(kv.Key)) {
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

void ThreadMethod1(object p){
    Results.numOfWords = NumberOfWords((MatchCollection) p);
    //System.Console.WriteLine("Thread 1 complete");
}

void ThreadMethod2(object p){
    Results.shortest_words = ShortestWords((MatchCollection) p);
    //System.Console.WriteLine("Thread 2 complete");
}

void ThreadMethod3(object p){
    Results.longest_words = LongestWords((MatchCollection) p);
    //System.Console.WriteLine("Thread 3 complete");
}

void ThreadMethod4(object p){
    Results.average_word_length = AverageWordLength((MatchCollection) p);
    //System.Console.WriteLine("Thread 4 complete");
}

void ThreadMethod5_6(object p){
    Dictionary<string,int> dict = ClassifyWords((MatchCollection) p);
    Results.least_common_words = Find5LeastCommonWords(dict);
    Results.most_common_words = Find5MostCommonWords(dict);
    //System.Console.WriteLine("Thread 5 complete.");
}

static class Results{
static public int numOfWords;
static public string shortest_words;
static public string longest_words;
static public double average_word_length;
static public string most_common_words;
static public string least_common_words;
}