using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace tak2flac
{
    public class CuesheetProbeInfo
    {
        public Dictionary<string, string> GetAllTags { get; } = new Dictionary<string, string>();
        public Dictionary<int, TimeSpan>? Indexes;

        public string? Title => GetTags(nameof(Title));

        public string? Performer => GetTags(nameof(Performer));

        [Obsolete("This property will return last index pointer. Use Indexes property to get more detailed list of index pointers.")]
        public TimeSpan? TimePointer => GetLastEndPointer(); 
        internal void AddIndex(string index, string ts)
        {
            Indexes ??= new Dictionary<int, TimeSpan>();
            
            var i = int.Parse(index);
            if (Indexes.ContainsKey(i))
                Indexes[i] = ts.ParseTimeSpan();
            else{
                Indexes.Add(i, ts.ParseTimeSpan());
            }
        }
        internal void AddTags(string key, string value)
        {
            GetAllTags.Add(key.ToLower(), value);
        }
        public string? GetTags(string key)
        {
            if (GetAllTags.TryGetValue(key.ToLower(), out var result))
                return result;
            else
                return null;
        }
        public TimeSpan? GetLastEndPointer()
        {
            if(Indexes != null)
            {
                CheckAvabilityIndexes();
                long maxTicks = 0;
                foreach (var item in Indexes)
                {
                    maxTicks = Math.Max(maxTicks, item.Value.Ticks);
                }
                return TimeSpan.FromTicks(maxTicks);
            }
            return null;
        }
        public TimeSpan? GetRecommentedStartIndex()
        {
            if (Indexes != null)
            {
                CheckAvabilityIndexes();
                if (Indexes.TryGetValue(1, out TimeSpan value))
                    return value;
                else if (Indexes.TryGetValue(0, out TimeSpan value2))
                    return value2;
            }
                return GetLastEndPointer();

            // else
                // return GetLastEndPointer();
        }
        public TimeSpan? GetEarlyIndex()
        {
            if (Indexes == null)
                return null;

            CheckAvabilityIndexes();
            long minTicks = Indexes.First().Value.Ticks;
            foreach (var item in Indexes)
            {
                minTicks = Math.Min(minTicks, item.Value.Ticks);
            }
            return TimeSpan.FromTicks(minTicks);
        }
        private void CheckAvabilityIndexes()
        {
            if (Indexes is null)
                throw new NullReferenceException("Indexes dictionary is not initialized.");
            if (Indexes.Count == 0)
                throw new ArgumentOutOfRangeException("Indexes dictionary is empty.");
        }

    }
    public class CuesheetInfo
    {
        public string? Genre => GetTags(nameof(Genre));
        public string? Date => GetTags(nameof(Date));
        public string? DiscId => GetTags(nameof(DiscId));
        public string? Comment => GetTags(nameof(Comment));
        public string? Performer => GetTags(nameof(Performer));
        public string? Title => GetTags(nameof(Title));
        internal void AddTags(string key, string value)
        {
            GetAllTags.Add(key.ToLower(), value);
        }
        public string? GetTags(string key)
        {
            if (GetAllTags.TryGetValue(key.ToLower(), out var result))
                return result;
            else
                return null;
        }

        public Dictionary<string, string> GetAllTags { get; } = new Dictionary<string, string>();
    }
    public class Cuesheet
    {
        public Cuesheet(string cueSheetData)
        {
            var root = new CuesheetInfo();
            List<CuesheetProbeInfo> Probes = [];
            string[] csDataLines = cueSheetData.Replace("\r\n", "\n").Split('\n');
            bool trackProbes = false;
            CuesheetProbeInfo? probe = new();

            foreach (var data in csDataLines)
            {
                var s = data.RemoveUnnecessarySpaces().SplitKeepQuotes();
                if (s[0].Equals("rem", StringComparison.CurrentCultureIgnoreCase))
                { 
                    root.AddTags(s[1], s[2]);
                }
                else if (s[0].Equals("performer", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!trackProbes) 
                        root.AddTags("performer", s[1].RemoveQuotes());
                    else 
                        probe.AddTags("performer", s[1].RemoveQuotes());
                }
                else if (s[0].Equals("title", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!trackProbes) 
                        root.AddTags("title", s[1].RemoveQuotes());
                    else 
                        probe.AddTags("title", s[1].RemoveQuotes());
                }
                else if (s[0].Equals("index", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (trackProbes)
                        probe.AddIndex(s[1], s[2]);
                    //probe.TimePointer = s[2].ParseTimeSpan();
                }
                else if (s[0].Equals("track", StringComparison.CurrentCultureIgnoreCase))
                {
                    trackProbes = true;
                    //int index = int.Parse(s[1]);
                    if (probe.Indexes != null)
                    {
                        Probes.Add(probe);
                    }
                    probe = new CuesheetProbeInfo();
                }
            }

            if (probe != null)
                Probes.Add(probe);
            Root = root;
            AudioProbes = [.. Probes];
        }

        public CuesheetInfo Root;
        public CuesheetProbeInfo[] AudioProbes;
    }
}
