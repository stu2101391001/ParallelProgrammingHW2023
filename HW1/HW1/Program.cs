using System.Diagnostics;
using System.Text.RegularExpressions;

StreamReader streamReader = new StreamReader("long_book.txt");
string book = streamReader.ReadToEnd().ToLower();
streamReader.Close();
string pattern = @"[а-яА-Я]{3,}";
Regex rex = new Regex(pattern, RegexOptions.Compiled);
MatchCollection matches_book = rex.Matches(book);

Dictionary<string, int> dict = new Dictionary<string, int>();
foreach (Match match in matches_book){
    if(dict.ContainsKey(match.Value)) dict[match.Value] +=1;
    else dict.Add(match.Value, 1);
}

Stopwatch stopwatch = new Stopwatch();

stopwatch.Start();
Thread[] threads = {new Thread(ThreadMethod1), 
                    new Thread(ThreadMethod2),
                    new Thread(ThreadMethod3),
                    new Thread(ThreadMethod4)};
foreach(Thread thread in threads) thread.Start(dict);
foreach(Thread thread in threads) thread.Join();
stopwatch.Stop();
System.Console.WriteLine(stopwatch.ElapsedMilliseconds);


/*stopwatch.Start();
string lW = FindLongedsWords(dict);
string sW = FindShortestWords(dict);
string mC = Find5MostCommonWords(dict);
string lC = Find5LeastCommonWords(dict);
stopwatch.Stop();
System.Console.WriteLine(stopwatch.ElapsedMilliseconds);*/


/*System.Console.WriteLine(lW);
System.Console.WriteLine(sW);
System.Console.WriteLine("Most commong words: ");
System.Console.WriteLine(mC);
System.Console.WriteLine("Least commong words: ");
System.Console.WriteLine(lC);*/

StreamWriter streamWriter = new StreamWriter("results.txt");
foreach(KeyValuePair<string,int> kv in dict) streamWriter.WriteLine($"{kv.Key} : {kv.Value}");
streamWriter.Flush();
streamWriter.Close();

string FindLongedsWords(Dictionary<string,int> dict){
    List<string> longest_words = new List<string>();
    int max_letters = 3;
    foreach(KeyValuePair<string,int> kv in dict){
        if (kv.Key.Length == max_letters) longest_words.Add(kv.Key);
        else if(kv.Key.Length > max_letters){
            max_letters = kv.Key.Length;
            longest_words.Clear();
            longest_words.Add(kv.Key);
        }
    }
    return string.Join("\n", longest_words);
}
string FindShortestWords(Dictionary<string,int> dict){
    List<string> shortest_words = new List<string>();
    int min_letters = 10_000;
    foreach(KeyValuePair<string,int> kv in dict){
        if (kv.Key.Length == min_letters) shortest_words.Add(kv.Key);
        else if(kv.Key.Length < min_letters){
            min_letters = kv.Key.Length;
            shortest_words.Clear();
            shortest_words.Add(kv.Key);
        }
    }
    return string.Join("\n", shortest_words);
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
    FindLongedsWords((Dictionary<string,int>) p);
}
void ThreadMethod2(object p){
    FindShortestWords((Dictionary<string,int>) p);
}
void ThreadMethod3(object p){
    Find5LeastCommonWords((Dictionary<string,int>) p);
}
void ThreadMethod4(object p){
    Find5MostCommonWords((Dictionary<string,int>) p);
}