namespace Monicais.Core
{

    public interface IDisplayable : INameable
    {
        string Description { get; set; }
    }

    public abstract class Displayable : IDisplayable, INameable
    {

        public Displayable(string name)
        {
            Name = name;
        }

        public Displayable(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        public string Description { get; set; }

        public string Name { get; set; }
    }

    public abstract class NonNullDisplayable : IDisplayable, INameable
    {
        private string desc;
        private string name;

        public NonNullDisplayable(string name)
        {
            Name = name;
        }

        public NonNullDisplayable(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        public string Description
        {
            get { return desc; }
            set { desc = value ?? ""; }
        }

        public string Name
        {
            get { return name; }
            set { name = value ?? ""; }
        }
    }
}

