namespace Monicais.Core
{

    public interface INameable
    {
        string Name { get; set; }
    }

    public interface IDescribable
    {
        string Description { get; set; }
    }

    public interface IDisplayable : INameable, IDescribable { }

    public abstract class Displayable : IDisplayable
    {

        public Displayable(string name, string desc = null)
        {
            Name = name;
            Description = desc;
        }

        public string Description { get; set; }

        public string Name { get; set; }
    }

    public abstract class NonNullDisplayable : IDisplayable
    {

        public NonNullDisplayable(string name, string desc = "")
        {
            Name = name;
            Description = desc;
        }

        public string Name
        {
            get { return name; }
            set { name = value ?? ""; }
        }
        private string name;

        public string Description
        {
            get { return desc; }
            set { desc = value ?? ""; }
        }
        private string desc;
    }
}
