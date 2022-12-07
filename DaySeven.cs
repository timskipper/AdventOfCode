namespace AdventOfCode
{
    public class DaySeven
    {
        public DaySeven(string dataFile)
        {
            var input = File.ReadAllLines(dataFile);
            var root = BuildFileSystem(input.ToList());

            root.Traverse(root, (Directory directory) =>
            {
                if (directory.TotalSize <= 100000)
                {
                    Part1Answer += directory.TotalSize;
                }
            });

            var directories = new List<KeyValuePair<string, int>>();
            root.Traverse(root, (Directory directory) =>
            {
                directories.Add(new KeyValuePair<string, int>(directory.Name, directory.TotalSize));
            });

            const int DiskSize = 70000000;
            const int UpdateSize = 30000000;

            var freeSpace = DiskSize - directories.Single(x => x.Key == "/").Value;
            var requiredSpace = UpdateSize - freeSpace;

            Part2Answer = directories
                .Where(x => x.Value >= requiredSpace).MinBy(y => y.Value)
                .Value;
        }

        public int Part1Answer { get; private set; }

        public int Part2Answer { get; }

        private Directory BuildFileSystem(List<string> input)
        {
            var root = new Directory("/");
            var currentDirectory = root;

            foreach (var line in input)
            {
                var l = line.Split(' ');

                if (l[0] == "$")
                {
                    if (l[1] == "cd")
                    {
                        switch (l[2])
                        {
                            case "/":
                                continue;

                            case "..":
                                currentDirectory = currentDirectory.Parent;
                                break;

                            default:
                                currentDirectory = currentDirectory.Children.Single(x => x.Name == l[2]);
                                break;
                        }
                    }
                }
                else
                {
                    if (int.TryParse(l[0], out var size))
                    {
                        currentDirectory.Files.Add(l[1], size);
                    }
                    else if (l[0] == "dir")
                    {
                        currentDirectory.AddDirectory(l[1]);
                    }
                }
            }

            return root;
        }
    }

    internal class Directory
    {
        public Directory(string name)
        {
            Name = name;
            Files = new Dictionary<string, int>();
            Children = new LinkedList<Directory>();
        }

        public Directory? Parent { get; set; }
        public LinkedList<Directory> Children { get; set; }
        public string Name { get; set; }
        public Dictionary<string, int> Files { get; set; }

        public int TotalFileSize => Files.Sum(x => x.Value);

        public int TotalChildrenSize
        {
            get
            {
                var size = 0;
                foreach (var child in Children)
                {
                    Traverse(child, (Directory directory) =>
                    {
                        size += directory.TotalFileSize;
                    });
                }
                return size;
            }
        }

        public int TotalSize => TotalFileSize + TotalChildrenSize;

        public void AddDirectory(string name)
        {
            Children ??= new LinkedList<Directory>();
            var newDirectory = new Directory(name)
            {
                Parent = this
            };
            Children.AddFirst(newDirectory);
        }

        public void Traverse(Directory node, Action<Directory> visitor)
        {
            visitor(node);
            foreach (var child in node.Children)
            {
                Traverse(child, visitor);
            }
        }
    }
}
